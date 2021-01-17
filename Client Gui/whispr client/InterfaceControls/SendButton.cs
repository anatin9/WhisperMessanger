using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace whispr_client.InterfaceControls
{
    public class SendButton
    {
        /*
         * when send is clicked, create a new message object, encrypt, trasmit to chat server.
         */ 
        public static ChatMessage SendClicked(TextBox tb, ListBox lb )
        {
            ChatMessage cm = new ChatMessage();
            if (tb.Text != "" && tb.Text != null)
            {
                cm.Message = tb.Text;
                cm.Sender = "Me";
                cm.Received = DateTime.Now;
                
                

                //
                //Initiate encryption and message sending here
                //

                tb.Text = "";
            }
            return cm;
        }
    }
}
