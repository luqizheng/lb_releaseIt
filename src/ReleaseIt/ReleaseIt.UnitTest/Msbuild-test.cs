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

        [TestCleanup]
        public void Clearup()
        {
            var webFolder = Path.Combine(Environment.CurrentDirectory, "PublishFolder", "web");
            if (Directory.Exists(webFolder))
            {
                Directory.Delete(webFolder, true);
            }
        }

        [TestMethod]
        public void WebBuild_Release()
        {
            var webFolder = Path.Combine(Environment.CurrentDirectory, "PublishFolder", "web");
            var setting = new CompileSetting
            {
                BuildConfiguration = "Release",
                IsWeb = true,
                ProjectPath = ProjectPath,
                BuildLogFile = false
                //OutputDirectory = "PublishFolder/Web"
            };

            var msbuild = new MsBuildCommand(setting);
            var executeSetting = new ExecuteSetting("./");
            var arguments = msbuild.BuildArguments(executeSetting);

            Assert.AreEqual(
                @"""" + ProjectPath + @""" /p:Configuration=Release /t:_CopyWebApplication;_WPPCopyWebApplication;TransformWebConfig",
                arguments);
        }

        [TestMethod]
        public void TestWebBuild()
        {
            var setting = new CompileSetting
            {
                BuildConfiguration = "Release",
                IsWeb = true,
                ProjectPath = ProjectPath,
                OutputDirectory = "PublishFolder/Web"
            };

            var target = new MsBuildCommand(setting);
            var executeSetting = new ExecuteSetting("./");
            var arguments = target.BuildArguments(executeSetting);
            var expected = "\"" + ProjectPath +
                           @""" /p:Configuration=Release;WebProjectOutputDir=""./PublishFolder/Web"" /t:_CopyWebApplication;_WPPCopyWebApplication;TransformWebConfig";
            Assert.AreEqual(expected, arguments);
        }

        [TestMethod]
        public void TestLogLevel()
        {
        }
    }
}