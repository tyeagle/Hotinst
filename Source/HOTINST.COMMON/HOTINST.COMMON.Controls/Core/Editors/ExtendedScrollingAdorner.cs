/**
 * ==============================================================================
 *
 * ClassName: ExtendedScrollingAdorner
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:17:01
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using HOTINST.COMMON.Controls.Controls.Editors;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Core.Editors
{
	[DesignTimeVisible(false)]
	public class ExtendedScrollingAdorner : Adorner
	{
		private Thumb valueThumb;
		private VisualCollection visualCollection;
		private Rect adornedElementRect;
		private FrameworkElement fElement;
		private double prevVerticalChange;
		private double prevHorizontalChange;
		private CursorHandler.POINT pnt;
		private bool isThumbMoved;
		private StreamResourceInfo info;
		private Cursor scrollingCursor;
		public bool IsReadOnly
		{
			get
			{
				if(this.fElement is EditorBase)
				{
					return (this.fElement as EditorBase).IsReadOnly;
				}
				return false;
			}
		}
		protected override int VisualChildrenCount
		{
			get
			{
				return this.visualCollection.Count;
			}
		}
		public ExtendedScrollingAdorner(UIElement adornedElement) : base(adornedElement)
		{
			this.valueThumb = new Thumb
			{
				Opacity = 0.0
			};
			this.visualCollection = new VisualCollection(this)
			{
				this.valueThumb
			};
			if(base.AdornedElement != null)
			{
				this.adornedElementRect = new Rect(base.AdornedElement.DesiredSize);
			}
			this.fElement = (adornedElement as FrameworkElement);
			ResourceDictionary resourceDictionary = new ResourceDictionary
			{
				Source = new Uri("/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
			};
			this.info = Application.GetResourceStream(new Uri("/Syncfusion.Shared.WPF;component/Controls/Editors/Cursors/SizeAllCursor.cur", UriKind.RelativeOrAbsolute));
			if(this.info != null)
			{
				this.scrollingCursor = new Cursor(this.info.Stream);
			}
			Style style = resourceDictionary["ExtendedScrollingAdornerStyle"] as Style;
			this.valueThumb.Style = style;
			if(this.fElement != null)
			{
				if(this.fElement.IsLoaded)
				{
					this.ArrangeThumb();
				}
				this.fElement.PreviewMouseMove += new MouseEventHandler(this.fElement_PreviewMouseMove);
				this.fElement.IsKeyboardFocusedChanged += new DependencyPropertyChangedEventHandler(this.fElement_IsKeyboardFocusedChanged);
				this.fElement.IsKeyboardFocusWithinChanged += new DependencyPropertyChangedEventHandler(this.fElement_IsKeyboardFocusWithinChanged);
			}
			this.valueThumb.DragDelta += new DragDeltaEventHandler(this.valueThumb_DragDelta);
			this.valueThumb.PreviewMouseUp += new MouseButtonEventHandler(this.valueThumb_PreviewMouseUp);
			this.valueThumb.PreviewMouseDown += new MouseButtonEventHandler(this.valueThumb_PreviewMouseDown);
			this.valueThumb.PreviewMouseMove += new MouseEventHandler(this.valueThumb_PreviewMouseMove);
		}
		private void valueThumb_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if(this.IsReadOnly && this.valueThumb != null)
			{
				this.valueThumb.Visibility = Visibility.Collapsed;
			}
		}
		private void fElement_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if((bool)e.NewValue && this.valueThumb != null)
			{
				this.valueThumb.Visibility = Visibility.Collapsed;
				this.valueThumb.Cursor = Cursors.IBeam;
			}
		}
		private void fElement_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if((bool)e.NewValue && this.valueThumb != null)
			{
				this.valueThumb.Visibility = Visibility.Collapsed;
				this.valueThumb.Cursor = Cursors.IBeam;
			}
		}
		private void valueThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			CursorHandler.GetCursorPos(out this.pnt);
		}
		private void fElement_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if(this.valueThumb != null)
			{
				if(!this.IsReadOnly)
				{
					if(!this.fElement.IsFocused)
					{
						this.valueThumb.Visibility = Visibility.Visible;
						this.valueThumb.Cursor = this.scrollingCursor;
						return;
					}
				}
				else
				{
					this.valueThumb.Visibility = Visibility.Collapsed;
					this.valueThumb.Cursor = Cursors.IBeam;
				}
			}
		}
		private void valueThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			TextBox textBox = base.AdornedElement as TextBox;
			if(textBox != null)
			{
				if(!textBox.IsFocused && !this.isThumbMoved)
				{
					textBox.Focus();
					if(this.valueThumb != null)
					{
						this.valueThumb.Visibility = Visibility.Collapsed;
					}
				}
				else
				{
					if(this.valueThumb != null)
					{
						this.valueThumb.Cursor = this.scrollingCursor;
					}
					CursorHandler.SetCursorPos(this.pnt.X, this.pnt.Y);
				}
			}
			this.isThumbMoved = false;
		}
		private void valueThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if(this.valueThumb != null)
			{
				this.valueThumb.Cursor = Cursors.None;
			}
			if(!this.IsReadOnly)
			{
				if(e.HorizontalChange > this.prevHorizontalChange || e.VerticalChange < this.prevVerticalChange)
				{
					this.ValueChange(true);
				}
				else
				{
					this.ValueChange(false);
				}
				this.prevHorizontalChange = e.HorizontalChange;
				this.prevVerticalChange = e.VerticalChange;
				if(System.Math.Abs(e.HorizontalChange) > 5.0 || System.Math.Abs(e.VerticalChange) > 5.0)
				{
					this.isThumbMoved = true;
				}
			}
		}
		private void ValueChange(bool increase)
		{
			if(base.AdornedElement is IntegerTextBox)
			{
				IntegerTextBox integerTextBox = base.AdornedElement as IntegerTextBox;
				if(increase)
				{
					long? value = integerTextBox.Value;
					long num = (long)integerTextBox.ScrollInterval;
					long? num2 = value.HasValue ? new long?(value.GetValueOrDefault() + num) : null;
					long? num3 = num2;
					long maxValue = integerTextBox.MaxValue;
					if(num3.GetValueOrDefault() <= maxValue && num3.HasValue && (integerTextBox.MaxLength == 0 || num2.ToString().Length <= integerTextBox.MaxLength))
					{
						integerTextBox.Value = num2;
					}
				}
				else
				{
					long? value2 = integerTextBox.Value;
					long num4 = (long)integerTextBox.ScrollInterval;
					long? num5 = value2.HasValue ? new long?(value2.GetValueOrDefault() - num4) : null;
					long? num6 = num5;
					long minValue = integerTextBox.MinValue;
					if(num6.GetValueOrDefault() >= minValue && num6.HasValue)
					{
						integerTextBox.Value = num5;
					}
				}
			}
			if(base.AdornedElement is DoubleTextBox)
			{
				DoubleTextBox doubleTextBox = base.AdornedElement as DoubleTextBox;
				if(increase)
				{
					double? value3 = doubleTextBox.Value;
					double scrollInterval = doubleTextBox.ScrollInterval;
					double? num7 = value3.HasValue ? new double?(value3.GetValueOrDefault() + scrollInterval) : null;
					double? num8 = num7;
					double maxValue2 = doubleTextBox.MaxValue;
					if(num8.GetValueOrDefault() <= maxValue2 && num8.HasValue && (doubleTextBox.MaxLength == 0 || num7.ToString().Length <= doubleTextBox.MaxLength))
					{
						doubleTextBox.Value = num7;
					}
				}
				else
				{
					double? value4 = doubleTextBox.Value;
					double scrollInterval2 = doubleTextBox.ScrollInterval;
					double? num9 = value4.HasValue ? new double?(value4.GetValueOrDefault() - scrollInterval2) : null;
					double? num10 = num9;
					double minValue2 = doubleTextBox.MinValue;
					if(num10.GetValueOrDefault() >= minValue2 && num10.HasValue)
					{
						doubleTextBox.Value = num9;
					}
				}
			}
		}
		private void ArrangeThumb()
		{
			if(this.valueThumb != null)
			{
				if(this.fElement != null)
				{
					this.valueThumb.Arrange(new Rect(this.adornedElementRect.TopLeft.X, this.adornedElementRect.TopLeft.Y, this.fElement.RenderSize.Width, this.fElement.RenderSize.Height));
				}
				this.valueThumb.Visibility = Visibility.Collapsed;
			}
		}
		protected override Visual GetVisualChild(int index)
		{
			if(this.visualCollection.Count > index)
			{
				return this.visualCollection[index];
			}
			return null;
		}
	}
}