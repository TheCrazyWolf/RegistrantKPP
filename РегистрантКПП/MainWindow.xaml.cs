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

            tb_ip.Text = Registrant.Default.IP;
            tb_port.Text = Registrant.Default.PORT;
            tb_bdname.Text = Registrant.Default.DB;
            tb_bdlogin.Text = Registrant.Default.LOGIN;
            tb_bdpass.Text = Registrant.Default.PASSWORD;

            var connect = @"metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=" + tb_ip.Text + ", " + tb_port.Text + ";initial catalog=" + tb_bdname.Text + ";persist security info=True;user id=" + tb_bdlogin.Text + ";password=" + tb_bdpass.Text + ";integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";

            MessageBox.Show(connect);

            Thread thread = new Thread(new ThreadStart(TestConnect));
            thread.Start();
        }

        void TestConnect()
        {
            try
            {
                DB.RegistrantEntities ef = new DB.RegistrantEntities();
                ef.Database.Exists();
                ef.Dispose();

                Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Visible);
                Dispatcher.Invoke(() => GridError.Visibility = Visibility.Hidden);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Hidden);
                Dispatcher.Invoke(() => GridError.Visibility = Visibility.Visible);
            }
        }


        private void btn_kpp_Click(object sender, RoutedEventArgs e)
        {
            KPP.WindowKPP windowKPP = new KPP.WindowKPP();
            windowKPP.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sklad.WindowSklad windowSklad = new Sklad.WindowSklad();
            windowSklad.Show();
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

            var connect = @"metadata=res://*/DB.Registrant.csdl|res://*/DB.Registrant.ssdl|res://*/DB.Registrant.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=" + tb_ip.Text + ", " + tb_port.Text + ";initial catalog=" + tb_bdname.Text + ";user id=" + tb_bdlogin.Text + ";password=" + tb_bdpass.Text + ";MultipleActiveResultSets=True;App=EntityFramework&quot;";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["RegistrantEntities"].ConnectionString = connect;
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");

            TestConnect();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sklad.WindowSklad windowSklad = new Sklad.WindowSklad();
            windowSklad.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => GridAuth.Visibility = Visibility.Hidden);
            Dispatcher.Invoke(() => GridError.Visibility = Visibility.Visible);
        }
    }
}
