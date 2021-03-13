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
        protected DB.Registrants registrants;
        public WindowSklad()
        {
            InitializeComponent();

            if (Registrant.Default.LoadAll == true)
            {
                ch_loadall.IsChecked = true;
            }
            else if (Registrant.Default.LoadAll == false)
            {
                ch_loadall.IsChecked = false;
            }

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
           /* btn_done_timeLeft.Visibility = Visibility.Collapsed;
            btn_done_TimeArrive.Visibility = Visibility.Collapsed;
            new_driver.Visibility = Visibility.Collapsed;
            Driver_Info.Visibility = Visibility.Visible;


            var current_driver = Drivers.SelectedItem as DriverV;

            try
            {
                DB.RegistrantEntities ef = new DB.RegistrantEntities();
                var temp = ef.Registrants.Where(x => x.Id == current_driver.Id).FirstOrDefault();

                
                Driver_Info.DataContext = temp;

            }
            catch (Exception)
            {

                throw;
            }


            if (tb_DateTime.Text != null)
            {
                if (tb_TimeLeft.Text != null)
                {
                    btn_done_TimeArrive.Visibility = Visibility.Visible;

                    if (tb_TimeArrive.Text != null)
                    {
                        btn_done_timeLeft.Visibility = Visibility.Visible;
                    }
                }
                
            } */


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            

            if (tb_search.Text == "")
            {
                Refresh();
                Grid_Banana.Visibility = Visibility.Hidden;
            }
            else
            {
                btn_search_Click(sender, e);
            }

        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Driver_Info.Visibility = Visibility.Hidden;
            Grid_ChooseDriver.Visibility = Visibility.Visible;
            Refresh();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 50;
            MainGrid.Effect = effect;

            EditDriver edit = new EditDriver(Convert.ToInt32(tb_id.Text));
            edit.ShowDialog();
            Refresh();

            /* MessageBoxResult result = MessageBox.Show("Вы действительно хотите обновить данные?","Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                { /*
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();
                    var driv = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                    driv.FirstName = tb_firstname.Text;
                    driv.SecondName = tb_secondname.Text;
                    driv.Phone = tb_phone.Text;
                    driv.Info = tb_info.Text + "\n" + DateTime.Now + " внесены изменения";

                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } */

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
                    driv.Deleted = "D";
                    driv.Info = driv.Info + "\n" + "[I]" + DateTime.Now + "(" + Registrant.Default.LastLogin + ") удалил карточку";
                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();
                    Driver_Info.Visibility = Visibility.Hidden;
                    Grid_ChooseDriver.Visibility = Visibility.Visible;

                    MessageBox.Show("Карточка успешно удалена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            
            MainGrid.Effect = null;
        }


        private void btn_done_TimeArrive_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 50;
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обнвить статус водителя " + tb_firstname.Text + " " + tb_secondname.Text + " на ПРИБЫЛ?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();
                    var driv = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                    driv.TimeArrive = DateTime.Now;
                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();
                    btn_done_TimeArrive.Visibility = Visibility.Collapsed;

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }

        private void btn_done_timeLeft_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 50;
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обнвить статус водителя " + tb_firstname.Text + " " + tb_secondname.Text + " на ПОКИНУЛ?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.RegistrantEntities ef = new DB.RegistrantEntities();
                    var driv = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                    driv.TimeLeft = DateTime.Now;

                    ef.SaveChanges();
                    ef.Dispose();
                    Refresh();

                    btn_done_TimeArrive.Visibility = Visibility.Collapsed;
                    btn_done_timeLeft.Visibility = Visibility.Collapsed;

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }


        private void btn_newdriver_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 20;
            MainGrid.Effect = effect;

            NewDriver newDriver = new NewDriver();
            newDriver.ShowDialog();
            Refresh();
            MainGrid.Effect = null;
        }



        private void btn_opentable_Click(object sender, RoutedEventArgs e)
        {
            Table table = new Table();
            table.Show();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Drivers.ItemsSource = null;
                Driver driver = new Driver();
                driver.LoadListAllWithDel();
                var data = driver.driverVs.Where(t => t.SecondName.ToUpper().StartsWith(tb_search.Text.ToUpper())).ToList();
                var sDOP = driver.driverVs.Where(t => t.SecondName.ToUpper().Contains(tb_search.Text.ToUpper())).ToList();
                data.AddRange(sDOP);

                // data.Distinct().ToArray();
                var noDupes = data.Distinct().ToList();
                Drivers.ItemsSource = noDupes;

                if (noDupes.Count == 0)
                {
                    Grid_Banana.Visibility = Visibility.Visible;
                }
                else
                {
                    Grid_Banana.Visibility = Visibility.Hidden;
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Произошла ошибка при обновление списка. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Drivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_done_timeLeft.Visibility = Visibility.Visible;
            btn_done_TimeArrive.Visibility = Visibility.Visible;



            var current_driver = Drivers.SelectedItem as DriverV;
            Driver_Info.DataContext = current_driver;

            if (tb_TimeArrive.Text != "")
            {
                btn_done_TimeArrive.Visibility = Visibility.Collapsed;

                if (tb_TimeLeft.Text != "")
                {
                    btn_done_timeLeft.Visibility = Visibility.Collapsed;
                }
            }

            if (tb_id.Text == "")
            {
                Driver_Info.Visibility = Visibility.Hidden;
                Grid_ChooseDriver.Visibility = Visibility.Visible;
            }
            else
            {
                Driver_Info.Visibility = Visibility.Visible;
                Grid_ChooseDriver.Visibility = Visibility.Hidden;
            }

        }


        private void ch_loadall_Checked(object sender, RoutedEventArgs e)
        {
            Registrant.Default.LoadAll = true;
            Registrant.Default.Save();
        }

        private void ch_loadall_Unchecked(object sender, RoutedEventArgs e)
        {
            Registrant.Default.LoadAll = false;
            Registrant.Default.Save();
        }
    }
}
