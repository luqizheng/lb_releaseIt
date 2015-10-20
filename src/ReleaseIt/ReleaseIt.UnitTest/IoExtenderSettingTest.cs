using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class IoExtenderSettingTest
    {
        [TestMethod]
        public void TestGetPath_Relative()
        {
            var working = Environment.CurrentDirectory;

            var path = IoExtender.GetPath(working, "../a");

            var expectDir = Path.GetFullPath(working + "/../a");
            Assert.AreEqual(expectDir, path);
        }

        [TestMethod]
        public void TestGetPath_subFolder()
        {
            var working = Environment.CurrentDirectory;

            var path = IoExtender.GetPath(working, "/a");

            var expectDir = Path.GetFullPath(working + "/a");
            Assert.AreEqual(expectDir, path);
        }

        [TestMethod]
        public void TestGetPath_networkPath()
        {
            var working = Environment.CurrentDirectory;

            var path = IoExtender.GetPath(working, @"\\192.168.0.182");


            Assert.AreEqual(@"\\192.168.0.182", path);
        }


        [TestMethod]
        public void TestAutoCreateDiur()
        {
            var dirPath = @"\\192.168.0.56\ETM_V3_Domain\newDir1\newdir2\newdir3";

            var myPath = new DirectoryInfo(dirPath);

            myPath.CreateEx();
        }
    }
}