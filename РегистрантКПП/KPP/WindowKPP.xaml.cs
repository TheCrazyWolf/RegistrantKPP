using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace РегистрантКПП.KPP
{
    public partial class WindowKPP : Window
    {
        protected DB.Registrants registrants;
        //protected DB.Chat chats;
        //Controllers.Chat chat = new Controllers.Chat();
        //Controllers.Driver driver = new Controllers.Driver();

        Sklad.Driver driver = new Sklad.Driver();
        
        public WindowKPP()
        {
            InitializeComponent();
            //Thread thread = new Thread(new ThreadStart(Refresher));
            //thread.Start();
            UpdateDrivers();
        }

        void Scroll()
        {
            Dispatcher.Invoke(() => lb_chat.SelectedIndex = lb_chat.Items.Count - 1);
            Dispatcher.Invoke(() => lb_chat.ScrollIntoView(lb_chat.SelectedItem));
        }

        void Refresher()
        {/*
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
            */
        }

        void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 20};
            MainGrid.Effect = effect;

            if (tb_FirstName.Text == "" && tb_secondname.Text == "" && tb_Phone.Text == "")
            {
                MessageBox.Show("Не все требуемые поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Будет произведена регистрация водителя: " + tb_FirstName.Text + " " + tb_secondname.Text + " " + tb_Phone.Text, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                        {
                            registrants = new DB.Registrants
                            {
                                FirstName = tb_FirstName.Text,
                                SecondName = tb_secondname.Text,
                                Phone = tb_Phone.Text,
                                DateTime = DateTime.Now,
                                Info = tb_info.Text
                            };

                            ef.Registrants.Add(registrants);
                            ef.SaveChanges();
                            MessageBox.Show("Водитель зарегистрирован", "Готово", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            tb_FirstName.Text = tb_secondname.Text = tb_Phone.Text = tb_info.Text = "";

                            driver.LoadList();
                            Drivers.ItemsSource = driver.driverVs.ToList();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Произошла ошибка при регистрации. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            MainGrid.Effect = null;
        }

        private void tb_enterchat_KeyDown(object sender, KeyEventArgs e)
        {/*
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
            }*/
        }

        private void btn_Arrive_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 20};
            MainGrid.Effect = effect;

            var bt = e.OriginalSource as Button;
            var currentDriver = bt?.DataContext as Sklad.DriverV;

            MessageBoxResult result = MessageBox.Show("Статус водителя будет изменен (ПРИБЫЛ) -: " + currentDriver?.FirstName + " " + currentDriver?.SecondName + " " + currentDriver?.Phone, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var currentDriverModel = ef.Registrants.FirstOrDefault(x => x.Id == currentDriver.Id);
                        if (currentDriverModel != null)
                        {
                            currentDriverModel.TimeArrive = DateTime.Now;
                        }

                        ef.SaveChanges();
                    }

                    UpdateDrivers();
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при смене статуса. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД / интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }

        private void btn_Left_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 20};
            MainGrid.Effect = effect;

            var bt = e.OriginalSource as Button;
            var currentDriver = bt?.DataContext as Sklad.DriverV;

            MessageBoxResult result = MessageBox.Show("Статус водителя будет изменен (ПОКИНУЛ) -: " + currentDriver?.FirstName + " " + currentDriver?.SecondName + " " + currentDriver?.Phone, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var currentDriverModel = ef.Registrants.FirstOrDefault(x => x.Id == currentDriver.Id);
                        if (currentDriverModel != null)
                        {
                            currentDriverModel.TimeLeft = DateTime.Now;
                        }
                        ef.SaveChanges();
                    }
                    UpdateDrivers();
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при смене статуса. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }

        void UpdateDrivers()
        {
            Drivers.ItemsSource = null;
            driver.LoadList();
            Drivers.ItemsSource = driver.driverVs.ToList();
            //Drivers.InvalidateArrange();
            //Drivers.UpdateLayout();
        }

    }
}
