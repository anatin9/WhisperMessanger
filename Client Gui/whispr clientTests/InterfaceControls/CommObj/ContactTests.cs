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
    public class ContactTests
    {
        [TestMethod()]
        public void CreateNewTest()
        {
            Contact c1 = new Contact();
            c1.CreateNew("Some User", true);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some User");
            Assert.AreEqual(c1.Online, true);
            Assert.IsNotNull(c1.LastOnline);
            Assert.IsNull(c1.GetKey());
        }

        [TestMethod()]
        public void CreateNewTest1()
        {
            Contact c1 = new Contact();
            DateTime currentTime = DateTime.Now;
            c1.CreateNew("Some new User", false, currentTime);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some new User");
            Assert.AreEqual(c1.Online, false);
            Assert.IsNotNull(c1.LastOnline);
            Assert.AreEqual(c1.LastOnline, currentTime);
            Assert.IsNull(c1.GetKey());
        }

        [TestMethod()]
        public void UpdateKeyTest()
        {
            Contact c1 = new Contact();
            c1.CreateNew("Some User", true);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some User");
            Assert.AreEqual(c1.Online, true);
            Assert.IsNotNull(c1.LastOnline);
            Assert.IsNull(c1.GetKey());

            c1.UpdateKey("ASDF()U&!@NTGAE)R");
            Assert.IsNotNull(c1.GetKey());
            Assert.AreEqual(c1.GetKey(), "ASDF()U&!@NTGAE)R");


            c1 = new Contact();
            DateTime currentTime = DateTime.Now;
            c1.CreateNew("Some new User", false, currentTime);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some new User");
            Assert.AreEqual(c1.Online, false);
            Assert.IsNotNull(c1.LastOnline);
            Assert.AreEqual(c1.LastOnline, currentTime);
            Assert.IsNull(c1.GetKey());

            c1.UpdateKey("ASDF()U&!@%sdfhxjty)R");
            Assert.IsNotNull(c1.GetKey());
            Assert.AreEqual(c1.GetKey(), "ASDF()U&!@%sdfhxjty)R");
        }

        [TestMethod()]
        public void SetOfflineTest()
        {
            Contact c1 = new Contact();
            c1.CreateNew("Some User", true);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some User");
            Assert.AreEqual(c1.Online, true);
            Assert.IsNotNull(c1.LastOnline);
            Assert.IsNull(c1.GetKey());

            c1.SetOffline();
            Assert.AreEqual(c1.Online, false);
            Assert.IsNotNull(c1.LastOnline);


            c1 = new Contact();
            DateTime currentTime = DateTime.Now;
            c1.CreateNew("Some new User", false, currentTime);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some new User");
            Assert.AreEqual(c1.Online, false);
            Assert.IsNotNull(c1.LastOnline);
            Assert.AreEqual(c1.LastOnline, currentTime);
            Assert.IsNull(c1.GetKey());

            c1.SetOffline();
            Assert.AreEqual(c1.Online, false);
            Assert.IsNotNull(c1.LastOnline);
        }

        [TestMethod()]
        public void SetOnlineTest()
        {
            Contact c1 = new Contact();
            c1.CreateNew("Some User", true);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some User");
            Assert.AreEqual(c1.Online, true);
            Assert.IsNotNull(c1.LastOnline);
            Assert.IsNull(c1.GetKey());

            c1.SetOnline();
            Assert.AreEqual(c1.Online, true);


            c1 = new Contact();
            DateTime currentTime = DateTime.Now;
            c1.CreateNew("Some new User", false, currentTime);

            Assert.IsNotNull(c1);
            Assert.AreEqual(c1.UserName, "Some new User");
            Assert.AreEqual(c1.Online, false);
            Assert.IsNotNull(c1.LastOnline);
            Assert.AreEqual(c1.LastOnline, currentTime);
            Assert.IsNull(c1.GetKey());

            c1.SetOnline();
            Assert.AreEqual(c1.Online, true);
            Assert.IsNotNull(c1.LastOnline);
        }
    }
}