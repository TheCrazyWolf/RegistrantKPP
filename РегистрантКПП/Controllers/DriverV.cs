using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace РегистрантКПП.Controllers
{
    public class DriverV
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronomic { get; set; }
        public string DateTime { get; set; }
        public string Info { get; set; }

        public DriverV(DB.Registrants registrants)
        {
            Id = registrants.Id;
            FirstName = registrants.FirstName;
            SecondName = registrants.SecondName;
            Patronomic = registrants.Patronomic;
            DateTime = registrants.DateTime.ToString();
            Info = registrants.Info;
        }


    }
}
