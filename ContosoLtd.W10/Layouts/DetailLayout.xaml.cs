using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ContosoLtd.Layouts
{
    public sealed partial class DetailLayout : UserControl
    {
        public static readonly DependencyProperty DetailLayoutTemplateProperty =
            DependencyProperty.Register("DetailLayoutTemplate", typeof(LayoutProperties.DetailLayoutTemplate), typeof(DetailLayout), new PropertyMetadata(LayoutProperties.DetailLayoutTemplate.Text));

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(DetailLayout), new PropertyMetadata(null, OnViewModelPropertyChanged));

        public static readonly DependencyProperty MaxWProperty =
            DependencyProperty.Register("MaxW", typeof(double), typeof(DetailLayout), new PropertyMetadata(0D));

        public static readonly DependencyProperty MaxHProperty =
            DependencyProperty.Register("MaxH", typeof(double), typeof(DetailLayout), new PropertyMetadata(0D));

        public LayoutProperties.DetailLayoutTemplate DetailLayoutTemplate
        {
            get { return (LayoutProperties.DetailLayoutTemplate)GetValue(DetailLayoutTemplateProperty); }
            set { SetValue(DetailLayoutTemplateProperty, value); }
        }

        public DetailLayout()
        {
            this.InitializeComponent();
        }

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public double MaxW
        {
            get { return (double)GetValue(MaxWProperty); }
            set { SetValue(MaxWProperty, value); }
        }

        public double MaxH
        {
            get { return (double)GetValue(MaxHProperty); }
            set { SetValue(MaxHProperty, value); }
        }

        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DetailLayout;
            self.ViewModel = e.NewValue;
        }

        private void WebView_Unloaded(object sender, RoutedEventArgs e)
        {
            WebView webView = sender as WebView;
            if (webView != null) webView = null;
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MaxH = e.NewSize.Height;
            MaxW = e.NewSize.Width;
        }
    }
}
