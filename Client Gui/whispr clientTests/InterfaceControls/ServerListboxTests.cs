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
    public class ServerListboxTests
    {
        [TestMethod()]
        public void ServerListboxTest()
        {
            ServerListbox slb = new ServerListbox();
            Assert.IsNotNull(slb);

        }

        /*
        [TestMethod()]
        public void UpdateFromOnlineListTest()
        {
            ServerListbox slb = new ServerListbox();
            List<String> serverlist = new List<string>{ "s1", "s2", "s3" };
            List<String> idlist = new List<string> { "id1", "id2", "id3" };
            slb.UpdateFromOnlineList(serverlist, idlist);
            Assert.IsNotNull(slb);
            Assert.AreEqual(slb.GetServerCount(), 3);
        }
        */
    }
}