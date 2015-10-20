using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseIt.Commands.Windows.MsBuilds;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class MsbuildTest
    {
        private string ProjectPath
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
            if (Directory.Exists(webFolder))
            {
                Directory.Delete(webFolder, true);
            }
            var setting = new BuildSetting
            {
                BuildConfiguration = "Release",
                IsWeb = true,
                ProjectPath = ProjectPath,
                OutputDirectory = "PublishFolder/Web"
            };

            var s = new MsBuildCommand(setting);
            var executeSetting = new ExecuteSetting("./");
            var arguments = s.BuildArguments(executeSetting);
            var expected =
                ProjectPath+ @" /t:_CopyWebApplication;_WPPCopyWebApplication;TransformWebConfig /p:Configuration:Release;WebProjectOutputDir:PublishFolder/Web";
            Assert.AreEqual(expected, arguments);
        }

        [TestMethod]
        public void TestLogLevel()
        {
        }
    }
}