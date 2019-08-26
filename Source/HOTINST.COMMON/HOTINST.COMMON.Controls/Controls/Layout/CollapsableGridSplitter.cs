/**
 * ==============================================================================
 *
 * ClassName: CollapsableGridSplitter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/5/26 10:45:03
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Layout
{
	public enum SplitterDirection
	{
		/// <summary>
		/// 不使用分割条
		/// </summary>
		None,
		/// <summary>
		/// 分割上下区域
		/// </summary>
		Horizontal,
		/// <summary>
		/// 分割左右区域
		/// </summary>
		Vertical
	}

	/// <summary>
	/// Specifies different collapse modes of a ExtendedGridSplitter.
	/// </summary>
	public enum GridSplitterCollapseMode
	{
		/// <summary>
		/// The ExtendedGridSplitter cannot be collapsed or expanded.
		/// </summary>
		None = 0,
		/// <summary>
		/// The column (or row) to the right (or below) the
		/// splitter's column, will be collapsed.
		/// </summary>
		Next = 1,
		/// <summary>
		/// The column (or row) to the left (or above) the
		/// splitter's column, will be collapsed.
		/// </summary>
		Previous = 2
	}

	/// <summary>
	/// An updated version of the standard ExtendedGridSplitter control that includes a centered handle
	/// which allows complete collapsing and expanding of the appropriate grid column or row.
	/// </summary>
	[TemplatePart(Name = ELEMENT_HORIZONTAL_HANDLE_NAME, Type = typeof(ToggleButton))]
	[TemplatePart(Name = ELEMENT_VERTICAL_HANDLE_NAME, Type = typeof(ToggleButton))]
	[TemplatePart(Name = ELEMENT_HORIZONTAL_TEMPLATE_NAME, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = ELEMENT_VERTICAL_TEMPLATE_NAME, Type = typeof(FrameworkElement))]
	public class CollapsableGridSplitter : GridSplitter
	{
		private const string ELEMENT_HORIZONTAL_HANDLE_NAME = "HorizontalGridSplitterHandle";
		private const string ELEMENT_VERTICAL_HANDLE_NAME = "VerticalGridSplitterHandle";
		private const string ELEMENT_HORIZONTAL_TEMPLATE_NAME = "HorizontalTemplate";
		private const string ELEMENT_VERTICAL_TEMPLATE_NAME = "VerticalTemplate";
		private const string ELEMENT_GRIDSPLITTER_BACKGROUND = "GridSplitterBackground";

		public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(SplitterDirection), typeof(CollapsableGridSplitter), new PropertyMetadata(SplitterDirection.None, OnDirectionPropertyChanged));
		public static readonly DependencyProperty CollapseModeProperty = DependencyProperty.Register("CollapseMode", typeof(GridSplitterCollapseMode), typeof(CollapsableGridSplitter), new PropertyMetadata(GridSplitterCollapseMode.None, OnCollapseModePropertyChanged));
		public static readonly DependencyProperty HorizontalHandleStyleProperty = DependencyProperty.Register("HorizontalHandleStyle", typeof(Style), typeof(CollapsableGridSplitter), null);
		public static readonly DependencyProperty VerticalHandleStyleProperty = DependencyProperty.Register("VerticalHandleStyle", typeof(Style), typeof(CollapsableGridSplitter), null);
		public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register("IsAnimated", typeof(bool), typeof(CollapsableGridSplitter), null);
		public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(CollapsableGridSplitter), new PropertyMetadata(OnIsCollapsedPropertyChanged));
		private static readonly DependencyProperty RowHeightAnimationProperty = DependencyProperty.Register("RowHeightAnimation", typeof(double), typeof(CollapsableGridSplitter), new PropertyMetadata(RowHeightAnimationChanged));
		private static readonly DependencyProperty ColWidthAnimationProperty = DependencyProperty.Register("ColWidthAnimation", typeof(double), typeof(CollapsableGridSplitter), new PropertyMetadata(ColWidthAnimationChanged));

		private FrameworkElement _elementHorizontal;
		private FrameworkElement _elementVertical;
		private ToggleButton _elementHorizontalGridSplitterButton;
		private ToggleButton _elementVerticalGridSplitterButton;
		private Rectangle _elementGridSplitterBackground;

		private GridCollapseDirection _gridCollapseDirection = GridCollapseDirection.Auto;
		private GridLength _savedGridLength;
		private double _savedActualValue;
		private const double _animationTimeMillis = 200;

		public event EventHandler PositionChanged;

		public CollapsableGridSplitter()
		{
			// Set default values
			DefaultStyleKey = typeof(CollapsableGridSplitter);

			CollapseMode = GridSplitterCollapseMode.None;
			IsAnimated = true;
			LayoutUpdated += delegate
			{
				_gridCollapseDirection = GetCollapseDirection();
				PositionChanged?.Invoke(this, EventArgs.Empty);
			};
		}

		public SplitterDirection Direction
		{
			get { return (SplitterDirection)GetValue(DirectionProperty); }
			set { SetValue(DirectionProperty, value); }
		}

		public GridSplitterCollapseMode CollapseMode
		{
			get { return (GridSplitterCollapseMode)GetValue(CollapseModeProperty); }
			set { SetValue(CollapseModeProperty, value); }
		}

		public Style HorizontalHandleStyle
		{
			get { return (Style)GetValue(HorizontalHandleStyleProperty); }
			set { SetValue(HorizontalHandleStyleProperty, value); }
		}

		public Style VerticalHandleStyle
		{
			get { return (Style)GetValue(VerticalHandleStyleProperty); }
			set { SetValue(VerticalHandleStyleProperty, value); }
		}

		public bool IsAnimated
		{
			get { return (bool)GetValue(IsAnimatedProperty); }
			set { SetValue(IsAnimatedProperty, value); }
		}

		public bool IsCollapsed
		{
			get { return (bool)GetValue(IsCollapsedProperty); }
			set { SetValue(IsCollapsedProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_elementHorizontal = GetTemplateChild(ELEMENT_HORIZONTAL_TEMPLATE_NAME) as FrameworkElement;
			_elementVertical = GetTemplateChild(ELEMENT_VERTICAL_TEMPLATE_NAME) as FrameworkElement;
			_elementHorizontalGridSplitterButton = GetTemplateChild(ELEMENT_HORIZONTAL_HANDLE_NAME) as ToggleButton;
			_elementVerticalGridSplitterButton = GetTemplateChild(ELEMENT_VERTICAL_HANDLE_NAME) as ToggleButton;
			_elementGridSplitterBackground = GetTemplateChild(ELEMENT_GRIDSPLITTER_BACKGROUND) as Rectangle;

			// Wire up the Checked and Unchecked events of the HorizontalGridSplitterHandle.
			if(_elementHorizontalGridSplitterButton != null)
			{
				_elementHorizontalGridSplitterButton.Checked += GridSplitterButton_Checked;
				_elementHorizontalGridSplitterButton.Unchecked += GridSplitterButton_Unchecked;
			}

			// Wire up the Checked and Unchecked events of the VerticalGridSplitterHandle.
			if(_elementVerticalGridSplitterButton != null)
			{
				_elementVerticalGridSplitterButton.Checked += GridSplitterButton_Checked;
				_elementVerticalGridSplitterButton.Unchecked += GridSplitterButton_Unchecked;
			}

			// Set default direction since we don't have all the components layed out yet.
			_gridCollapseDirection = GridCollapseDirection.Auto;

			// Directely call these events so design-time view updates appropriately
			OnDirectionChanged(Direction);
			OnCollapseModeChanged(CollapseMode);
			OnIsCollapsedChanged(IsCollapsed);
		}

		private static void OnIsCollapsedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollapsableGridSplitter s = d as CollapsableGridSplitter;

			bool value = (bool)e.NewValue;
			if(s != null)
				s.OnIsCollapsedChanged(value);
		}

		private static void OnDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollapsableGridSplitter s = d as CollapsableGridSplitter;

			SplitterDirection value = (SplitterDirection)e.NewValue;
			if(s != null)
				s.OnDirectionChanged(value);
		}

		private static void OnCollapseModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollapsableGridSplitter s = d as CollapsableGridSplitter;

			GridSplitterCollapseMode value = (GridSplitterCollapseMode)e.NewValue;
			if(s != null)
				s.OnCollapseModeChanged(value);
		}

		/// <summary>
		/// Handles the property change event of the IsCollapsed property.
		/// </summary>
		/// <param name="isCollapsed">The new value for the IsCollapsed property.</param>
		protected virtual void OnIsCollapsedChanged(bool isCollapsed)
		{
			// Determine if we are dealing with a vertical or horizontal splitter.
			if(_gridCollapseDirection == GridCollapseDirection.Rows)
			{
				if(_elementHorizontalGridSplitterButton != null)
				{
					// Sets the target ToggleButton's IsChecked property equal
					// to the provided isCollapsed property.
					_elementHorizontalGridSplitterButton.IsChecked = isCollapsed;
				}
			}
			else
			{
				if(_elementVerticalGridSplitterButton != null)
				{
					// Sets the target ToggleButton's IsChecked property equal
					// to the provided isCollapsed property.
					_elementVerticalGridSplitterButton.IsChecked = isCollapsed;
				}
			}
		}

		protected virtual void OnDirectionChanged(SplitterDirection direction)
		{
			switch(direction)
			{
				case SplitterDirection.None:
					if(_elementHorizontal != null)
					{
						_elementHorizontal.Visibility = Visibility.Collapsed;
					}
					if(_elementVertical != null)
					{
						_elementVertical.Visibility = Visibility.Collapsed;
					}
					break;
				case SplitterDirection.Horizontal:
					if(_elementHorizontal != null)
					{
						_elementHorizontal.Visibility = Visibility.Visible;
					}
					if(_elementVertical != null)
					{
						_elementVertical.Visibility = Visibility.Collapsed;
					}
					break;
				case SplitterDirection.Vertical:
					if(_elementHorizontal != null)
					{
						_elementHorizontal.Visibility = Visibility.Collapsed;
					}
					if(_elementVertical != null)
					{
						_elementVertical.Visibility = Visibility.Visible;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		/// <summary>
		/// Handles the property change event of the CollapseMode property.
		/// </summary>
		/// <param name="collapseMode">The new value for the CollapseMode property.</param>
		protected virtual void OnCollapseModeChanged(GridSplitterCollapseMode collapseMode)
		{
			// Hide the handles if the CollapseMode is set to None.
			if(collapseMode == GridSplitterCollapseMode.None)
			{
				if(_elementHorizontalGridSplitterButton != null)
				{
					_elementHorizontalGridSplitterButton.Visibility = Visibility.Collapsed;
				}
				if(_elementVerticalGridSplitterButton != null)
				{
					_elementVerticalGridSplitterButton.Visibility = Visibility.Collapsed;
				}
			}
			else
			{
				// Ensure the handles are Visible.
				if (_elementHorizontalGridSplitterButton != null)
				{
					_elementHorizontalGridSplitterButton.Visibility = Visibility.Visible;
				}
				if(_elementVerticalGridSplitterButton != null)
				{
					_elementVerticalGridSplitterButton.Visibility = Visibility.Visible;
				}

				// Rotate the direction that the handle is facing depending on the CollapseMode.
				switch(collapseMode)
				{
					case GridSplitterCollapseMode.Previous:
						if (_elementHorizontalGridSplitterButton != null)
						{
							_elementHorizontalGridSplitterButton.RenderTransform = new ScaleTransform() { ScaleY = -1.0 };
						}
						if(_elementVerticalGridSplitterButton != null)
						{
							_elementVerticalGridSplitterButton.RenderTransform = new ScaleTransform() { ScaleX = -1.0 };
						}
						break;
					case GridSplitterCollapseMode.Next:
						if(_elementHorizontalGridSplitterButton != null)
						{
							_elementHorizontalGridSplitterButton.RenderTransform = new ScaleTransform() { ScaleY = 1.0 };
						}
						if(_elementVerticalGridSplitterButton != null)
						{
							_elementVerticalGridSplitterButton.RenderTransform = new ScaleTransform() { ScaleX = 1.0 };
						}
						break;
					case GridSplitterCollapseMode.None:
						break;
					default:
						throw new ArgumentOutOfRangeException("collapseMode", collapseMode, null);
				}
			}
		}

		/// <summary>
		/// Collapses the target ColumnDefinition or RowDefinition.
		/// </summary>
		private void Collapse()
		{
			Grid parentGrid = Parent as Grid;
			int splitterIndex = int.MinValue;

			if(_gridCollapseDirection == GridCollapseDirection.Rows)
			{
				// Get the index of the row containing the splitter
				splitterIndex = (int)GetValue(Grid.RowProperty);

				// Determing the curent CollapseMode
				if(CollapseMode == GridSplitterCollapseMode.Next)
				{
					// Save the next rows Height information
					if(parentGrid != null)
					{
						_savedGridLength = parentGrid.RowDefinitions[splitterIndex + 1].Height;
						_savedActualValue = parentGrid.RowDefinitions[splitterIndex + 1].ActualHeight;

						// Collapse the next row
						if(IsAnimated)
							AnimateCollapse(parentGrid.RowDefinitions[splitterIndex + 1]);
						else
							parentGrid.RowDefinitions[splitterIndex + 1].SetValue(RowDefinition.HeightProperty, new GridLength(0));
					}
				}
				else
				{
					// Save the previous row's Height information
					if(parentGrid != null)
					{
						_savedGridLength = parentGrid.RowDefinitions[splitterIndex - 1].Height;
						_savedActualValue = parentGrid.RowDefinitions[splitterIndex - 1].ActualHeight;

						// Collapse the previous row
						if(IsAnimated)
							AnimateCollapse(parentGrid.RowDefinitions[splitterIndex - 1]);
						else
							parentGrid.RowDefinitions[splitterIndex - 1].SetValue(RowDefinition.HeightProperty, new GridLength(0));
					}
				}
			}
			else
			{
				// Get the index of the column containing the splitter
				splitterIndex = (int)GetValue(Grid.ColumnProperty);

				// Determing the curent CollapseMode
				if(CollapseMode == GridSplitterCollapseMode.Next)
				{
					// Save the next column's Width information
					if(parentGrid != null)
					{
						_savedGridLength = parentGrid.ColumnDefinitions[splitterIndex + 1].Width;
						_savedActualValue = parentGrid.ColumnDefinitions[splitterIndex + 1].ActualWidth;

						// Collapse the next column
						if(IsAnimated)
							AnimateCollapse(parentGrid.ColumnDefinitions[splitterIndex + 1]);
						else
							parentGrid.ColumnDefinitions[splitterIndex + 1].SetValue(ColumnDefinition.WidthProperty, new GridLength(0));
					}
				}
				else
				{
					// Save the previous column's Width information
					if(parentGrid != null)
					{
						_savedGridLength = parentGrid.ColumnDefinitions[splitterIndex - 1].Width;
						_savedActualValue = parentGrid.ColumnDefinitions[splitterIndex - 1].ActualWidth;

						// Collapse the previous column
						if(IsAnimated)
							AnimateCollapse(parentGrid.ColumnDefinitions[splitterIndex - 1]);
						else
							parentGrid.ColumnDefinitions[splitterIndex - 1].SetValue(ColumnDefinition.WidthProperty, new GridLength(0));
					}
				}
			}
		}

		/// <summary>
		/// Expands the target ColumnDefinition or RowDefinition.
		/// </summary>
		private void Expand()
		{
			Grid parentGrid = Parent as Grid;
			int splitterIndex = int.MinValue;

			if(_gridCollapseDirection == GridCollapseDirection.Rows)
			{
				// Get the index of the row containing the splitter
				splitterIndex = (int)GetValue(Grid.RowProperty);

				// Determine the curent CollapseMode
				if(CollapseMode == GridSplitterCollapseMode.Next)
				{
					// Expand the next row
					if(parentGrid != null)
					{
						if(IsAnimated)
							AnimateExpand(parentGrid.RowDefinitions[splitterIndex + 1]);
						else
							parentGrid.RowDefinitions[splitterIndex + 1].SetValue(RowDefinition.HeightProperty, _savedGridLength);
					}
				}
				else
				{
					// Expand the previous row
					if(parentGrid != null)
					{
						if(IsAnimated)
							AnimateExpand(parentGrid.RowDefinitions[splitterIndex - 1]);
						else
							parentGrid.RowDefinitions[splitterIndex - 1].SetValue(RowDefinition.HeightProperty, _savedGridLength);
					}
				}
			}
			else
			{
				// Get the index of the column containing the splitter
				splitterIndex = (int)GetValue(Grid.ColumnProperty);

				// Determine the curent CollapseMode
				if(CollapseMode == GridSplitterCollapseMode.Next)
				{
					// Expand the next column
					if(parentGrid != null)
						if(IsAnimated)
						AnimateExpand(parentGrid.ColumnDefinitions[splitterIndex + 1]);
					else
						parentGrid.ColumnDefinitions[splitterIndex + 1].SetValue(ColumnDefinition.WidthProperty, _savedGridLength);
				}
				else
				{
					// Expand the previous column
					if(parentGrid != null)
						if(IsAnimated)
						AnimateExpand(parentGrid.ColumnDefinitions[splitterIndex - 1]);
					else
						parentGrid.ColumnDefinitions[splitterIndex - 1].SetValue(ColumnDefinition.WidthProperty, _savedGridLength);
				}
			}
		}

		/// <summary>
		/// Determine the collapse direction based on the horizontal and vertical alignments
		/// </summary>
		private GridCollapseDirection GetCollapseDirection()
		{
			if(HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				return GridCollapseDirection.Columns;
			}
			if((VerticalAlignment == VerticalAlignment.Stretch) && (ActualWidth <= ActualHeight))
			{
				return GridCollapseDirection.Columns;
			}
			return GridCollapseDirection.Rows;
		}

		// Define Collapsed and Expanded evenets
		public event EventHandler<EventArgs> Collapsed;
		public event EventHandler<EventArgs> Expanded;

		/// <summary>
		/// Raises the Collapsed event.
		/// </summary>
		/// <param name="e">Contains event arguments.</param>
		protected virtual void OnCollapsed(EventArgs e)
		{
			EventHandler<EventArgs> handler = Collapsed;
			if(handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the Expanded event.
		/// </summary>
		/// <param name="e">Contains event arguments.</param>
		protected virtual void OnExpanded(EventArgs e)
		{
			EventHandler<EventArgs> handler = Expanded;
			if(handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Handles the Checked event of either the Vertical or Horizontal
		/// GridSplitterHandle ToggleButton.
		/// </summary>
		/// <param name="sender">An instance of the ToggleButton that fired the event.</param>
		/// <param name="e">Contains event arguments for the routed event that fired.</param>
		private void GridSplitterButton_Checked(object sender, RoutedEventArgs e)
		{
			if(IsCollapsed != true)
			{
				// In our case, Checked = Collapsed.  Which means we want everything
				// ready to be expanded.
				Collapse();

				IsCollapsed = true;

				// Deactivate the background so the splitter can not be dragged.
				_elementGridSplitterBackground.IsHitTestVisible = false;
				//_elementGridSplitterBackground.Opacity = 0.5;

				// Raise the Collapsed event.
				OnCollapsed(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Handles the Unchecked event of either the Vertical or Horizontal
		/// GridSplitterHandle ToggleButton.
		/// </summary>
		/// <param name="sender">An instance of the ToggleButton that fired the event.</param>
		/// <param name="e">Contains event arguments for the routed event that fired.</param>
		private void GridSplitterButton_Unchecked(object sender, RoutedEventArgs e)
		{
			if(IsCollapsed)
			{
				// In our case, Unchecked = Expanded.  Which means we want everything
				// ready to be collapsed.
				Expand();

				IsCollapsed = false;

				// Activate the background so the splitter can be dragged again.
				_elementGridSplitterBackground.IsHitTestVisible = true;
				//_elementGridSplitterBackground.Opacity = 1;

				// Raise the Expanded event.
				OnExpanded(EventArgs.Empty);
			}
		}

		private RowDefinition _animatingRow;

		private double RowHeightAnimation
		{
			get { return (double)GetValue(RowHeightAnimationProperty); }
			set { SetValue(RowHeightAnimationProperty, value); }
		}

		private static void RowHeightAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollapsableGridSplitter gridSplitter = d as CollapsableGridSplitter;
			if(gridSplitter != null)
				gridSplitter._animatingRow.Height = new GridLength((double)e.NewValue);
		}

		private ColumnDefinition _animatingColumn;

		private double ColWidthAnimation
		{
			get { return (double)GetValue(ColWidthAnimationProperty); }
			set { SetValue(ColWidthAnimationProperty, value); }
		}

		private static void ColWidthAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollapsableGridSplitter gridSplitter = d as CollapsableGridSplitter;
			if(gridSplitter != null)
				gridSplitter._animatingColumn.Width = new GridLength((double)e.NewValue);
		}

		/// <summary>
		/// Uses DoubleAnimation and a StoryBoard to animated the collapsing
		/// of the specificed ColumnDefinition or RowDefinition.
		/// </summary>
		/// <param name="definition">The RowDefinition or ColumnDefintition that will be collapsed.</param>
		private void AnimateCollapse(object definition)
		{
			double currentValue;

			// Setup the animation and StoryBoard
			DoubleAnimation gridLengthAnimation = new DoubleAnimation() { Duration = new Duration(TimeSpan.FromMilliseconds(_animationTimeMillis)) };
			Storyboard sb = new Storyboard();

			// Add the animation to the StoryBoard
			sb.Children.Add(gridLengthAnimation);

			if(_gridCollapseDirection == GridCollapseDirection.Rows)
			{
				// Specify the target RowDefinition and property (Height) that will be altered by the animation.
				_animatingRow = (RowDefinition)definition;
				Storyboard.SetTarget(gridLengthAnimation, this);
				Storyboard.SetTargetProperty(gridLengthAnimation, new PropertyPath("RowHeightAnimation"));

				currentValue = _animatingRow.ActualHeight;
			}
			else
			{
				// Specify the target ColumnDefinition and property (Width) that will be altered by the animation.
				_animatingColumn = (ColumnDefinition)definition;
				Storyboard.SetTarget(gridLengthAnimation, this);
				Storyboard.SetTargetProperty(gridLengthAnimation, new PropertyPath("ColWidthAnimation"));

				currentValue = _animatingColumn.ActualWidth;
			}

			gridLengthAnimation.From = currentValue;
			gridLengthAnimation.To = 0;

			// Start the StoryBoard.
			sb.Begin();
		}

		/// <summary>
		/// Uses DoubleAnimation and a StoryBoard to animate the expansion
		/// of the specificed ColumnDefinition or RowDefinition.
		/// </summary>
		/// <param name="definition">The RowDefinition or ColumnDefintition that will be expanded.</param>
		private void AnimateExpand(object definition)
		{
			double currentValue;

			// Setup the animation and StoryBoard
			DoubleAnimation gridLengthAnimation = new DoubleAnimation() { Duration = new Duration(TimeSpan.FromMilliseconds(_animationTimeMillis)) };
			Storyboard sb = new Storyboard();

			// Add the animation to the StoryBoard
			sb.Children.Add(gridLengthAnimation);

			if(_gridCollapseDirection == GridCollapseDirection.Rows)
			{
				// Specify the target RowDefinition and property (Height) that will be altered by the animation.
				_animatingRow = (RowDefinition)definition;
				Storyboard.SetTarget(gridLengthAnimation, this);
				Storyboard.SetTargetProperty(gridLengthAnimation, new PropertyPath("RowHeightAnimation"));

				currentValue = _animatingRow.ActualHeight;
			}
			else
			{
				// Specify the target ColumnDefinition and property (Width) that will be altered by the animation.
				_animatingColumn = (ColumnDefinition)definition;
				Storyboard.SetTarget(gridLengthAnimation, this);
				Storyboard.SetTargetProperty(gridLengthAnimation, new PropertyPath("ColWidthAnimation"));

				currentValue = _animatingColumn.ActualWidth;
			}
			gridLengthAnimation.From = currentValue;
			gridLengthAnimation.To = _savedActualValue;

			// Start the StoryBoard.
			sb.Begin();
		}

		/// <summary>
		/// An enumeration that specifies the direction the ExtendedGridSplitter will
		/// be collapased (Rows or Columns).
		/// </summary>
		internal enum GridCollapseDirection
		{
			Auto,
			Columns,
			Rows
		}
	}
}