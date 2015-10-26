using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AppStudio.Common.Services;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ContosoLtd.Layouts.Controls
{
    public sealed partial class DataUpdateInformationControl : UserControl
    {
        public static readonly DependencyProperty LastUpdateDateTimeProperty =
            DependencyProperty.Register("LastUpdateDateTime", typeof(string), typeof(DataUpdateInformationControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(DataUpdateInformationControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty HasLocalDataProperty =
            DependencyProperty.Register("HasLocalData", typeof(bool), typeof(DataUpdateInformationControl), new PropertyMetadata(false));

        public DataUpdateInformationControl()
        {
            this.InitializeComponent();
        }

        public string LastUpdateDateTime
        {
            get { return (string)GetValue(LastUpdateDateTimeProperty); }
            set { SetValue(LastUpdateDateTimeProperty, value); }
        }

        public bool HasLocalData
        {
            get { return (bool)GetValue(HasLocalDataProperty); }
            set { SetValue(HasLocalDataProperty, value); }
        }

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public bool IsNetworkAvailable
        {
            get
            {
                return InternetConnection.IsInternetAvailable();
            }
        }
    }
}
