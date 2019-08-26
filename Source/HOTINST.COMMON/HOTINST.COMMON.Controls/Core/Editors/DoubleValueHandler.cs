/**
 * ==============================================================================
 *
 * ClassName: DoubleValueHandler
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:29:46
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
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HOTINST.COMMON.Controls.Controls.Editors;

namespace HOTINST.COMMON.Controls.Core.Editors
{
	internal class DoubleValueHandler
	{
		public static DoubleValueHandler DoubleValHandler = new DoubleValueHandler();

		internal string OldunmaskedText = "";
		internal bool CanUpdate;
		internal bool Allow;
		internal bool AllowSelectionStart;
		internal bool AllowChange;
		internal int Count;

		public bool MatchWithMask(DoubleTextBox doubleTextBox, string text)
		{
			if(doubleTextBox.IsReadOnly)
			{
				return true;
			}
			bool flag = false;
			doubleTextBox.negativeFlag = false;
			Count = 0;
			int caretIndex = doubleTextBox.CaretIndex;
			if(doubleTextBox.mValue.HasValue)
			{
				double? num = doubleTextBox.mValue;
				if((num.GetValueOrDefault() != 0.0 || !num.HasValue) && !Equals(doubleTextBox.mValue, double.NaN))
				{
					goto IL_167;
				}
			}
			if(text == "-" && (doubleTextBox.MinValidation == MinValidation.OnLostFocus || (doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.MinValue < 0.0)))
			{
				doubleTextBox.minusPressed = true;
				if(doubleTextBox.count % 2 == 0)
				{
					doubleTextBox.Foreground = doubleTextBox.PositiveForeground;
					doubleTextBox.IsNegative = false;
				}
				else
				{
					double? value = doubleTextBox.Value;
					if(value.GetValueOrDefault() != 0.0 || !value.HasValue)
					{
						doubleTextBox.Foreground = doubleTextBox.NegativeForeground;
						doubleTextBox.IsNegative = true;
					}
				}
				doubleTextBox.count++;
				doubleTextBox.MaskedText = "-";
				doubleTextBox.Value = new double?(0.0);
				doubleTextBox.CaretIndex = 1;
				return true;
			}
			if(doubleTextBox.minusPressed && text != ".")
			{
				doubleTextBox.minusPressed = false;
				flag = true;
			}
			IL_167:
			NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
			int num2 = 0;
			if(!string.IsNullOrEmpty(doubleTextBox.SelectedText))
			{
				num2 = doubleTextBox.SelectedText.Length;
			}
			if(text == "-" || text == "+")
			{
				if(doubleTextBox.mValue.HasValue)
				{
					double num3 = doubleTextBox.mValue.Value;
					if(text == "+")
					{
						double? value2 = doubleTextBox.Value;
						if(value2.GetValueOrDefault() < 1.0 && value2.HasValue)
						{
							num3 = doubleTextBox.mValue.Value * -1.0;
						}
						else
						{
							num3 = doubleTextBox.mValue.Value * 1.0;
						}
					}
					else
					{
						if((doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.MinValue < 0.0) || doubleTextBox.MinValidation == MinValidation.OnLostFocus)
						{
							num3 = doubleTextBox.mValue.Value * -1.0;
						}
					}
					if(num3 > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
					{
						if(!doubleTextBox.MaxValueOnExceedMaxDigit)
						{
							return true;
						}
						num3 = doubleTextBox.MaxValue;
					}
					if(num2 == doubleTextBox.MaskedText.Length)
					{
						num2 = 0;
						if(text == "-" && (doubleTextBox.MinValidation == MinValidation.OnLostFocus || (doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.MinValue < 0.0)))
						{
							doubleTextBox.minusPressed = true;
							if(doubleTextBox.UseNullOption)
							{
								doubleTextBox.SetValue(new bool?(false), null);
							}
							else
							{
								doubleTextBox.Value = new double?(0.0);
							}
							doubleTextBox.MaskedText = "-";
							doubleTextBox.CaretIndex = 1;
							return true;
						}
					}
					if(doubleTextBox.MinValidation == MinValidation.OnKeyPress)
					{
						if(num3 > doubleTextBox.MinValue)
						{
							if(doubleTextBox.MinValueOnExceedMinDigit)
							{
								int arg_35D_0 = num3.ToString().Length;
								double num4 = doubleTextBox.MinValue;
								if(arg_35D_0 > num4.ToString().Length)
								{
									num3 = doubleTextBox.MinValue;
									goto IL_3A4;
								}
							}
							if(num3.ToString().Length > doubleTextBox.MinValue.ToString().Length)
							{
								return true;
							}
							doubleTextBox.MaskedText = num3.ToString();
						}
						else
						{
							num3 = doubleTextBox.MinValue;
						}
					}
					IL_3A4:
					int selectionStart = doubleTextBox.SelectionStart;
					bool isNegative = doubleTextBox.IsNegative;
					doubleTextBox.MaskedText = num3.ToString("N", numberFormat);
					if(doubleTextBox.MaskedText.Contains("-"))
					{
						doubleTextBox.SelectionStart = selectionStart + 1;
					}
					else
					{
						if(isNegative)
						{
							doubleTextBox.SelectionStart = ((selectionStart > 0) ? (selectionStart - 1) : 0);
						}
					}
					doubleTextBox.SetValue(new bool?(false), new double?(num3));
				}
				return true;
			}
			int num5 = 0;
			int num6 = 0;
			if(!doubleTextBox.Value.HasValue)
			{
				doubleTextBox.MaskedText = "";
			}
			string maskedText = doubleTextBox.MaskedText;
			int num7 = maskedText.IndexOf(numberFormat.NumberDecimalSeparator);
			int num8 = num7 + numberFormat.NumberDecimalSeparator.Length;
			if(text == numberFormat.NumberDecimalSeparator || text == ".")
			{
				if(doubleTextBox.NumberDecimalDigits == 0)
				{
					if(doubleTextBox.MaximumNumberDecimalDigits > 0 || doubleTextBox.numberDecimalDigits > 0)
					{
						CanUpdate = true;
						doubleTextBox.NumberDecimalDigits++;
						CanUpdate = false;
						doubleTextBox.SelectionStart = num8 + maskedText.Length + numberFormat.NumberDecimalSeparator.Length;
					}
				}
				else
				{
					if(doubleTextBox.MinimumNumberDecimalDigits == 0 && doubleTextBox.NumberDecimalDigits > doubleTextBox.MinimumNumberDecimalDigits)
					{
						CanUpdate = true;
						doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits + 1;
						CanUpdate = false;
						doubleTextBox.SelectionStart = num8 + maskedText.Length + numberFormat.NumberDecimalSeparator.Length;
					}
					else
					{
						doubleTextBox.SelectionStart = num8;
					}
				}
				if((doubleTextBox.Text == "" || num2 == doubleTextBox.MaskedText.Length) && (text == "." || text == numberFormat.NumberDecimalSeparator))
				{
					doubleTextBox.MaskedText = "0" + numberFormat.NumberDecimalSeparator + "00";
					doubleTextBox.Value = new double?(Convert.ToDouble(doubleTextBox.MaskedText));
					doubleTextBox.Select(2, 0);
					if(num2 == doubleTextBox.MaskedText.Length)
					{
						num2 = 0;
					}
				}
				if(doubleTextBox.MaskedText == "-" && (doubleTextBox.minusPressed || flag))
				{
					doubleTextBox.MaskedText += doubleTextBox.Value.Value.ToString("N", numberFormat);
					doubleTextBox.CaretIndex = doubleTextBox.MaskedText.IndexOf((doubleTextBox.NumberDecimalSeparator == "") ? numberFormat.NumberDecimalSeparator : doubleTextBox.NumberDecimalSeparator) + 1;
				}
				return true;
			}
			if(text == numberFormat.NumberGroupSeparator)
			{
				if(doubleTextBox.SelectionStart < doubleTextBox.Text.Length)
				{
					char c = doubleTextBox.Text[doubleTextBox.SelectionStart];
					if(c.ToString() == numberFormat.NumberGroupSeparator.ToString())
					{
						doubleTextBox.SelectionStart++;
						return true;
					}
				}
				return true;
			}
			int num9 = doubleTextBox.SelectionStart;
			string text2 = "";
			for(int i = 0; i <= maskedText.Length; i++)
			{
				if(i == doubleTextBox.SelectionStart)
				{
					num5 = text2.Length;
					num9 = num5;
				}
				if(i == doubleTextBox.SelectionStart + doubleTextBox.SelectionLength)
				{
					num6 = text2.Length;
				}
				if(i == num8)
				{
					num8 = text2.Length;
				}
				if(i == num7)
				{
					num7 = text2.Length;
					text2 += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
				}
				if(i < maskedText.Length && char.IsDigit(maskedText[i]))
				{
					if(text2.Length == 0)
					{
						if(maskedText[i] != '0' || (maskedText[i] == '0' && i == 0))
						{
							text2 += maskedText[i];
						}
						else
						{
							if(maskedText.Contains("-") && (maskedText[i] != '0' || (maskedText[i] == '0' && i == 1)))
							{
								text2 += maskedText[i];
							}
						}
					}
					else
					{
						text2 += maskedText[i];
					}
				}
			}
			int num10 = num6 - num5;
			if(num7 < 0)
			{
				num7 = text2.Length;
				num8 = text2.Length;
			}
			if(text != string.Empty)
			{
				if(num5 <= num7 && num6 >= num8 && char.IsDigit(text[0]))
				{
					for(int j = num8; j < num6; j++)
					{
						if(j != text2.Length)
						{
							text2 = text2.Remove(j, 1);
							text2 = text2.Insert(j, "0");
						}
					}
					text2 = text2.Remove(num5, num7 - num5);
					num9 = num5;
					text2 = text2.Insert(num5, text);
					if(text2[0] != '0')
					{
						num9 += text.Length;
					}
				}
				else
				{
					if(num5 <= num7 && num6 < num8)
					{
						text2 = text2.Remove(num5, num10);
						if(num2 == 0 && text2.Length == 1)
						{
							double? value3 = doubleTextBox.Value;
							if(value3.GetValueOrDefault() == 0.0 && value3.HasValue)
							{
								text2 = text2.Remove(num5, 1);
							}
						}
						num9 = num5;
						if(text2.Length > 0 && text2[0] == '0')
						{
							if(num5 == 1)
							{
								text2 = text2.Insert(0, text);
								text2 = text2.Remove(num5, text.Length);
							}
							if(num5 == 0)
							{
								text2 = text2.Insert(0, text);
								num9 += text.Length;
							}
						}
						else
						{
							text2 = text2.Insert(num5, text);
							num9 += text.Length;
						}
					}
					else
					{
						if(num5 == num6)
						{
							bool flag2 = true;
							if(doubleTextBox.NumberDecimalDigits > 0)
							{
								int num11 = 0;
								for(int k = text2.Length - 1; k >= 0; k--)
								{
									if(numberFormat != null)
									{
										if(text2[k].ToString() == numberFormat.NumberDecimalSeparator)
										{
											break;
										}
										int length = text2.Length;
										if(length.ToString() == doubleTextBox.NumberDecimalSeparator)
										{
											break;
										}
										num11++;
									}
								}
								if((doubleTextBox.MaximumNumberDecimalDigits >= 0 && num11 == doubleTextBox.MaximumNumberDecimalDigits) || (doubleTextBox.numberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits < 0 && doubleTextBox.numberDecimalDigits == num11))
								{
									flag2 = true;
								}
								else
								{
									if(num5 != text2.Length)
									{
										flag2 = false;
									}
								}
							}
							if(num5 == text2.Length && flag2 && !string.IsNullOrEmpty(text2))
							{
								text2 = text2.Insert(text2.Length - 1, text[0].ToString());
								text2 = text2.Remove(text2.Length - 1, 1);
							}
							else
							{
								if(!doubleTextBox.IsExceedDecimalDigits && num5 < text2.Length && numberFormat != null && num5 > text2.IndexOf(numberFormat.NumberDecimalSeparator) && doubleTextBox.MaximumNumberDecimalDigits < 0)
								{
									text2 = text2.Insert(num5, text[0].ToString());
									string s = (doubleTextBox.NumberGroupSeparator == string.Empty) ? numberFormat.NumberGroupSeparator : doubleTextBox.NumberGroupSeparator;
									if(maskedText.Contains('-'))
									{
										text2 = text2.Remove(doubleTextBox.CaretIndex - doubleTextBox.MaskedText.Count((char p) => p.ToString() == s), 1);
									}
								}
								else
								{
									if(num5 != text2.Length)
									{
										text2 = text2.Insert(num5, text[0].ToString());
										if(doubleTextBox.IsExceedDecimalDigits && AllowChange)
										{
											for(int l = text2.Length - 1; l >= 0; l--)
											{
												if(numberFormat != null)
												{
													if(text2[l].ToString() == numberFormat.NumberDecimalSeparator || text2.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
													{
														break;
													}
													Count++;
												}
											}
											if(Count >= doubleTextBox.MinimumNumberDecimalDigits && Count < doubleTextBox.MaximumNumberDecimalDigits)
											{
												if(doubleTextBox.MaximumNumberDecimalDigits > 0)
												{
													if(doubleTextBox.MinimumNumberDecimalDigits > 0)
													{
														CanUpdate = true;
														doubleTextBox.NumberDecimalDigits = Count;
														AllowChange = true;
														CanUpdate = false;
													}
													else
													{
														if(Count <= doubleTextBox.numberDecimalDigits)
														{
															doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
														}
														else
														{
															if(Count <= doubleTextBox.MaximumNumberDecimalDigits)
															{
																CanUpdate = true;
																doubleTextBox.NumberDecimalDigits = Count;
																AllowChange = true;
																CanUpdate = false;
															}
														}
													}
												}
											}
											else
											{
												if(doubleTextBox.MaximumNumberDecimalDigits > 0)
												{
													CanUpdate = true;
													doubleTextBox.NumberDecimalDigits = doubleTextBox.MaximumNumberDecimalDigits;
													AllowChange = false;
													CanUpdate = false;
												}
											}
											numberFormat = doubleTextBox.GetCulture().NumberFormat;
										}
										double? value4 = doubleTextBox.Value;
										if(value4.GetValueOrDefault() > -1.0 && value4.HasValue)
										{
											double? value5 = doubleTextBox.Value;
											if(value5.GetValueOrDefault() < 1.0 && value5.HasValue)
											{
												num9 = num5 + 1;
											}
										}
									}
									else
									{
										if(!doubleTextBox.IsExceedDecimalDigits)
										{
											return true;
										}
										if(doubleTextBox.MaximumNumberDecimalDigits > 0 || doubleTextBox.numberDecimalDigits > doubleTextBox.MaximumNumberDecimalDigits)
										{
											if(text2.Length <= doubleTextBox.SelectionStart)
											{
												text2 = text2.Insert(text2.Length, "0");
												text2 = text2.Remove(text2.Length - 1);
												text2 = text2.Insert(text2.Length, text[0].ToString());
											}
											else
											{
												text2 = text2.Insert(doubleTextBox.SelectionStart - 1, "0");
												text2 = text2.Remove(doubleTextBox.SelectionStart - 1);
												text2 = text2.Insert(doubleTextBox.SelectionStart - 1, text[0].ToString());
											}
											double? num = doubleTextBox.Value;
											if(num.GetValueOrDefault() > -1.0 && num.HasValue)
											{
												num = doubleTextBox.Value;
												if(num.GetValueOrDefault() < 1.0 && num.HasValue)
												{
													num9 = num5 + 1;
												}
											}
											for(int m = text2.Length - 1; m >= 0; m--)
											{
												if(numberFormat != null)
												{
													char c = text2[m];
													if(c.ToString() == numberFormat.NumberDecimalSeparator)
													{
														break;
													}
													int length = text2.Length;
													if(length.ToString() == doubleTextBox.NumberDecimalSeparator)
													{
														break;
													}
													Count++;
												}
											}
											AllowChange = true;
											if(numberFormat != null)
											{
												if(numberFormat.NumberDecimalDigits != doubleTextBox.NumberDecimalDigits && doubleTextBox.NumberDecimalDigits == -1)
												{
													doubleTextBox.NumberDecimalDigits = numberFormat.NumberDecimalDigits;
													CanUpdate = true;
													doubleTextBox.NumberDecimalDigits = numberFormat.NumberDecimalDigits + 1;
													CanUpdate = false;
												}
												else
												{
													CanUpdate = true;
													if(doubleTextBox.NumberDecimalDigits < doubleTextBox.MaximumNumberDecimalDigits)
													{
														doubleTextBox.NumberDecimalDigits++;
													}
													else
													{
														if(Count <= doubleTextBox.numberDecimalDigits)
														{
															doubleTextBox.NumberDecimalDigits = Count;
														}
														else
														{
															if(text2.Length > 0)
															{
																text2 = text2.Remove(num5);
															}
														}
													}
													CanUpdate = false;
												}
												numberFormat = doubleTextBox.GetCulture().NumberFormat;
											}
										}
									}
								}
							}
						}
						else
						{
							if(char.IsDigit(text[0]))
							{
								int num12 = 0;
								for(int n = num5; n < num6; n++)
								{
									text2 = text2.Remove(n, 1);
									if(num12 < text.Length)
									{
										string arg_FBA_0 = text2;
										int arg_FBA_1 = n;
										char c = text[num12];
										text2 = arg_FBA_0.Insert(arg_FBA_1, c.ToString());
									}
									else
									{
										text2 = text2.Insert(n, "0");
									}
									num12++;
								}
								num9 = num5 + text.Length;
							}
							else
							{
								if(!doubleTextBox.IsExceedDecimalDigits)
								{
									doubleTextBox.negativeFlag = true;
									text2 = doubleTextBox.MaskedText;
								}
							}
						}
					}
				}
			}
			OldunmaskedText = text2;
			double num13;
			if(double.TryParse(text2, out num13))
			{
				if(doubleTextBox.MaskedText.Length >= 15 && doubleTextBox.MaxValidation == MaxValidation.OnLostFocus)
				{
					if((doubleTextBox.IsNegative || flag) && !doubleTextBox.negativeFlag)
					{
						text2 = "-" + text2;
					}
					try
					{
						decimal num14 = decimal.Parse(text2);
						int length2 = doubleTextBox.MaskedText.Length;
						num5 = doubleTextBox.SelectionStart;
						doubleTextBox.MaskedText = num14.ToString("N", numberFormat);
						doubleTextBox.SelectionStart = num5 + 1;
						int length3 = doubleTextBox.MaskedText.Length;
						int num15 = length3 - length2;
						if(num15 == 0)
						{
							char c = doubleTextBox.MaskedText[num5 - 1];
							if(c.ToString() == numberFormat.CurrencyDecimalSeparator)
							{
								doubleTextBox.SelectionStart = num5 + 1;
								goto IL_1116;
							}
						}
						if(num5 + 1 == length3)
						{
							doubleTextBox.SelectionStart = num5 + 1;
						}
						else
						{
							if(num15 == 1)
							{
								doubleTextBox.SelectionStart = num5 + 1;
							}
							else
							{
								if(num15 == 2)
								{
									doubleTextBox.SelectionStart = num5 + 2;
								}
							}
						}
						IL_1116:
						;
					}
					catch
					{
					}
					return true;
				}
				double? num;
				if(!doubleTextBox.IsNegative && !flag)
				{
					if(!maskedText.Contains("-"))
					{
						goto IL_11DA;
					}
					num = doubleTextBox.Value;
					if(num.GetValueOrDefault() != 0.0 || !num.HasValue)
					{
						goto IL_11DA;
					}
				}
				if(!(doubleTextBox.SelectedText == doubleTextBox.Text.ToString()) && !doubleTextBox.SelectedText.Contains("-"))
				{
					num13 *= -1.0;
				}
				else
				{
					num = doubleTextBox.Value;
					if(((num.GetValueOrDefault() == 0.0 && num.HasValue) || doubleTextBox.IsNull) && flag)
					{
						num13 *= -1.0;
					}
				}
				IL_11DA:
				if(num13 > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					if(!doubleTextBox.MaxValueOnExceedMaxDigit)
					{
						return true;
					}
					num13 = doubleTextBox.MaxValue;
				}
				if(num13 < doubleTextBox.MinValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					if(num13 <= doubleTextBox.MinValue && doubleTextBox.MinValue >= 0.0)
					{
						if(numberFormat != null)
						{
							if(doubleTextBox.UseNullOption)
							{
								text2 = num13.ToString("N", numberFormat);
							}
							int arg_1280_0 = text2.Length - (numberFormat.NumberDecimalDigits + 1);
							double num4 = doubleTextBox.MinValue;
							if(arg_1280_0 >= num4.ToString("N", numberFormat).Length)
							{
								num13 = doubleTextBox.MinValue;
							}
							else
							{
								int arg_12B9_0 = text2.Length - (numberFormat.NumberDecimalDigits + 1);
								num4 = doubleTextBox.MinValue;
								if(arg_12B9_0 <= num4.ToString("N", numberFormat).Length)
								{
									doubleTextBox.checktext += text;
									if(double.Parse(doubleTextBox.checktext) >= doubleTextBox.MinValue)
									{
										doubleTextBox.Value = new double?(double.Parse(doubleTextBox.checktext));
										num = doubleTextBox.Value;
										doubleTextBox.CaretIndex = num.ToString().Length;
										doubleTextBox.checktext = "";
									}
									return true;
								}
							}
						}
					}
					else
					{
						if(num13 > doubleTextBox.MinValue)
						{
							doubleTextBox.MaskedText = text2;
						}
						else
						{
							if(num13 >= doubleTextBox.MinValue)
							{
								double num4;
								if(doubleTextBox.MinValueOnExceedMinDigit)
								{
									int arg_1373_0 = text2.Length - (numberFormat.NumberDecimalDigits + 1);
									num4 = doubleTextBox.MinValue;
									if(arg_1373_0 > num4.ToString().Length)
									{
										num13 = doubleTextBox.MinValue;
										goto IL_140D;
									}
								}
								int arg_13A6_0 = text2.Length - (numberFormat.NumberDecimalDigits + 1);
								num4 = doubleTextBox.MinValue;
								if(arg_13A6_0 > num4.ToString().Length)
								{
									return true;
								}
								doubleTextBox.MaskedText = text2;
							}
							else
							{
								if(!doubleTextBox.MinValueOnExceedMinDigit)
								{
									return true;
								}
								num13 = doubleTextBox.MinValue;
							}
						}
					}
				}
				else
				{
					if(num13 >= doubleTextBox.MinValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.checktext != "" && double.Parse(doubleTextBox.checktext) == 0.0)
					{
						doubleTextBox.checktext = "";
					}
				}
				IL_140D:
				if(doubleTextBox.MaxLength != 0 && text2.Length > doubleTextBox.MaxLength && doubleTextBox.NumberDecimalDigits <= doubleTextBox.MaxLength)
				{
					int numberDecimalDigits = doubleTextBox.NumberDecimalDigits;
					if(numberDecimalDigits < 0 && doubleTextBox.MaxLength > 3)
					{
						num13 = double.Parse(text2.Remove(doubleTextBox.MaxLength - 3));
						num9++;
						doubleTextBox.CaretIndex = num9;
					}
					else
					{
						num13 = double.Parse(text2.Remove(doubleTextBox.MaxLength - 1 - numberDecimalDigits));
					}
					doubleTextBox.SetValue(new bool?(false), new double?(num13));
					doubleTextBox.MaskedText = num13.ToString("N", numberFormat);
					if(num9 == doubleTextBox.CaretIndex)
					{
						num9++;
					}
					doubleTextBox.CaretIndex = num9;
					return true;
				}
				if(doubleTextBox.checktext != "" && num13 >= doubleTextBox.MinValue)
				{
					num9++;
				}
				double num16 = double.Parse(doubleTextBox.checktext + num13.ToString());
				if(num16 <= doubleTextBox.MaxValue)
				{
					num13 = num16;
				}
				if(text2.Length > 1 && maskedText.Length - 1 == num5 && doubleTextBox.NumberDecimalDigits > 0 && !maskedText.Contains("-") && !maskedText.StartsWith("."))
				{
					string value6 = (doubleTextBox.GetCulture() != null && doubleTextBox.GetCulture().NumberFormat != null && string.IsNullOrEmpty(doubleTextBox.GetCulture().NumberFormat.NumberDecimalSeparator)) ? doubleTextBox.GetCulture().NumberFormat.NumberDecimalSeparator : ".";
					if(text2.Contains(value6))
					{
						double num4 = double.Parse(text2);
						int arg_15E3_0 = num4.ToString().Length - 1;
						num4 = double.Parse(text2);
						if(arg_15E3_0 - num4.ToString().IndexOf(value6) != doubleTextBox.NumberDecimalDigits)
						{
							num13 = double.Parse(text2.Remove(text2.Length - 1));
						}
					}
				}
				doubleTextBox.MaskedText = num13.ToString("N", numberFormat);
				num = doubleTextBox.Value;
				if(num.GetValueOrDefault() == 0.0 && num.HasValue && maskedText.Contains("-"))
				{
					if(num2 != maskedText.Length && !doubleTextBox.MaskedText.Contains("-"))
					{
						doubleTextBox.MaskedText = "-" + doubleTextBox.MaskedText;
					}
					if(text == numberFormat.NumberDecimalSeparator || text == doubleTextBox.NumberDecimalSeparator)
					{
						doubleTextBox.CaretIndex = doubleTextBox.MaskedText.IndexOf((doubleTextBox.NumberDecimalSeparator == "") ? numberFormat.NumberDecimalSeparator : doubleTextBox.NumberDecimalSeparator) + 1;
					}
					else
					{
						doubleTextBox.CaretIndex = caretIndex + text.Length;
					}
					return true;
				}
				maskedText = doubleTextBox.MaskedText;
				if(!string.IsNullOrEmpty(maskedText) && maskedText[0] != '0' && num5 != 0 && num10 != 0 && num5 + text.Length != num9)
				{
					num9--;
				}
				doubleTextBox.SetValue(new bool?(false), new double?(num13));
				int num17 = 0;
				int i = 0;
				while(i < maskedText.Length && i != num9 && num17 != maskedText.Length)
				{
					if(char.IsDigit(maskedText[num17]))
					{
						num17++;
					}
					else
					{
						int num18 = num17;
						while(num18 < maskedText.Length && !char.IsDigit(maskedText[num18]))
						{
							num17++;
							num18++;
						}
						i--;
					}
					i++;
				}
				if(maskedText.StartsWith("0") && !maskedText.Contains("-") && num8 < num9 && num9 != num17 && (doubleTextBox.NumberDecimalDigits == doubleTextBox.MaximumNumberDecimalDigits || doubleTextBox.NumberDecimalDigits >= num5) && num9 == num5 + 1)
				{
					num17 = num9;
				}
				doubleTextBox.SelectionStart = num17;
				doubleTextBox.SelectionLength = 0;
				if(!doubleTextBox.OnValidating(new CancelEventArgs(false)) && doubleTextBox.ValueValidation == StringValidation.OnKeyPress)
				{
					string message = "";
					bool flag3 = true;
					string arg_1841_0 = doubleTextBox.ValidationValue;
					num = doubleTextBox.Value;
					flag3 = (arg_1841_0 == num.ToString());
					string messageBoxText = flag3 ? "String validation succeeded" : "String validation failed";
					if(!flag3)
					{
						if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
						{
							MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
							doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag3, message, doubleTextBox.ValidationValue));
							doubleTextBox.OnValidated(EventArgs.Empty);
							return true;
						}
						if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
						{
							doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag3, message, doubleTextBox.ValidationValue));
							doubleTextBox.OnValidated(EventArgs.Empty);
							return true;
						}
						if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
						{
							doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag3, message, doubleTextBox.ValidationValue));
							doubleTextBox.OnValidated(EventArgs.Empty);
						}
					}
					else
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag3, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
					}
					return true;
				}
			}
			return true;
		}
		public double? ValueFromText(DoubleTextBox doubleTextBox, string maskedText)
		{
			double value;
			if(double.TryParse(maskedText, NumberStyles.Number, doubleTextBox.GetCulture().NumberFormat, out value))
			{
				return new double?(value);
			}
			if(doubleTextBox.UseNullOption)
			{
				return null;
			}
			double num = 0.0;
			if(num > doubleTextBox.MaxValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
			{
				num = doubleTextBox.MaxValue;
			}
			else
			{
				if(num < doubleTextBox.MinValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					num = doubleTextBox.MinValue;
				}
			}
			return new double?(num);
		}
		public bool HandleKeyDown(DoubleTextBox doubleTextBox, KeyEventArgs eventArgs)
		{
			if(eventArgs.Key == Key.Space)
			{
				return true;
			}
			if(eventArgs.Key == Key.Right || eventArgs.Key == Key.Left)
			{
				DoubleValHandler.AllowSelectionStart = false;
			}
			switch(eventArgs.Key)
			{
				case Key.Back:
					{
						doubleTextBox.count = 1;
						return HandleBackSpaceKey(doubleTextBox);
					}
				case Key.Up:
					{
						return HandleUpKey(doubleTextBox);
					}
				case Key.Down:
					{
						return HandleDownKey(doubleTextBox);
					}
				case Key.Delete:
					{
						doubleTextBox.count = 1;
						return HandleDeleteKey(doubleTextBox);
					}
			}
			return false;
		}
		public bool HandleBackSpaceKey(DoubleTextBox doubleTextBox)
		{
			if(doubleTextBox.IsReadOnly)
			{
				return true;
			}
			NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
			Count = 0;
			string maskedText = doubleTextBox.MaskedText;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = maskedText.IndexOf(numberFormat.NumberDecimalSeparator);
			int num5 = num4 + numberFormat.NumberDecimalSeparator.Length;
			int num6 = 0;
			int num7 = doubleTextBox.SelectionStart;
			string text = "";
			if(doubleTextBox.SelectionLength == 1)
			{
				if(!char.IsDigit(maskedText[doubleTextBox.SelectionStart]))
				{
					if(maskedText[doubleTextBox.SelectionStart] == '-')
					{
						double? value = doubleTextBox.Value;
						doubleTextBox.Value = (value.HasValue ? new double?(value.GetValueOrDefault() * -1.0) : null);
					}
					doubleTextBox.SelectionLength = 0;
					return true;
				}
			}
			else
			{
				if(doubleTextBox.SelectionLength == 0 && doubleTextBox.SelectionStart == 1 && doubleTextBox.SelectionStart != maskedText.Length - 2 && !char.IsDigit(maskedText[doubleTextBox.SelectionStart - 1]) && maskedText[0] == '-')
				{
					double? value2 = doubleTextBox.Value;
					doubleTextBox.Value = (value2.HasValue ? new double?(value2.GetValueOrDefault() * -1.0) : null);
					doubleTextBox.SelectionLength = 0;
					return true;
				}
			}
			if(doubleTextBox.SelectionLength == 0 && doubleTextBox.SelectionStart != 0)
			{
				string numberGroupSeparator;
				if(numberFormat == null)
				{
					numberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
				}
				else
				{
					numberGroupSeparator = numberFormat.NumberGroupSeparator;
				}
				if((doubleTextBox.NumberGroupSeparator == string.Empty && maskedText[doubleTextBox.SelectionStart - 1].ToString() == numberGroupSeparator) || maskedText[doubleTextBox.SelectionStart - 1].ToString() == numberGroupSeparator)
				{
					text = "";
					doubleTextBox.SelectionStart--;
					return true;
				}
			}
			for(int i = 0; i <= maskedText.Length; i++)
			{
				if(i == doubleTextBox.SelectionStart)
				{
					num = text.Length;
					num7 = num;
				}
				if(i == doubleTextBox.SelectionStart + doubleTextBox.SelectionLength)
				{
					num2 = text.Length;
				}
				if(i == num5)
				{
					num5 = text.Length;
				}
				if(i == num4)
				{
					num4 = text.Length;
					text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
				}
				if(i < maskedText.Length && char.IsDigit(maskedText[i]))
				{
					text += maskedText[i];
				}
			}
			num3 = num2 - num;
			if(num <= num4 && num2 >= num5 && text.Length > 0)
			{
				if(numberFormat != null && num3 < text.Length)
				{
					for(int j = 0; j < doubleTextBox.SelectionLength; j++)
					{
						if(text.Length > 0 && numberFormat != null && text != string.Empty && text.Length > num)
						{
							if(text[num].ToString() != numberFormat.NumberDecimalSeparator && text[num].ToString() != doubleTextBox.NumberDecimalSeparator)
							{
								text = text.Remove(num, 1);
							}
							else
							{
								num++;
							}
						}
					}
					if(doubleTextBox.IsExceedDecimalDigits)
					{
						for(int k = text.Length - 1; k >= 0; k--)
						{
							if(numberFormat != null)
							{
								if(text[k].ToString() == numberFormat.NumberDecimalSeparator || text.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
								{
									break;
								}
								Count++;
							}
						}
						if(doubleTextBox.MinimumNumberDecimalDigits >= 0)
						{
							if(Count >= doubleTextBox.MinimumNumberDecimalDigits)
							{
								CanUpdate = true;
								doubleTextBox.NumberDecimalDigits = Count;
								AllowChange = true;
								CanUpdate = false;
							}
							else
							{
								CanUpdate = true;
								doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
								CanUpdate = false;
								AllowChange = false;
							}
						}
						else
						{
							if(Count <= doubleTextBox.numberDecimalDigits)
							{
								doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
								AllowChange = false;
							}
							else
							{
								CanUpdate = true;
								doubleTextBox.NumberDecimalDigits = Count;
								AllowChange = true;
								CanUpdate = false;
							}
						}
						num7++;
						Allow = true;
					}
				}
				else
				{
					if(num3 == text.Length)
					{
						text = "";
						AllowChange = false;
					}
				}
			}
			else
			{
				if(num <= num4 && num2 < num5)
				{
					if(num3 == 0)
					{
						if(num == 0)
						{
							if(doubleTextBox.MaskedText.StartsWith("\t") && doubleTextBox != null && doubleTextBox.AcceptsTab)
							{
								for(int l = 0; l < text.Length; l++)
								{
									if(doubleTextBox.MaskedText[l] == '\t')
									{
										doubleTextBox.MaskedText = doubleTextBox.MaskedText.Remove(l, 1);
										break;
									}
								}
							}
							return true;
						}
						num3 = 1;
						if(doubleTextBox.MinValue == double.Parse(text))
						{
							if(doubleTextBox.UseNullOption && text.Length > 0)
							{
								text = text.Remove(num - 1, num3);
							}
							num7 = num - 1;
						}
						else
						{
							if(text.Length > 0 && !doubleTextBox.MaskedText.Contains('\t'))
							{
								text = text.Remove(num - 1, num3);
								num7 = num - 1;
								if(doubleTextBox.SelectionStart == 2)
								{
									num6 = 1;
								}
							}
							else
							{
								if(doubleTextBox.MaskedText.Contains('\t') && doubleTextBox.AcceptsTab && doubleTextBox.AcceptsTab)
								{
									int selectionStart = doubleTextBox.SelectionStart;
									doubleTextBox.MaskedText = doubleTextBox.MaskedText.Remove(doubleTextBox.SelectionStart - 1, 1);
									doubleTextBox.SelectionStart = selectionStart - 1;
								}
							}
						}
					}
					else
					{
						if(text.Length > 0)
						{
							if(maskedText[doubleTextBox.SelectionStart] == '-')
							{
								double? value3 = doubleTextBox.Value;
								doubleTextBox.Value = (value3.HasValue ? new double?(value3.GetValueOrDefault() * -1.0) : null);
							}
							text = text.Remove(num, num3);
							num7 = num;
							if(doubleTextBox.SelectionStart > 0 && doubleTextBox.Text[doubleTextBox.SelectionStart - 1] == '-')
							{
								num6 = 1;
							}
						}
					}
				}
				else
				{
					if(num4 < 0)
					{
						if(num >= 0)
						{
							if(num == 0 && num3 >= 1 && maskedText[doubleTextBox.SelectionStart] == '-')
							{
								double? value4 = doubleTextBox.Value;
								doubleTextBox.Value = (value4.HasValue ? new double?(value4.GetValueOrDefault() * -1.0) : null);
							}
							if(num3 >= 1 && text.Length > 0)
							{
								if(num3 == text.Length)
								{
									double num8 = doubleTextBox.MinValue;
									if(num8.ToString() != "" && doubleTextBox.MinValue > 0.0)
									{
										text = doubleTextBox.MinValue.ToString();
										goto IL_BA7;
									}
								}
								text = text.Remove(num, num3);
								num7 = num;
								if(doubleTextBox.SelectionStart == 1)
								{
									num6 = 1;
								}
							}
							else
							{
								if(num3 == 0 && num == text.Length && text.Length > 0)
								{
									if(double.Parse(text) == doubleTextBox.MinValue)
									{
										num7 = num - 1;
									}
									else
									{
										text = text.Remove(num - 1, 1);
										num7 = num;
									}
								}
								else
								{
									if(num3 == 0 && text.Length > 0 && num != text.Length && num != 0 && maskedText[doubleTextBox.SelectionStart - 1] != ',')
									{
										if(doubleTextBox.SelectionStart == 2)
										{
											text = text.Remove(num - 1, 1);
											num6 = 1;
										}
										else
										{
											text = text.Remove(num - 1, 1);
											num7 = num - 1;
										}
									}
									else
									{
										if(num3 == 0 && num != text.Length && num != 0 && maskedText[doubleTextBox.SelectionStart - 1] == ',')
										{
											text = "";
											doubleTextBox.SelectionStart--;
											return true;
										}
										if(num != 0 && text.Length > 0)
										{
											text = text.Remove(num, 1);
											num7 = num;
										}
									}
								}
							}
						}
					}
					else
					{
						if(num == num2 && text.Length > 0)
						{
							if(num == num5 || doubleTextBox.MaskedText.Contains('\t'))
							{
								if(doubleTextBox.MaskedText.Contains('\t') && doubleTextBox.AcceptsTab && doubleTextBox != null)
								{
									int selectionStart2 = doubleTextBox.SelectionStart;
									if(char.IsDigit(maskedText[doubleTextBox.SelectionStart - 1]))
									{
										doubleTextBox.MaskedText = doubleTextBox.MaskedText.Remove(doubleTextBox.SelectionStart - 1, 1);
									}
									doubleTextBox.SelectionStart = selectionStart2 - 1;
								}
								else
								{
									doubleTextBox.SelectionStart--;
								}
								return true;
							}
							text = text.Remove(num - 1, 1);
							for(int m = text.Length - 1; m >= 0; m--)
							{
								if(numberFormat != null && text != string.Empty)
								{
									if(text[m].ToString() == numberFormat.NumberDecimalSeparator || text.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
									{
										break;
									}
									Count++;
								}
							}
							if(doubleTextBox.IsExceedDecimalDigits)
							{
								if(Count >= doubleTextBox.MinimumNumberDecimalDigits)
								{
									if(doubleTextBox.MinimumNumberDecimalDigits >= 0)
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = Count;
										AllowChange = true;
										CanUpdate = false;
									}
									else
									{
										if(Count >= doubleTextBox.numberDecimalDigits)
										{
											CanUpdate = true;
											doubleTextBox.NumberDecimalDigits = Count;
											AllowChange = false;
											CanUpdate = false;
										}
									}
								}
								else
								{
									doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
									AllowChange = false;
								}
								Allow = true;
							}
							num7 = num - 1;
						}
						else
						{
							if(text.Length > 0)
							{
								if(!doubleTextBox.IsExceedDecimalDigits)
								{
									for(int n = 0; n < doubleTextBox.SelectionLength; n++)
									{
										text = text.Remove(num, 1);
									}
									num7--;
								}
								else
								{
									for(int num9 = 0; num9 < doubleTextBox.SelectionLength; num9++)
									{
										text = text.Remove(num, 1);
									}
									for(int num10 = text.Length - 1; num10 >= 0; num10--)
									{
										if(numberFormat != null && text != string.Empty)
										{
											if(text[num10].ToString() == numberFormat.NumberDecimalSeparator || text.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
											{
												break;
											}
											Count++;
										}
									}
									if(doubleTextBox.IsExceedDecimalDigits && doubleTextBox.MinimumNumberDecimalDigits >= 0)
									{
										if(Count >= doubleTextBox.MinimumNumberDecimalDigits)
										{
											CanUpdate = true;
											doubleTextBox.NumberDecimalDigits = Count;
											AllowChange = true;
											CanUpdate = false;
										}
										else
										{
											CanUpdate = true;
											doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
											CanUpdate = true;
											AllowChange = false;
										}
										Allow = true;
									}
									else
									{
										if(Count <= doubleTextBox.numberDecimalDigits)
										{
											doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
											AllowChange = false;
										}
										else
										{
											CanUpdate = true;
											doubleTextBox.NumberDecimalDigits = Count;
											AllowChange = true;
											CanUpdate = false;
										}
									}
								}
							}
						}
					}
				}
			}
			IL_BA7:
			double num11;
			if(doubleTextBox.IsExceedDecimalDigits && !Allow && doubleTextBox.MinimumNumberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits > 0 && double.TryParse(text, out num11) && num3 > 0 && numberFormat != null)
			{
				CanUpdate = true;
				doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
				AllowChange = false;
				CanUpdate = false;
			}
			OldunmaskedText = text;
			bool flag = false;
			double num12;
			if(double.TryParse(text, out num12))
			{
				if(doubleTextBox.MaskedText.Length >= 15 && doubleTextBox.MaxValidation == MaxValidation.OnLostFocus)
				{
					if(doubleTextBox.IsNegative && !doubleTextBox.negativeFlag)
					{
						text = "-" + text;
					}
					try
					{
						decimal num13 = decimal.Parse(text);
						int length = doubleTextBox.MaskedText.Length;
						num = doubleTextBox.SelectionStart;
						doubleTextBox.MaskedText = num13.ToString("N", numberFormat);
						int length2 = doubleTextBox.MaskedText.Length;
						if(length != 0)
						{
							int num14 = length - length2;
							if(num14 == 0 && doubleTextBox.MaskedText[num - 2].ToString() == numberFormat.CurrencyDecimalSeparator)
							{
								doubleTextBox.SelectionStart = num - 1;
							}
							else
							{
								if(num == length2)
								{
									doubleTextBox.SelectionStart = num - 1;
								}
								else
								{
									if(num14 == 1)
									{
										doubleTextBox.SelectionStart = num - 1;
									}
									else
									{
										if(num14 == 2)
										{
											doubleTextBox.SelectionStart = num - 2;
										}
									}
								}
							}
						}
					}
					catch
					{
					}
					return true;
				}
				if(num12 == 0.0)
				{
					if(doubleTextBox.IsExceedDecimalDigits && doubleTextBox.MinimumNumberDecimalDigits >= 0)
					{
						if(numberFormat != null)
						{
							CanUpdate = true;
							doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
							AllowChange = false;
							CanUpdate = false;
						}
					}
					else
					{
						if(doubleTextBox.NumberDecimalDigits < 0)
						{
							doubleTextBox.NumberDecimalDigits = numberFormat.NumberDecimalDigits;
						}
						else
						{
							doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
						}
					}
					if(doubleTextBox.UseNullOption)
					{
						doubleTextBox.SetValue(new bool?(true), null);
					}
					else
					{
						if(doubleTextBox.MinValue.ToString() == "0" || doubleTextBox.MinValue < 0.0)
						{
							if(doubleTextBox.SelectionStart != 0)
							{
								doubleTextBox.SelectionStart--;
							}
							doubleTextBox.SetValue(new bool?(true), new double?(0.0));
						}
						else
						{
							doubleTextBox.SetValue(new bool?(true), new double?(doubleTextBox.MinValue));
						}
					}
					return true;
				}
				numberFormat = doubleTextBox.GetCulture().NumberFormat;
				if(doubleTextBox.IsNegative)
				{
					num12 *= -1.0;
				}
				if(num12 > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					if(!doubleTextBox.MaxValueOnExceedMaxDigit)
					{
						return true;
					}
					num12 = doubleTextBox.MaxValue;
				}
				if(num12 <= doubleTextBox.MinValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					if(num12 < doubleTextBox.MinValue && doubleTextBox.MinValue >= 0.0)
					{
						if(numberFormat == null)
						{
							return true;
						}
						if(text.Length - (numberFormat.NumberDecimalDigits + 1) >= doubleTextBox.MinValue.ToString().Length)
						{
							if(!doubleTextBox.MinValueOnExceedMinDigit)
							{
								return true;
							}
							num12 = doubleTextBox.MinValue;
						}
						else
						{
							if(text.Length - (numberFormat.NumberDecimalDigits + 1) <= doubleTextBox.MinValue.ToString().Length)
							{
								if(doubleTextBox.MinValueOnExceedMinDigit)
								{
									num12 = doubleTextBox.MinValue;
								}
								else
								{
									if(num12 < doubleTextBox.MinValue)
									{
										return true;
									}
									doubleTextBox.MaskedText = text;
								}
							}
						}
					}
					else
					{
						if(num12 > doubleTextBox.MinValue)
						{
							doubleTextBox.MaskedText = text;
						}
						else
						{
							if(num12 >= doubleTextBox.MinValue)
							{
								if(doubleTextBox.MinValueOnExceedMinDigit && text.Length - (numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
								{
									num12 = doubleTextBox.MinValue;
								}
								else
								{
									if(text.Length - (numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
									{
										return true;
									}
									doubleTextBox.MaskedText = text;
								}
							}
							else
							{
								if(!doubleTextBox.MinValueOnExceedMinDigit)
								{
									return true;
								}
								num12 = doubleTextBox.MinValue;
							}
						}
					}
				}
				doubleTextBox.MaskedText = num12.ToString("N", numberFormat);
				maskedText = doubleTextBox.MaskedText;
				doubleTextBox.SetValue(new bool?(false), new double?(num12));
				if(num6 == 0)
				{
					int num15 = 0;
					int i = 0;
					while(i < text.Length && i != num7 && num15 != maskedText.Length)
					{
						if(char.IsDigit(maskedText[num15]))
						{
							num15++;
						}
						else
						{
							for(int num16 = num15; num16 < maskedText.Length; num16++)
							{
								if(num15 == maskedText.IndexOf(numberFormat.NumberDecimalSeparator))
								{
									flag = true;
								}
								if(char.IsDigit(maskedText[num16]))
								{
									break;
								}
								num15++;
							}
							if(!flag)
							{
								i--;
							}
							flag = false;
						}
						i++;
					}
					doubleTextBox.SelectionStart = num15;
				}
				else
				{
					doubleTextBox.SelectionStart = 1;
				}
				doubleTextBox.SelectionLength = 0;
				num6 = 0;
			}
			else
			{
				if(num12 == 0.0)
				{
					if(doubleTextBox.IsExceedDecimalDigits && doubleTextBox.MinimumNumberDecimalDigits >= 0)
					{
						if(numberFormat != null)
						{
							CanUpdate = true;
							doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
							CanUpdate = false;
							AllowChange = false;
						}
					}
					else
					{
						if(doubleTextBox.NumberDecimalDigits < 0)
						{
							doubleTextBox.NumberDecimalDigits = numberFormat.NumberDecimalDigits;
						}
						else
						{
							doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
						}
					}
					if(doubleTextBox.UseNullOption)
					{
						doubleTextBox.SetValue(new bool?(true), null);
					}
					else
					{
						double num8 = doubleTextBox.MinValue;
						if(num8.ToString() == "0" || doubleTextBox.MinValue < 0.0)
						{
							doubleTextBox.SetValue(new bool?(true), new double?(0.0));
							double? value = doubleTextBox.Value;
							num8 = value.Value;
							doubleTextBox.MaskedText = num8.ToString("N", numberFormat);
						}
						else
						{
							doubleTextBox.SetValue(new bool?(true), new double?(doubleTextBox.MinValue));
						}
					}
					return true;
				}
				numberFormat = doubleTextBox.GetCulture().NumberFormat;
				doubleTextBox.MaskedText = num12.ToString("N", numberFormat);
				maskedText = doubleTextBox.MaskedText;
				doubleTextBox.SetValue(new bool?(false), new double?(num12));
			}
			if(!doubleTextBox.OnValidating(new CancelEventArgs(false)) && doubleTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag2 = true;
				string arg_1238_0 = doubleTextBox.ValidationValue;
				double? value = doubleTextBox.Value;
				flag2 = (arg_1238_0 == value.ToString());
				string messageBoxText = flag2 ? "String validation succeeded" : "String validation failed";
				if(!flag2)
				{
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag2, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag2, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag2, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag2, message, doubleTextBox.ValidationValue));
					doubleTextBox.OnValidated(EventArgs.Empty);
				}
				return false;
			}
			return true;
		}
		public bool HandleDeleteKey(DoubleTextBox doubleTextBox)
		{
			if(doubleTextBox.IsReadOnly)
			{
				return true;
			}
			NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
			Count = 0;
			string maskedText = doubleTextBox.MaskedText;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = maskedText.IndexOf(numberFormat.NumberDecimalSeparator);
			int num5 = num4 + numberFormat.NumberDecimalSeparator.Length;
			int num6 = 0;
			int num7 = doubleTextBox.SelectionStart;
			string text = "";
			if(doubleTextBox.SelectionLength <= 1 && doubleTextBox.SelectionStart != maskedText.Length)
			{
				if(!char.IsDigit(maskedText[doubleTextBox.SelectionStart]) && maskedText[doubleTextBox.SelectionStart] == '-')
				{
					double? value = doubleTextBox.Value;
					if(value.GetValueOrDefault() >= 0.0 || !value.HasValue || doubleTextBox.MaxValue >= 0.0)
					{
						double? value2 = doubleTextBox.Value;
						doubleTextBox.Value = (value2.HasValue ? new double?(value2.GetValueOrDefault() * -1.0) : null);
						doubleTextBox.SelectionLength = 0;
						return true;
					}
				}
				if(doubleTextBox.NumberFormat != null && doubleTextBox.SelectionStart == maskedText.Length - (doubleTextBox.NumberFormat.NumberDecimalDigits + 2) && maskedText[doubleTextBox.SelectionStart] == '0' && doubleTextBox.SelectionStart == 0)
				{
					num6 = 1;
				}
				if(doubleTextBox.NumberFormat == null)
				{
					if(maskedText[doubleTextBox.SelectionStart] == '0' && doubleTextBox.SelectionStart == 0)
					{
						num6 = 1;
						text = "";
						doubleTextBox.SelectionStart++;
						return true;
					}
					if(doubleTextBox.SelectionStart == 1)
					{
						num6 = 1;
					}
				}
				string numberGroupSeparator;
				if(numberFormat == null)
				{
					numberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
				}
				else
				{
					numberGroupSeparator = numberFormat.NumberGroupSeparator;
				}
				if((doubleTextBox.NumberGroupSeparator == string.Empty && maskedText[doubleTextBox.SelectionStart].ToString() == numberGroupSeparator) || maskedText[doubleTextBox.SelectionStart].ToString() == numberGroupSeparator)
				{
					text = "";
					doubleTextBox.SelectionStart++;
					doubleTextBox.SelectionLength = 0;
					return true;
				}
			}
			for(int i = 0; i <= maskedText.Length; i++)
			{
				if(i == doubleTextBox.SelectionStart)
				{
					num = text.Length;
					num7 = num;
				}
				if(i == doubleTextBox.SelectionStart + doubleTextBox.SelectionLength)
				{
					num2 = text.Length;
				}
				if(i == num5)
				{
					num5 = text.Length;
				}
				if(i == num4)
				{
					num4 = text.Length;
					text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
				}
				if(i < maskedText.Length && char.IsDigit(maskedText[i]))
				{
					text += maskedText[i];
				}
			}
			num3 = num2 - num;
			if(num4 < 0)
			{
				num4 = text.Length;
				num5 = text.Length;
			}
			if(num <= num4 && num2 >= num5 && text.Length > 0)
			{
				if(numberFormat != null && num3 < text.Length)
				{
					for(int j = 0; j < doubleTextBox.SelectionLength; j++)
					{
						if(text.Length > 0 && numberFormat != null && text != string.Empty && text.Length != num)
						{
							if(text[num].ToString() != numberFormat.NumberDecimalSeparator && text[num].ToString() != doubleTextBox.NumberDecimalSeparator)
							{
								text = text.Remove(num, 1);
							}
							else
							{
								num++;
							}
						}
					}
					if(doubleTextBox.IsExceedDecimalDigits && text.Contains(numberFormat.NumberDecimalSeparator))
					{
						for(int k = text.Length - 1; k >= 0; k--)
						{
							if(numberFormat != null)
							{
								if(text[k].ToString() == numberFormat.NumberDecimalSeparator || text.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
								{
									break;
								}
								Count++;
							}
						}
						if(doubleTextBox.MinimumNumberDecimalDigits >= 0)
						{
							if(Count >= doubleTextBox.MinimumNumberDecimalDigits)
							{
								CanUpdate = true;
								doubleTextBox.NumberDecimalDigits = Count;
								AllowChange = true;
								CanUpdate = false;
							}
							else
							{
								doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
								AllowChange = false;
							}
						}
						else
						{
							if(Count <= doubleTextBox.numberDecimalDigits)
							{
								doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
								AllowChange = false;
							}
							else
							{
								CanUpdate = true;
								doubleTextBox.NumberDecimalDigits = Count;
								CanUpdate = true;
								AllowChange = false;
							}
						}
						Allow = true;
					}
				}
				else
				{
					if(num3 == text.Length)
					{
						text = "";
						AllowChange = false;
					}
				}
				if(num3 == text.Length)
				{
					double minValue = doubleTextBox.MinValue;
					if(minValue.ToString() != "" && doubleTextBox.MinValue > 0.0)
					{
						text = doubleTextBox.MinValue.ToString();
					}
				}
			}
			else
			{
				if(num <= num4 && num2 < num5 && text.Length > 0)
				{
					if(num3 == 0)
					{
						if(num == num4)
						{
							doubleTextBox.SelectionStart++;
							AllowSelectionStart = true;
							return true;
						}
						num3 = 1;
						if(doubleTextBox.MinValue == double.Parse(text))
						{
							if(doubleTextBox.UseNullOption && text.Length > 0)
							{
								text = text.Remove(num, num3);
							}
							num7 = num + 1;
						}
						else
						{
							text = text.Remove(num, num3);
							if(num6 == 1)
							{
								num7 = num + 1;
								num6 = 0;
							}
							Allow = true;
						}
						if(doubleTextBox.SelectionStart == 1)
						{
							num6 = 1;
						}
					}
					else
					{
						if(num == 0 && maskedText[doubleTextBox.SelectionStart] == '-')
						{
							double? value3 = doubleTextBox.Value;
							doubleTextBox.Value = (value3.HasValue ? new double?(value3.GetValueOrDefault() * -1.0) : null);
						}
						text = text.Remove(num, num3);
						num7 = num;
						if(doubleTextBox.SelectionStart == 1)
						{
							num6 = 1;
						}
					}
				}
				else
				{
					if(text.Length > 0)
					{
						if(num == num2)
						{
							if(num == text.Length)
							{
								return true;
							}
							if(numberFormat != null && text != string.Empty)
							{
								if(text.Length >= num && (text[num - 1].ToString() == numberFormat.NumberDecimalSeparator || text[num - 1].ToString() == doubleTextBox.NumberDecimalSeparator))
								{
									AllowSelectionStart = true;
								}
								else
								{
									AllowSelectionStart = false;
								}
								if(text[num].ToString() != numberFormat.NumberDecimalSeparator && text[num].ToString() != doubleTextBox.NumberDecimalSeparator)
								{
									if(num5 > num)
									{
										text = text.Remove(num, 1);
									}
									else
									{
										text = text.Remove(num, 1);
										if(doubleTextBox.MinimumNumberDecimalDigits == -1 && doubleTextBox.MaximumNumberDecimalDigits == -1)
										{
											text = text.Insert(num, "0");
										}
									}
								}
							}
							if(doubleTextBox.IsExceedDecimalDigits)
							{
								for(int l = text.Length - 1; l >= 0; l--)
								{
									if(numberFormat != null)
									{
										if(text[l].ToString() == numberFormat.NumberDecimalSeparator || text.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
										{
											break;
										}
										Count++;
									}
								}
								if(doubleTextBox.MinimumNumberDecimalDigits >= 0)
								{
									if(Count >= doubleTextBox.MinimumNumberDecimalDigits)
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = Count;
										AllowChange = true;
										CanUpdate = false;
									}
									else
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
										CanUpdate = false;
										AllowChange = false;
									}
								}
								else
								{
									if(Count <= doubleTextBox.numberDecimalDigits)
									{
										doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
										AllowChange = false;
									}
									else
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = Count;
										CanUpdate = true;
										AllowChange = false;
									}
								}
								Allow = true;
							}
							if(num2 == num && num5 <= num && doubleTextBox.MinimumNumberDecimalDigits == -1 && doubleTextBox.MaximumNumberDecimalDigits == -1)
							{
								num7 = num;
							}
							else
							{
								num7 = num - 1;
							}
						}
						else
						{
							if(!doubleTextBox.IsExceedDecimalDigits)
							{
								for(int m = 0; m < doubleTextBox.SelectionLength; m++)
								{
									if(text.Length > 0 && numberFormat != null && text != string.Empty && text[num].ToString() != numberFormat.NumberDecimalSeparator)
									{
										text = text.Remove(num, 1);
									}
								}
							}
							else
							{
								for(int n = 0; n < doubleTextBox.SelectionLength; n++)
								{
									if(text.Length > 0 && numberFormat != null && text[num].ToString() != numberFormat.NumberDecimalSeparator && text[num - 1].ToString() != doubleTextBox.NumberDecimalSeparator)
									{
										text = text.Remove(num, 1);
									}
								}
								for(int num8 = text.Length - 1; num8 >= 0; num8--)
								{
									if(numberFormat != null)
									{
										if(text[num8].ToString() == numberFormat.NumberDecimalSeparator || text.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
										{
											break;
										}
										Count++;
									}
								}
								if(doubleTextBox.MinimumNumberDecimalDigits >= 0)
								{
									if(Count >= doubleTextBox.MinimumNumberDecimalDigits)
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = Count;
										AllowChange = true;
										CanUpdate = false;
									}
									else
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
										CanUpdate = false;
										AllowChange = false;
									}
								}
								else
								{
									if(Count <= doubleTextBox.numberDecimalDigits)
									{
										doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
										AllowChange = false;
									}
									else
									{
										CanUpdate = true;
										doubleTextBox.NumberDecimalDigits = Count;
										AllowChange = true;
										CanUpdate = false;
									}
								}
								num7--;
								Allow = true;
							}
						}
					}
				}
			}
			double num9;
			if(doubleTextBox.IsExceedDecimalDigits && !Allow && doubleTextBox.MinimumNumberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits > 0 && double.TryParse(text, out num9) && num3 > 0 && numberFormat != null)
			{
				doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
				AllowChange = false;
			}
			OldunmaskedText = text;
			double num10;
			if(double.TryParse(text, out num10))
			{
				if(doubleTextBox.MaskedText.Length >= 15 && doubleTextBox.MaxValidation == MaxValidation.OnLostFocus)
				{
					if(doubleTextBox.IsNegative && !doubleTextBox.negativeFlag)
					{
						text = "-" + text;
					}
					try
					{
						decimal num11 = decimal.Parse(text);
						int length = doubleTextBox.MaskedText.Length;
						num = doubleTextBox.SelectionStart;
						doubleTextBox.MaskedText = num11.ToString("N", numberFormat);
						int length2 = doubleTextBox.MaskedText.Length;
						if(length != 0)
						{
							int num12 = length - length2;
							if(num12 == 0 && doubleTextBox.MaskedText[num - 1].ToString() == numberFormat.CurrencyDecimalSeparator)
							{
								doubleTextBox.SelectionStart = num;
							}
							else
							{
								if(num + 1 == length2)
								{
									doubleTextBox.SelectionStart = num + 1;
								}
								else
								{
									if(num12 == 1)
									{
										doubleTextBox.SelectionStart = num;
									}
									else
									{
										if(num12 == 2)
										{
											doubleTextBox.SelectionStart = num - 1;
										}
									}
								}
							}
						}
					}
					catch
					{
					}
					return true;
				}
				if(num10 == 0.0)
				{
					if(doubleTextBox.IsExceedDecimalDigits && doubleTextBox.MinimumNumberDecimalDigits >= 0)
					{
						if(numberFormat != null)
						{
							CanUpdate = true;
							doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
							CanUpdate = false;
						}
					}
					else
					{
						if(doubleTextBox.NumberDecimalDigits < 0)
						{
							doubleTextBox.NumberDecimalDigits = numberFormat.NumberDecimalDigits;
						}
						else
						{
							doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
						}
					}
					if(doubleTextBox.UseNullOption)
					{
						doubleTextBox.SetValue(new bool?(true), null);
					}
					else
					{
						if(num3 == 0)
						{
							doubleTextBox.SelectionStart++;
						}
						doubleTextBox.SetValue(new bool?(true), new double?(0.0));
					}
					return true;
				}
				if(doubleTextBox.IsNegative)
				{
					num10 *= -1.0;
				}
				numberFormat = doubleTextBox.GetCulture().NumberFormat;
				if(num10 > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					if(!doubleTextBox.MaxValueOnExceedMaxDigit)
					{
						return true;
					}
					num10 = doubleTextBox.MaxValue;
				}
				if(num10 < doubleTextBox.MinValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					if(num10 <= doubleTextBox.MinValue && doubleTextBox.MinValue >= 0.0)
					{
						if(numberFormat != null)
						{
							if(text.Length - (numberFormat.NumberDecimalDigits + 1) >= doubleTextBox.MinValue.ToString().Length)
							{
								if(!doubleTextBox.MinValueOnExceedMinDigit)
								{
									return true;
								}
								num10 = doubleTextBox.MinValue;
							}
							else
							{
								if(text.Length - (numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
								{
									return true;
								}
								if(doubleTextBox.MinValueOnExceedMinDigit)
								{
									num10 = doubleTextBox.MinValue;
								}
								else
								{
									if(num10 < doubleTextBox.MinValue)
									{
										return true;
									}
									doubleTextBox.MaskedText = text;
								}
							}
						}
					}
					else
					{
						if(num10 > doubleTextBox.MinValue)
						{
							doubleTextBox.MaskedText = text;
						}
						else
						{
							if(num10 >= doubleTextBox.MinValue)
							{
								if(doubleTextBox.MinValueOnExceedMinDigit && text.Length - (numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
								{
									num10 = doubleTextBox.MinValue;
								}
								else
								{
									if(text.Length - (numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
									{
										return true;
									}
									doubleTextBox.MaskedText = text;
								}
							}
							else
							{
								if(!doubleTextBox.MinValueOnExceedMinDigit)
								{
									return true;
								}
								num10 = doubleTextBox.MinValue;
							}
						}
					}
				}
				doubleTextBox.MaskedText = num10.ToString("N", numberFormat);
				maskedText = doubleTextBox.MaskedText;
				doubleTextBox.SetValue(new bool?(false), new double?(num10));
				if(num6 == 0)
				{
					int num13 = 0;
					int i = 0;
					while(i < text.Length && i != num7 && num13 != maskedText.Length)
					{
						if(char.IsDigit(maskedText[num13]))
						{
							num13++;
						}
						else
						{
							int num14 = num13;
							while(num14 < maskedText.Length && !char.IsDigit(maskedText[num14]))
							{
								num13++;
								num14++;
							}
							i--;
						}
						i++;
					}
					if(!AllowSelectionStart)
					{
						doubleTextBox.SelectionStart = num13;
						num = num13;
					}
					else
					{
						if(num == num5 && num5 <= num && doubleTextBox.MinimumNumberDecimalDigits == -1 && doubleTextBox.MaximumNumberDecimalDigits == -1)
						{
							doubleTextBox.SelectionStart = num13;
						}
						else
						{
							if(num13 > 0)
							{
								doubleTextBox.SelectionStart = num13 + 1;
							}
						}
					}
				}
				else
				{
					double? value4 = doubleTextBox.Value;
					if(value4.GetValueOrDefault() < 0.0 && value4.HasValue)
					{
						doubleTextBox.SelectionStart++;
						num++;
					}
					else
					{
						doubleTextBox.SelectionStart = 1;
						num = 1;
					}
				}
				doubleTextBox.SelectionLength = 0;
				num6 = 0;
			}
			else
			{
				if(num10 == 0.0)
				{
					if(doubleTextBox.IsExceedDecimalDigits && doubleTextBox.MinimumNumberDecimalDigits >= 0)
					{
						if(numberFormat != null)
						{
							CanUpdate = true;
							doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
							AllowChange = false;
							CanUpdate = false;
						}
					}
					else
					{
						if(doubleTextBox.NumberDecimalDigits < 0)
						{
							doubleTextBox.NumberDecimalDigits = numberFormat.NumberDecimalDigits;
						}
						else
						{
							doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
						}
					}
					numberFormat = doubleTextBox.GetCulture().NumberFormat;
					if(doubleTextBox.UseNullOption)
					{
						doubleTextBox.SetValue(new bool?(true), null);
					}
					else
					{
						double minValue = doubleTextBox.MinValue;
						if(minValue.ToString() == "0" || doubleTextBox.MinValue < 0.0)
						{
							doubleTextBox.SetValue(new bool?(true), new double?(0.0));
						}
						else
						{
							doubleTextBox.SetValue(new bool?(true), new double?(doubleTextBox.MinValue));
						}
					}
					return true;
				}
				doubleTextBox.MaskedText = num10.ToString("N", numberFormat);
				maskedText = doubleTextBox.MaskedText;
				doubleTextBox.SetValue(new bool?(false), new double?(num10));
			}
			if(!doubleTextBox.OnValidating(new CancelEventArgs(false)) && doubleTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag = true;
				string arg_10F3_0 = doubleTextBox.ValidationValue;
				double? value = doubleTextBox.Value;
				flag = (arg_10F3_0 == value.ToString());
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
					doubleTextBox.OnValidated(EventArgs.Empty);
				}
				return true;
			}
			return true;
		}
		public bool HandleDownKey(DoubleTextBox doubleTextBox)
		{
			if(doubleTextBox.IsReadOnly)
			{
				return true;
			}
			if(doubleTextBox.mValue.HasValue)
			{
				double? mValue = doubleTextBox.mValue;
				double scrollInterval = doubleTextBox.ScrollInterval;
				double? num = mValue.HasValue ? new double?(mValue.GetValueOrDefault() - scrollInterval) : null;
				double minValue = doubleTextBox.MinValue;
				if(num.GetValueOrDefault() < minValue && num.HasValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					return true;
				}
				bool? arg_B6_1 = new bool?(true);
				double? mValue2 = doubleTextBox.mValue;
				double scrollInterval2 = doubleTextBox.ScrollInterval;
				doubleTextBox.SetValue(arg_B6_1, mValue2.HasValue ? new double?(mValue2.GetValueOrDefault() - scrollInterval2) : null);
			}
			if(!doubleTextBox.OnValidating(new CancelEventArgs(false)) && doubleTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag = true;
				flag = (doubleTextBox.ValidationValue == doubleTextBox.Value.ToString());
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
					doubleTextBox.OnValidated(EventArgs.Empty);
				}
				return true;
			}
			return true;
		}
		public bool HandleUpKey(DoubleTextBox doubleTextBox)
		{
			if(doubleTextBox.IsReadOnly)
			{
				return true;
			}
			if(doubleTextBox.mValue.HasValue)
			{
				double? mValue = doubleTextBox.mValue;
				double scrollInterval = doubleTextBox.ScrollInterval;
				double? num = mValue.HasValue ? new double?(mValue.GetValueOrDefault() + scrollInterval) : null;
				double maxValue = doubleTextBox.MaxValue;
				if(num.GetValueOrDefault() > maxValue && num.HasValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
				{
					return true;
				}
				double? mValue2 = doubleTextBox.mValue;
				double scrollInterval2 = doubleTextBox.ScrollInterval;
				double? num2 = mValue2.HasValue ? new double?(mValue2.GetValueOrDefault() + scrollInterval2) : null;
				double minValue = doubleTextBox.MinValue;
				if(num2.GetValueOrDefault() < minValue && num2.HasValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
				{
					return true;
				}
				bool? arg_117_1 = new bool?(true);
				double? mValue3 = doubleTextBox.mValue;
				double scrollInterval3 = doubleTextBox.ScrollInterval;
				doubleTextBox.SetValue(arg_117_1, mValue3.HasValue ? new double?(mValue3.GetValueOrDefault() + scrollInterval3) : null);
			}
			if(!doubleTextBox.OnValidating(new CancelEventArgs(false)) && doubleTextBox.ValueValidation == StringValidation.OnKeyPress)
			{
				string message = "";
				bool flag = true;
				flag = (doubleTextBox.ValidationValue == doubleTextBox.Value.ToString());
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
						return true;
					}
					if(doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
					{
						doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
						doubleTextBox.OnValidated(EventArgs.Empty);
					}
				}
				else
				{
					doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, doubleTextBox.ValidationValue));
					doubleTextBox.OnValidated(EventArgs.Empty);
				}
				return true;
			}
			return true;
		}
	}
}