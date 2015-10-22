using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseIt.IniStore;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class IniWriterUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var executeSetting = new ExecuteSetting("./");
            executeSetting.ForWidnow();
            var commandSet = new CommandSet(executeSetting);
           
            commandSet.Svn("svn_sample")
                .Url("http://svn.address.com/trunk")
                .Auth("username", "password")
                .Tags("tag1", "tab2")
                .WorkingCopy("workongfolder")
                .Tags("tag1", "tag2");


            var manager = new SettingManager();
            manager.Save(commandSet, "svn.ini", true);

            Assert.IsTrue(File.Exists("svn.ini"));

            var target = new CommandSet(new ExecuteSetting("./"));
            manager.ReadSetting(target, "svn.ini");

            var setting = (SvnSetting)target.Settings.First();

            Assert.AreEqual("http://svn.address.com/trunk", setting.Url);
            Assert.AreEqual("username", setting.UserName);
            Assert.AreEqual("password", setting.Password);
            Assert.AreEqual("tag1,tag2", String.Join(",", setting.Tags));
            Assert.AreEqual("workongfolder", setting.WorkingCopy);
            Assert.AreEqual("svn_sample", setting.Id);
        }
    }
}