using System;
using Windows.Storage;
using AppStudio.Common.Commands;

namespace AppStudio.Common.Fonts
{
    public class FontSettings
    {
        static FontSettings()
        {
            Current = new FontSizeSettings();
        }

        public static RelayCommand<string> ChangeFontSizeCommand
        {
            get
            {
                return new RelayCommand<string>((s) =>
                {
                    FontSizes fontSize;
                    Enum.TryParse<FontSizes>(s, out fontSize);
                    Current.FontSize = fontSize;
                }); ;
            }
        }

        public static FontSizeSettings Current { get; private set; }
    }

    public class FontSizeSettings : ObservableBase
    {
        public FontSizes FontSize
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LocalSettingNames.TextViewerFontSizeSetting))
                {
                    FontSizes fontSizes;
                    Enum.TryParse<FontSizes>(ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSizeSetting].ToString(), out fontSizes);
                    return fontSizes;
                }
                return FontSizes.Normal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSizeSetting] = value.ToString();
                this.OnPropertyChanged("FontSize");
            }
        }
    }
}
