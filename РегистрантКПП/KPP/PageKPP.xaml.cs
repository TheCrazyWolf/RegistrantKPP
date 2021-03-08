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
            //Scroll();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            chat.Refresh();
            Scroll();
            lb_chat.ItemsSource = chat.Chats;
        }

        public void Scroll()
        {
            lb_chat.SelectedIndex = lb_chat.Items.Count - 1;
            lb_chat.ScrollIntoView(lb_chat.SelectedItem);
        }
    }
}
