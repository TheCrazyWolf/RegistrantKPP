using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;


namespace РегистрантКПП.KPP
{
    /// <summary>
    /// Логика взаимодействия для PageKPP.xaml
    /// </summary>
    public partial class PageKPP : Page
    {
        Controllers.Chat chat = new Controllers.Chat();
        public PageKPP()
        {
            InitializeComponent();
            lb_chat.ItemsSource = chat.Chats;

            Thread thread = new Thread(new ThreadStart(Scroll));
            thread.Start();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Scroll));
            thread.Start();
            chat.Refresh();
            Scroll();
            lb_chat.ItemsSource = chat.Chats;
        }

        public void Scroll()
        {
            Thread.Sleep(2000);
            Dispatcher.Invoke(() => lb_chat.Focus());
            Dispatcher.Invoke(() => lb_chat.SelectedIndex = lb_chat.Items.Count - 1);
            Dispatcher.Invoke(() => lb_chat.ScrollIntoView(lb_chat.SelectedItem));
        }
    }
}
