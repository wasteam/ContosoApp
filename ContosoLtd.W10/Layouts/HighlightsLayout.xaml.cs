using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ContosoLtd.Layouts
{
    public sealed partial class HighlightsLayout : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(HighlightsLayout), new PropertyMetadata(string.Empty, OnTitlePropertyChange));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(HighlightsLayout), new PropertyMetadata(null, OnItemsSourcePropertyChange));

        public static readonly DependencyProperty HasLoadDataErrorsProperty =
            DependencyProperty.Register("HasLoadDataErrors", typeof(bool), typeof(HighlightsLayout), new PropertyMetadata(false, OnHasLoadDataErrorsPropertyChange));

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(HighlightsLayout), new PropertyMetadata(false, OnIsBusyPropertyChange));

        public static readonly DependencyProperty HasMoreItemsProperty =
            DependencyProperty.Register("HasMoreItems", typeof(bool), typeof(HighlightsLayout), new PropertyMetadata(false, OnHasMoreItemsPropertyChange));

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(HighlightsLayout), new PropertyMetadata(null, OnItemClickCommandPropertyChange));

        public static readonly DependencyProperty SectionHeaderClickCommandProperty =
            DependencyProperty.Register("SectionHeaderClickCommand", typeof(ICommand), typeof(HighlightsLayout), new PropertyMetadata(null, OnSectionHeaderClickCommandPropertyChange));

        public static readonly DependencyProperty ListLayoutTemplateProperty =
            DependencyProperty.Register("ListLayoutTemplate", typeof(LayoutProperties.ListLayoutTemplate), typeof(HighlightsLayout), new PropertyMetadata(LayoutProperties.ListLayoutTemplate.VerticalCard));

        public HighlightsLayout()
        {
            this.InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
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

        public bool HasMoreItems
        {
            get { return (bool)GetValue(HasMoreItemsProperty); }
            set { SetValue(HasMoreItemsProperty, value); }
        }

        public ICommand SectionHeaderClickCommand
        {
            get { return (ICommand)GetValue(SectionHeaderClickCommandProperty); }
            set { SetValue(SectionHeaderClickCommandProperty, value); }
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

        private static void OnTitlePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.Title = e.NewValue as string;
        }

        private static void OnItemsSourcePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.ItemsSource = e.NewValue;
        }

        private static void OnHasLoadDataErrorsPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.HasLoadDataErrors = (bool)e.NewValue;
        }

        private static void OnIsBusyPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.IsBusy = (bool)e.NewValue;
        }

        private static void OnHasMoreItemsPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.HasMoreItems = (bool)e.NewValue;
        }

        private static void OnItemClickCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.ItemClickCommand = e.NewValue as ICommand;
        }

        private static void OnSectionHeaderClickCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HighlightsLayout;
            self.SectionHeaderClickCommand = e.NewValue as ICommand;
        }
    }
}
