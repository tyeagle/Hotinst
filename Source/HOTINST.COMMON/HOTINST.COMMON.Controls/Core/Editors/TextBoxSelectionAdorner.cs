/**
 * ==============================================================================
 *
 * ClassName: TextBoxSelectionAdorner
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:14:08
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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Core.Editors
{
	[DesignTimeVisible(false)]
	public class TextBoxSelectionAdorner : Adorner
	{
		private Thumb selectionThumb1;
		private Thumb selectionThumb2;
		private VisualCollection visualCollection;
		private bool isCalledByDragDelta;
		private bool isCalledByMouseUp;
		private double widthChange2;
		private double widthChange1;
		private ThumbDirection thumbDirection;
		private TextBox iTextBox;
		protected override int VisualChildrenCount
		{
			get
			{
				return this.visualCollection.Count;
			}
		}
		public TextBoxSelectionAdorner(UIElement adornedElement) : base(adornedElement)
		{
			this.selectionThumb1 = new Thumb();
			this.selectionThumb2 = new Thumb();
			this.visualCollection = new VisualCollection(this);
			this.selectionThumb1.Height = (this.selectionThumb1.Width = (this.selectionThumb2.Height = (this.selectionThumb2.Width = 30.0)));
			this.selectionThumb1.Margin = (this.selectionThumb2.Margin = new Thickness(-15.0, 0.0, 0.0, 0.0));
			this.selectionThumb1.DragDelta += new DragDeltaEventHandler(this.selectionThumb_DragDelta);
			this.selectionThumb2.DragDelta += new DragDeltaEventHandler(this.selectionThumb_DragDelta);
			this.selectionThumb1.Visibility = Visibility.Collapsed;
			this.selectionThumb2.Visibility = Visibility.Collapsed;
			this.visualCollection.Add(this.selectionThumb1);
			this.visualCollection.Add(this.selectionThumb2);
			this.iTextBox = (adornedElement as TextBox);
			this.selectionThumb1.PreviewMouseDown += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseDown);
			this.selectionThumb2.PreviewMouseDown += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseDown);
			this.selectionThumb1.PreviewMouseUp += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseUp);
			this.selectionThumb2.PreviewMouseUp += new MouseButtonEventHandler(this.selectionThumb_PreviewMouseUp);
			if(this.iTextBox != null)
			{
				this.iTextBox.PreviewMouseDown += new MouseButtonEventHandler(this.iTextBox_PreviewMouseDown);
				this.iTextBox.PreviewMouseUp += new MouseButtonEventHandler(this.iTextBox_PreviewMouseUp);
				this.iTextBox.PreviewKeyDown += new KeyEventHandler(this.iTextBox_PreviewKeyDown);
				this.iTextBox.SelectionChanged += new RoutedEventHandler(this.iTextBox_SelectionChanged);
				this.iTextBox.LostFocus += new RoutedEventHandler(this.iTextBox_LostFocus);
			}
			ResourceDictionary resourceDictionary = new ResourceDictionary
			{
				Source = new Uri("/Syncfusion.Shared.WPF;component/Controls/Editors/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
			};
			Style style = resourceDictionary["SelectionThumbStyle"] as Style;
			this.selectionThumb1.Style = style;
			this.selectionThumb2.Style = style;
		}
		private void selectionThumb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			if(e.StylusDevice != null)
			{
				this.HandleThumbVisiblity();
				return;
			}
			if(this.selectionThumb2 != null)
			{
				this.selectionThumb2.Visibility = Visibility.Collapsed;
			}
			if(this.selectionThumb1 != null)
			{
				this.selectionThumb1.Visibility = Visibility.Collapsed;
			}
		}
		private void iTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if(this.selectionThumb1 != null)
			{
				this.selectionThumb1.Visibility = Visibility.Collapsed;
			}
			if(this.selectionThumb2 != null)
			{
				this.selectionThumb2.Visibility = Visibility.Collapsed;
			}
		}
		private void selectionThumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if(e.StylusDevice == null)
			{
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Visibility = Visibility.Collapsed;
				}
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Visibility = Visibility.Collapsed;
				}
			}
		}
		private void iTextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			TextBox textBox = this.iTextBox;
			if(this.selectionThumb2 != null && this.selectionThumb1 != null && textBox.Text != null && textBox != null && textBox.Text.Length > 0 && this.iTextBox.SelectionLength == 0 && !this.isCalledByDragDelta && !this.isCalledByMouseUp && this.selectionThumb1.Visibility == Visibility.Visible && this.selectionThumb2.Visibility == Visibility.Visible)
			{
				FormattedText formattedText = new FormattedText(this.iTextBox.Text.Substring(0, this.iTextBox.SelectionStart), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
				double width = formattedText.Width;
				int num;
				if(this.iTextBox.SelectionLength == 0)
				{
					num = this.iTextBox.SelectionStart;
				}
				else
				{
					num = this.iTextBox.SelectionLength + this.iTextBox.SelectionStart;
				}
				formattedText = new FormattedText(this.iTextBox.Text.Substring(0, num), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
				double width2 = formattedText.Width;
				this.selectionThumb1.Arrange(new Rect(width, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
				this.selectionThumb1.Tag = new ThumbPosition
				{
					Position = this.iTextBox.SelectionStart,
					StartingPoint = width
				};
				this.selectionThumb2.Arrange(new Rect(width2, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
				this.selectionThumb2.Tag = new ThumbPosition
				{
					Position = num,
					StartingPoint = width2
				};
			}
		}
		public void iTextBox_PreviewMouseUp(object sender, MouseEventArgs e)
		{
			this.isCalledByMouseUp = true;
			if(e.StylusDevice != null)
			{
				this.HandleThumbVisiblity();
			}
			else
			{
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Visibility = Visibility.Collapsed;
				}
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Visibility = Visibility.Collapsed;
				}
			}
			this.isCalledByMouseUp = false;
		}
		private void HandleThumbVisiblity()
		{
			if(this.selectionThumb1 != null && this.selectionThumb2 != null && this.iTextBox != null && this.iTextBox.SelectionLength == 0 && (this.selectionThumb2.Visibility == Visibility.Visible || this.selectionThumb1.Visibility == Visibility.Visible))
			{
				this.selectionThumb2.Visibility = Visibility.Collapsed;
				this.selectionThumb1.Visibility = Visibility.Collapsed;
				return;
			}
			TextBox textBox = this.iTextBox;
			if(textBox != null && this.iTextBox != null && !string.IsNullOrEmpty(textBox.Text) && this.iTextBox.SelectionStart <= this.iTextBox.Text.Length)
			{
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Visibility = Visibility.Hidden;
				}
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Visibility = Visibility.Hidden;
				}
				FormattedText formattedText = new FormattedText(this.iTextBox.Text.Substring(0, this.iTextBox.SelectionStart), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
				double width = formattedText.Width;
				int num;
				if(this.iTextBox.SelectionLength == 0)
				{
					num = this.iTextBox.SelectionStart;
				}
				else
				{
					num = this.iTextBox.SelectionLength + this.iTextBox.SelectionStart;
				}
				formattedText = new FormattedText(this.iTextBox.Text.Substring(0, num), Thread.CurrentThread.CurrentUICulture, this.iTextBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
				double width2 = formattedText.Width;
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Tag = new ThumbPosition
					{
						Position = this.iTextBox.SelectionStart,
						StartingPoint = width
					};
					this.selectionThumb1.Arrange(new Rect(width, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
				}
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Arrange(new Rect(width2, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
					this.selectionThumb2.Tag = new ThumbPosition
					{
						Position = num,
						StartingPoint = width2
					};
				}
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Visibility = Visibility.Visible;
				}
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Visibility = Visibility.Visible;
					return;
				}
			}
			else
			{
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Visibility = Visibility.Collapsed;
				}
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Visibility = Visibility.Collapsed;
				}
			}
		}
		private void iTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if(this.selectionThumb2 != null)
			{
				this.selectionThumb2.Visibility = Visibility.Collapsed;
			}
			if(this.selectionThumb1 != null)
			{
				this.selectionThumb1.Visibility = Visibility.Collapsed;
			}
		}
		private void iTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if(e.StylusDevice == null)
			{
				if(this.selectionThumb2 != null)
				{
					this.selectionThumb2.Visibility = Visibility.Collapsed;
				}
				if(this.selectionThumb1 != null)
				{
					this.selectionThumb1.Visibility = Visibility.Collapsed;
				}
			}
		}
		private void selectionThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			this.isCalledByDragDelta = true;
			TextBox textBox = this.iTextBox;
			if(textBox != null && textBox.Text != null)
			{
				FormattedText formattedText = new FormattedText(textBox.Text.Substring(0, this.iTextBox.Text.Length), Thread.CurrentThread.CurrentUICulture, textBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
				if(e.HorizontalChange > 0.0)
				{
					if(this.thumbDirection == ThumbDirection.Backward)
					{
						if(sender == this.selectionThumb2)
						{
							this.widthChange2 = 0.0;
						}
						else
						{
							this.widthChange1 = 0.0;
						}
					}
					this.thumbDirection = ThumbDirection.Forward;
					if(sender == this.selectionThumb2)
					{
						this.widthChange2 += e.HorizontalChange;
					}
					else
					{
						this.widthChange1 += e.HorizontalChange;
					}
					if(sender != null)
					{
						object tag = ((Thumb)sender).Tag;
						if(tag != null)
						{
							ThumbPosition thumbPosition = (ThumbPosition)tag;
							if(thumbPosition.Position < this.iTextBox.Text.Length)
							{
								FormattedText formattedText2 = new FormattedText(textBox.Text.Substring(thumbPosition.Position, 1), Thread.CurrentThread.CurrentUICulture, textBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
								double num;
								if(sender == this.selectionThumb2)
								{
									num = this.widthChange2;
								}
								else
								{
									num = this.widthChange1;
								}
								if(num >= formattedText2.Width)
								{
									thumbPosition.Position++;
									((Thumb)sender).Tag = thumbPosition;
									ThumbPosition thumbPosition2 = default(ThumbPosition);
									if(sender == this.selectionThumb2)
									{
										if(this.selectionThumb1 != null)
										{
											thumbPosition2 = (ThumbPosition)this.selectionThumb1.Tag;
										}
									}
									else
									{
										thumbPosition2 = (ThumbPosition)this.selectionThumb2.Tag;
									}
									if(thumbPosition.Position > thumbPosition2.Position)
									{
										if(this.iTextBox.SelectionLength + 1 <= this.iTextBox.Text.Length)
										{
											this.iTextBox.SelectionLength = this.iTextBox.SelectionLength + 1;
										}
									}
									else
									{
										if(thumbPosition.Position < thumbPosition2.Position)
										{
											if(this.iTextBox.SelectionStart + 1 <= this.iTextBox.Text.Length)
											{
												this.iTextBox.SelectionStart = this.iTextBox.SelectionStart + 1;
											}
											if(this.iTextBox.SelectionLength > 0 && thumbPosition2.Position != this.iTextBox.Text.Length)
											{
												this.iTextBox.SelectionLength = this.iTextBox.SelectionLength - 1;
											}
										}
										else
										{
											this.iTextBox.SelectionLength = 0;
											if(this.iTextBox.SelectionStart + 1 <= this.iTextBox.Text.Length)
											{
												this.iTextBox.SelectionStart = this.iTextBox.SelectionStart + 1;
											}
										}
									}
									if(sender == this.selectionThumb2)
									{
										this.widthChange2 = 0.0;
									}
									else
									{
										this.widthChange1 = 0.0;
									}
								}
							}
						}
					}
				}
				else
				{
					if(e.HorizontalChange < 0.0)
					{
						if(this.thumbDirection == ThumbDirection.Forward)
						{
							if(sender == this.selectionThumb2)
							{
								this.widthChange2 = 0.0;
							}
							else
							{
								this.widthChange1 = 0.0;
							}
						}
						this.thumbDirection = ThumbDirection.Backward;
						if(sender == this.selectionThumb2)
						{
							this.widthChange2 += System.Math.Abs(e.HorizontalChange);
						}
						else
						{
							this.widthChange1 += System.Math.Abs(e.HorizontalChange);
						}
						if(sender != null)
						{
							object tag2 = ((Thumb)sender).Tag;
							if(tag2 != null)
							{
								ThumbPosition thumbPosition3 = (ThumbPosition)tag2;
								if(thumbPosition3.Position > 0 && textBox.Text.Length > thumbPosition3.Position - 1)
								{
									FormattedText formattedText3 = new FormattedText(textBox.Text.Substring(thumbPosition3.Position - 1, 1), Thread.CurrentThread.CurrentUICulture, textBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
									double num2;
									if(this.selectionThumb2 != null && sender == this.selectionThumb2)
									{
										num2 = this.widthChange2;
									}
									else
									{
										num2 = this.widthChange1;
									}
									if(num2 >= formattedText3.Width)
									{
										thumbPosition3.Position--;
										((Thumb)sender).Tag = thumbPosition3;
										ThumbPosition thumbPosition4 = default(ThumbPosition);
										if(this.selectionThumb2 != null && sender == this.selectionThumb2)
										{
											if(this.selectionThumb1.Tag != null)
											{
												thumbPosition4 = (ThumbPosition)this.selectionThumb1.Tag;
											}
										}
										else
										{
											if(this.selectionThumb2 != null && this.selectionThumb2.Tag != null)
											{
												thumbPosition4 = (ThumbPosition)this.selectionThumb2.Tag;
											}
										}
										if(thumbPosition3.Position > thumbPosition4.Position)
										{
											if(this.iTextBox.SelectionLength > 0)
											{
												this.iTextBox.SelectionLength = this.iTextBox.SelectionLength - 1;
											}
										}
										else
										{
											if(thumbPosition3.Position < thumbPosition4.Position)
											{
												if(this.iTextBox.SelectionStart > 0)
												{
													this.iTextBox.SelectionStart = this.iTextBox.SelectionStart - 1;
													this.iTextBox.SelectionLength = this.iTextBox.SelectionLength + 1;
												}
											}
											else
											{
												this.iTextBox.SelectionLength = 0;
											}
										}
										if(this.selectionThumb2 != null && sender == this.selectionThumb2)
										{
											this.widthChange2 = 0.0;
										}
										else
										{
											this.widthChange1 = 0.0;
										}
									}
								}
							}
						}
					}
				}
				ThumbPosition thumbPosition5 = default(ThumbPosition);
				if(this.selectionThumb2 != null && sender == this.selectionThumb2)
				{
					if(this.selectionThumb2.Tag != null)
					{
						thumbPosition5 = (ThumbPosition)this.selectionThumb2.Tag;
					}
				}
				else
				{
					if(this.selectionThumb1 != null && this.selectionThumb1.Tag != null)
					{
						thumbPosition5 = (ThumbPosition)this.selectionThumb1.Tag;
					}
				}
				formattedText = new FormattedText(textBox.Text.Substring(0, thumbPosition5.Position), Thread.CurrentThread.CurrentUICulture, textBox.FlowDirection, new Typeface(this.iTextBox.FontFamily, this.iTextBox.FontStyle, this.iTextBox.FontWeight, this.iTextBox.FontStretch), this.iTextBox.FontSize, this.iTextBox.Foreground);
				if(this.selectionThumb2 != null && sender == this.selectionThumb2)
				{
					this.selectionThumb2.Arrange(new Rect(formattedText.Width, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
				}
				else
				{
					if(this.selectionThumb1 != null)
					{
						this.selectionThumb1.Arrange(new Rect(formattedText.Width, this.iTextBox.RenderSize.Height - 15.0, 30.0, 30.0));
					}
				}
			}
			this.isCalledByDragDelta = false;
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