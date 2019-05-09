/**
 * ==============================================================================
 *
 * ClassName: DraggingAdorner
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/8 15:47:25
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Extension;
using HOTINST.COMMON.Win32;

namespace HOTINST.COMMON.Controls.Attaches
{
    /// <summary>
    /// 拖动时的装饰器
    /// </summary>
    public class DraggingAdorner : Adorner
	{
		#region fields

		private readonly FrameworkElement _draggingElement;

		#endregion
		
		#region .ctor

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="element"></param>
		public DraggingAdorner(UIElement element)
			: base(element)
		{
			IsHitTestVisible = false;
			_draggingElement = element as FrameworkElement;
		}

		#endregion

		#region Overrides of UIElement

		/// <summary>When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing. </summary>
		/// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			if(_draggingElement == null)
			{
				return;
			}

			if(Win32API.GetCursorPos(out POINT screenPos))
			{
				Point pos = PointFromScreen(new Point(screenPos.x, screenPos.y));
				Rect rect = new Rect(pos.X, pos.Y, _draggingElement.ActualWidth, _draggingElement.ActualHeight);
				drawingContext.PushOpacity(0.7);
				if(_draggingElement.TryFindResource(SystemColors.HighlightBrushKey) is Brush highlight)
				{
					Color c = highlight.ToColor();
					c.A = (byte)(c.A / 2);
					drawingContext.DrawRectangle(new SolidColorBrush(c), new Pen(Brushes.Transparent, 0), rect);
				}
				drawingContext.DrawRectangle(new VisualBrush(_draggingElement), new Pen(Brushes.Transparent, 0), rect);
				drawingContext.Pop();
			}
		}

		#endregion
	}
}