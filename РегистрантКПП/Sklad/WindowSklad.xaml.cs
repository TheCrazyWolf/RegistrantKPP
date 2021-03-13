using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace РегистрантКПП.Sklad
{
    public partial class WindowSklad : Window
    {
        protected DB.Registrants registrants;
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
            new_driver.Visibility = Visibility.Hidden;
            Driver_Info.Visibility = Visibility.Visible;

            var currentDriver = Drivers.SelectedItem as DriverV;
            Driver_Info.DataContext = currentDriver;
     
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
            if (tb_search.Text == "")
            {
                Refresh();
            }
            else
            {
                btn_search_Click(sender, e);
            }

        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Driver_Info.Visibility = Visibility.Hidden;
            Refresh();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обновить данные?","Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.FirstName = tb_firstname.Text;
                            driver.SecondName = tb_secondname.Text;
                            driver.Phone = tb_phone.Text;
                            driver.Info = tb_info.Text + "\n" + DateTime.Now + " внесены изменения";
                            
                            ef.SaveChanges();
                        }
                    }
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
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;
            
             MessageBoxResult result = MessageBox.Show("Вы действительно ходите удалить карточку №" + tb_id.Text + " на имя " + tb_firstname.Text + " " + tb_secondname.Text, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.Deleted = "D";
                            driver.Info = tb_info.Text + "\n" + DateTime.Now + " карточка удалена";
                            
                            ef.SaveChanges();
                        }
                    }
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


        private void btn_done_TimeArrive_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обнвить статус водителя " + tb_firstname.Text + " " + tb_secondname.Text + " на ПРИБЫЛ?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.TimeArrive = DateTime.Now;
                            ef.SaveChanges();
                        }
                    }
                    Refresh();
                    btn_done_TimeArrive.Visibility = Visibility.Hidden;

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
            BlurEffect effect = new BlurEffect {Radius = 50};
            MainGrid.Effect = effect;

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите обнвить статус водителя " + tb_firstname.Text + " " + tb_secondname.Text + " на ПОКИНУЛ?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                    {
                        var driver = ef.Registrants.FirstOrDefault(x => x.Id.ToString() == tb_id.Text);
                        if (driver != null)
                        {
                            driver.TimeLeft = DateTime.Now;
                        }
                        ef.SaveChanges();
                    }
                    Refresh();

                    btn_done_TimeArrive.Visibility = Visibility.Hidden;
                    btn_done_timeLeft.Visibility = Visibility.Hidden;

                    MessageBox.Show("Данные успешно обновлены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных. Проверьте подключение к БД/правильность данных и еще что нибудь да проверьте", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainGrid.Effect = null;
        }

        private void btn_regist_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect effect = new BlurEffect {Radius = 20};
            MainGrid.Effect = effect;

            if (tbx_FirstName.Text == "" && tb_secondname.Text == "" && tbx_Phone.Text == "")
            {
                MessageBox.Show("Не все требуемые поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Будет произведена регистрация водителя: " + tbx_FirstName.Text + " " + tbx_secondname.Text + " " + tbx_Phone.Text, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                        {
                            registrants = new DB.Registrants
                            {
                                FirstName = tbx_FirstName.Text,
                                SecondName = tbx_secondname.Text,
                                Phone = tbx_Phone.Text,
                                DateTime = DateTime.Now,
                                Info = tbx_info.Text
                            };
                            ef.Registrants.Add(registrants);
                            ef.SaveChanges();
                            MessageBox.Show("Водитель зарегистрирован", "Готово", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            tbx_FirstName.Text = tbx_secondname.Text = tbx_Phone.Text = tbx_info.Text = "";
                        }

                        Refresh();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Произошла ошибка при регистрации. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            MainGrid.Effect = null;
            new_driver.Visibility = Visibility.Hidden;
        }

        private void btn_closed(object sender, RoutedEventArgs e)
        {
            new_driver.Visibility = Visibility.Hidden;
        }

        private void btn_newdriver_Click(object sender, RoutedEventArgs e)
        {
            Driver_Info.Visibility = Visibility.Hidden;
            new_driver.Visibility = Visibility.Visible;
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
                Drivers.ItemsSource = data.Distinct().ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при обновление списка. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

 
    }
}
