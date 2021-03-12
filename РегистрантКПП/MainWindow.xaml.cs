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
using System.Configuration;
using System.Threading;



namespace РегистрантКПП
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CheckDay();

            tb_ip.Text = Registrant.Default.IP;
            tb_port.Text = Registrant.Default.PORT;
            tb_bdname.Text = Registrant.Default.DB;
            tb_bdlogin.Text = Registrant.Default.LOGIN;
            tb_bdpass.Text = Registrant.Default.PASSWORD;

            tb_lastlogin.Text = Registrant.Default.LastLogin;
            tb_lastpassword.Password = Registrant.Default.LastPassword;


            Thread thread = new Thread(new ThreadStart(TestConnect));
            thread.Start();
        }

        private void CheckDay()
        {
            double x = Convert.ToDouble(DateTime.Now.Hour);
            if (x > 6.0 && x < 12.0)
            {
                tb_welcome.Text = "Доброе утро";
            }
            else if (x >= 12.0 && x < 15.0)
            {
                tb_welcome.Text = "Добрый день";
            }
            else if (x >= 15.0 && x < 21.0)
            {
                tb_welcome.Text = "Добрый вечер";
            }

            else if (x >= 21.0)
            {
                tb_welcome.Text = "Доброй ночи";
            }
        }

        void TestConnect()
        {
            try
            {
                Dispatcher.Invoke(() => GridWait.Visibility = Visibility.Visible);

                DB.RegistrantEntities ef = new DB.RegistrantEntities();
                var test = ef.Registrants.ToList();
                ef.Dispose();

                Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => GridError.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridWait.Visibility = Visibility.Hidden);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridWait.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridError.Visibility = Visibility.Visible);
            }
        }


        private void btn_save_settings_Click(object sender, RoutedEventArgs e)
        {
            Registrant.Default.IP = tb_ip.Text;
            Registrant.Default.PORT = tb_port.Text;
            Registrant.Default.DB = tb_bdname.Text;
            Registrant.Default.LOGIN = tb_bdlogin.Text;
            Registrant.Default.PASSWORD = tb_bdpass.Text;
            Registrant.Default.Save();

            //var connect = @"metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=" + tb_ip.Text + ", " + tb_port.Text + ";initial catalog=" + tb_bdname.Text + ";persist security info=True;user id=" + tb_bdlogin.Text + ";password=" + tb_bdpass.Text + ";integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
            //string connect = "metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=" + tb_ip.Text + "," + tb_port.Text + ";initial catalog=" + tb_bdname.Text + ";user id=" + tb_bdlogin.Text + ";password=" + tb_bdpass.Text + ";MultipleActiveResultSets=True;App=EntityFramework&quot;";

            //var connect = @"metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=""data source=" + tb_ip.Text + ", " + tb_port.Text + ";initial catalog=" + tb_bdname.Text + ";user id=" + tb_bdlogin.Text + ";password=" + tb_bdpass.Text + ";MultipleActiveResultSets=True;App=EntityFramework";
            
            var connect = $"metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=\"data source={tb_ip.Text},{tb_port.Text};initial catalog={tb_bdname.Text};user id={tb_bdlogin.Text};password={tb_bdpass.Text};MultipleActiveResultSets=True;App=EntityFramework\";";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["RegistrantEntities"].ConnectionString = connect;
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");

            GridRestart.Visibility = Visibility.Visible;
            GridError.Visibility = Visibility.Hidden;
        }

        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            if (tb_lastlogin.Text == "кпп")
            {
                KPP.WindowKPP window = new KPP.WindowKPP();
                window.Show();
                this.Close();

                Registrant.Default.LastLogin = tb_lastlogin.Text;
                Registrant.Default.LastPassword = tb_lastpassword.Password;
                Registrant.Default.Save();
            }
            else if (tb_lastlogin.Text == "админ" && tb_lastpassword.Password == "админ")
            {
                Sklad.WindowSklad window = new Sklad.WindowSklad();
                window.Show();
                this.Close();

                Registrant.Default.LastLogin = tb_lastlogin.Text;
                Registrant.Default.LastPassword = tb_lastpassword.Password;
                Registrant.Default.Save();
            }

        }
    }
}
