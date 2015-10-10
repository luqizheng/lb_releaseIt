using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class MsbuildTest
    {
        public string ProjectPath
        {
            get
            {
                var debugFolder = new DirectoryInfo(Environment.CurrentDirectory);
                var c = debugFolder.Parent.Parent.Parent;
                var csproj = Path.Combine(c.FullName, "compileExample", "web", "web.csproj");
                return csproj;
            }
        }

        [TestMethod]
        public void TestWebBuild()
        {
            var webFolder = Path.Combine(Environment.CurrentDirectory, "PublishFolder", "web");
            Directory.Delete(webFolder, true);
            var faoCommandFactory = new CommandSet();
            faoCommandFactory.MsBuild(true)
                .ProjectPath(ProjectPath)
                .Release()
                .CopyTo("PublishFolder/Web");

            faoCommandFactory.Invoke();
            var debugFolder = Path.Combine(webFolder, "Web.config");

            Assert.IsTrue(File.Exists(debugFolder));
        }

        [TestMethod]
        public void TestLogLevel()
        {
        }
    }
}