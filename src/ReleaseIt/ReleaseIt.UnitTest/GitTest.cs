using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseIt.Commands.Windows.VersionControls;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class GitTest
    {
        [TestMethod]
        public void GitMakeUrl()
        {
            var gitSetting = new VersionControlSetting
            {
                Url = "https://github.com/aspnet/Identity.git",
                UserName = "Test",
                Password = "Pwd"
            };
            var git = new Git(gitSetting);
            var s = git.MakeUrl();
            Assert.AreEqual(
                "https://" + gitSetting.UserName + ":" + gitSetting.Password + "@github.com/aspnet/Identity.git", s);
        }

        [TestMethod]
        public void Git_Clone_BuildArguments()
        {
            var gitSetting = new VersionControlSetting
            {
                Url = "https://github.com/aspnet/Identity.git",
                UserName = "Test",
                Password = "Pwd",
                WorkingCopy = "%gitName%"
            };
            var git = new Git(gitSetting);

            var executeSetting = new ExecuteSetting("./noexistFolder");
            var arguments = git.BuildArguments(executeSetting);
            var expect = @"clone https://Test:Pwd@github.com/aspnet/Identity.git ./noexistFolder\Identity";
            Assert.AreEqual(expect, arguments);
        }

        [TestMethod]
        public void Git_pull_BuildArguments()
        {
            var gitSetting = new VersionControlSetting
            {
                Url = "https://github.com/aspnet/Identity.git",
                UserName = "Test",
                Password = "Pwd",
                WorkingCopy = "../"
            };
            var git = new Git(gitSetting);

            var executeSetting = new ExecuteSetting("./noexistFolder");
            var arguments = git.BuildArguments(executeSetting);
            var expect = @"pull";
            Assert.AreEqual(expect, arguments);
        }

        [TestMethod]
        public void BuildArguments_clone_without_pwd()
        {
            var gitSetting = new VersionControlSetting
            {
                Url = "https://github.com/aspnet/Identity.git",
                WorkingCopy = "newcopy"
            };
            var git = new Git(gitSetting);

            var executeSetting = new ExecuteSetting(Environment.CurrentDirectory);
            var arguments = git.BuildArguments(executeSetting);
            var expect = @"clone https://github.com/aspnet/Identity.git " +
                         Path.Combine(Environment.CurrentDirectory, gitSetting.WorkingCopy);
            Assert.AreEqual(expect, arguments);
        }
    }
}