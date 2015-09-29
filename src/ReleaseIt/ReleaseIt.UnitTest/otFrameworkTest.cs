using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class DotFrameworkManager
    {
        [TestMethod]
        public void TestMethod1()
        {
            DotNetFrameworkManager frameworkManager = new DotNetFrameworkManager();
            var dotFramework = frameworkManager.GetVersion();
            Assert.IsNotNull(dotFramework);
            Assert.IsTrue(dotFramework.Count > 0);
        }
    }
}
