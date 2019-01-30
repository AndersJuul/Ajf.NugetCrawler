using System.Windows;

namespace Ajf.NugetCrawler.Wpf
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var view = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            view.ShowDialog();
        }
    }
}