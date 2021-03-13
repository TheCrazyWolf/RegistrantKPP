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
using System.Windows.Media.Effects;


namespace РегистрантКПП.KPP
{
    /// <summary>
    /// Логика взаимодействия для WindowKPP.xaml
    /// </summary>
    public partial class WindowKPP : Window
    {
        protected DB.Registrants registrants;
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
            BlurEffect effect = new BlurEffect();
            effect.Radius = 20;
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
                        DB.RegistrantEntities ef = new DB.RegistrantEntities();
                        registrants = new DB.Registrants();

                        registrants.FirstName = tb_FirstName.Text;
                        registrants.SecondName = tb_secondname.Text;
                        registrants.Phone = tb_Phone.Text;
                        registrants.DateTime = DateTime.Now;
                        registrants.Info = tb_info.Text + "\n-----" + "\n[I]" + DateTime.Now + " (" + Registrant.Default.LastLogin + ") создал карточку (" + tb_secondname.Text + " " + tb_secondname.Text + ", " + tb_secondname.Text + ")"; 
                        ef.Registrants.Add(registrants);
                        ef.SaveChanges();
                        MessageBox.Show("Водитель зарегистрирован", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb_FirstName.Text = ""; tb_secondname.Text = ""; tb_Phone.Text = ""; tb_info.Text = "";

                        driver.LoadList();
                        Drivers.ItemsSource = driver.driverVs.ToList();
                        ef.Dispose();
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
            BlurEffect effect = new BlurEffect();
            effect.Radius = 20;
            MainGrid.Effect = effect;

            var bt = e.OriginalSource as Button;
            var current_driver = bt.DataContext as Sklad.DriverV;

            MessageBoxResult result = MessageBox.Show("Статус водителя будет изменен (ПРИБЫЛ) -: " + current_driver.FirstName + " " + current_driver.SecondName + " " + current_driver.Phone, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();

                    var ef_driver_curr = ef.Registrants.FirstOrDefault(x => x.Id == current_driver.Id);
                    if (ef_driver_curr != null)
                    {
                        ef_driver_curr.TimeArrive = DateTime.Now;
                    }
                    ef.SaveChanges();
                    ef.Dispose();

                    UpdateDrivers();
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при смене статуса. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            MainGrid.Effect = null;
        }

        private void btn_Left_Click(object sender, RoutedEventArgs e)
        {

            BlurEffect effect = new BlurEffect();
            effect.Radius = 20;
            MainGrid.Effect = effect;

            var bt = e.OriginalSource as Button;
            var current_driver = bt.DataContext as Sklad.DriverV;

            MessageBoxResult result = MessageBox.Show("Статус водителя будет изменен (ПОКИНУЛ) -: " + current_driver.FirstName + " " + current_driver.SecondName + " " + current_driver.Phone, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var ef_driver_curr = ef.Registrants.FirstOrDefault(x => x.Id == current_driver.Id);
                        if (ef_driver_curr != null)
                        {
                            ef_driver_curr.TimeLeft = DateTime.Now;
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
            try
            {
                do
                {
                    Thread.Sleep(600000);
                    Dispatcher.Invoke(() => Drivers.ItemsSource = null);
                    driver.LoadList();
                    Dispatcher.Invoke(() => Drivers.ItemsSource = driver.driverVs.ToList());
                } while (true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            thread.Abort();
        }
    }
}
