using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using РегистрантКПП.DB;

namespace РегистрантКПП.KPP
{
    public partial class WindowKPP : Window
    {
        protected Registrants registrants;
        private Thread thread;
        Sklad.Driver driver = new Sklad.Driver();
        
        public WindowKPP()
        {
            InitializeComponent();
            
            UpdateDrivers();

            thread = new Thread(new ThreadStart(RefreshThread));
            thread.Start();

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
                        using (RegistrantEntities ef = new RegistrantEntities())
                        {
                            registrants = new Registrants
                            {
                                FirstName = tb_FirstName.Text,
                                SecondName = tb_secondname.Text,
                                Phone = tb_Phone.Text,
                                DateTime = DateTime.Now,
                                Info = $"{tb_info.Text}\n-----\n[I]{DateTime.Now} ({Registrant.Default.LastLogin}) создал карточку ({tb_secondname.Text} {tb_secondname.Text}, {tb_secondname.Text})"
                            };
                            
                            ef.Registrants.Add(registrants);
                            ef.SaveChanges();
                            MessageBox.Show("Водитель зарегистрирован", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                            tb_FirstName.Text = ""; tb_secondname.Text = ""; tb_Phone.Text = ""; tb_info.Text = "";
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
                    using (RegistrantEntities ef = new RegistrantEntities())
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
                    using (RegistrantEntities ef = new RegistrantEntities())
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
        }

        void RefreshThread()
        {
            do
            {
                Thread.Sleep(60000);
                Dispatcher.Invoke(() => Drivers.ItemsSource = null);
                driver.LoadList();
                Dispatcher.Invoke(() => Drivers.ItemsSource = driver.driverVs.ToList());
            } while (true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            thread.Abort();
        }
    }
}
