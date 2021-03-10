using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace РегистрантКПП.Controllers
{
    public class Driver
    {
        //Лист со списком водителей (??)
        public ObservableCollection<DriverV> driverVs { get; set; }

        //Подкл
        

        public Driver()
        {
            LoadList();
        }


        public void LoadList()
        {
            driverVs = new ObservableCollection<DriverV>();

            //Временный лист из бд
            DB.RegistrantEntities ef = new DB.RegistrantEntities();
            List<DB.Registrants> drivers = ef.Registrants.Where(x => x.TimeLeft == null).ToList();

            //Перебор в листах данных
            foreach (var item in drivers)
            {
                 DriverV driverV = new DriverV(item);
                 driverVs.Add(driverV);
   
            }

            //Сортировка по последним
            var d = driverVs.OrderByDescending(x => x.Id).ToList();
            driverVs.Clear();
            d.ForEach(x => driverVs.Add(x));
            //Возращаем обратно в лист
            ef.Dispose();

        }

    }
}
