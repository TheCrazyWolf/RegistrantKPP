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
using System.Windows.Media.Effects;
using System.Threading;

namespace РегистрантКПП.Sklad
{
    /// <summary>
    /// Логика взаимодействия для WindowSklad.xaml
    /// </summary>
    public partial class WindowSklad : Window
    {
        public bool AdminMode { get; set; }

        public WindowSklad()
        {
            InitializeComponent();
            Refresh();
        }

        void Refresh()
        {
            Drivers.ItemsSource = null;
            Driver driver = new Driver();

            if (ch_loadall.IsChecked == true)
            {
                driver.LoadListAll();
                Drivers.ItemsSource = driver.driverVs.ToList();
            }
            else
            {
                driver.LoadList();
                Drivers.ItemsSource = driver.driverVs.ToList();
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_done_timeLeft.Visibility = Visibility.Visible;
            btn_done_TimeArrive.Visibility = Visibility.Visible;
            Driver_Info.Visibility = Visibility.Visible;


            var current_driver = Drivers.SelectedItem as DriverV;
            Driver_Info.DataContext = current_driver;
     
                if (tb_TimeArrive.Text != "")
                {
                    btn_done_TimeArrive.Visibility = Visibility.Hidden;

                    if (tb_TimeLeft.Text != "")
                    {
                        btn_done_timeLeft.Visibility = Visibility.Hidden;
                    }
                }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Driver_Info.Visibility = Visibility.Hidden;
            Refresh();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 50;
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обновить данные?","Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();
                    var driv = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                    driv.FirstName = tb_firstname.Text;
                    driv.SecondName = tb_secondname.Text;
                    driv.Phone = tb_phone.Text;
                    driv.Info = tb_info.Text;

                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 50;
            MainGrid.Effect = effect;
            
                MessageBoxResult result = MessageBox.Show("Вы действительно ходите удалить карточку №" + tb_id.Text + " на имя " + tb_firstname.Text + " " + tb_secondname.Text, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();
                    var driv = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                    ef.Registrants.Remove(driv);
                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();
                    Driver_Info.Visibility = Visibility.Hidden;

                    MessageBox.Show("Карточка успешно удалена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AdminMode = true;
        }
    }
}
