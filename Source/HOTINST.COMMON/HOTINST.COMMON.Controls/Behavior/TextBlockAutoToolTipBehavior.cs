/**
 * ==============================================================================
 *
 * ClassName: TextBlockAutoToolTipBehavior
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/1/19 18:13:12
 * Compiler: Visual Studio 2017
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
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Behavior
{
	/// <summary>
	/// TextBlock 自动显示 ToolTip 行为
	/// </summary>
	public class TextBlockAutoToolTipBehavior : Behavior<TextBlock>
	{
		#region fields

		private ToolTip _toolTip;

		#endregion

		#region Overrides of Behavior

		/// <summary>在行为附加到 AssociatedObject 后调用。</summary>
		/// <remarks>替代它以便将功能挂钩到 AssociatedObject。</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();
			_toolTip = new ToolTip
			{
				Placement = PlacementMode.Relative,
				VerticalOffset = 0,
				HorizontalOffset = 0
			};

			ToolTipService.SetShowDuration(_toolTip, int.MaxValue);

			_toolTip.SetBinding(ContentControl.ContentProperty, new Binding
			{
				Path = new PropertyPath("Text"),
				Source = AssociatedObject
			});

			AssociatedObject.TextTrimming = TextTrimming.CharacterEllipsis;
			AssociatedObject.AddValueChanged(TextBlock.TextProperty, TextBlockOnTextChanged);
			AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
		}

		#endregion

		#region Overrides of Behavior

		/// <summary>在行为与其 AssociatedObject 分离时（但在它实际发生之前）调用。</summary>
		/// <remarks>替代它以便将功能从 AssociatedObject 中解除挂钩。</remarks>
		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.RemoveValueChanged(TextBlock.TextProperty, TextBlockOnTextChanged);
			AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
		}

		#endregion

		private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			CheckToolTipVisibility();
		}

		private void TextBlockOnTextChanged(object sender, EventArgs eventArgs)
		{
			CheckToolTipVisibility();
		}

		private void CheckToolTipVisibility()
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if(AssociatedObject.ActualWidth == 0)
				Dispatcher.BeginInvoke(new Action(() => AssociatedObject.ToolTip = CalculateIsTextTrimmed(AssociatedObject) ? _toolTip : null), DispatcherPriority.Loaded);
			else
				AssociatedObject.ToolTip = CalculateIsTextTrimmed(AssociatedObject) ? _toolTip : null;
		}

		//Source: https://stackoverflow.com/questions/1041820/how-can-i-determine-if-my-textblock-text-is-being-trimmed
		private static bool CalculateIsTextTrimmed(TextBlock textBlock)
		{
			Typeface typeface = new Typeface(
				textBlock.FontFamily,
				textBlock.FontStyle,
				textBlock.FontWeight,
				textBlock.FontStretch);

			// FormattedText is used to measure the whole width of the text held up by TextBlock container
			FormattedText formattedText = new FormattedText(
					textBlock.Text,
					System.Threading.Thread.CurrentThread.CurrentCulture,
					textBlock.FlowDirection,
					typeface,
					textBlock.FontSize,
					textBlock.Foreground)
				{ MaxTextWidth = textBlock.ActualWidth };


			// When the maximum text width of the FormattedText instance is set to the actual
			// width of the textBlock, if the textBlock is being trimmed to fit then the formatted
			// text will report a larger height than the textBlock. Should work whether the
			// textBlock is single or multi-line.
			// The width check detects if any single line is too long to fit within the text area, 
			// this can only happen if there is a long span of text with no spaces.
			return formattedText.Height > textBlock.ActualHeight || formattedText.MinWidth > formattedText.MaxTextWidth;
		}
	}
}