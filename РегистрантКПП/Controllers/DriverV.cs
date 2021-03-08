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
        public string DateTime { get; set; }
        public string Phone { get; set; }
        public string Info { get; set; }
        
        public bool DriverLeft { get; set; }

        public int SordID { get; set; }

        public string btn_Arrive { get; set; }
        public string btn_Left { get; set; }

        public DriverV(DB.Registrants registrants)
        {
            Id = registrants.Id;
            FirstName = registrants.FirstName;
            SecondName = registrants.SecondName;
            DateTime = registrants.DateTime.ToString();
            Phone = registrants.Phone;
            Info = registrants.Info;

            if (registrants.TimeLeft == null)
            {
                if (registrants.TimeArrive == null)
                {
                    btn_Arrive = "Collapsed";
                    btn_Arrive = "Visible";
                }
                btn_Arrive = "Visible";
                btn_Arrive = "Collapsed";
            }

        }
    }
}
