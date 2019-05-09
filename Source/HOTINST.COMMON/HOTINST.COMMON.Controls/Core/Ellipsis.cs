using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Control = System.Windows.Controls.Control;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// Specifies ellipsis format and alignment.
	/// </summary>
	[Flags]
	public enum EllipsisFormat
	{
		/// <summary>
		/// Text is not modified.
		/// </summary>
		None = 0,
		/// <summary>
		/// Text is trimmed at the end of the string. An ellipsis (...) is drawn in place of remaining text.
		/// </summary>
		End = 1,
		/// <summary>
		/// Text is trimmed at the begining of the string. An ellipsis (...) is drawn in place of remaining text. 
		/// </summary>
		Start = 2,
		/// <summary>
		/// Text is trimmed in the middle of the string. An ellipsis (...) is drawn in place of remaining text.
		/// </summary>
		Middle = 3,
		/// <summary>
		/// Preserve as much as possible of the drive and filename information. Must be combined with alignment information.
		/// </summary>
		Path = 4,
		/// <summary>
		/// Text is trimmed at a word boundary. Must be combined with alignment information.
		/// </summary>
		Word = 8,
		/// <summary>
		/// Middle | Path
		/// </summary>
		MiddleAndPath = 7
	}

	/// <summary>
	/// 
	/// </summary>
	public class Ellipsis
	{
		/// <summary>
		/// String used as a place holder for trimmed text.
		/// </summary>
		public static readonly string EllipsisChars = "...";

		private static readonly Regex PrevWord = new Regex(@"\W*\w*$");
		private static readonly Regex NextWord = new Regex(@"\w*\W*");

		/// <summary>
		/// Truncates a text string to fit within a given control width by replacing trimmed text with ellipses. 
		/// </summary>
		/// <param name="text">String to be trimmed.</param>
		/// <param name="ctrl">text must fit within ctrl width.
		///	The ctrl's Font is used to measure the text string.</param>
		/// <param name="options">Format and alignment of ellipsis.</param>
		/// <returns>This function returns text trimmed to the specified witdh.</returns>
		public static string Compact(string text, Control ctrl, EllipsisFormat options)
		{
			if(string.IsNullOrEmpty(text))
				return text;

			// no aligment information
			if((EllipsisFormat.Middle & options) == 0)
				return text;

			if(ctrl == null)
				throw new ArgumentNullException(nameof(ctrl));

			using (Graphics dc = Graphics.FromImage(new Bitmap(1, 1)))
			{
				Font f = new Font(ctrl.FontFamily.FamilyNames.ToString(), (float)ctrl.FontSize, FontStyle.Regular);
				Size s = TextRenderer.MeasureText(dc, text, f, new Size(0, 0), TextFormatFlags.Left | TextFormatFlags.NoPadding);

				// control is large enough to display the whole text
				if (s.Width <= ctrl.ActualWidth)
					return text;

				string pre = "";
				string mid = text;
				string post = "";

				bool isPath = (EllipsisFormat.Path & options) != 0;

				// split path string into <drive><directory><filename>
				if (isPath)
				{
					pre = Path.GetPathRoot(text);
					mid = Path.GetDirectoryName(text).Substring(pre.Length);
					post = Path.GetFileName(text);
				}

				int len = 0;
				int seg = mid.Length;
				string fit = "";

				// find the longest string that fits into 
				// the control boundaries using bisection method
				while (seg > 1)
				{
					seg -= seg / 2;

					int left = len + seg;
					int right = mid.Length;

					if (left > right)
						continue;

					if ((EllipsisFormat.Middle & options) == EllipsisFormat.Middle)
					{
						right -= left / 2;
						left -= left / 2;
					}
					else if ((EllipsisFormat.Start & options) != 0)
					{
						right -= left;
						left = 0;
					}

					// trim at a word boundary using regular expressions
					if ((EllipsisFormat.Word & options) != 0)
					{
						if ((EllipsisFormat.End & options) != 0)
						{
							left -= PrevWord.Match(mid, 0, left).Length;
						}
						if ((EllipsisFormat.Start & options) != 0)
						{
							right += NextWord.Match(mid, right).Length;
						}
					}

					// build and measure a candidate string with ellipsis
					string tst = mid.Substring(0, left) + EllipsisChars + mid.Substring(right);

					// restore path with <drive> and <filename>
					if (isPath)
					{
						tst = Path.Combine(Path.Combine(pre, tst), post);
					}
					s = TextRenderer.MeasureText(dc, tst, f, new Size(0, 0), TextFormatFlags.Left | TextFormatFlags.NoPadding);
					
					// candidate string fits into control boundaries, try a longer string
					// stop when seg <= 1
					if (s.Width <= ctrl.ActualWidth)
					{
						len += seg;
						fit = tst;
					}
				}

				if (len == 0) // string can't fit into control
				{
					// "path" mode is off, just return ellipsis characters
					if (!isPath)
						return EllipsisChars;

					// <drive> and <directory> are empty, return <filename>
					if (pre.Length == 0 && mid.Length == 0)
						return post;

					// measure "C:\...\filename.ext"
					fit = Path.Combine(Path.Combine(pre, EllipsisChars), post);

					s = TextRenderer.MeasureText(dc, fit, f, new Size(0, 0), TextFormatFlags.Left | TextFormatFlags.NoPadding);

					// if still not fit then return "...\filename.ext"
					if (s.Width > ctrl.ActualWidth)
						fit = Path.Combine(EllipsisChars, post);
				}
				return fit;
			}
		}

		///// <summary>
		///// MeasureText always adds about 1/2 em width of white space on the right,
		///// even when NoPadding is specified. It returns zero for an empty string.
		///// To get the precise string width, measure the width of a string containing a
		///// single period and subtract that from the width of our original string plus a period.
		///// </summary>
		///// <param name="text"></param>
		///// <param name="font"></param>
		///// <returns></returns>
		//public static Size MeasureText(string text, Font font)
		//{
		//	TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.TextBoxControl;
		//	Size szProposed = new Size(0, 0);
		//	Size sz1 = TextRenderer.MeasureText(".", font, szProposed, flags);
		//	Size sz2 = TextRenderer.MeasureText(text + ".", font, szProposed, flags);
		//	return new Size(sz2.Width - sz1.Width, sz2.Height);
		//}
	}
}