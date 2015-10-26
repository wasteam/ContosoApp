// ***********************************************************************
// <copyright file="TextPlainConverter.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common.Converters
{
    using System;
    using System.Net;
    using HtmlAgilityPack;
    using AppStudio.Common;
    using AppStudio.DataProviders.Core;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// this class converts an Html value and returns a vanilla string with its contents without html tags..
    /// </summary>
    public class TextPlainConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string plainText = string.Empty;

            if (value != null)
            {
                string text = value.ToString();
                if (text.Length > 0)
                {
                    plainText = Utility.DecodeHtml(text);
                    if (parameter != null)
                    {
                        int maxLength = 0;
                        int.TryParse(parameter.ToString(), out maxLength);
                        if (maxLength > 0)
                        {
                            plainText = Utility.Truncate(plainText, maxLength);
                        }
                    }
                }
            }

            return plainText;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        /// <exception cref="System.NotImplementedException"> This method is not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
