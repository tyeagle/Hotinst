/**
 * ==============================================================================
 *
 * ClassName: TreeRootLine
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/11/23 11:11:05
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// Represents the RootLine for TreeView control
	/// </summary>
	public class TreeRootLine : FrameworkElement
	{
		#region fields

		/// <summary>
		/// Represents an ordered collection
		/// </summary>
		private static DoubleCollection _dcollection = new DoubleCollection(new List<double> { 2.0, 2.0 });

		#endregion

		#region porps
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register(
			"LineBrush", typeof(Brush), typeof(TreeRootLine), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// 
		/// </summary>
		public Brush LineBrush
		{
			get { return (Brush)GetValue(LineBrushProperty); }
			set { SetValue(LineBrushProperty, value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty LinePenProperty = DependencyProperty.Register(
			"LinePen", typeof(Pen), typeof(TreeRootLine), new FrameworkPropertyMetadata(default(Pen), FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// 
		/// </summary>
		public Pen LinePen
		{
			get { return (Pen)GetValue(LinePenProperty); }
			set { SetValue(LinePenProperty, value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty LineStrokeThicknessProperty = DependencyProperty.Register(
			"LineStrokeThickness", typeof(double), typeof(TreeRootLine), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// 
		/// </summary>
		public double LineStrokeThickness
		{
			get { return (double)GetValue(LineStrokeThicknessProperty); }
			set { SetValue(LineStrokeThicknessProperty, value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty LineStrokeDashArrayProperty = DependencyProperty.Register(
			"LineStrokeDashArray", typeof(DoubleCollection), typeof(TreeRootLine), new FrameworkPropertyMetadata(_dcollection, FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// 
		/// </summary>
		public DoubleCollection LineStrokeDashArray
		{
			get { return (DoubleCollection)GetValue(LineStrokeDashArrayProperty); }
			set { SetValue(LineStrokeDashArrayProperty, value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty LineStrokeDashOffsetProperty = DependencyProperty.Register(
			"LineStrokeDashOffset", typeof(double), typeof(TreeRootLine), new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsRender));
		/// <summary>
		/// 
		/// </summary>
		public double LineStrokeDashOffset
		{
			get { return (double)GetValue(LineStrokeDashOffsetProperty); }
			set { SetValue(LineStrokeDashOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is vertical line.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is vertical line; otherwise, <c>false</c>.
		/// </value>
		public bool IsVerticalLine { get; set; }

		#endregion

		#region .ctor

		/// <summary>
		/// 
		/// </summary>
		public TreeRootLine()
		{

		}

		#endregion

		#region Overrides of UIElement

		/// <summary>在派生类中重写时，会参与由布局系统控制的呈现操作。调用此方法时，不直接使用此元素的呈现指令，而是将其保留供布局和绘制在以后异步使用。</summary>
		/// <param name="drawingContext">特定元素的绘制指令。此上下文是为布局系统提供的。</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			drawingContext.PushGuidelineSet(new GuidelineSet
			{
				GuidelinesX = { 0.5 },
				GuidelinesY = { 0.5 }
			});
			LinePen = LinePen ?? GetLinePen();
			drawingContext.DrawLine(LinePen, new Point(0.0, 0.0), IsVerticalLine
				? new Point(0.0, IsVista() ? RenderSize.Height : RenderSize.Height - 0.7)
				: new Point(RenderSize.Width, 0.0));
		}

		#endregion

		/// <summary>Gets the line pen.</summary>
		/// <returns>Pen of linePen</returns>
		private Pen GetLinePen()
		{
			return new Pen(LineBrush, 1.0)
			{
				DashStyle = DashStyles.Dot,
				DashCap = PenLineCap.Square,
				StartLineCap = PenLineCap.Square,
				EndLineCap = PenLineCap.Square,
				LineJoin = PenLineJoin.Miter,
				Thickness = LineStrokeThickness
			};
		}

		/// <summary>Defines whether this is Vista OS.</summary>
		/// <returns>True if this is Vista OS; otherwise, false.</returns>
		/// <property name="flag" value="Finished" />
		internal static bool IsVista()
		{
			return Environment.OSVersion.Version.Major >= 6;
		}
	}
}