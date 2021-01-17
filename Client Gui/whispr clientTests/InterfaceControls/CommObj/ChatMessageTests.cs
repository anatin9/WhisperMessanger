using Microsoft.VisualStudio.TestTools.UnitTesting;
using whispr_client.InterfaceControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whispr_client.InterfaceControls.Tests
{
    [TestClass()]
    public class ChatMessageTests
    {
        [TestMethod()]
        public void GetMessageAsGridTest()
        {
            ChatMessage cm = new ChatMessage();
            cm.Message = "Some New Test !%^86453][/CCVBN@#%^UR%ISX\t";
            cm.Sender = "Some User";
            DateTime now = DateTime.Now;
            cm.Received = now;
            var test = cm.GetMessageAsGrid();
            Assert.IsNotNull(test);
            Assert.AreEqual(cm.Message, "Some New Test !%^86453][/CCVBN@#%^UR%ISX\t");
            Assert.AreEqual(cm.Sender, "Some User");
            Assert.AreEqual(cm.Received, now);
            Assert.AreEqual(test.RowDefinitions.Count, 2);

        }
    }
}