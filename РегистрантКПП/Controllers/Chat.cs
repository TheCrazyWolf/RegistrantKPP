using System.Collections.Generic;
using System.Linq;

namespace РегистрантКПП.Controllers
{
    public class Chat 
    {
        public List<ChatV> Chats {get;set;}
        public DB.RegistrantEntities ef = new DB.RegistrantEntities();

        public Chat()
        {
            Chats = new List<ChatV>();
            List<DB.Chat> chats = ef.Chat.ToList();

            chats = ef.Chat.ToList();

            foreach (var item in chats)
            {
                ChatV chatV = new ChatV(item);
                Chats.Add(chatV);
            }
        }

        public void Refresh()
        {
            Chats = null;
            Chats = new List<ChatV>();
            List<DB.Chat> chats = ef.Chat.ToList();

            chats = ef.Chat.ToList();

            foreach (var item in chats)
            {
                ChatV chatV = new ChatV(item);
                Chats.Add(chatV);
            }

        }
    }
}
