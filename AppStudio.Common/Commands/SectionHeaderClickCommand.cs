using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace AppStudio.Common.Commands
{
    public class SectionHeaderClickCommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", 
            typeof(ICommand),
            typeof(SectionHeaderClickCommand), 
            new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Hub;
            if (control != null)
            {
                control.SectionHeaderClick += OnSectionHeaderClick;
            }
        }

        private static void OnSectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            var control = sender as Hub;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(e.Section.DataContext))
            {
                command.Execute(e.Section.DataContext);
            }
        }
    }
}
