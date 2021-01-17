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
    public class SendButtonTests
    {
        [TestMethod()]
        public void SendClickedTest()
        {
            TextBox tb = new TextBox();
            tb.Text = "Some test input";
            ListBox lb = new ListBox();

            var output = SendButton.SendClicked(tb, lb);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ChatMessage));
        }
    }
}