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
    public partial class NewDriver : Window
    {
        protected DB.Registrants registrants;

        public NewDriver()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (tb_firstname.Text == "" && tb_secondname.Text == "" && tb_phone.Text == "")
            {
                MessageBox.Show("Не все требуемые поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Будет произведена регистрация водителя: " + tb_firstname.Text + " " + tb_secondname.Text + " " + tb_phone.Text, "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DB.RegistrantEntities ef = new DB.RegistrantEntities();
                        registrants = new DB.Registrants();

                        registrants.FirstName = tb_firstname.Text;
                        registrants.SecondName = tb_secondname.Text;
                        registrants.Phone = tb_phone.Text;
                        registrants.DateTime = DateTime.Now;
                        registrants.Info = tb_info.Text  + "\n-----" +"\n[I]" + DateTime.Now + " (" + Registrant.Default.LastLogin + ") создал карточку (" + tb_secondname.Text + " " + tb_secondname.Text + ", " + tb_secondname.Text + ")";
                        ef.Registrants.Add(registrants);
                        ef.SaveChanges();
                        MessageBox.Show("Водитель зарегистрирован", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        ef.Dispose();

                        this.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Произошла ошибка при регистрации. Пожалуйста обратитесь к персоналу. Проверьте подключение к БД/интернет или еще что нибудь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
    }
}
