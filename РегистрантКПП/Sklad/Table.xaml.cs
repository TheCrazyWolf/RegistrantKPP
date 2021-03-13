using System.Windows;

namespace РегистрантКПП.Sklad
{
    public partial class Table : Window
    {
        public Table()
        {
            InitializeComponent();

            Driver driver = new Driver();
            driver.LoadListAllWithDel();
            data.ItemsSource = driver.driverVs;
        }
    }
}
