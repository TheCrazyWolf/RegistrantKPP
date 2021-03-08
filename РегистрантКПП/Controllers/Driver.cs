using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace РегистрантКПП.Controllers
{
    public class Driver
    {
        public List<DriverV> driverVs { get; set; }
        DB.RegistrantEntities ef = new DB.RegistrantEntities();
        

        public Driver()
        {
            driverVs = new List<DriverV>();
            List<DB.Registrants> drivers = ef.Registrants.ToList();

            drivers = ef.Registrants.ToList();

            foreach (var item in drivers)
            {
                DriverV driverV = new DriverV(item);
                driverVs.Add(driverV);
            }
        }

        public void Refresh()
        {
            driverVs = null;
            driverVs = new List<DriverV>();
            List<DB.Registrants> drivers = ef.Registrants.ToList();

            drivers = ef.Registrants.ToList();

            foreach (var item in drivers)
            {
                DriverV driverV = new DriverV(item);
                driverVs.Add(driverV);
            }

        }
    }
}
