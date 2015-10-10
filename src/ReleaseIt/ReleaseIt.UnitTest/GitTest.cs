using System;
using System.IO;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseIt.WindowCommand.VersionControls;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class GitTest
    {
        [TestMethod]
        public void GitMakeUrl()
        {
            var git = new Git()
            {
                Url = "https://github.com/aspnet/Identity.git",
                UserName = "Test",
                Password = "Pwd"
            };

            var s = git.MakeUrl();
            Assert.AreEqual("https://" + git.UserName + ":" + git.Password + "@github.com/aspnet/Identity.git", s);
        }

        [TestMethod]
        public void TryToGet()
        {
            var commandFactory = CommandSet.Create();
            commandFactory.Git().Url("https://github.com/aspnet/Identity.git");
            //commandFactory.Invoke("test");
            commandFactory.Save("a.json");
            Assert.IsTrue(Directory.Exists("test"));
            Assert.IsTrue(Directory.Exists("test/Identity"));



        }
    }
}
