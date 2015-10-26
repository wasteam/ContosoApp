using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace ContosoLtd.Layouts
{
    public sealed partial class ListLayout : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ListLayout), new PropertyMetadata(null, OnItemsSourcePropertyChange));

        public static readonly DependencyProperty HasLoadDataErrorsProperty =
            DependencyProperty.Register("HasLoadDataErrors", typeof(bool), typeof(ListLayout), new PropertyMetadata(false, OnHasLoadDataErrorsPropertyChange));

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(ListLayout), new PropertyMetadata(false, OnIsBusyPropertyChange));

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(ListLayout), new PropertyMetadata(null, OnItemClickCommandPropertyChange));

        public static readonly DependencyProperty ListLayoutTemplateProperty =
            DependencyProperty.Register("ListLayoutTemplate", typeof(LayoutProperties.ListLayoutTemplate), typeof(ListLayout), new PropertyMetadata(LayoutProperties.ListLayoutTemplate.VerticalCard));

        public ListLayout()
        {
            this.InitializeComponent();
        }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public bool HasLoadDataErrors
        {
            get { return (bool)GetValue(HasLoadDataErrorsProperty); }
            set { SetValue(HasLoadDataErrorsProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public LayoutProperties.ListLayoutTemplate ListLayoutTemplate
        {
            get { return (LayoutProperties.ListLayoutTemplate)GetValue(ListLayoutTemplateProperty); }
            set { SetValue(ListLayoutTemplateProperty, value); }
        }

        private static void OnItemsSourcePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ListLayout;
            self.ItemsSource = e.NewValue;
        }

        private static void OnHasLoadDataErrorsPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ListLayout;
            self.HasLoadDataErrors = (bool)e.NewValue;
        }


        private static void OnIsBusyPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ListLayout;
            self.IsBusy = (bool)e.NewValue;
        }

        private static void OnItemClickCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ListLayout;
            self.ItemClickCommand = e.NewValue as ICommand;
        }
    }
}
