using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ContosoLtd.Layouts.Controls
{
    public sealed partial class ResponsiveGridView : UserControl
    {
        private double _desiredWidth;
        private int _columns;

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0D, OnItemsWidthPropertyChange));

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ResponsiveGridView), new PropertyMetadata(null, OnItemsSourcePropertyChange));

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ResponsiveGridView), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty CancelScrollProperty =
            DependencyProperty.Register("CancelScroll", typeof(bool), typeof(ResponsiveGridView), new PropertyMetadata(false));

        public bool CancelScroll
        {
            get { return (bool)GetValue(CancelScrollProperty); }
            set { SetValue(CancelScrollProperty, value); UpdateScroll(); }
        }

        private void UpdateScroll()
        {
            if (CancelScroll)
            {
                ScrollViewer.SetVerticalScrollMode(gridView, ScrollMode.Disabled);
                ScrollViewer.SetVerticalScrollBarVisibility(gridView, ScrollBarVisibility.Disabled);
            }
            else
            {
                ScrollViewer.SetVerticalScrollMode(gridView, ScrollMode.Auto);
                ScrollViewer.SetVerticalScrollBarVisibility(gridView, ScrollBarVisibility.Auto);
            }
        }

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(ResponsiveGridView), new PropertyMetadata(null, OnItemClickCommandPropertyChange));
        
        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public ResponsiveGridView()
        {
            this.InitializeComponent();

            gridView.SizeChanged += GridView_SizeChanged;
        }

        private void GridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                //initialize 
                if (_columns == 0)
                {
                    _columns = CalculateColumns(gridView.ActualWidth, ItemWidth);
                }
                else
                {
                    var desiredColumns = CalculateColumns(gridView.ActualWidth, _desiredWidth);
                    if (desiredColumns != _columns)
                    {
                        _columns = desiredColumns;
                    }
                }
                ItemWidth = (e.NewSize.Width / _columns) - 5;
            }
        }

        private static void OnItemsSourcePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ResponsiveGridView;
            self.ItemsSource = e.NewValue;
        }        

        private static void OnItemsWidthPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ResponsiveGridView;

            self.ItemWidth = (Double)e.NewValue;

            //initialize
            if (self._desiredWidth == 0)
            {
                self._desiredWidth = self.ItemWidth;
            }
        }

        private static void OnItemClickCommandPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ResponsiveGridView;
            self.ItemClickCommand = e.NewValue as ICommand;
        }

        private static int CalculateColumns(double containerWidth, double itemWidth)
        {
            var columns = (int)(containerWidth / itemWidth);
            if (columns == 0)
            {
                columns = 1;
            }

            return columns;
        }
    }
}
