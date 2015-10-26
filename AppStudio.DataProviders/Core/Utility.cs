// ***********************************************************************
// <copyright file="Utility.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.DataProviders.Core
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;

    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Checks if two strings are equal ignoring the case.
        /// </summary>
        /// <param name="value">The first string.</param>
        /// <param name="content">The second string.</param>
        /// <returns><c>true</c> if strings are equal, <c>false</c> otherwise.</returns>
        public static bool EqualNoCase(this string value, string content)
        {
            if (value != null)
            {
                return value.Equals(content, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="str">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="ellipsis">if set to <c>true</c> add a text ellipsis.</param>
        /// <returns>System.String.</returns>
        public static string Truncate(this String str, int length, bool ellipsis = false)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                if (str.Length > length)
                {
                    if (ellipsis)
                    {
                        return str.Substring(0, length) + "...";
                    }
                    else
                    {
                        return str.Substring(0, length);
                    }
                }
            }
            return str ?? string.Empty;
        }

        /// <summary>
        /// Decodes the HTML.
        /// </summary>
        /// <param name="htmlText">The html text.</param>
        /// <returns>String without any html tags.</returns>
        public static string DecodeHtml(string htmlText)
        {
            htmlText = htmlText.Replace("<p>", "").Replace("</p>", "\r\n\r\n");

            string decoded = String.Empty;
            if (htmlText.IndexOf('<') > -1 || htmlText.IndexOf('>') > -1 || htmlText.IndexOf('&') > -1)
            {
                try
                {
                    HtmlDocument document = new HtmlDocument();

                    var decode = document.CreateElement("div");
                    htmlText = htmlText.Replace(".<", ". <").Replace("?<", "? <").Replace("!<", "! <").Replace("&#039;", "'");
                    decode.InnerHtml = htmlText;

                    decoded = WebUtility.HtmlDecode(decode.InnerText);
                    decoded = Regex.Replace(decoded, "<!--.*?-->", string.Empty, RegexOptions.Singleline);
                }
                catch { }
            }
            else
            {
                decoded = htmlText;
            }
            return decoded;
        }

        public static string ToSafeString(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return obj.ToString();
        }
    }
}
