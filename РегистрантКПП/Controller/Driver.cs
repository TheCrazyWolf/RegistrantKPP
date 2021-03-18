using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace РегистрантКПП.Sklad
{
    public class Driver
    {
        /// <summary>
        /// Основной лист с водителями в который помещается уже обработанные данные
        /// </summary>
        public ObservableCollection<DriverV> driverVs { get; set; }
        public Driver()
        {
        }

        public void LoadList()
        {
            driverVs = new ObservableCollection<DriverV>();

            //Временный лист из бд
            DB.RegistrantEntities ef = new DB.RegistrantEntities();

            //Создаем ввременный лист в котором будут данные из бд, у которые не покинули склад и не удалены
            List<DB.Registrants> drivers = ef.Registrants.Where(x => (x.Deleted != "D") && (x.TimeLeft == null)).ToList();

            //Перебор в листах данных
            foreach (var item in drivers)
            {
                DriverV driverV = new DriverV(item);
                //И добавляем в основной лист
                driverVs.Add(driverV);

            }

            // Сортировка водителей по последним записям (по ID)
            var d = driverVs.OrderByDescending(x => x.Id).ToList();

            //Очищаем основной лист с водителями
            driverVs.Clear();

            //Тот же самый перебор данных только в уже в основной лист (который был очищен) типа как цикл foreach
            d.ForEach(x => driverVs.Add(x));

            // ДИСПОЗЕ  )(()))
            ef.Dispose();

        }

        /// <summary>
        /// ПРогрузка всего, удаленные не показываются
        /// </summary>
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


        /// <summary>
        /// Прогрузка всего в том числе удаленных
        /// </summary>
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
