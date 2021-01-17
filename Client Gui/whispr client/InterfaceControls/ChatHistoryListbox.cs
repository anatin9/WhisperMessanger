using Communication.MessageClasses.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Whispr;
using Whispr.Client.Components;

namespace whispr_client.InterfaceControls
{
    public class ChatHistoryListbox
    {
        private List<ChatMessage> CMList;
        private readonly int MessageLimit = 1000;
        public readonly int ReduceWidthBy = 30;
        


        //constructor
        public ChatHistoryListbox() => CMList = new List<ChatMessage>();
        
        /*
         * format a message for an outgoing message.
         */
        public void NewOutgoingChatMessage(ChatMessage cm, ListBox lb, ClientProgram clp)
        {
            ListBoxItem lbi = GetCommonFormatting(cm, lb);
            lbi.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

            lb.Items.Add(lbi);
            LimitMessageCount(lb);
            lb.ScrollIntoView(lb.Items[lb.Items.Count - 1]);
            clp.SendChatMessage(cm.Message);
        }
        

        public void ChatMessageThread(SynchronizationContext MainThread, ClientProgram clp, ListBox lb)
        {
            while (clp.isRunning)
            {
                MainThread.Send((object state) => {
                    KeepUpdated(clp, lb);
                }, null);
                Thread.Sleep(500);
            }
        }

        private void KeepUpdated(ClientProgram clp, ListBox lb)
        {
            ChatServerResourceManager CRSM = ChatServerResourceManager.GetInstance();
            ChatServerManager CSM = CRSM.GetChatServerManager(clp.SLRM.ActiveServer.ChatServerEndpoint);
            
            List < EncryptedMessage > messages = CSM.GetMessages(true);
            List<string> Readable = new List<string>();
            
            foreach(EncryptedMessage E in messages)
            {
                if (E.SenderId != clp.ULRM.GetUserId())
                {
                    E.Displayed = true;
                    ChatMessage CM = new ChatMessage(E.PlainText, clp.ULRM.GetUsername(E.SenderId));
                    NewIncomingChatMessage(CM, lb);
                    
                }
            }
        }

        /*
         * foramt a message for an incoming message.
         */
        public void NewIncomingChatMessage(ChatMessage cm, ListBox lb)
        {
            ListBoxItem lbi = GetCommonFormatting(cm, lb);

            lb.Items.Add(lbi);
            LimitMessageCount(lb);
            lb.ScrollIntoView(lb.Items[lb.Items.Count - 1]);
        }

        /*
         * limit the displayed messages.
         */
        private void LimitMessageCount(ListBox lb)
        {
            if (CMList.Count > MessageLimit)
            {
                CMList.RemoveAt(0);
                lb.Items.RemoveAt(0);
            }
        }

        /*
         * returns a listboxitem with formatting common between incoming and outgoing mesage types.
         */
        private ListBoxItem GetCommonFormatting(ChatMessage cm, ListBox lb)
        {
            Grid grid = cm.GetMessageAsGrid();
            ListBoxItem lbi = new ListBoxItem();
            CMList.Add(cm);

            lbi.Content = grid;
            if (lb.ActualWidth - ReduceWidthBy > 5)
                lbi.Width = lb.ActualWidth - ReduceWidthBy;
            else
                lbi.Width = 5;
            lbi.Focusable = false;
            lbi.BorderThickness = new Thickness(0);
            return lbi;
        }
    }
}
