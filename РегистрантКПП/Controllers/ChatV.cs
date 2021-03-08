using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace РегистрантКПП.Controllers
{
    public class ChatV
    {
        public int Id { get; set; }
        public string NamePC { get; set; }
        public string TextMSG { get; set; }
        public string Time { get; set; }
        public string SortMSG { get; set; }

        public ChatV(DB.Chat chat)
        {
            Id = chat.Id;
            NamePC = chat.NamePC;
            TextMSG = chat.TextMSG;
            Time = chat.Time.ToString();
        }
    }
}
