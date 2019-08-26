using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace HOTINST.COMMON.Controls.Service
{
	internal class DialogLayeringHelper : IDialogHost
	{
		public DialogLayeringHelper(ContentControl parent)
		{
			_parent = parent;
		}

		private readonly ContentControl _parent;
		private readonly List<object> _layerStack = new List<object>();

		public bool HasDialogLayers => _layerStack.Any();

		private object CreateContent(DialogBaseControl dialog)
		{
			Grid grid = new Grid();

			object oldContent = _parent.Content;
			_parent.Content = null;
			((UIElement)oldContent).IsEnabled = false;
			grid.Children.Add((UIElement)oldContent);
			grid.Children.Add(dialog);

			return grid;
		}

		private object ExtractContent(object content)
		{
			if(content is Grid gd && gd.Children.Count == 2)
			{
				UIElement dialog = gd.Children[1];
				gd.Children.Clear();
				return dialog;
			}

			return null;
		}

		#region Implementation of IDialogHost

		public void ShowDialog(DialogBaseControl dialog)
		{
			_layerStack.Add(_parent.Content);
			_parent.Content = CreateContent(dialog);
		}

		public void HideDialog(DialogBaseControl dialog)
		{
			if (Equals(ExtractContent(_parent.Content), dialog))
			{
				object oldContent = _layerStack.Last();
				_layerStack.Remove(oldContent);
				_parent.Content = oldContent;
				((UIElement)oldContent).IsEnabled = true;
			}
			else
			{
				_layerStack.Remove(dialog);
			}
		}

		public FrameworkElement GetCurrentContent()
		{
			return _parent;
		}

		#endregion
	}
}