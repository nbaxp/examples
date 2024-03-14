using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public int ErrorsCount { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.ErrorsCount == 0)
            {
                this.Hide();
                new MainWindow().Show();
                this.Close();
            }
        }

        private void Grid_Error(object sender, ValidationErrorEventArgs e)
        {
            this.ErrorsCount += e.Action == ValidationErrorEventAction.Added ? 1 : -1;
        }
    }
}