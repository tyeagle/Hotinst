using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.VisualUtil
{
	/// <summary>
	/// 虚拟可视化树通用操作类。
	/// </summary>
	public static class VisualUtils
	{
		private const string RootPopupTypeName = "System.Windows.Controls.Primitives.PopupRoot";
		/// <summary>
		/// 
		/// </summary>
		public static Type RootPopupType;

		static VisualUtils()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(FrameworkElement));
			string typeName = Assembly.CreateQualifiedName(assembly.FullName, RootPopupTypeName);
			RootPopupType = Type.GetType(typeName);
		}

		/// <summary>
		/// 获取命中测试结果
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="element"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static T GetHitTest<T>(UIElement element, Point position) where T : DependencyObject
		{
			HitTestResult result = VisualTreeHelper.HitTest(element, position);
			if(result == null)
			{
				return default(T);
			}
			T obj = VisualUpSearch<T>(result.VisualHit);
			return obj;
		}
		
		/// <summary>
		/// 查找根元素
		/// </summary>
		/// <param name="startingFrom">开始查找的元素</param>
		/// <returns></returns>
		public static Visual FindRootVisual(Visual startingFrom)
		{
			Visual result = null;
			if (startingFrom != null)
			{
				result = startingFrom;
				while ((startingFrom = VisualTreeHelper.GetParent(startingFrom) as Visual) != null)
				{
					result = startingFrom;
				}
			}
			return result;
		}

		/// <summary>
		/// 查找可视化树中指定类型的对象实例
		/// </summary>
		/// <param name="startingFrom">开始查找的元素</param>
		/// <param name="typeAncestor">要查找的对象的类型</param>
		/// <returns></returns>
		public static Visual FindAncestor(Visual startingFrom, Type typeAncestor)
		{
			if (startingFrom != null)
			{
				DependencyObject parent = VisualTreeHelper.GetParent(startingFrom);
				while (parent != null && !typeAncestor.IsInstanceOfType(parent))
				{
					parent = VisualTreeHelper.GetParent(parent);
				}
				return parent as Visual;
			}
			return null;
		}

		/// <summary>
		/// 查找逻辑树中指定类型的对象实例
		/// </summary>
		/// <param name="startingFrom">开始查找的元素</param>
		/// <param name="typeAncestor">要查找的对象的类型</param>
		/// <returns></returns>
		public static Visual FindLogicalAncestor(Visual startingFrom, Type typeAncestor)
		{
			if (startingFrom != null)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(startingFrom);
				while (parent != null && !typeAncestor.IsInstanceOfType(parent))
				{
					parent = LogicalTreeHelper.GetParent(parent);
				}
				return parent as Visual;
			}
			return null;
		}

		/// <summary>
		/// 查找可视化树中指定类型的子级元素
		/// </summary>
		/// <param name="startingFrom">开始查找的元素</param>
		/// <param name="typeDescendant">要查找元素的类型</param>
		/// <returns></returns>
		public static Visual FindDescendant(Visual startingFrom, Type typeDescendant)
		{
			Visual visual = null;
			bool flag = false;
			int childrenCount = VisualTreeHelper.GetChildrenCount(startingFrom);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual2 = VisualTreeHelper.GetChild(startingFrom, i) as Visual;
				if (typeDescendant.IsInstanceOfType(visual2))
				{
					visual = visual2;
					flag = true;
				}
				if (flag)
				{
					break;
				}
				if (visual2 != null)
				{
					visual = FindDescendant(visual2, typeDescendant);
					if (visual != null)
					{
						break;
					}
				}
			}
			return visual;
		}

		/// <summary>
		/// 公开枚举可视化树中指定类型的子级元素
		/// </summary>
		/// <param name="rootelement"></param>
		/// <param name="typeChild"></param>
		/// <returns></returns>
		public static IEnumerable<Visual> EnumChildrenOfType(Visual rootelement, Type typeChild)
		{
			if (rootelement != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(rootelement);
				for (int i = 0; i < childrenCount; i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(rootelement, i);
					if (child is Visual)
					{
						Visual visual = (Visual)child;
						if (typeChild.IsInstanceOfType(visual))
						{
							yield return visual;
						}
						foreach (Visual current in EnumChildrenOfType(visual, typeChild))
						{
							yield return current;
						}
					}
				}
				yield break;
			}
			throw new ArgumentNullException("rootelement");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="panelType"></param>
		/// <returns></returns>
		public static Panel GetItemsPanel(ItemsControl owner, Type panelType)
		{
			Panel result = null;
			if (owner != null && panelType != null)
			{
				foreach (Visual current in EnumChildrenOfType(owner, panelType))
				{
					Panel panel = current as Panel;
					if (panel != null && Equals(GetItemsControlFromChildren(panel), owner))
					{
						result = panel;
						break;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static ItemsControl GetItemsControlFromChildren(FrameworkElement element)
		{
			ItemsControl itemsControl = null;
			if (element != null)
			{
				itemsControl = element as ItemsControl;
				if (itemsControl == null)
				{
					while (element != null)
					{
						element = VisualTreeHelper.GetParent(element) as FrameworkElement;
						if (element is ItemsControl)
						{
							itemsControl = (ItemsControl)element;
							break;
						}
					}
				}
			}
			return itemsControl;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootelement"></param>
		/// <param name="typeChild"></param>
		/// <returns></returns>
		public static IEnumerable<DependencyObject> EnumLogicalChildrenOfType(DependencyObject rootelement, Type typeChild)
		{
			foreach (object current in LogicalTreeHelper.GetChildren(rootelement))
			{
				if (current is DependencyObject)
				{
					if (typeChild.IsInstanceOfType(current))
					{
						yield return current as DependencyObject;
					}
					foreach (DependencyObject current2 in EnumLogicalChildrenOfType(current as DependencyObject, typeChild))
					{
						yield return current2;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reference"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public static bool IsDescendant(DependencyObject reference, DependencyObject node)
		{
			bool result = false;
			while (node != null)
			{
				if (Equals(node, reference))
				{
					result = true;
					break;
				}
				if (node.GetType() == RootPopupType)
				{
					Popup popup = (node as FrameworkElement).Parent as Popup;
					node = popup;
					if (popup != null)
					{
						node = popup.Parent;
						if (node == null)
						{
							node = popup.PlacementTarget;
						}
					}
				}
				else
				{
					node = FindParent(node);
				}
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		public static void InvalidateParentMeasure(FrameworkElement element)
		{
			FrameworkElement frameworkElement = VisualTreeHelper.GetParent(element) as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.InvalidateMeasure();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootelement"></param>
		/// <param name="typeParent"></param>
		/// <returns></returns>
		public static FrameworkElement FindSomeParent(FrameworkElement rootelement, Type typeParent)
		{
			FrameworkElement frameworkElement = rootelement.Parent as FrameworkElement;
			while (frameworkElement != null && !typeParent.IsInstanceOfType(frameworkElement))
			{
				frameworkElement = frameworkElement.Parent as FrameworkElement;
			}
			return frameworkElement;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="child"></param>
		/// <param name="name"></param>
		/// <param name="breakName">遇到此名字, 跳出搜索, 优先级高于name</param>
		/// <returns></returns>
		public static T FindVisualParent<T>(DependencyObject child, string name = "", string breakName = "") where T : DependencyObject
		{
			if(child == null)
			{
				return default(T);
			}
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			if (parent == null)
			{
				return default(T);
			}
			if(!string.IsNullOrEmpty(breakName) && parent is FrameworkElement fe && fe.Name.Equals(breakName))
			{
				return default(T);
			}
			if (parent is T t)
			{
				if (!string.IsNullOrEmpty(name))
				{
					if (parent is FrameworkElement e && e.Name.Equals(name))
					{
						return t;
					}
				}
				return t;
			}
			return FindVisualParent<T>(parent, name, breakName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetElement"></param>
		/// <param name="dependencyProperty"></param>
		/// <param name="value"></param>
		[Obsolete("SetDependencyPropretyUsedByAnimation is deprecated, please use SetDependencyPropertyUsedByAnimation instead.")]
		public static void SetDependencyPropretyUsedByAnimation(UIElement targetElement, DependencyProperty dependencyProperty, double value)
		{
			SetDependencyPropertyUsedByAnimation(targetElement, dependencyProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetElement"></param>
		/// <param name="dependencyProperty"></param>
		/// <param name="value"></param>
		public static void SetDependencyPropertyUsedByAnimation(UIElement targetElement, DependencyProperty dependencyProperty, double value)
		{
			targetElement.BeginAnimation(dependencyProperty, null);
			targetElement.SetValue(dependencyProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootelEment"></param>
		/// <param name="searchType"></param>
		/// <returns></returns>
		public static bool HasChildOfType(Visual rootelEment, Type searchType)
		{
			bool flag = false;
			int childrenCount = VisualTreeHelper.GetChildrenCount(rootelEment);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual = VisualTreeHelper.GetChild(rootelEment, i) as Visual;
				flag = searchType.IsInstanceOfType(visual);
				if (flag)
				{
					break;
				}
				if (visual != null)
				{
					flag = HasChildOfType(visual, searchType);
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="visual"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static Point PointToScreen(UIElement visual, Point point)
		{
			if (PermissionHelper.HasUnmanagedCodePermission)
			{
				return PermissionHelper.GetSafePointToScreen(visual, point);
			}
			Point pointRelativeTo = GetPointRelativeTo(visual, null);
			return new Point(pointRelativeTo.X + point.X, pointRelativeTo.Y + point.Y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="visual"></param>
		/// <param name="relativeTo"></param>
		/// <returns></returns>
		public static Point GetPointRelativeTo(UIElement visual, UIElement relativeTo)
		{
			Point position = Mouse.PrimaryDevice.GetPosition(visual);
			Point position2 = Mouse.PrimaryDevice.GetPosition(relativeTo);
			return new Point(position2.X - position.X, position2.Y - position.Y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		private static DependencyObject FindParent(DependencyObject d)
		{
			Visual visual = d as Visual;
			ContentElement contentElement = visual == null ? d as ContentElement : null;
			if (contentElement != null)
			{
				d = ContentOperations.GetParent(contentElement);
				if (d != null)
				{
					return d;
				}
				FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return frameworkContentElement.Parent;
				}
			}
			else
			{
				if (visual != null)
				{
					return VisualTreeHelper.GetParent(visual);
				}
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Page GetPageFromChildren(FrameworkElement element)
		{
			Page page = null;
			if (element != null)
			{
				page = element as Page;
				if (page == null)
				{
					while (element != null)
					{
						element = VisualTreeHelper.GetParent(element) as FrameworkElement;
						if (element is Page)
						{
							page = (Page)element;
							break;
						}
					}
				}
			}
			return page;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Window GetWindowFromChildren(FrameworkElement element)
		{
			Window window = null;
			if (element != null)
			{
				window = element as Window;
				if (window == null)
				{
					while (element != null)
					{
						element = VisualTreeHelper.GetParent(element) as FrameworkElement;
						if (element is Window)
						{
							window = (Window)element;
							break;
						}
					}
				}
			}
			return window;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static T VisualUpSearch<T>(DependencyObject source) where T : DependencyObject
		{
			while(source != null && source.GetType() != typeof(T))
				source = VisualTreeHelper.GetParent(source);

			return (T)source;
		}

		/// <summary>
		/// 在Visual里找到想要的元素
		/// childName可为空，不为空就按名字找
		/// </summary>
		public static T FindChild<T>(DependencyObject parent, string childName = "") where T : DependencyObject
		{
			if(parent == null)
				return null;

			T foundChild = null;

			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for(int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);

				T childType = child as T;
				if(childType == null)
				{
					// 住下查要找的元素
					foundChild = FindChild<T>(child, childName);

					// 如果找不到就反回
					if(foundChild != null)
						break;
				}
				else if(!string.IsNullOrEmpty(childName))
				{
					var frameworkElement = child as FrameworkElement;
					// 看名字是不是一样
					if(frameworkElement != null && frameworkElement.Name == childName)
					{
						//如果名字一样返回
						foundChild = (T)child;
						break;
					}
					// 如果名字不一样就继续向下找
					foundChild = FindChild<T>(child, childName);
				}
				else
				{
					// 找到相应的元素了就返回 
					foundChild = (T)child;
					break;
				}
			}

			return foundChild;
		}

		/// <summary>
		/// 查找子控件
		/// </summary>
		/// <typeparam name="T">子控件的类型</typeparam>
		/// <param name="obj">要找的是obj的子控件</param>
		/// <param name="name">想找的子控件的Name属性</param>
		/// <returns>目标子控件</returns>
		public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
		{
			List<T> childList = new List<T>();

			for(int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if(child is T childObject && (childObject.Name == name) | string.IsNullOrEmpty(name))
				{
					childList.Add(childObject);
				}
				childList.AddRange(GetChildObjects<T>(child, name));
			}
			return childList;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="depObj"></param>
		/// <returns></returns>
		public static T GetFirstVisualChild<T>(DependencyObject depObj) where T : DependencyObject
		{
			if(depObj != null)
			{
				for(int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if(child is T visualChild)
					{
						return visualChild;
					}

					T childItem = GetFirstVisualChild<T>(child);
					if(childItem != null)
					{
						return childItem;
					}
				}
			}

			return null;
		}
	}
}