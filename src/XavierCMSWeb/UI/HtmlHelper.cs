// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;


namespace XavierCMSWeb.UI
{

	/// <summary>
	/// Html helper methods for write to the browser correct styles and markup.
	/// </summary>
	public static class HtmlHelper
	{




		/// <summary>
		/// Strips all illegal characters from the specified title.
		/// </summary>
		public static string TitleRemoveIllegalCharacters(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			text = text.Replace(":", string.Empty);
			text = text.Replace("/", string.Empty);
			text = text.Replace("?", string.Empty);
			text = text.Replace("#", string.Empty);
			text = text.Replace("[", string.Empty);
			text = text.Replace("]", string.Empty);
			text = text.Replace("@", string.Empty);
			text = text.Replace(".", string.Empty);
			text = text.Replace(",", string.Empty);
			text = text.Replace("\"", string.Empty);
			text = text.Replace("&", string.Empty);
			text = text.Replace("'", string.Empty);
			text = text.Replace(" ", "-");
			text = RemoveDiacritics(text);
			text = RemoveExtraHyphen(text);

			return HttpUtility.UrlEncode(text).Replace("%", string.Empty);
		}

		private static string RemoveExtraHyphen(string text)
		{
			if (text.Contains("--"))
			{
				text = text.Replace("--", "-");
				return RemoveExtraHyphen(text);
			}

			return text;
		}

		private static String RemoveDiacritics(string text)
		{
			var normalized = text.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();

			for (var i = 0; i < normalized.Length; i++)
			{
				var c = normalized[i];
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(c);
				}
			}

			return sb.ToString();
		}

		private static readonly Regex STRIP_HTML = new Regex("<[^>]*>", RegexOptions.Compiled);
		/// <summary>
		/// Strips all HTML tags from the specified string.
		/// </summary>
		/// <param name="html">The string containing HTML</param>
		/// <returns>A string without HTML tags</returns>
		public static string StripHtml(string html)
		{
			if (string.IsNullOrEmpty(html))
			{
				return string.Empty;
			}
			return STRIP_HTML.Replace(html, string.Empty);
		}

		private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+", RegexOptions.Compiled);
		private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

		/// <summary>
		/// Removes the HTML whitespace.
		/// </summary>
		/// <param name="html">The HTML.</param>
		public static string RemoveHtmlWhitespace(string html)
		{
			if (string.IsNullOrEmpty(html))
			{
				return string.Empty;
			}
			html = REGEX_BETWEEN_TAGS.Replace(html, "> ");
			html = REGEX_LINE_BREAKS.Replace(html, string.Empty);

			return html.Trim();
		}


	}
}
