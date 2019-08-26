/**
 * ==============================================================================
 *
 * ClassName: IntegerValueHandler
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:46:08
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using HOTINST.COMMON.Controls.Controls.Editors;

namespace HOTINST.COMMON.Controls.Core.Editors
{
	internal class IntegerValueHandler
	{
		public static IntegerValueHandler integerValueHandler = new IntegerValueHandler();
		public bool MatchWithMask(IntegerTextBox integerTextBox, string text)
		{
			if(integerTextBox.IsReadOnly)
			{
				return true;
			}
			bool flag = false;
			if(integerTextBox.mValue.HasValue)
			{
				long? mValue = integerTextBox.mValue;
				if(mValue.GetValueOrDefault() != 0L || !mValue.HasValue)
				{
					goto IL_11D;
				}
			}
			if(text == "-")
			{
				integerTextBox.minusPressed = true;
				string s = text + 1;
				long num;
				bool flag2 = long.TryParse(s, out num);
				if(flag2 && integerTextBox != null && num < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					integerTextBox.Value = integerTextBox.MinValue;
					return true;
				}
				if(integerTextBox.count % 2 == 0)
				{
					integerTextBox.Foreground = integerTextBox.PositiveForeground;
					integerTextBox.IsNegative = false;
				}
				else
				{
					integerTextBox.Foreground = integerTextBox.NegativeForeground;
					integerTextBox.IsNegative = true;
				}
				integerTextBox.count++;
				integerTextBox.MaskedText = "-";
				if(!integerTextBox.UseNullOption)
				{
					integerTextBox.Value = 0L;
				}
				integerTextBox.CaretIndex = 1;
				integerTextBox.IsNegative = true;
				return true;
			}
			else
			{
				if(integerTextBox.minusPressed)
				{
					integerTextBox.minusPressed = false;
					if(integerTextBox.count % 2 == 0)
					{
						flag = true;
					}
				}
			}
			IL_11D:
			NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
			if(text == "-" || text == "+")
			{
				if(integerTextBox.mValue.HasValue)
				{
					long num2;
					if(text == "+")
					{
						long? value = integerTextBox.Value;
						if(value.GetValueOrDefault() < 1L && value.HasValue)
						{
							num2 = integerTextBox.mValue.Value * -1L;
						}
						else
						{
							num2 = integerTextBox.mValue.Value;
						}
					}
					else
					{
						num2 = integerTextBox.mValue.Value * -1L;
					}
					if(num2 > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
					{
						if(!integerTextBox.MaxValueOnExceedMaxDigit)
						{
							return true;
						}
						num2 = integerTextBox.MaxValue;
					}
					if(num2 < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
					{
						if(!integerTextBox.MinValueOnExceedMinDigit)
						{
							return true;
						}
						num2 = integerTextBox.MinValue;
					}
					bool flag3 = false;
					int length = integerTextBox.SelectedText.Length;
					if(length == integerTextBox.MaskedText.Length)
					{
						flag3 = true;
						if(text == "-")
						{
							integerTextBox.minusPressed = true;
							if(integerTextBox.UseNullOption)
							{
								integerTextBox.SetValue(false, null);
							}
							else
							{
								integerTextBox.Value = 0L;
							}
							integerTextBox.MaskedText = "-";
							integerTextBox.CaretIndex = 1;
							return true;
						}
					}
					if(integerTextBox.MaskedText.Length == integerTextBox.MaxLength)
					{
						integerTextBox.MaskedText = integerTextBox.MaskedText;
					}
					int selectionStart = integerTextBox.SelectionStart;
					integerTextBox.MaskedText = num2.ToString("N", numberFormat);
					if(text != "+")
					{
						int num3 = integerTextBox.MaskedText.Contains("-") ? selectionStart + 1 : selectionStart - 1;
						if(num3 < 0)
						{
							num3 = 0;
						}
						integerTextBox.SelectionStart = num3;
					}
					integerTextBox.SetValue(false, num2);
					if(flag3)
					{
						integerTextBox.SelectAll();
					}
				}
				integerTextBox.SelectionLength = 0;
				return true;
			}
			int num4 = 0;
			int num5 = 0;
			string text2 = integerTextBox.Text;
			string maskedText = integerTextBox.MaskedText;
			if(text2 != "")
			{
				if(text2.Length == 1)
				{
					if(text2 == "0")
					{
						text2 = text;
						goto IL_514;
					}
				}
				else
				{
					if(text2[0] == '0' && text2.Length == 1)
					{
						text2 = text + text2;
						goto IL_514;
					}
				}
			}
			text2 = "";
			for(int i = 0; i <= maskedText.Length; i++)
			{
				if(i == integerTextBox.SelectionStart)
				{
					num4 = text2.Length;
					int num6 = text2.Length;
				}
				if(i == integerTextBox.SelectionStart + integerTextBox.SelectionLength)
				{
					num5 = text2.Length;
				}
				if(i < maskedText.Length)
				{
					if(integerTextBox.Value.HasValue)
					{
						long? value2 = integerTextBox.Value;
						if(value2.GetValueOrDefault() != 0L || !value2.HasValue)
						{
							if(char.IsDigit(maskedText[i]))
							{
								text2 += maskedText[i];
								goto IL_46B;
							}
							goto IL_46B;
						}
					}
					if(char.IsDigit(maskedText[i]) || maskedText[i] == '-')
					{
						text2 += maskedText[i];
					}
				}
				IL_46B:
				;
			}
			int num7 = num5 - num4;
			if(text2.Length <= 0 || text2[0] != '0' || num4 != 0 || num7 != 0)
			{
				text2 = text2.Remove(num4, num7);
			}
			if(text != string.Empty && char.IsDigit(text[0]))
			{
				if(num4 == 0 && text == "0")
				{
					if(integerTextBox.Text.Length == num7 || text2 == "")
					{
						text2 = text2.Insert(num4, text);
					}
				}
				else
				{
					text2 = text2.Insert(num4, text);
				}
			}
			IL_514:
			if(text != string.Empty && char.IsDigit(text[0]))
			{
				if(text2.Contains("-"))
				{
					text2 = text2.Remove(0, 1);
				}
				long num8;
				if(!long.TryParse(text2, NumberStyles.Number, numberFormat, out num8))
				{
					return true;
				}
				if(integerTextBox.MaxValidation == MaxValidation.OnLostFocus && integerTextBox.SelectedText != "")
				{
					long? mValue2 = integerTextBox.mValue;
					if((mValue2.GetValueOrDefault() != 0L || !mValue2.HasValue) && !integerTextBox.IsNegative)
					{
						goto IL_61A;
					}
				}
				if(integerTextBox.IsNegative || flag)
				{
					if(!(integerTextBox.SelectedText == integerTextBox.Text.ToString()) && !integerTextBox.SelectedText.Contains("-"))
					{
						num8 *= -1L;
					}
					else
					{
						long? value3 = integerTextBox.Value;
						if(((value3.GetValueOrDefault() == 0L && value3.HasValue) || integerTextBox.IsNull) && flag)
						{
							num8 *= -1L;
						}
					}
				}
				IL_61A:
				if(num8 > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					if(!integerTextBox.MaxValueOnExceedMaxDigit)
					{
						return true;
					}
					num8 = integerTextBox.MaxValue;
				}
				if(num8 <= integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					if(num8 <= integerTextBox.MinValue && integerTextBox.MinValue >= 0L)
					{
						if(numberFormat != null)
						{
							if(integerTextBox.UseNullOption)
							{
								text2 = num8.ToString("N", numberFormat);
							}
							if(text2.Length - (numberFormat.NumberDecimalDigits + 1) >= integerTextBox.MinValue.ToString().Length)
							{
								num8 = integerTextBox.MinValue;
							}
							else
							{
								if(text2.Length - (numberFormat.NumberDecimalDigits + 1) <= integerTextBox.MinValue.ToString().Length)
								{
									integerTextBox.checktext += text;
									if(int.Parse(integerTextBox.checktext) >= integerTextBox.MinValue)
									{
										integerTextBox.Value = int.Parse(integerTextBox.checktext);
										integerTextBox.CaretIndex = integerTextBox.Value.ToString().Length;
										integerTextBox.checktext = "";
									}
									return true;
								}
							}
						}
					}
					else
					{
						if(num8 > integerTextBox.MinValue)
						{
							integerTextBox.MaskedText = text2;
						}
						else
						{
							if(num8 >= integerTextBox.MinValue)
							{
								if(integerTextBox.MinValueOnExceedMinDigit && text2.Length - (numberFormat.NumberDecimalDigits + 1) > integerTextBox.MinValue.ToString().Length)
								{
									num8 = integerTextBox.MinValue;
								}
								else
								{
									if(text2.Length - (numberFormat.NumberDecimalDigits + 1) > integerTextBox.MinValue.ToString().Length)
									{
										return true;
									}
									integerTextBox.MaskedText = text2;
								}
							}
							else
							{
								if(!integerTextBox.MinValueOnExceedMinDigit)
								{
									return true;
								}
								num8 = integerTextBox.MinValue;
							}
						}
					}
				}
				if(integerTextBox.MaxLength != 0 && text2.Length > integerTextBox.MaxLength)
				{
					int num6 = integerTextBox.CaretIndex;
					if(num8 < 0L)
					{
						num8 = long.Parse(text2.Remove(integerTextBox.MaxLength)) * -1L;
					}
					else
					{
						num8 = long.Parse(text2.Remove(integerTextBox.MaxLength));
					}
					integerTextBox.SetValue(false, num8);
					integerTextBox.MaskedText = num8.ToString("N", numberFormat);
					num6++;
					integerTextBox.CaretIndex = num6;
					return true;
				}
				if(integerTextBox.checktext != "" && num8 >= integerTextBox.MinValue)
				{
					num4++;
				}
				num8 = long.Parse(integerTextBox.checktext + num8.ToString());
				integerTextBox.SetValue(true, num8);
				integerTextBox.MaskedText = integerTextBox.Value.Value.ToString("N", numberFormat);
				integerTextBox.checktext = "";
				if(text2 != "")
				{
					if(text2[0] == '-')
					{
						long? oldValue = integerTextBox.OldValue;
						if(oldValue.GetValueOrDefault() == 0L && oldValue.HasValue)
						{
							integerTextBox.MaskedText = text2;
							maskedText = integerTextBox.MaskedText;
							integerTextBox.SelectionStart = 2;
							return true;
						}
					}
					if(text2[0] == '0')
					{
						long? value4 = integerTextBox.Value;
						if(value4.GetValueOrDefault() < 0L && value4.HasValue)
						{
							char c = '-';
							integerTextBox.MaskedText = c + text2;
						}
						else
						{
							integerTextBox.MaskedText = text2;
						}
					}
					maskedText = integerTextBox.MaskedText;
				}
				int num9 = -1;
				int j = 0;
				int num10 = 0;
				bool flag4 = false;
				bool flag5 = false;
				int num11 = 0;
				for(j = 0; j < maskedText.Length; j++)
				{
					if(char.IsDigit(maskedText[j]))
					{
						num9++;
					}
					if(maskedText.Contains("-") && maskedText.Length == 2 && !flag4)
					{
						num9++;
						flag4 = true;
					}
					if(num9 == num4)
					{
						num10 = j;
						flag5 = true;
					}
					if(flag5 && char.IsDigit(maskedText[j]))
					{
						num11++;
					}
					if(flag5 && num11 == text.Length)
					{
						break;
					}
				}
				integerTextBox.SelectionStart = num10 + num11;
				integerTextBox.SelectionLength = 0;
			}
			if(!integerTextBox.OnValidating(new CancelEventArgs(false)) && integerTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag6 = true;
				flag6 = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
				string messageBoxText = flag6 ? "String validation succeeded" : "String validation failed";
				if(!flag6)
				{
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag6, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag6, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag6, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag6, message, integerTextBox.ValidationValue));
					integerTextBox.OnValidated(EventArgs.Empty);
				}
				return true;
			}
			return true;
		}
		public long? ValueFromText(IntegerTextBox integerTextBox, string maskedText)
		{
			long value;
			if(long.TryParse(maskedText, NumberStyles.Number, integerTextBox.GetCulture().NumberFormat, out value))
			{
				return value;
			}
			if(integerTextBox.UseNullOption)
			{
				return null;
			}
			long num = 0L;
			if(num > integerTextBox.MaxValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
			{
				num = integerTextBox.MaxValue;
			}
			else
			{
				if(num < integerTextBox.MinValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					num = integerTextBox.MinValue;
				}
			}
			return num;
		}
		public bool HandleKeyDown(IntegerTextBox integerTextBox, KeyEventArgs eventArgs)
		{
			if(eventArgs.Key == Key.Space)
			{
				return true;
			}
			switch(eventArgs.Key)
			{
				case Key.Back:
					{
						integerTextBox.count = 1;
						return this.HandleBackSpaceKey(integerTextBox);
					}
				case Key.Up:
					{
						return this.HandleUpKey(integerTextBox);
					}
				case Key.Down:
					{
						return this.HandleDownKey(integerTextBox);
					}
				case Key.Delete:
					{
						integerTextBox.count = 1;
						return this.HandleDeleteKey(integerTextBox);
					}
			}
			return false;
		}
		public bool HandleBackSpaceKey(IntegerTextBox integerTextBox)
		{
			if(integerTextBox.IsReadOnly)
			{
				return true;
			}
			NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			string text2 = integerTextBox.MaskedText;
			if(integerTextBox.SelectionLength == 1)
			{
				if(!char.IsDigit(text2[integerTextBox.SelectionStart]))
				{
					if(text2[integerTextBox.SelectionStart] == '-')
					{
						long? value = integerTextBox.Value;
						integerTextBox.Value = value.HasValue ? new long?(value.GetValueOrDefault() * -1L) : null;
					}
					integerTextBox.SelectionLength = 0;
					return true;
				}
			}
			else
			{
				if(integerTextBox.SelectionLength == 0 && integerTextBox.SelectionStart != 0 && !char.IsDigit(text2[integerTextBox.SelectionStart - 1]))
				{
					if(text2[0] == '-')
					{
						long? mValue = integerTextBox.mValue;
						integerTextBox.Value = mValue.HasValue ? new long?(-1L * mValue.GetValueOrDefault()) : null;
						integerTextBox.SelectionLength = 0;
						long? value2 = integerTextBox.Value;
						if(value2.GetValueOrDefault() == 0L && value2.HasValue)
						{
							text2 = text2.Remove(0, 1);
							integerTextBox.MaskedText = text2;
						}
						return true;
					}
					integerTextBox.SelectionStart--;
					long? mValue2 = integerTextBox.mValue;
					integerTextBox.Value = mValue2.HasValue ? new long?(mValue2.GetValueOrDefault()) : null;
					integerTextBox.SelectionLength = 0;
					return true;
				}
			}
			for(int i = 0; i <= text2.Length; i++)
			{
				if(i == integerTextBox.SelectionStart)
				{
					num = text.Length;
				}
				if(i == integerTextBox.SelectionStart + integerTextBox.SelectionLength)
				{
					num2 = text.Length;
				}
				if(i < text2.Length && (char.IsDigit(text2[i]) || text2[i] == '-'))
				{
					text += text2[i];
				}
			}
			num3 = num2 - num;
			if(num3 != 0)
			{
				text = text.Remove(num, num3);
			}
			else
			{
				if(num == 0)
				{
					return true;
				}
				num3 = 1;
				text = text.Remove(num - 1, num3);
			}
			num -= num3;
			if(text.Length == 0)
			{
				if(integerTextBox.UseNullOption)
				{
					integerTextBox.SetValue(true, null);
				}
				else
				{
					if(text == "")
					{
						if(integerTextBox.MinValue.ToString() == "0" || integerTextBox.MinValue < 0L)
						{
							integerTextBox.MaskedText = "0";
							integerTextBox.Value = 0L;
						}
						else
						{
							integerTextBox.SetValue(true, integerTextBox.MinValue);
						}
					}
				}
				return true;
			}
			long num4;
			if(!long.TryParse(text, NumberStyles.Number, numberFormat, out num4))
			{
				if(text == "-")
				{
					if(integerTextBox.UseNullOption)
					{
						integerTextBox.MaskedText = "";
						integerTextBox.SetValue(true, null);
					}
					else
					{
						integerTextBox.MaskedText = "0";
						integerTextBox.Value = 0L;
					}
				}
				return true;
			}
			if(integerTextBox.IsNegative)
			{
			}
			if(num4 > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
			{
				if(!integerTextBox.MaxValueOnExceedMaxDigit)
				{
					return true;
				}
				num4 = integerTextBox.MaxValue;
			}
			if(num4 < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
			{
				if(!integerTextBox.MinValueOnExceedMinDigit)
				{
					return true;
				}
				num4 = integerTextBox.MinValue;
			}
			integerTextBox.SetValue(false, num4);
			integerTextBox.MaskedText = num4.ToString("N", numberFormat);
			text2 = integerTextBox.MaskedText;
			integerTextBox.CaretIndex = num + num3;
			if(integerTextBox.MaskedText.Contains(integerTextBox.NumberGroupSeparator))
			{
				int num5 = 0;
				string maskedText = integerTextBox.MaskedText;
				for(int j = 0; j < maskedText.Length; j++)
				{
					char c = maskedText[j];
					if(integerTextBox.NumberFormat != null)
					{
						if(c.ToString() == integerTextBox.NumberFormat.NumberGroupSeparator)
						{
							num5++;
						}
					}
					else
					{
						if(c.ToString() == integerTextBox.NumberGroupSeparator || c == ',')
						{
							num5++;
						}
						else
						{
							if(c.ToString() == integerTextBox.NumberGroupSeparator || c == '\u00a0')
							{
								num5++;
							}
						}
					}
				}
				integerTextBox.CaretIndex += num5;
			}
			int num6 = -1;
			int k = 0;
			for(k = 0; k < text2.Length; k++)
			{
				if(char.IsDigit(text2[k]))
				{
					num6++;
				}
				if(num6 == num)
				{
					break;
				}
			}
			if(text[0] == '-')
			{
				if(text.Length == num)
				{
					integerTextBox.SelectionStart = k;
				}
				else
				{
					integerTextBox.SelectionStart = k - 1;
				}
			}
			else
			{
				integerTextBox.SelectionStart = k;
			}
			integerTextBox.SelectionLength = 0;
			if(!integerTextBox.OnValidating(new CancelEventArgs(false)) && integerTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag = true;
				flag = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(false, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(false, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(false, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(true, message, integerTextBox.ValidationValue));
					integerTextBox.OnValidated(EventArgs.Empty);
				}
				return false;
			}
			return true;
		}
		public bool HandleDeleteKey(IntegerTextBox integerTextBox)
		{
			if(integerTextBox.IsReadOnly)
			{
				return true;
			}
			NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
			int num = 0;
			int num2 = 0;
			string text = "";
			string text2 = integerTextBox.MaskedText;
			if(integerTextBox.SelectionLength <= 1 && integerTextBox.SelectionStart != text2.Length && !char.IsDigit(text2[integerTextBox.SelectionStart]))
			{
				if(text2[0] == '-')
				{
					long? mValue = integerTextBox.mValue;
					integerTextBox.Value = mValue.HasValue ? new long?(-1L * mValue.GetValueOrDefault()) : null;
					integerTextBox.SelectionLength = 0;
					long? value = integerTextBox.Value;
					if(value.GetValueOrDefault() == 0L && value.HasValue)
					{
						text2 = text2.Remove(0, 1);
						integerTextBox.MaskedText = text2;
					}
					return true;
				}
				integerTextBox.SelectionStart++;
				long? mValue2 = integerTextBox.mValue;
				integerTextBox.Value = mValue2.HasValue ? new long?(mValue2.GetValueOrDefault()) : null;
				integerTextBox.SelectionLength = 0;
				return true;
			}
			else
			{
				for(int i = 0; i <= text2.Length; i++)
				{
					if(i == integerTextBox.SelectionStart)
					{
						num = text.Length;
					}
					if(i == integerTextBox.SelectionStart + integerTextBox.SelectionLength)
					{
						num2 = text.Length;
					}
					if(i < text2.Length && (char.IsDigit(text2[i]) || text2[i] == '-'))
					{
						text += text2[i];
					}
				}
				int num3 = num2 - num;
				if(num3 != 0)
				{
					text = text.Remove(num, num3);
				}
				else
				{
					if(num == text.Length)
					{
						return true;
					}
					num3 = 1;
					text = text.Remove(num, num3);
				}
				if(text.Length == 0)
				{
					if(integerTextBox.UseNullOption)
					{
						integerTextBox.MaskedText = "";
						integerTextBox.SetValue(true, null);
					}
					else
					{
						if(text == "")
						{
							if((integerTextBox.MinValue.ToString() == "0" || integerTextBox.MinValue < 0L) && integerTextBox.MinValue != integerTextBox.MaxValue)
							{
								integerTextBox.MaskedText = "0";
								integerTextBox.Value = 0L;
							}
							else
							{
								integerTextBox.SetValue(true, integerTextBox.MinValue);
							}
						}
					}
					return true;
				}
				long num4;
				if(!long.TryParse(text, NumberStyles.Number, numberFormat, out num4))
				{
					if(text == "-")
					{
						if(integerTextBox.UseNullOption)
						{
							integerTextBox.SetValue(true, null);
						}
						else
						{
							integerTextBox.MaskedText = "0";
						}
					}
					return true;
				}
				if(integerTextBox.IsNegative)
				{
				}
				if(num4 > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					if(!integerTextBox.MaxValueOnExceedMaxDigit)
					{
						return true;
					}
					num4 = integerTextBox.MaxValue;
				}
				if(num4 < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					if(!integerTextBox.MinValueOnExceedMinDigit)
					{
						return true;
					}
					num4 = integerTextBox.MinValue;
				}
				integerTextBox.SetValue(false, num4);
				long? value2 = integerTextBox.Value;
				if(value2.GetValueOrDefault() == 0L && value2.HasValue)
				{
					if(integerTextBox.UseNullOption)
					{
						integerTextBox.Value = 0L;
						integerTextBox.MaskedText = "";
					}
					else
					{
						integerTextBox.MaskedText = text;
					}
				}
				integerTextBox.MaskedText = num4.ToString("N", numberFormat);
				text2 = integerTextBox.MaskedText;
				int num5 = -1;
				int j = 0;
				for(j = 0; j < text2.Length; j++)
				{
					if(char.IsDigit(text2[j]))
					{
						num5++;
					}
					if(num5 == num)
					{
						break;
					}
				}
				if(text.Length == num)
				{
					integerTextBox.SelectionStart = j;
				}
				else
				{
					if(text[0] == '-')
					{
						integerTextBox.SelectionStart = j - 1;
					}
					else
					{
						integerTextBox.SelectionStart = j;
					}
				}
				integerTextBox.SelectionLength = 0;
				if(!integerTextBox.OnValidating(new CancelEventArgs(false)) && integerTextBox.ValueValidation == StringValidation.OnKeyPress)
				{
					string message = "";
					bool flag = true;
					flag = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
					string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
					if(!flag)
					{
						if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
						{
							MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
							integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(false, message, integerTextBox.ValidationValue));
							integerTextBox.OnValidated(EventArgs.Empty);
							return true;
						}
						if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
						{
							integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(false, message, integerTextBox.ValidationValue));
							integerTextBox.OnValidated(EventArgs.Empty);
							return true;
						}
						if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
						{
							integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(false, message, integerTextBox.ValidationValue));
							integerTextBox.OnValidated(EventArgs.Empty);
						}
					}
					else
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(true, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
					}
					return true;
				}
				return true;
			}
		}
		public bool HandleDownKey(IntegerTextBox integerTextBox)
		{
			if(integerTextBox.IsReadOnly && !integerTextBox.IsScrollingOnCircle)
			{
				return true;
			}
			if(integerTextBox.IsReadOnly)
			{
				return false;
			}
			if(!integerTextBox.IsScrollingOnCircle)
			{
				return false;
			}
			if(integerTextBox.mValue.HasValue)
			{
				long? mValue = integerTextBox.mValue;
				long num = integerTextBox.ScrollInterval;
				long? num2 = mValue.HasValue ? new long?(mValue.GetValueOrDefault() - num) : null;
				long minValue = integerTextBox.MinValue;
				if(num2.GetValueOrDefault() < minValue && num2.HasValue && integerTextBox.MinValidation != MinValidation.OnLostFocus)
				{
					if(integerTextBox.ScrollInterval < 0)
					{
						bool? arg_D9_1 = true;
						long? mValue2 = integerTextBox.mValue;
						long num3 = integerTextBox.ScrollInterval;
						integerTextBox.SetValue(arg_D9_1, mValue2.HasValue ? new long?(mValue2.GetValueOrDefault() - num3) : null);
					}
					return true;
				}
				if(integerTextBox.IsNegative)
				{
					if(integerTextBox.MaxLength != 0)
					{
						long? mValue3 = integerTextBox.mValue;
						long num4 = integerTextBox.ScrollInterval;
						if((mValue3.HasValue ? new long?(mValue3.GetValueOrDefault() - num4) : null).ToString().Length > integerTextBox.MaxLength + 1)
						{
							goto IL_229;
						}
					}
					bool? arg_185_1 = true;
					long? mValue4 = integerTextBox.mValue;
					long num5 = integerTextBox.ScrollInterval;
					integerTextBox.SetValue(arg_185_1, mValue4.HasValue ? new long?(mValue4.GetValueOrDefault() - num5) : null);
				}
				else
				{
					if(integerTextBox.MaxLength != 0)
					{
						long? mValue5 = integerTextBox.mValue;
						long num6 = integerTextBox.ScrollInterval;
						if((mValue5.HasValue ? new long?(mValue5.GetValueOrDefault() - num6) : null).ToString().Length > integerTextBox.MaxLength)
						{
							goto IL_229;
						}
					}
					bool? arg_224_1 = true;
					long? mValue6 = integerTextBox.mValue;
					long num7 = integerTextBox.ScrollInterval;
					integerTextBox.SetValue(arg_224_1, mValue6.HasValue ? new long?(mValue6.GetValueOrDefault() - num7) : null);
				}
			}
			IL_229:
			if(!integerTextBox.OnValidating(new CancelEventArgs(false)) && integerTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag = true;
				flag = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
					integerTextBox.OnValidated(EventArgs.Empty);
				}
				return true;
			}
			return true;
		}
		public bool HandleUpKey(IntegerTextBox integerTextBox)
		{
			if(integerTextBox.IsReadOnly || !integerTextBox.IsScrollingOnCircle)
			{
				return true;
			}
			if(integerTextBox.mValue.HasValue)
			{
				long? mValue = integerTextBox.mValue;
				long? num = mValue.HasValue ? new long?(mValue.GetValueOrDefault() + 1L) : null;
				long maxValue = integerTextBox.MaxValue;
				if(num.GetValueOrDefault() > maxValue && num.HasValue && integerTextBox.MaxValidation != MaxValidation.OnLostFocus)
				{
					if(integerTextBox.ScrollInterval < 0)
					{
						bool? arg_BC_1 = true;
						long? mValue2 = integerTextBox.mValue;
						long num2 = integerTextBox.ScrollInterval;
						integerTextBox.SetValue(arg_BC_1, mValue2.HasValue ? new long?(mValue2.GetValueOrDefault() + num2) : null);
					}
					return true;
				}
				if(integerTextBox.IsNegative)
				{
					if(integerTextBox.MaxLength != 0)
					{
						long? mValue3 = integerTextBox.mValue;
						long num3 = integerTextBox.ScrollInterval;
						if((mValue3.HasValue ? new long?(mValue3.GetValueOrDefault() + num3) : null).ToString().Length > integerTextBox.MaxLength + 1)
						{
							goto IL_20C;
						}
					}
					bool? arg_168_1 = true;
					long? mValue4 = integerTextBox.mValue;
					long num4 = integerTextBox.ScrollInterval;
					integerTextBox.SetValue(arg_168_1, mValue4.HasValue ? new long?(mValue4.GetValueOrDefault() + num4) : null);
				}
				else
				{
					if(integerTextBox.MaxLength != 0)
					{
						long? mValue5 = integerTextBox.mValue;
						long num5 = integerTextBox.ScrollInterval;
						if((mValue5.HasValue ? new long?(mValue5.GetValueOrDefault() + num5) : null).ToString().Length > integerTextBox.MaxLength)
						{
							goto IL_20C;
						}
					}
					bool? arg_207_1 = true;
					long? mValue6 = integerTextBox.mValue;
					long num6 = integerTextBox.ScrollInterval;
					integerTextBox.SetValue(arg_207_1, mValue6.HasValue ? new long?(mValue6.GetValueOrDefault() + num6) : null);
				}
			}
			IL_20C:
			if(!integerTextBox.OnValidating(new CancelEventArgs(false)) && integerTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag = true;
				flag = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
						integerTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, integerTextBox.ValidationValue));
					integerTextBox.OnValidated(EventArgs.Empty);
				}
				return true;
			}
			return true;
		}
	}
}