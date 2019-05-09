using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// Interaction logic for DialogBaseControl.xaml
	/// </summary>
	internal partial class DialogBaseControl : INotifyPropertyChanged
	{
		public DialogBaseControl(FrameworkElement originalContent, DialogBase dialog)
		{
			Caption = dialog.Caption;
			Icon = dialog.Icon;

			InitializeComponent();

			var backgroundImage = originalContent.CaptureImage();
			backgroundImage.Stretch = System.Windows.Media.Stretch.Fill;
			BackgroundImageHolder.Content = backgroundImage;

			_dialog = dialog;
			CreateButtons();
			CreateIcons();

			_sbBlink = FindResource("sbBlink") as Storyboard;
		}

		private readonly Storyboard _sbBlink;
		private readonly DialogBase _dialog;

		public string Caption { get; private set; }
		public DialogIcon Icon { get; private set; }

		public Visibility CaptionVisibility => string.IsNullOrWhiteSpace(Caption) ? Visibility.Collapsed : Visibility.Visible;

		public Visibility ImgVisibility => Icon == DialogIcon.None ? Visibility.Collapsed : Visibility.Visible;

		private VerticalAlignment _verticalDialogAlignment = VerticalAlignment.Center;
		public VerticalAlignment VerticalDialogAlignment
		{
			get { return _verticalDialogAlignment; }
			set
			{
				_verticalDialogAlignment = value;
				OnPropertyChanged("VerticalDialogAlignment");
			}
		}

		private HorizontalAlignment _horizontalDialogAlignment = HorizontalAlignment.Center;
		public HorizontalAlignment HorizontalDialogAlignment
		{
			get { return _horizontalDialogAlignment; }
			set
			{
				_horizontalDialogAlignment = value;
				OnPropertyChanged("HorizontalDialogAlignment");
			}
		}

		public void SetCustomContent(object content)
		{
			CustomContent.Content = content;
		}

		private void CreateButtons()
		{
			switch (_dialog.Mode)
			{
				case DialogButtons.None:
					break;
				case DialogButtons.Ok:
					AddOkButton();
					break;
				case DialogButtons.Cancel:
					AddCancelButton();
					break;
				case DialogButtons.OkCancel:
					AddOkButton();
					AddCancelButton();
					break;
				case DialogButtons.YesNo:
					AddYesButton();
					AddNoButton();
					break;
				case DialogButtons.YesNoCancel:
					AddYesButton();
					AddNoButton();
					AddCancelButton();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void CreateIcons()
		{
			switch(_dialog.Icon)
			{
				case DialogIcon.None:
					break;
				case DialogIcon.Info:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/info_128px.png");
					break;
				case DialogIcon.Hint:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/hint_128px.png");
					break;
				case DialogIcon.Question:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/question_128px.png");
					break;
				case DialogIcon.Warning:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/warning_128px.png");
					break;
				case DialogIcon.Stop:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/stop_128px.png");
					break;
				case DialogIcon.Error:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/error_128px.png");
					break;
				case DialogIcon.Deny:
					Img.FromBitmapResource(GetType(), "Service/DialogManage/Icons/deny_128px.png");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void AddNoButton()
		{
			AddButton(_dialog.NoText, GetCallback(_dialog.No, DialogResultState.No), false, true, "CanNo");
		}

		public void AddYesButton()
		{
			AddButton(_dialog.YesText, GetCallback(_dialog.Yes, DialogResultState.Yes), true, false, "CanYes");
		}

		public void AddCancelButton()
		{
			AddButton(_dialog.CancelText, GetCallback(_dialog.Cancel, DialogResultState.Cancel), false, true, "CanCancel");
		}

		public void AddOkButton()
		{
			AddButton(_dialog.OkText, GetCallback(_dialog.Ok, DialogResultState.Ok), true, true, "CanOk");
		}

		private void AddButton(
			string buttonText,
			Action callback,
			bool isDefault,
			bool isCancel,
			string bindingPath)
		{
			var btn = new Button
			{
				Content = buttonText,
				//MinWidth = 85,
				MinHeight = 28,
				IsDefault = isDefault,
				IsCancel = isCancel,
				Margin = new Thickness(5)
			};

			var enabledBinding = new Binding(bindingPath) { Source = _dialog };
			btn.SetBinding(IsEnabledProperty, enabledBinding);

			btn.Click += (s, e) => callback();

			ButtonsGrid.Columns++;
			ButtonsGrid.Children.Add(btn);
		}

		internal void RemoveButtons()
		{
			ButtonsGrid.Children.Clear();
		}

		private Action GetCallback(
			Action dialogCallback,
			DialogResultState result)
		{
			_dialog.Result = result;
			Action callback = () =>
			{
				if (dialogCallback != null)
					dialogCallback();
				if (_dialog.CloseBehavior == DialogCloseBehavior.AutoCloseOnButtonClick)
					_dialog.Close();
			};

			return callback;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UIElement_OnMouseDownOut(object sender, MouseButtonEventArgs e)
		{
			_sbBlink?.Begin();
		}

		private void UIElement_OnMouseDownIn(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
	}
}
