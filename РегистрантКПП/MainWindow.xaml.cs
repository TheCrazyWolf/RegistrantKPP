using System;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Threading;

namespace РегистрантКПП
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            tb_welcome.Text = CheckDay();
            LoadDataFromSettigns();

            // Иниц потока для проверки соединения дабы приложениен не висло
            Thread thread = new Thread(new ThreadStart(TestConnect));
            thread.Start();
        }

        /// <summary>
        /// Подгрузка в полях последние данные для подключения из файла настроек
        /// </summary>
        void LoadDataFromSettigns()
        {
            tb_ip.Text = Registrant.Default.IP;
            tb_port.Text = Registrant.Default.PORT;
            tb_bdname.Text = Registrant.Default.DB;
            tb_bdlogin.Text = Registrant.Default.LOGIN;
            tb_bdpass.Text = Registrant.Default.PASSWORD;

            tb_lastlogin.Text = Registrant.Default.LastLogin;
            tb_lastpassword.Password = Registrant.Default.LastPassword;
        }

        /// <summary>
        /// Пожелание юзеру доброго дня
        /// </summary>
        /// <returns></returns>
        private string CheckDay()
        {
            double x = DateTime.Now.Hour;
            if (x > 6 && x < 12)
            {
                return "Доброе утро";
            }
            else if (x >= 12 && x < 15)
            {
                return "Добрый день";
            }
            else if (x >= 15 && x < 21)
            {
                return "Добрый вечер";
            }
            else if (x >= 21)
            {
                return "Доброй ночи";
            }

            return "Какое то магическое измерение";
        }

        /// <summary>
        /// Проверка соединения
        /// </summary>
        void TestConnect()
        {
            try
            {
                Dispatcher.Invoke(() => GridWait.Visibility = Visibility.Visible);

                using (DB.RegistrantEntities ef = new DB.RegistrantEntities())
                {
                    var test = ef.Registrants.ToList();
                }

                Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => GridError.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridWait.Visibility = Visibility.Hidden);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => tb_debug.Text = ex.ToString());

                Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridWait.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridError.Visibility = Visibility.Visible);
            }
        }

        /// <summary>
        /// Сохранения настроек для подключения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_settings_Click(object sender, RoutedEventArgs e)
        {
            Registrant.Default.IP = tb_ip.Text;
            Registrant.Default.PORT = tb_port.Text;
            Registrant.Default.DB = tb_bdname.Text;
            Registrant.Default.LOGIN = tb_bdlogin.Text;
            Registrant.Default.PASSWORD = tb_bdpass.Text;
            Registrant.Default.Save();

            var connect = $"metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=\"data source={tb_ip.Text},{tb_port.Text};initial catalog={tb_bdname.Text};user id={tb_bdlogin.Text};password={tb_bdpass.Text};MultipleActiveResultSets=True;App=EntityFramework\";";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["RegistrantEntities"].ConnectionString = connect;
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");

            GridRestart.Visibility = Visibility.Visible;
            GridError.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Выполнение входа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            if (tb_lastlogin.Text == "кпп")
            {
                KPP.WindowKPP window = new KPP.WindowKPP();
                window.Show();
                Close();

                Registrant.Default.LastLogin = tb_lastlogin.Text;
                Registrant.Default.LastPassword = tb_lastpassword.Password;
                Registrant.Default.Save();
            }
            else if (tb_lastlogin.Text == "админ" && tb_lastpassword.Password == "админ")
            {
                Sklad.WindowSklad window = new Sklad.WindowSklad();
                window.Show();
                Close();

                Registrant.Default.LastLogin = tb_lastlogin.Text;
                Registrant.Default.LastPassword = tb_lastpassword.Password;
                Registrant.Default.Save();
            }

        }
    }
}
