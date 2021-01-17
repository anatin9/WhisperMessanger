using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;


namespace WhisprUnitTests
{
    [TestClass]
    public class ConvIDTest
    {
        [TestMethod]
        public void TestConvID()
        {
            ConversationId test = new ConversationId();
            string NewID = test.Id;
            Assert.IsNotNull(NewID);
        }
    }

}
