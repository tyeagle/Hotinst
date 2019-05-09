using System.Windows;
using System.Windows.Controls;

namespace HOTINST.COMMON.Controls.Controls.Dragablz.Themes
{
	/// <summary>
	/// 
	/// </summary>
    public enum SystemCommandType
    {
		/// <summary>
		/// 
		/// </summary>
        CloseWindow,
		/// <summary>
		/// 
		/// </summary>
        MaximizeWindow,
		/// <summary>
		/// 
		/// </summary>
        MinimzeWindow,
		/// <summary>
		/// 
		/// </summary>
        RestoreWindow
    }

	/// <summary>
	/// 
	/// </summary>
    public class SystemCommandIcon : Control
    {
        static SystemCommandIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SystemCommandIcon), new FrameworkPropertyMetadata(typeof(SystemCommandIcon)));
        }

		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty SystemCommandTypeProperty = DependencyProperty.Register(
            "SystemCommandType", typeof (SystemCommandType), typeof (SystemCommandIcon), new PropertyMetadata(default(SystemCommandType)));
		/// <summary>
		/// 
		/// </summary>
        public SystemCommandType SystemCommandType
        {
            get { return (SystemCommandType) GetValue(SystemCommandTypeProperty); }
            set { SetValue(SystemCommandTypeProperty, value); }
        }
    }
}
