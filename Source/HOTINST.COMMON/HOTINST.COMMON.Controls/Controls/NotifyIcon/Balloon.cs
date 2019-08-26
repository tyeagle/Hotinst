/**
 * ==============================================================================
 *
 * ClassName: Balloon
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/8/26 15:54:45
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Controls.NotifyIcon
{
	/// <summary>
	/// 
	/// </summary>
	public class Balloon : Popup, IDisposable
	{
		#region fields

		private readonly TaskbarIcon _taskbarIcon;

		/// <summary>
		/// A timer that is used to close open balloon tooltips.
		/// </summary>
		private readonly System.Threading.Timer _balloonCloseTimer;

		private int? _timeout;

		#endregion

		#region props

		#endregion

		#region .ctor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="taskbarIcon"></param>
		public Balloon(TaskbarIcon taskbarIcon)
		{
			_taskbarIcon = taskbarIcon ?? throw new ArgumentNullException(nameof(taskbarIcon));

			AllowsTransparency = true;

			//don't set the PlacementTarget as it causes the popup to become hidden if the
			//TaskbarIcon's parent is hidden, too...
			//popup.PlacementTarget = this;

			Placement = PlacementMode.AbsolutePoint;
			StaysOpen = true;

			_balloonCloseTimer = new System.Threading.Timer(CloseBalloonCallback);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="balloon"></param>
		/// <returns></returns>
		public Size Measure(UIElement balloon)
		{
			Child = balloon ?? throw new ArgumentNullException(nameof(balloon));
			double opacity = balloon.Opacity;
			balloon.Opacity = 0;
			IsOpen = true;
			IsOpen = false;
			balloon.Opacity = opacity;
			return balloon.DesiredSize;
		}

		/// <summary>
		/// Shows a custom control as a tooltip in the tray location.
		/// </summary>
		/// <param name="balloon"></param>
		/// <param name="animation">An optional animation for the popup.</param>
		/// <param name="position">position for the popup.</param>
		/// <param name="timeout">The time after which the popup is being closed.
		/// Submit null in order to keep the balloon open inde
		/// </param>
		/// <exception cref="ArgumentNullException">If <paramref name="balloon"/>
		/// is a null reference.</exception>
		public void Show(UIElement balloon, PopupAnimation animation, Interop.Point position, int? timeout)
		{
			if(!Application.Current.Dispatcher.CheckAccess())
			{
				Action action = () => Show(balloon, animation, position, timeout);
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, action);
				return;
			}

			if(timeout.HasValue && timeout < 500)
			{
				string msg = "Invalid timeout of {0} milliseconds. Timeout must be at least 500 ms";
				msg = string.Format(msg, timeout);
				throw new ArgumentOutOfRangeException(nameof(timeout), msg);
			}

			EnsureNotDisposed();

			//provide the popup with the taskbar icon's data context
			_taskbarIcon.UpdateDataContext(this, null, _taskbarIcon.DataContext);

			//don't animate by default - devs can use attached
			//events or override
			PopupAnimation = animation;

			Child = balloon ?? throw new ArgumentNullException(nameof(balloon));

			HorizontalOffset = position.X - 1;
			VerticalOffset = position.Y - 1;
			
			//assign this instance as an attached property
			TaskbarIcon.SetParentTaskbarIcon(balloon, _taskbarIcon);
			TaskbarIcon.SetParentBalloon(balloon, this);

			//fire attached event
			TaskbarIcon.RaiseBalloonShowingEvent(balloon, _taskbarIcon);

			//display item
			IsOpen = true;

			_timeout = timeout;
			ResetBalloonCloseTimer();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newValue"></param>
		public void AnimateNewPosition(double newValue)
		{
			Storyboard sb = new Storyboard();

			DoubleAnimation da = new DoubleAnimation(newValue, new Duration(TimeSpan.FromMilliseconds(150)));

			Storyboard.SetTarget(da, this);
			Storyboard.SetTargetProperty(da, new PropertyPath(nameof(VerticalOffset)));

			sb.Children.Add(da);

			sb.Begin();
		}

		/// <summary>
		/// Closes this popup.
		/// </summary>
		public void Close()
		{
			if(IsDisposed)
				return;

			if(!Application.Current.Dispatcher.CheckAccess())
			{
				Action action = Close;
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, action);
				return;
			}

			lock(this)
			{
				//reset timer in any case
				_balloonCloseTimer.Change(Timeout.Infinite, Timeout.Infinite);

				UIElement element = Child;

				//announce closing
				RoutedEventArgs eventArgs = TaskbarIcon.RaiseBalloonClosingEvent(element, _taskbarIcon);
				if(!eventArgs.Handled)
				{
					//if the event was handled, clear the reference to the popup,
					//but don't close it - the handling code has to manage this stuff now

					//close the popup
					DoClose(element);
				}
			}
		}

		private void DoClose(UIElement element)
		{
			Storyboard sb = new Storyboard();

			DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(300)));

			Storyboard.SetTarget(da, Child);
			Storyboard.SetTargetProperty(da, new PropertyPath(nameof(Opacity)));

			sb.Children.Add(da);

			sb.Completed += (sender, args) =>
			{
				IsOpen = false;
				TaskbarIcon.RaiseBalloonClosedEvent(element, _taskbarIcon);

				TaskbarIcon.SetParentBalloon(element, null);
			};
			sb.Begin();
		}

		/// <summary>
		/// Resets the closing timeout, which effectively
		/// keeps a displayed balloon message open until
		/// it is either closed programmatically through
		/// CloseBalloon or due to a new
		/// message being displayed.
		/// </summary>
		private void ResetBalloonCloseTimer()
		{
			if(IsDisposed) return;

			lock(this)
			{
				if(_timeout.HasValue)
				{
					_balloonCloseTimer.Change(_timeout.Value, Timeout.Infinite);
				}
			}
		}

		private void PauseBalloonCloseTimer()
		{
			if(IsDisposed)
				return;

			lock(this)
			{
				if(_timeout.HasValue)
				{
					_balloonCloseTimer.Change(Timeout.Infinite, Timeout.Infinite);
				}
			}
		}

		private void ResumeBalloonCloseTimer()
		{
			if(IsDisposed)
				return;

			lock(this)
			{
				if(_timeout.HasValue)
				{
					_balloonCloseTimer.Change(_timeout.Value, Timeout.Infinite);
				}
			}
		}

		/// <summary>
		/// Timer-invoke event which closes this balloon.
		/// </summary>
		private void CloseBalloonCallback(object state)
		{
			if(IsDisposed) return;

			//switch to UI thread
			Action action = Close;
			Application.Current.Dispatcher.Invoke(action);
		}

		#region Overrides of UIElement

		/// <summary>
		///     在此元素上引发未处理的 <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> 附加事件时，调用此方法。实现此方法可为此事件添加类处理。
		/// </summary>
		/// <param name="e">
		///     包含事件数据的 <see cref="T:System.Windows.Input.MouseEventArgs" />。
		/// </param>
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			PauseBalloonCloseTimer();
			base.OnMouseEnter(e);
		}

		/// <summary>
		///     在此元素上引发未处理的 <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> 附加事件时，调用此方法。实现此方法可为此事件添加类处理。
		/// </summary>
		/// <param name="e">
		///     包含事件数据的 <see cref="T:System.Windows.Input.MouseEventArgs" />。
		/// </param>
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			ResumeBalloonCloseTimer();
			base.OnMouseLeave(e);
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Set to true as soon as Dispose
		/// has been invoked.
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// Checks if the object has been disposed and
		/// raises a <see cref="ObjectDisposedException"/> in case
		/// the <see cref="IsDisposed"/> flag is true.
		/// </summary>
		private void EnsureNotDisposed()
		{
			if(IsDisposed) throw new ObjectDisposedException(GetType().FullName);
		}

		/// <summary>
		/// This destructor will run only if the <see cref="Dispose()"/>
		/// method does not get called. This gives this base class the
		/// opportunity to finalize.
		/// <para>
		/// Important: Do not provide destructors in types derived from
		/// this class.
		/// </para>
		/// </summary>
		~Balloon()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes the object.
		/// </summary>
		/// <remarks>This method is not virtual by design. Derived classes
		/// should override <see cref="Dispose(bool)"/>.
		/// </remarks>
		public void Dispose()
		{
			Dispose(true);

			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Closes the tray and releases all resources.
		/// </summary>
		/// <summary>
		/// <c>Dispose(bool disposing)</c> executes in two distinct scenarios.
		/// If disposing equals <c>true</c>, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// </summary>
		/// <param name="disposing">If disposing equals <c>false</c>, the method
		/// has been called by the runtime from inside the finalizer and you
		/// should not reference other objects. Only unmanaged resources can
		/// be disposed.</param>
		/// <remarks>Check the <see cref="IsDisposed"/> property to determine whether
		/// the method has already been called.</remarks>
		private void Dispose(bool disposing)
		{
			//don't do anything if the component is already disposed
			if(IsDisposed || !disposing) return;

			lock(this)
			{
				IsDisposed = true;

				//stop timers
				_balloonCloseTimer.Dispose();
			}
		}

		#endregion
	}
}