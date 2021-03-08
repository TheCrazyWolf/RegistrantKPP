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
using System.Windows.Shapes;
using System.Threading;

namespace РегистрантКПП.KPP
{
    /// <summary>
    /// Логика взаимодействия для WindowKPP.xaml
    /// </summary>
    public partial class WindowKPP : Window
    {

        protected DB.Registrants registrants;
        protected DB.Chat chats;
        Controllers.Chat chat = new Controllers.Chat();
        Controllers.Driver driver = new Controllers.Driver();

        public WindowKPP()
        {
            InitializeComponent();
            Thread thread = new Thread(new ThreadStart(Refresher));
            thread.Start();

            var s = driver.driverVs.ToList().OrderByDescending(x => x.Id);
            Drivers.ItemsSource = s;
        }


        void Scroll()
        {
            Dispatcher.Invoke(() => lb_chat.SelectedIndex = lb_chat.Items.Count - 1);
            Dispatcher.Invoke(() => lb_chat.ScrollIntoView(lb_chat.SelectedItem));
        }

        void Refresher()
        {
            try
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
            catch (Exception)
            {

                throw;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DB.RegistrantEntities ef = new DB.RegistrantEntities();
            registrants = new DB.Registrants();

            registrants.FirstName = tb_FirstName.Text;
            registrants.SecondName = tb_secondname.Text;
            registrants.Phone = tb_Phone.Text;
            registrants.DateTime = DateTime.Now;
            registrants.Info = tb_info.Text;

            try
            {
                ef.Registrants.Add(registrants);
                ef.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                tb_FirstName.Text = ""; tb_secondname.Text = ""; tb_Phone.Text = ""; tb_info.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при регистрации. Пожалуйста обратитесь к персоналу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
