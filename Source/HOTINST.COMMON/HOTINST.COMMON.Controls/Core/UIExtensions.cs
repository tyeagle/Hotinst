/**
 * ==============================================================================
 *
 * ClassName: UIExtensions
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/20 13:33:25
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Globalization;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// Contains UI-related extension methods.
	/// </summary>
	public static class UIExtensions
	{
		/// <summary>
		/// Determines whether the specified element is fully in the viewport of the first scrollviewr parent
		/// (i.e. not clipped).
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public static bool IsFullyInsideScrollViewer(this UIElement element)
		{
			// modified from http://blogs.msdn.com/b/llobo/archive/2007/01/18/elements-visibility-inside-scrollviewer.aspx

			if(element != null)
			{
				ScrollViewer scroll = FindParentInVisualTree<ScrollViewer>(element);
				if(scroll != null)
				{
					// position of your visual inside the scrollviewer    
					GeneralTransform childTransform = element.TransformToAncestor(scroll);
					Rect testRect = childTransform.TransformBounds(new Rect(new Point(), element.RenderSize));

					//Check if the elements Rect intersects with that of the scrollviewer's
					Rect viewPortRect = new Rect(new Point(), new Size(scroll.ViewportWidth, scroll.ViewportHeight));
					Rect result = Rect.Intersect(viewPortRect, testRect);

					// if clipped the result will be diff from element rect.
					return testRect == result;
				}
			}
			return false;
		}

		/// <summary>
		/// Finds the first parent of the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="control">The control.</param>
		/// <returns></returns>
		public static T FindParentInVisualTree<T>(this DependencyObject control) where T : DependencyObject
		{
			while(control != null)
			{
				T test = control as T;
				if(test != null)
					return test;

				control = VisualTreeHelper.GetParent(control);
			}
			return null;
		}

		/// <summary>
		/// Finds the first specified object type in visual tree.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="control">The control.</param>
		/// <returns></returns>
		public static T FindChildInVisualTree<T>(this DependencyObject control) where T : DependencyObject
		{
			return FindChildInVisualTree<T>(control, false);
		}

		/// <summary>
		/// Finds the first specified object type in visual tree.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="control">The control.</param>
		/// <param name="reverse">if set to <c>true</c> then find in reverse order (last child first).</param>
		/// <returns></returns>
		public static T FindChildInVisualTree<T>(this DependencyObject control, bool reverse) where T : DependencyObject
		{
			if(control != null)
			{
				T casted = control as T;
				if(casted != null)
				{ return casted; }

				int count = VisualTreeHelper.GetChildrenCount(control);

				if(reverse)
				{
					for(int i = count - 1; i >= 0; i--)
					{
						DependencyObject c = VisualTreeHelper.GetChild(control, i);
						casted = c as T;
						if(casted != null)
						{
							return casted;
						}
						if(c != null)
						{
							T subHit = FindChildInVisualTree<T>(c, true);
							if(subHit != null)
							{ return subHit; }
						}
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						DependencyObject c = VisualTreeHelper.GetChild(control, i);
						casted = c as T;
						if(casted != null)
						{
							return casted;
						}
						if(c != null)
						{
							T subHit = FindChildInVisualTree<T>(c, false);
							if(subHit != null)
							{ return subHit; }
						}
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Processes all children that are of the specified type in the visual tree.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="container">The control.</param>
		/// <param name="callback">The callback. Return <code>true</code> to stop the tree walk.</param>
		/// <returns></returns>
		public static bool ProcessInVisualTree<T>(this DependencyObject container, Predicate<T> callback) where T : DependencyObject
		{
			if(container != null)
			{
				int count = VisualTreeHelper.GetChildrenCount(container);

				for(int i = 0; i < count; i++)
				{
					DependencyObject c = VisualTreeHelper.GetChild(container, i);
					T casted = c as T;
					if(casted != null && callback != null)
					{
						bool result = callback(casted);
						if(result)
						{ return true; }
					}
					if(c != null)
					{
						bool subResult = ProcessInVisualTree(c, callback);
						if(subResult)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		



		// from msdn http://msdn.microsoft.com/library/system.windows.threading.dispatcher.pushframe.aspx

		/// <summary>
		/// Simulate the famous DoEvents() method from winform days. This may or may not work.
		/// </summary>
		/// <param name="application">The application.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void DoEvents(this Application application)
		{
			if(application == null)
			{
				throw new ArgumentNullException("application");
			}
			application.Dispatcher.DoEvents();
		}

		/// <summary>
		/// Simulate the famous DoEvents() method from winform days.
		/// </summary>
		/// <param name="dispatcher">The dispatcher.</param>
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void DoEvents(this Dispatcher dispatcher)
		{
			if(dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}
			DispatcherFrame frame = new DispatcherFrame();
			dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
			Dispatcher.PushFrame(frame);
		}

		private static object ExitFrame(object f)
		{
			((DispatcherFrame)f).Continue = false;
			return null;
		}

		internal static T AddHandler<T>(this T element, RoutedEvent routedEvent, Delegate handler)
			where T : DependencyObject
		{
			if(element == null)
			{
				throw new ArgumentNullException("element");
			}

			if(!DoSomethingAs(element as UIElement, uie => uie.AddHandler(routedEvent, handler)) &&
				!DoSomethingAs(element as ContentElement, ce => ce.AddHandler(routedEvent, handler)) &&
				!DoSomethingAs(element as UIElement3D, u3D => u3D.AddHandler(routedEvent, handler)))
			{
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid element {0}.", element.GetType()));
			}
			return element;
		}

		internal static T RemoveHandler<T>(this T element, RoutedEvent routedEvent, Delegate handler)
			where T : DependencyObject
		{
			if(element == null)
			{ throw new ArgumentNullException("element"); }

			if(!DoSomethingAs(element as UIElement, uie => uie.RemoveHandler(routedEvent, handler)) &&
				!DoSomethingAs(element as ContentElement, ce => ce.RemoveHandler(routedEvent, handler)) &&
				!DoSomethingAs(element as UIElement3D, u3D => u3D.RemoveHandler(routedEvent, handler)))
			{
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid element {0}.", element.GetType()));
			}
			return element;
		}
		
		private static bool DoSomethingAs<T>(T something, Action<T> callback) where T : class
		{
			if(something == null)
			{
				return false;
			}
			callback(something);
			return true;
		}
	}
}