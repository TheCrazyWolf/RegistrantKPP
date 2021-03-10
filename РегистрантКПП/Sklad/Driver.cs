using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace РегистрантКПП.Sklad
{
    public class Driver
    {
        //Лист со списком водителей (??)
        public ObservableCollection<DriverV> driverVs { get; set; }
        public Driver()
        {
            
        }

        public void LoadList()
        {
            driverVs = new ObservableCollection<DriverV>();

            //Временный лист из бд
            DB.RegistrantEntities ef = new DB.RegistrantEntities();

            //Пихаем в листы только у тех у кого не проставлено время ппокидания скалада
            List<DB.Registrants> drivers = ef.Registrants.Where(x => x.TimeLeft == null && x.Deleted == null).ToList();

            //Перебор в листах данных
            foreach (var item in drivers)
            {
                DriverV driverV = new DriverV(item);
                driverVs.Add(driverV);

            }

            //Сортировка по последним записям
            var d = driverVs.OrderByDescending(x => x.Id).ToList();

            //Очистка шлаков и токсинов
            driverVs.Clear();

            //Тот же самый перебор данных только в уже в основной лист (который был очищен) типа как цикл foreach
            d.ForEach(x => driverVs.Add(x));

            ef.Dispose();

        }

        public void LoadListAll()
        {
            driverVs = new ObservableCollection<DriverV>();

            //Временный лист из бд
            DB.RegistrantEntities ef = new DB.RegistrantEntities();

            //Пихаем в листы только у тех у кого не проставлено время ппокидания скалада
            List<DB.Registrants> drivers = ef.Registrants.Where(x => x.Deleted != "D").ToList(); 

            //Перебор в листах данных
            foreach (var item in drivers)
            {
                DriverV driverV = new DriverV(item);
                driverVs.Add(driverV);

            }

            //Сортировка по последним записям
            var d = driverVs.OrderByDescending(x => x.Id).ToList();

            //Очистка шлаков и токсинов
            driverVs.Clear();

            //Тот же самый перебор данных только в уже в основной лист (который был очищен) типа как цикл foreach
            d.ForEach(x => driverVs.Add(x));

            ef.Dispose();

        }


        public void LoadListAllWithDel()
        {
            driverVs = new ObservableCollection<DriverV>();

            //Временный лист из бд
            DB.RegistrantEntities ef = new DB.RegistrantEntities();

            //Пихаем в листы только у тех у кого не проставлено время ппокидания скалада
            List<DB.Registrants> drivers = ef.Registrants.ToList();

            //Перебор в листах данных
            foreach (var item in drivers)
            {
                DriverV driverV = new DriverV(item);
                driverVs.Add(driverV);

            }

            //Сортировка по последним записям
            var d = driverVs.OrderByDescending(x => x.Id).ToList();

            //Очистка шлаков и токсинов
            driverVs.Clear();

            //Тот же самый перебор данных только в уже в основной лист (который был очищен) типа как цикл foreach
            d.ForEach(x => driverVs.Add(x));

            ef.Dispose();

        }
    }
}
