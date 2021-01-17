using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whispr_client.InterfaceControls
{
    public class Contact
    {
        public string UserName { get; set; }
        private string EncrKey { get; set; }
        public bool Online { get; set; }
        public DateTime LastOnline { get; set; }

        //create new contact object
        public void CreateNew(string name, bool online)
        {
            UserName = name;
            Online = online;
        }

        //create new contact object, with last online.
        public void CreateNew(string name, bool online, DateTime lastonline)
        {
            UserName = name;
            Online = online;
            LastOnline = lastonline;
        }

        //store the encryption key.
        public void UpdateKey(string key)
        {
            EncrKey = key;
        }

        //update to offline, set last time online.
        public void SetOffline()
        {
            Online = false;
            LastOnline = DateTime.Now;
        }

        //set online.
        public void SetOnline()
        {
            Online = true;
        }

        //get encryption key
        public string GetKey()
        {
            return EncrKey;
        }
    }
}
