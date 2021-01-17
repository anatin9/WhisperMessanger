using Microsoft.VisualStudio.TestTools.UnitTesting;
using whispr_client.InterfaceControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace whispr_client.InterfaceControls.Tests
{
    [TestClass()]
    public class ChatHistoryListboxTests
    {
        [TestMethod()]
        public void ChatHistoryListboxTest()
        {
            ChatMessage cm = new ChatMessage();
            ListBox testlb = new ListBox();
            cm.Message = "!ASGHG#$T@asd;o8ag";
            cm.Sender = "Someone";
            cm.Received = DateTime.Now;

            ChatHistoryListbox chl = new ChatHistoryListbox();

            Assert.IsNotNull(chl);
            
        }

        
        [TestMethod()]
        public void NewIncomingChatMessageTest()
        {
            ChatMessage cm = new ChatMessage();
            ListBox testlb = new ListBox();
            cm.Message = "!ASGHG#$T@asd;o8ag";
            cm.Sender = "Someone";
            cm.Received = DateTime.Now;
            ChatHistoryListbox chl = new ChatHistoryListbox();
            chl.NewIncomingChatMessage(cm, testlb);
            Assert.AreEqual(testlb.Items.Count, 1);
        }
    }
}