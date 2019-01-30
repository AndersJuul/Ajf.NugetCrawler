using System.Windows;

namespace Ajf.NugetCrawler.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            (DataContext as MainWindowViewModel).Activated();
        }
    }
}