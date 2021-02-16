using System.Windows;
using TeaMan.ViewModels;

namespace TeaMan.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}
