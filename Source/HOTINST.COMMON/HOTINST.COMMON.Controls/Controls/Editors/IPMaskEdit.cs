/**
 * ==============================================================================
 * Classname   : IPMaskEdit
 * Description : 
 *
 * Compiler    : Visual Studio 2013
 * CLR Version : 4.0.30319.42000
 * Created     : 2017/3/14 14:46:41
 *
 * Author  : caixs
 * Company : Hotinst
 *
 * Copyright © Hotinst 2017. All rights reserved.
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// IP输入框
	/// </summary>
	public class IPMaskEdit : TextBox
	{
		/// <summary>
		/// InputMask Dependency Property
		/// </summary>
		public static readonly DependencyProperty InputMaskProperty;

		private readonly List<InputMaskChar> _maskChars;
		private int _caretIndex;

		private static int _currentSectionIndex;
		private static bool _shouldReplace;

		private string _preText = string.Empty;

		static IPMaskEdit()
		{
			InputMaskProperty = DependencyProperty.Register("InputMask", typeof(string),
				typeof(IPMaskEdit), new PropertyMetadata(string.Empty, InputMask_Changed));
		}

		/// <summary>
		/// .ctor
		/// </summary>
		public IPMaskEdit()
		{
			_maskChars = new List<InputMaskChar>();
			DataObject.AddPastingHandler(this, MaskedTextBox_Paste);
		}

		/// <summary>
		/// Get or Set the input mask.
		/// </summary>
		public string InputMask
		{
			get { return GetValue(InputMaskProperty) as string; }
			set { SetValue(InputMaskProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		[Flags]
		protected enum InputMaskValidationFlags
		{
			/// <summary>
			/// 
			/// </summary>
			None = 0,
			/// <summary>
			/// 
			/// </summary>
			AllowInteger = 1,
			/// <summary>
			/// 
			/// </summary>
			AllowDecimal = 2,
			/// <summary>
			/// 
			/// </summary>
			AllowAlphabet = 4,
			/// <summary>
			/// 
			/// </summary>
			AllowAlphanumeric = 8
		}

		private class InputMaskChar
		{
			public InputMaskChar(InputMaskValidationFlags validationFlags)
			{
				ValidationFlags = validationFlags;
				Literal = (char)0;
			}

			public InputMaskChar(char literal)
			{
				Literal = literal;
			}

			public InputMaskValidationFlags ValidationFlags { get; private set; }

			private char Literal { get; set; }

			public bool IsLiteral()
			{
				return Literal != (char)0;
			}

			public char GetDefaultChar()
			{
				return IsLiteral() ? Literal : '_';
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);
			if(string.IsNullOrEmpty(Text))
				Text = "___.___.___.___";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			_caretIndex = CaretIndex;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			OnKeyDown(e);

			//no mask specified, just function as a normal textbox
			if(_maskChars.Count == 0)
				return;

			switch(e.Key)
			{
				case Key.Delete:
					//delete key pressed: delete all text
					Text = GetDefaultText();
					_caretIndex = CaretIndex = 0;
					e.Handled = true;
					break;
				case Key.Back:
					if(_caretIndex > 0 || SelectionLength > 0)
					{
						if(SelectionLength > 0)
						{
							//if one or more characters selected, delete them
							DeleteSelectedText();
						}
						else
						{
							//if no characters selected, shift the caret back to the previous non-literal char and delete it
							MoveBack();

							char[] characters = Text.ToCharArray();
							characters[_caretIndex] = _maskChars[_caretIndex].GetDefaultChar();
							Text = new string(characters);
						}

						//update the base class caret index, and swallow the event
						CaretIndex = _caretIndex;
						e.Handled = true;
					}
					break;
				case Key.Left:
					//move back to the previous non-literal character
					MoveBack();
					e.Handled = true;
					break;
				case Key.Right:
					//move forwards to the next non-literal character
					MoveForward();
					e.Handled = true;
					break;
				case Key.Space:
					e.Handled = true;
					break;
				case Key.Home:
					_caretIndex = CaretIndex = 0;
					e.Handled = true;
					break;
				case Key.End:
					_caretIndex = CaretIndex = 15;
					e.Handled = true;
					break;
				case Key.Enter:
					if(!IsSectionValueNull(CaretIndex / 4))
					{
						ValidateSectionText(CaretIndex / 4);
					}
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			base.OnPreviewTextInput(e);

			//no mask specified, just function as a normal textbox
			if(_maskChars.Count == 0)
				return;

			if(CaretIndex == _maskChars.Count)
			{
				//at the end of the character count defined by the input mask- no more characters allowed
				e.Handled = true;
				return;
			}
			if(e.Text.Equals("."))
			{
				if(!IsSectionValueNull(CaretIndex / 4))
				{
					ValidateSectionText(CaretIndex / 4);
				}
				e.Handled = true;
				return;
			}

			//validate the character against its validation scheme
			bool isValid = ValidateInputChar(char.Parse(e.Text), _maskChars[CaretIndex].ValidationFlags);

			if(isValid)
			{
				//delete any selected text
				if(SelectionLength > 0)
				{
					DeleteSelectedText();
				}

				_caretIndex = CaretIndex = SelectionStart;

				//insert the new character
				char[] characters = Text.ToCharArray();
				characters[_caretIndex] = char.Parse(e.Text);
				Text = new string(characters);

				//move the caret on
				if(IsEndSection(_caretIndex))
					ValidateSectionText(_caretIndex / 4);
				else
					MoveForward();
			}

			e.Handled = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(RoutedEventArgs e)
		{
			base.OnSelectionChanged(e);

			if(string.IsNullOrEmpty(Text) || Text.Equals(_preText) || Text.Equals("___.___.___.___"))
				return;

			StringBuilder sb = new StringBuilder();
			string[] ipStrings = Text.Split('.');
			for(int i = 0; i < ipStrings.Length; i++)
			{
				sb.Append(ipStrings[i].PadLeft(3, ' '));
				if(i < ipStrings.Length - 1)
					sb.Append(".");
			}
			Text = sb.ToString();
			_preText = Text;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			
			if(!IsSectionValueNull(CaretIndex / 4))
			{
				ValidateSectionText(CaretIndex / 4);
			}
		}

		private bool IsSectionValueNull(int sectionIndex)
		{
			return !IsNumeric(GetIPSectionShort(sectionIndex));
		}

		private bool IsEndSection(int sectionIndex)
		{
			return sectionIndex == 2 || sectionIndex == 6 || sectionIndex == 10 || sectionIndex == 14;
		}

		/// <summary>
		/// Validates the specified character against all selected validation schemes.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="validationFlags"></param>
		/// <returns></returns>
		protected virtual bool ValidateInputChar(char input, InputMaskValidationFlags validationFlags)
		{
			bool valid = validationFlags == InputMaskValidationFlags.None;

			if(!valid)
			{
				Array values = Enum.GetValues(typeof(InputMaskValidationFlags));

				//iterate through the validation schemes
				foreach(object o in values)
				{
					InputMaskValidationFlags instance = (InputMaskValidationFlags)(int)o;
					if((instance & validationFlags) != 0)
					{
						if(ValidateCharInternal(input, instance))
						{
							valid = true;
							break;
						}
					}
				}
			}

			return valid;
		}

		/// <summary>
		/// Returns a value indicating if the current text value is valid.
		/// </summary>
		/// <returns></returns>
		protected virtual bool ValidateTextInternal(string text, out string displayText)
		{
			if(_maskChars.Count == 0)
			{
				displayText = text;
				return true;
			}

			StringBuilder displayTextBuilder = new StringBuilder(GetDefaultText());

			if(_shouldReplace)
			{
				_shouldReplace = false;
				_currentSectionIndex = 0;
				char[] sc = GetIPSectionFull(text, _currentSectionIndex).ToCharArray();
				for(int i = _currentSectionIndex * 4; i < _currentSectionIndex * 4 + 3; i++)
				{
					displayTextBuilder[i] = sc[i % 4];
				}
			}

			bool valid = !string.IsNullOrEmpty(text) && text.Length <= _maskChars.Count;

			if(valid)
			{
				for(int i = 0; i < text.Length; i++)
				{
					if(!_maskChars[i].IsLiteral())
					{
						if(ValidateInputChar(text[i], _maskChars[i].ValidationFlags) && !_shouldReplace)
						{
							displayTextBuilder[i] = text[i];
						}
						else
						{
							valid = false;
						}
					}
				}
			}

			displayText = displayTextBuilder.ToString();

			return valid;
		}

		/// <summary>
		/// Deletes the currently selected text.
		/// </summary>
		protected virtual void DeleteSelectedText()
		{
			string defaultText = GetDefaultText();
			if(SelectionLength == Text.Length)
			{
				Text = defaultText;
				//reset the caret position
				CaretIndex = _caretIndex = 0;
				return;
			}

			StringBuilder text = new StringBuilder(Text);
			int selectionStart = SelectionStart;
			int selectionLength = SelectionLength;

			text.Remove(selectionStart, selectionLength);
			text.Insert(selectionStart, defaultText.Substring(selectionStart, selectionLength));
			Text = text.ToString();

			//reset the caret position
			CaretIndex = _caretIndex = selectionStart;
		}

		/// <summary>
		/// Returns a value indicating if the specified input mask character is a placeholder.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="validationFlags">If the character is a placeholder, returns the relevant validation scheme.</param>
		/// <returns></returns>
		protected virtual bool IsPlaceholderChar(char character, out InputMaskValidationFlags validationFlags)
		{
			validationFlags = InputMaskValidationFlags.None;

			switch(character.ToString().ToUpper())
			{
				case "I":
					validationFlags = InputMaskValidationFlags.AllowInteger;
					break;
				case "D":
					validationFlags = InputMaskValidationFlags.AllowDecimal;
					break;
				case "A":
					validationFlags = InputMaskValidationFlags.AllowAlphabet;
					break;
				case "W":
					validationFlags = InputMaskValidationFlags.AllowAlphanumeric;
					break;
			}

			return validationFlags != InputMaskValidationFlags.None;
		}

		/// <summary>
		/// Invoked when the InputMask dependency property reports a change.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="e"></param>
		private static void InputMask_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			IPMaskEdit ipMaskEdit = obj as IPMaskEdit;
			if(ipMaskEdit != null) ipMaskEdit.UpdateInputMask();
		}

		/// <summary>
		/// Invokes when a paste event is raised.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MaskedTextBox_Paste(object sender, DataObjectPastingEventArgs e)
		{
			if(e.DataObject.GetDataPresent(typeof(string)))
			{
				object data = e.DataObject.GetData(typeof(string));
				if(data != null)
				{
					string value = data.ToString();
					string displayText;

					if(ValidateTextInternal(value, out displayText))
					{
						Text = displayText;
					}
				}
			}

			e.CancelCommand();
		}

		/// <summary>
		/// Rebuilds the InputMaskChars collection when the input mask property is updated.
		/// </summary>
		private void UpdateInputMask()
		{
			string text = Text;
			_maskChars.Clear();

			Text = string.Empty;

			string mask = InputMask;

			if(string.IsNullOrEmpty(mask))
				return;

			foreach(char t in mask)
			{
				var validationFlags = InputMaskValidationFlags.None;
				bool isPlaceholder = IsPlaceholderChar(t, out validationFlags);

				_maskChars.Add(isPlaceholder ? new InputMaskChar(validationFlags) : new InputMaskChar(t));
			}

			string displayText;
			if(text.Length > 0 && ValidateTextInternal(text, out displayText))
			{
				Text = displayText;
			}
			else
			{
				Text = GetDefaultText();
			}
		}

		/// <summary>
		/// Validates the specified character against its input mask validation scheme.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="validationType"></param>
		/// <returns></returns>
		private bool ValidateCharInternal(char input, InputMaskValidationFlags validationType)
		{
			bool valid = false;

			switch(validationType)
			{
				case InputMaskValidationFlags.AllowInteger:
				case InputMaskValidationFlags.AllowDecimal:
					if(validationType == InputMaskValidationFlags.AllowDecimal && input == '.' && !Text.Contains('.'))
					{
						valid = true;
					}
					else
					{
						int i;
						valid = int.TryParse(input.ToString(), out i);
					}
					break;
				case InputMaskValidationFlags.AllowAlphabet:
					valid = char.IsLetter(input);
					break;
				case InputMaskValidationFlags.AllowAlphanumeric:
					valid = char.IsLetter(input) || char.IsNumber(input);
					break;
			}

			return valid;
		}

		private void ValidateSectionText(int sectionIndex)
		{
			int partIp = int.Parse(Text.Substring(sectionIndex * 4, 3).Replace("_", ""));
			if(partIp > 255)
			{
				SetIPSection(sectionIndex, "255");
			}
			else if(partIp == 0)
			{
				SetIPSection(sectionIndex, " 0 ");
			}
			else if(partIp > 0 && partIp <= 9)
			{
				SetIPSection(sectionIndex, " " + partIp + " ");
			}
			else if(partIp > 9 && partIp <= 99)
			{
				SetIPSection(sectionIndex, " " + partIp);
			}
			else if(partIp > 99 && partIp <= 255)
			{
				SetIPSection(sectionIndex, partIp.ToString());
			}
		}

		private void SetIPSection(int sectionIndex, string text)
		{
			Text = Text.Substring(0, sectionIndex * 4) + text + Text.Substring((sectionIndex + 1) * 4 - 1);
			MoveForwardSection(sectionIndex);
		}

		private string GetIPSectionShort(int sectionIndex)
		{
			return Text.Substring(sectionIndex * 4, 3).Replace("_", "");
		}

		private string GetIPSectionFull(string ip, int sectionIndex)
		{
			return ip.Substring(sectionIndex * 4, 3).Replace("_", " ");
		}

		/// <summary>
		/// Builds the default display text for the control.
		/// </summary>
		/// <returns></returns>
		private string GetDefaultText()
		{
			StringBuilder text = new StringBuilder();
			foreach(InputMaskChar maskChar in _maskChars)
			{
				text.Append(maskChar.GetDefaultChar());
			}
			return text.ToString();
		}

		/// <summary>
		/// Moves the caret forward to the next non-literal position.
		/// </summary>
		private void MoveForward()
		{
			int pos = _caretIndex;
			while(pos < _maskChars.Count)
			{
				if(++pos == _maskChars.Count || !_maskChars[pos].IsLiteral())
				{
					_caretIndex = CaretIndex = pos;
					break;
				}
			}
		}

		/// <summary>
		/// Moves the caret backward to the previous non-literal position.
		/// </summary>
		private void MoveBack()
		{
			int pos = _caretIndex;
			while(pos > 0)
			{
				if(--pos == 0 || !_maskChars[pos].IsLiteral())
				{
					_caretIndex = CaretIndex = pos;
					break;
				}
			}
		}

		private void MoveForwardSection(int sectionIndex)
		{
			int index = 0;
			for(int i = 0; i < _maskChars.Count; i++)
			{
				if(i == _maskChars.Count - 1)
				{
					_caretIndex = CaretIndex = i + 1;
					break;
				}
				if(sectionIndex + 1 == index)
				{
					_caretIndex = CaretIndex = i;
					SelectionStart = i;
					SelectionLength = 3;
					break;
				}
				if(_maskChars[i].IsLiteral())
				{
					index++;
				}
			}
		}

		/// <summary>
		/// 判断字符串是否是数字
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNumeric(string value)
		{
			return Regex.IsMatch(value.Trim(), @"^\d+$");
		}
	}
}