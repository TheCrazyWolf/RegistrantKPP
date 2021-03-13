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

namespace РегистрантКПП.Sklad
{
    /// <summary>
    /// Логика взаимодействия для EditDriver.xaml
    /// </summary>
    public partial class EditDriver : Window
    {
        public EditDriver(int id)
        {
            InitializeComponent();

            try
            {
                DB.RegistrantEntities ef = new DB.RegistrantEntities();

                var temp = ef.Registrants.Where(x => x.Id == id).FirstOrDefault();
                tb_id.Text = temp.Id.ToString();
                tb_firstname.Text = temp.FirstName;
                tb_secondname.Text = temp.SecondName;
                tb_phone.Text = temp.Phone;

                tb_DateTime.Text = temp.DateTime.ToString();
                tb_TimeArrive.Text = temp.TimeArrive.ToString();
                tb_TimeLEft.Text = temp.TimeLeft.ToString();

                ef.Dispose();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB.RegistrantEntities ef = new DB.RegistrantEntities();
                var temp = ef.Registrants.Where(x => x.Id.ToString() == tb_id.Text).FirstOrDefault();
                temp.FirstName = tb_firstname.Text;
                temp.SecondName = tb_secondname.Text;
                temp.Phone = tb_phone.Text;

                var info2 = temp.Info + "\n-----" + tb_info.Text + "\n[I]" + DateTime.Now + "(" + Registrant.Default.LastLogin + ") изменил данные. (" + temp.FirstName + " " + temp.SecondName + ", " + temp.Phone + ")";
                temp.Info = info2;

                ef.SaveChanges();
                ef.Dispose();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            this.Close();

        }
    }
}
