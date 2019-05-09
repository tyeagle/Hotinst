using HOTINST.COMMON.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// DialogChrome.xaml 的交互逻辑
	/// </summary>
	public partial class DialogChrome : Window
	{
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler DataUpdated;

		/// <summary>
		/// 
		/// </summary>
		public DialogChrome()
		{
			InitializeComponent();
			
			this.DisableAndHideMinMaxButtonAndIcon();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="viewModel"></param>
		/// <param name="onlyClose"></param>
		public DialogChrome(UIElement content, object viewModel, bool onlyClose)
			: this()
		{
			contentGrid.Children.Add(content);
			DataContext = viewModel;

			SetContentBinding(Window.TitleProperty, TitleProperty, content);
			SetContentBinding(Window.SizeToContentProperty, SizeToContentProperty, content);
			SetContentBinding(Window.WindowStyleProperty, WindowStyleProperty, content);
			SetContentBinding(FrameworkElement.WidthProperty, WidthProperty, content);
			SetContentBinding(FrameworkElement.HeightProperty, HeightProperty, content);
			SetContentBinding(Window.ResizeModeProperty, ResizeModeProperty, content);
			SetContentBinding(Control.BackgroundProperty, BackgroundProperty, content);

			WindowStartupLocation = WindowStartupLocation.CenterOwner;

			if(onlyClose)
			{
				btnCancel.Content = "关  闭";
				btnYes.Visibility = Visibility.Collapsed;
			}
			else
			{
				btnCancel.Content = "取  消";
				btnYes.Visibility = Visibility.Visible;
			}

			if(GetHideBottomBar(content))
				toolbarPanel.Visibility = Visibility.Collapsed;
		}

		private void SetContentBinding(DependencyProperty dp, DependencyProperty sourceDp, object content)
		{
			SetBinding(dp, new Binding { Path = new PropertyPath(sourceDp), Source = content });
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDataUpdated(EventArgs e)
		{
			EventHandler handler = DataUpdated;
			handler?.Invoke(this, e);
		}

		#region Title

		/// <summary>
		/// Title Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached(
			"Title", typeof(string), typeof(DialogChrome), new PropertyMetadata("", OnTitleChanged));

		/// <summary>
		/// Gets the Title property. This dependency property 
		/// indicates ....
		/// </summary>
		public static string GetTitle(DependencyObject d)
		{
			return (string)d.GetValue(TitleProperty);
		}

		/// <summary>
		/// Sets the Title property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetTitle(DependencyObject d, string value)
		{
			d.SetValue(TitleProperty, value);
		}

		/// <summary>
		/// Handles changes to the Title property.
		/// </summary>
		private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			string newTitle = (string)d.GetValue(TitleProperty);

			if(newTitle != null && newTitle.EndsWith("..."))
				SetTitle(d, newTitle.Substring(0, newTitle.Length - 3));
		}

		#endregion

		#region Width

		/// <summary>
		/// Width Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
			"Width", typeof(double), typeof(DialogChrome), new PropertyMetadata((double)300));

		/// <summary>
		/// Gets the Width property. This dependency property 
		/// indicates ....
		/// </summary>
		public static double GetWidth(DependencyObject d)
		{
			return (double)d.GetValue(WidthProperty);
		}

		/// <summary>
		/// Sets the Width property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetWidth(DependencyObject d, double value)
		{
			d.SetValue(WidthProperty, value);
		}

		#endregion

		#region Height

		/// <summary>
		/// Height Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
			"Height", typeof(double), typeof(DialogChrome), new PropertyMetadata((double)200));

		/// <summary>
		/// Gets the Height property. This dependency property 
		/// indicates ....
		/// </summary>
		public static double GetHeight(DependencyObject d)
		{
			return (double)d.GetValue(HeightProperty);
		}

		/// <summary>
		/// Sets the Height property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetHeight(DependencyObject d, double value)
		{
			d.SetValue(HeightProperty, value);
		}

		#endregion

		#region WindowStyle

		/// <summary>
		/// WindowStyle Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty WindowStyleProperty = DependencyProperty.RegisterAttached(
			"WindowStyle", typeof(WindowStyle), typeof(DialogChrome), new PropertyMetadata(WindowStyle.SingleBorderWindow));

		/// <summary>
		/// Gets the WindowStyle property. This dependency property 
		/// indicates ....
		/// </summary>
		public static WindowStyle GetWindowStyle(DependencyObject d)
		{
			return (WindowStyle)d.GetValue(WindowStyleProperty);
		}

		/// <summary>
		/// Sets the WindowStyle property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetWindowStyle(DependencyObject d, WindowStyle value)
		{
			d.SetValue(WindowStyleProperty, value);
		}

		#endregion

		#region SizeToContent

		/// <summary>
		/// SizeToContent Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty SizeToContentProperty = DependencyProperty.RegisterAttached(
			"SizeToContent", typeof(SizeToContent), typeof(DialogChrome), new PropertyMetadata(SizeToContent.Manual));

		/// <summary>
		/// Gets the SizeToContent property. This dependency property 
		/// indicates ....
		/// </summary>
		public static SizeToContent GetSizeToContent(DependencyObject d)
		{
			return (SizeToContent)d.GetValue(SizeToContentProperty);
		}

		/// <summary>
		/// Sets the SizeToContent property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetSizeToContent(DependencyObject d, SizeToContent value)
		{
			d.SetValue(SizeToContentProperty, value);
		}

		#endregion

		#region ResizeMode

		/// <summary>
		/// ResizeMode Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty ResizeModeProperty = DependencyProperty.RegisterAttached(
			"ResizeMode", typeof(ResizeMode), typeof(DialogChrome), new PropertyMetadata(ResizeMode.NoResize));

		/// <summary>
		/// Gets the ResizeMode property. This dependency property 
		/// indicates ....
		/// </summary>
		public static ResizeMode GetResizeMode(DependencyObject d)
		{
			return (ResizeMode)d.GetValue(ResizeModeProperty);
		}

		/// <summary>
		/// Sets the ResizeMode property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetResizeMode(DependencyObject d, ResizeMode value)
		{
			d.SetValue(ResizeModeProperty, value);
		}

		#endregion

		#region HideBottomBar

		/// <summary>
		/// HideBottomBar Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty HideBottomBarProperty = DependencyProperty.RegisterAttached(
			"HideBottomBar", typeof(bool), typeof(DialogChrome), new PropertyMetadata(false));

		/// <summary>
		/// Gets the HideBottomBar property. This dependency property 
		/// indicates ....
		/// </summary>
		public static bool GetHideBottomBar(DependencyObject d)
		{
			return (bool)d.GetValue(HideBottomBarProperty);
		}

		/// <summary>
		/// Sets the HideBottomBar property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetHideBottomBar(DependencyObject d, bool value)
		{
			d.SetValue(HideBottomBarProperty, value);
		}

		#endregion

		#region Background

		/// <summary>
		/// Background Attached Dependency Property
		/// </summary>
		public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
			"Background", typeof(Brush), typeof(DialogChrome), new PropertyMetadata(SystemColors.ControlBrush));

		/// <summary>
		/// Gets the Background property. This dependency property 
		/// indicates ....
		/// </summary>
		public static Brush GetBackground(DependencyObject d)
		{
			return (Brush)d.GetValue(BackgroundProperty);
		}

		/// <summary>
		/// Sets the Background property. This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetBackground(DependencyObject d, Brush value)
		{
			d.SetValue(BackgroundProperty, value);
		}

		#endregion

		private void BtnOk_Click(object sender, RoutedEventArgs e)
		{
			Close();
			OnDataUpdated(EventArgs.Empty);
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}