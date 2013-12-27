using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimeWallPaper
{
    public partial class SearchControl : UserControl
    {
        public delegate void SearchEventHandler(string query);

        public event SearchEventHandler Search;

        public void OnSearch(string query)
        {
            SearchEventHandler handler = Search;
            if (handler != null) handler(query);
        }

        public SearchControl()
        {
            InitializeComponent();
        }

        private string _query = string.Empty;

        private void InputBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnSearch(_query);
            }
        }

        private void InputBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InputBox.Text = string.Empty;
            InputBox.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void InputBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_query == string.Empty && InputBox.Text == "Search") return;            
            _query = InputBox.Text;
        }

        private void InputBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            InputBox.Foreground = new SolidColorBrush(Colors.DarkGray);
            if (InputBox.Text == string.Empty)
            {
                InputBox.Text = "Search";
            }
        }       
    }
}
