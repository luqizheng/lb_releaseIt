using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class Msbuild_test
    {
        public MsBuild GetMsBuild()
        {
            var manager = new DotNetFrameworkManager();
            var version = manager.GetVersion().Last();
            var msBuild = version.MsBuild;
            return msBuild;
        }

        [TestMethod]
        public void TestBuild()
        {

            var msBuild = GetMsBuild();

            Assert.IsTrue(msBuild.IsEffect);

            var debugFolder = new DirectoryInfo(Environment.CurrentDirectory);
            var c = debugFolder.Parent.Parent.Parent;

            
            var csproj = Path.Combine(c.FullName, "ReleaseIt\\ReleaseIt.csproj");
            msBuild.LogLevel = LogLevel.quite;
            msBuild.BuildTo(csproj);

        }

        [TestMethod]
        public void TestLogLevel()
        {
            var msBuild = GetMsBuild();
            msBuild.LogLevel = LogLevel.quite;

        }
    }
}
