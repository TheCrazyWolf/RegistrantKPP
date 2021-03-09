using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace РегистрантКПП.Controllers
{
    public class Driver
    {
        //Лист со списком водителей (??)
        public List<DriverV> driverVs { get; set; }

        //Подкл
        DB.RegistrantEntities ef = new DB.RegistrantEntities();

        public Driver()
        {
            LoadList();
        }


        public void LoadList()
        {
            driverVs = null;
            //Новый лист
            driverVs = new List<DriverV>();

            //Временный лист из бд
            List<DB.Registrants> drivers = ef.Registrants.ToList();

            //drivers = ef.Registrants.ToList(); непонятная хрень??! без неё прекрасно все работает

            //Перебор в листах данных
            foreach (var item in drivers)
            {
                //Проверка убыл ли водитель?
                if (item.TimeLeft == null)
                {
                    //Если не убыл, то -
                    DriverV driverV = new DriverV(item);

                    //Добавляем
                    driverVs.Add(driverV);
                    
                }
                else
                {
                    //Если он уже убыл, то скипаем
                }
            }

            //Сортировка по последним
            var Sort = driverVs.OrderByDescending(x => x.Id).ToList();
            //Возращаем обратно в лист
            driverVs = Sort;

        }

    }
}
