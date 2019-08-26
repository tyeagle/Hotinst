/**
 * ==============================================================================
 *
 * ClassName: TreeNodeItem
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/10 16:23:59
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Core;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	#region 接口

	/// <summary>
	/// 定义节点的接口(为了在附加属性里面操作目标控件的ItemsSource)
	/// </summary>
	public interface INode : ICloneable<INode>
	{
		/// <summary>
		/// 
		/// </summary>
		bool IsExpanderEnable { get; set; }
		/// <summary>
		/// 图标
		/// </summary>
		ImageSource Icon { get; set; }
		/// <summary>
		/// 显示名称
		/// </summary>
		string Caption { get; set; }
		/// <summary>
		/// 显示名称格式化字符串
		/// </summary>
		string CaptionFormat { get; set; }
		/// <summary>
		/// 显示名称扩展后缀
		/// </summary>
		string CaptionEx { get; set; }
		/// <summary>
		/// 扩展名称颜色
		/// </summary>
		Brush CaptionExBrush { get; set; }
		/// <summary>
		/// 是否可编辑显示名称
		/// </summary>
		bool AllowEdit { get; set; }
		/// <summary>
		/// 是否允许拖放
		/// </summary>
		bool AllowDrag { get; set; }
		/// <summary>
		/// 是否处于编辑模式
		/// </summary>
		bool IsInEditMode { get; set; }
		/// <summary>
		/// 是否选中
		/// </summary>
		bool IsSelected { get; set; }
		/// <summary>
		/// 是否展开
		/// </summary>
		bool IsExpanded { get; set; }
		/// <summary>
		/// 是否处于勾选状态
		/// </summary>
		bool? IsChecked { get; set; }
		/// <summary>
		/// 是否显示勾选框
		/// </summary>
		bool ShowCheckBox { get; set; }
		/// <summary>
		/// 勾选框是否可用
		/// </summary>
		bool IsCheckBoxEnable { get; set; }
		/// <summary>
		/// 是否显示旋转齿轮
		/// </summary>
		bool ShowSpinner { get; set; }
		/// <summary>
		/// 父节点
		/// </summary>
		INode Parent { get; set; }
		/// <summary>
		/// 子节点集合
		/// </summary>
		ObservableCollection<INode> Children { get; }
		/// <summary>
		/// 
		/// </summary>
		TreeViewItem TreeViewItem { get; }

		/// <summary>
		/// 添加子节点
		/// </summary>
		/// <param name="child"></param>
		void AppendChild(INode child);

		/// <summary>
		/// 添加子节点(目前只适用于子级节点同为平级情况, 否则嵌套的子节点可能会不进行父节点的设置)
		/// </summary>
		/// <param name="children"></param>
		void AppendChildren(IEnumerable<INode> children);

		/// <summary>
		/// 从父节点中移除自身
		/// </summary>
		void RemoveSelf();
	}

	/// <summary>
	/// 查询是否可以作为拖放操作放的结果
	/// </summary>
	public interface IConfirmDropable
	{
		/// <summary>
		/// 确定是否可以进行放操作
		/// 调用者本身为 DragOverItem
		/// </summary>
		/// <returns>通过验证返回true, 否则返回false.</returns>
		bool CanDrop(INode draggingItem);
	}

	/// <summary>
	/// 自定义排序
	/// </summary>
	public interface ICustomSort
	{
		/// <summary>
		/// 排序
		/// </summary>
		void Sort();
	}

	#endregion

	/// <summary>
	/// TreeNodeItemBase
	/// </summary>
	public class TreeNodeItemBase : BindableBase, IConfirmDropable, INode, ICustomSort
	{
		#region fields

		private static bool _updateFlag;

		private bool _isExpanderEnable;
		private ImageSource _icon;
		private string _caption;
		private string _captionFormat;
		private string _captionEx;
		private Brush _captionExBrush;
		private bool _allowEdit;
		private bool _allowDrag;
		private bool _isInEditMode;
		private bool _isSelected;
		private bool? _isChecked = false;
		private bool _showCheckBox;
		private bool _isCheckBoxEnable = true;
		private bool _showSpinner;
		private bool _isExpanded;

		#endregion

		#region props

		/// <summary>
		/// 
		/// </summary>
		public bool IsExpanderEnable
		{
			get => _isExpanderEnable;
			set
			{
				_isExpanderEnable = value;
				OnPropertyChanged(nameof(IsExpanderEnable));
			}
		}
		/// <summary>
		/// 图标
		/// </summary>
		public ImageSource Icon
		{
			get => _icon;
			set
			{
				_icon = value;
				OnPropertyChanged(nameof(Icon));
			}
		}
		/// <summary>
		/// 显示名称
		/// </summary>
		public string Caption
		{
			get => _caption;
			set
			{
				_caption = value;
				OnPropertyChanged(nameof(Caption));
			}
		}
		/// <summary>
		/// 显示名称格式化字符串
		/// </summary>
		public string CaptionFormat
		{
			get => _captionFormat;
			set
			{
				_captionFormat = value;
				OnPropertyChanged(nameof(CaptionFormat));
			}
		}
		/// <summary>
		/// 显示名称扩展后缀
		/// </summary>
		public string CaptionEx
		{
			get => _captionEx;
			set
			{
				_captionEx = value;
				OnPropertyChanged(nameof(CaptionEx));
			}
		}
		/// <summary>
		/// 显示名称扩展后缀颜色
		/// </summary>
		public Brush CaptionExBrush
		{
			get => _captionExBrush;
			set
			{
				_captionExBrush = value;
				OnPropertyChanged(nameof(CaptionExBrush));
			}
		}
		/// <summary>
		/// 是否可编辑显示名称
		/// </summary>
		public bool AllowEdit
		{
			get => _allowEdit;
			set
			{
				_allowEdit = value;
				OnPropertyChanged(nameof(AllowEdit));
			}
		}
		/// <summary>
		/// 是否允许拖放
		/// </summary>
		public bool AllowDrag
		{
			get => _allowDrag;
			set
			{
				_allowDrag = value;
				OnPropertyChanged(nameof(AllowDrag));
			}
		}
		/// <summary>
		/// 是否处于编辑模式
		/// </summary>
		public bool IsInEditMode
		{
			get => _isInEditMode;
			set
			{
				_isInEditMode = value;
				OnPropertyChanged(nameof(IsInEditMode));
			}
		}
		/// <summary>
		/// 是否处于编辑模式
		/// </summary>
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				OnPropertyChanged(nameof(IsSelected));
			}
		}
		/// <summary>
		/// 是否处于编辑模式
		/// </summary>
		public bool? IsChecked
		{
			get => _isChecked;
			set
			{
				if(_isChecked != value)
				{
					_isChecked = value;
					OnPropertyChanged(nameof(IsChecked));

					CheckedChanged?.Invoke(this, EventArgs.Empty);

					UpdateCheckState();
				}
			}
		}
		/// <summary>
		/// 是否处于编辑模式
		/// </summary>
		public bool ShowCheckBox
		{
			get => _showCheckBox;
			set
			{
				_showCheckBox = value;
				OnPropertyChanged(nameof(ShowCheckBox));
			}
		}
		/// <summary>
		/// 勾选框是否可用
		/// </summary>
		public bool IsCheckBoxEnable
		{
			get => _isCheckBoxEnable;
			set
			{
				_isCheckBoxEnable = value;
				OnPropertyChanged(nameof(IsCheckBoxEnable));
			}
		}
		/// <summary>
		/// 是否处于编辑模式
		/// </summary>
		public bool ShowSpinner
		{
			get => _showSpinner;
			set
			{
				_showSpinner = value;
				OnPropertyChanged(nameof(ShowSpinner));
			}
		}
		/// <summary>
		/// 是否展开
		/// </summary>
		public bool IsExpanded
		{
			get => _isExpanded;
			set
			{
				_isExpanded = value;
				OnPropertyChanged(nameof(IsExpanded));

				ExpandChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		/// <summary>
		/// 父节点
		/// </summary>
		public INode Parent { get; set; }
		/// <summary>
		/// 子节点集合
		/// </summary>
		public ObservableCollection<INode> Children { get; } = new ObservableCollection<INode>();
		/// <summary>
		/// 右键菜单
		/// </summary>
		public ObservableCollection<MenuItemViewModel> MenuItems { get; } = new ObservableCollection<MenuItemViewModel>();

		/// <summary>
		/// 
		/// </summary>
		public TreeViewItem TreeViewItem { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<INode> RootCollection { get; set; }

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public ICommand CmdLoaded => new DelegateCommand(OnLoaded);

		#region event

		/// <summary>
		/// 展开状态变化事件
		/// </summary>
		public event EventHandler ExpandChanged;

		/// <summary>
		/// 勾选状态变化事件
		/// </summary>
		public event EventHandler CheckedChanged;

		#endregion

		#region .ctor

		/// <summary>
		/// .ctor
		/// </summary>
		public TreeNodeItemBase()
		{
			
		}

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="rootCollection"></param>
		public TreeNodeItemBase(string caption, ObservableCollection<INode> rootCollection = null)
		{
			AllowDrag = true;
			AllowEdit = true;
			IsExpanderEnable = true;
			Caption = caption;

			RootCollection = rootCollection;
		}

		#endregion

		private void SetChildNodeState(bool check, IList<INode> chidren)
		{
			foreach(INode node in chidren)
			{
				node.IsChecked = check;
				SetChildNodeState(check, node.Children);
			}
		}

		private void SetParentNodeState(INode parent)
		{
			if(parent == null)
			{
				return;
			}

			if(parent.Children.Count > 0 && parent.Children.All(c => c.IsChecked.HasValue && c.IsChecked.Value))
			{
				parent.IsChecked = true;
			}
			else if(parent.Children.All(c => c.IsChecked.HasValue && !c.IsChecked.Value))
			{
				parent.IsChecked = false;
			}
			else
			{
				parent.IsChecked = null;
			}

			SetParentNodeState(parent.Parent);
		}

		private void UpdateCheckState(INode node = null)
		{
			if(_updateFlag)
			{
				return;
			}

			_updateFlag = true;

			if(node != null)
			{
				// 处理子节点勾选状态
				if(node.IsChecked.HasValue)
				{
					SetChildNodeState(node.IsChecked.Value, node.Children);
				}

				// 处理父节点勾选状态
				SetParentNodeState(node.Parent);
			}
			else
			{
				// 处理子节点勾选状态
				if(IsChecked.HasValue)
				{
					SetChildNodeState(IsChecked.Value, Children);
				}

				// 处理父节点勾选状态
				SetParentNodeState(Parent);
			}
			
			_updateFlag = false;
		}

		#region 公共方法

		/// <summary>
		/// 添加子节点
		/// </summary>
		/// <param name="child"></param>
		public void AppendChild(INode child)
		{
			child.Parent = this;
			Children.Add(child);

			(child as ICustomSort)?.Sort();

			UpdateCheckState(child);
		}

		/// <summary>
		/// 添加子节点(目前只适用于子级节点同为平级情况, 否则嵌套的子节点可能会不进行父节点的设置)
		/// </summary>
		/// <param name="children"></param>
		public void AppendChildren(IEnumerable<INode> children)
		{
			foreach(INode item in children)
			{
				AppendChild(item);
			}
		}

		/// <summary>
		/// 从父节点中移除自身
		/// </summary>
		public void RemoveSelf()
		{
			if(Parent == null)
			{
				RootCollection?.Remove(this);
			}
			else
			{
				Parent.Children.Remove(this);
			}

			UpdateCheckState();
		}

		#endregion

		#region 虚方法

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter"></param>
		public virtual void OnLoaded(object parameter)
		{
			TreeViewItem = parameter as TreeViewItem;
		}

		#endregion

		#region Implementation of IConfirmDropable

		/// <summary>
		/// 确定是否可以进行放操作
		/// 调用者本身为 DragOverItem
		/// </summary>
		/// <returns>通过验证返回true, 否则返回false.</returns>
		public virtual bool CanDrop(INode draggingItem)
		{
			return true;
		}

		#endregion

		#region Implementation of ICustomSort

		/// <summary>
		/// 排序
		/// </summary>
		public virtual void Sort()
		{

		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>创建作为当前实例副本的新对象。</summary>
		/// <returns>作为此实例副本的新对象。</returns>
		public virtual INode Clone()
		{
			return new TreeNodeItemBase(Caption);
		}

		#endregion
	}

	/// <summary>
	/// 菜单项
	/// </summary>
	public class MenuItemViewModel : BindableBase
	{
		private ImageSource _icon;
		private string _caption;
		private ICommand _command;
		private object _commandParameter;
		private string _inputGestureText;
		private bool _staysOpenOnClick;
		private bool _isCheckable;
		private bool _isChecked;
		private bool _isSeparator;

		/// <summary>
		/// 图标
		/// </summary>
		public ImageSource Icon
		{
			get => _icon;
			set
			{
				_icon = value;
				OnPropertyChanged(nameof(Icon));
			}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Caption
		{
			get => _caption;
			set
			{
				_caption = value;
				OnPropertyChanged(nameof(Caption));
			}
		}
		/// <summary>
		/// 命令
		/// </summary>
		public ICommand Command
		{
			get => _command;
			set
			{
				_command = value;
				OnPropertyChanged(nameof(Command));
			}
		}
		/// <summary>
		/// 命令参数
		/// </summary>
		public object CommandParameter
		{
			get => _commandParameter;
			set
			{
				_commandParameter = value;
				OnPropertyChanged(nameof(CommandParameter));
			}
		}
		/// <summary>
		/// 设置文本，以描述将调用与指定项关联的命令的输入笔势。
		/// </summary>
		public string InputGestureText
		{
			get => _inputGestureText;
			set
			{
				_inputGestureText = value;
				OnPropertyChanged(nameof(InputGestureText));
			}
		}
		/// <summary>
		/// 点击后是否保持打开状态
		/// </summary>
		public bool StaysOpenOnClick
		{
			get => _staysOpenOnClick;
			set
			{
				_staysOpenOnClick = value;
				OnPropertyChanged(nameof(StaysOpenOnClick));
			}
		}
		/// <summary>
		/// 是否为Check模式
		/// </summary>
		public bool IsCheckable
		{
			get => _isCheckable;
			set
			{
				_isCheckable = value;
				OnPropertyChanged(nameof(IsCheckable));
			}
		}
		/// <summary>
		/// 是否Checked
		/// </summary>
		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				_isChecked = value;
				OnPropertyChanged(nameof(IsChecked));
			}
		}
		/// <summary>
		/// 是否是分割条
		/// </summary>
		public bool IsSeparator
		{
			get => _isSeparator;
			set
			{
				_isSeparator = value;
				OnPropertyChanged(nameof(IsSeparator));
			}
		}
		/// <summary>
		/// 子项
		/// </summary>
		public ObservableCollection<MenuItemViewModel> Children { get; } = new ObservableCollection<MenuItemViewModel>();

		/// <summary>
		/// .ctor
		/// </summary>
		public MenuItemViewModel()
		{
			
		}

		/// <summary>
		/// .ctor
		/// </summary>
		public MenuItemViewModel(string caption)
		{
			Caption = caption;
		}

		/// <summary>
		/// .ctor
		/// </summary>
		public MenuItemViewModel(string caption, ImageSource icon)
			: this(caption)
		{
			Icon = icon;
		}

		/// <summary>
		/// 分割条
		/// </summary>
		public static MenuItemViewModel Separator => new MenuItemViewModel { IsSeparator = true };
	}
}