using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class SvnTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var faoCommandFactory = new CommandFactory();
            faoCommandFactory.MsBuildForWeb();
        }
    }
}
