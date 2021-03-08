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
        protected DB.Chat chats;
        Controllers.Chat chat = new Controllers.Chat();


        public PageKPP()
        {
            InitializeComponent();
            Thread thread = new Thread(new ThreadStart(Refresher));
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
            Dispatcher.Invoke(() => lb_chat.SelectedIndex = lb_chat.Items.Count - 1);
            Dispatcher.Invoke(() => lb_chat.ScrollIntoView(lb_chat.SelectedItem));
        }

        void Refresher()
        {
            chat.Refresh();
            var chatix = chat.Chats;
            Dispatcher.Invoke(() => lb_chat.ItemsSource = chatix);
            Scroll();

            Thread.Sleep(5000);

            chat.Refresh();
            chatix = chat.Chats;
            if (Dispatcher.Invoke(() => lb_chat.ItemsSource != chatix))
            {
                chat.Refresh();
                Dispatcher.Invoke(() => lb_chat.ItemsSource = chatix);
                Scroll();
                Refresher();
            }
            else
            {
                Refresher();
            }
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
                tb_FirstName.Text = ""; tb_secondname.Text = ""; tb_Patronomic.Text = ""; tb_Phone.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при регистрации. Пожалуйста обратитесь к персоналу");
            }
        }

        private void tb_enterchat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DB.RegistrantEntities ef = new DB.RegistrantEntities();

                chats = new DB.Chat();
                chats.NamePC = "КПП";
                chats.TextMSG = tb_enterchat.Text;
                chats.Time = DateTime.Now;

                try
                {
                    ef.Chat.Add(chats);
                    ef.SaveChanges();
                    tb_enterchat.Text = "";
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
