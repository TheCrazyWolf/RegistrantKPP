using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace РегистрантКПП.Sklad
{
    public class DriverV
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string DateTime { get; set; }
        public string Phone { get; set; }
        public string Info { get; set; }

        public string TimeArrive { get; set; }
        public string TimeLeft { get; set; }
        public string Status { get; set; }

        public string Deleted { get; set; }


        public string btn_Arrive { get; set; }
        public string btn_Left { get; set; }

        public DriverV()
        {

        }

        /// <summary>
        /// магия
        /// </summary>
        /// <param name="registrants"></param>
        public DriverV(DB.Registrants registrants)
        {
            Id = registrants.Id;
            FirstName = registrants.FirstName;
            SecondName = registrants.SecondName;
            DateTime = registrants.DateTime.ToString();
            Phone = registrants.Phone;
            Info = registrants.Info;
            TimeArrive = registrants.TimeArrive.ToString();
            TimeLeft = registrants.TimeLeft.ToString();
            Deleted = registrants.Deleted;

            if (registrants.DateTime != null)
            {
                Status = "Зарегистрирован";

                if (registrants.TimeArrive != null)
                {
                    Status = "Прибыл на территорию склада";

                    if (registrants.TimeLeft != null)
                    {
                        Status = "Покинул территорию склада";
                    }
                }
            }

            if (registrants.TimeArrive == null)
            {
                btn_Left = "Collapsed";
                btn_Arrive = "Visible";
            }
            else
            {
                btn_Left = "Visible";
                btn_Arrive = "Collapsed";
            }

        }

    }
}
