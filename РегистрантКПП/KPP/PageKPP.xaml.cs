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
        protected DB.Registrants registrants;
        
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

        void Scroll()
        {
            Thread.Sleep(2000);
            Dispatcher.Invoke(() => lb_chat.Focus());
            Dispatcher.Invoke(() => lb_chat.SelectedIndex = lb_chat.Items.Count - 1);
            Dispatcher.Invoke(() => lb_chat.ScrollIntoView(lb_chat.SelectedItem));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DB.RegistrantEntities ef = new DB.RegistrantEntities();
            registrants = new DB.Registrants();

            registrants.FirstName = tb_FirstName.Text;
            registrants.SecondName = tb_secondname.Text;
            registrants.Patronomic = tb_Patronomic.Text;
            registrants.Phone = tb_Phone.Text;
            registrants.DateTime = DateTime.Now;
            registrants.Info = "";

            try
            {
                ef.Registrants.Add(registrants);
                ef.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при регистрации. Пожалуйста обратитесь к персоналу");
            }
        }
    }
}
