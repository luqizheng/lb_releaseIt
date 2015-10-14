using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseIt.WindowCommand.VersionControls;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class SvnTest
    {
        [TestMethod]
        public void TestCheckout()
        {
            var version = new VersionControlSetting
            {
                Url = "http://www.svnchina.com/svn/release_it "
            };

            Svn svn = new Svn(version);
            var actual = svn.BuildArguments(new ExecuteSetting("./"));
            var expect = "checkout " + version.Url + " ./release_it ";

            Assert.AreEqual(expect, actual);

        }
        [TestMethod]
        public void TestCheckout_withAuth()
        {
            var version = new VersionControlSetting
            {
                Url = "http://www.svnchina.com/svn/release_it",
                UserName = "username",
                Password = "wd"
            };

            Svn svn = new Svn(version);
            var actual = svn.BuildArguments(new ExecuteSetting("./"));
            var expect = "checkout " + version.Url + " ./release_it --username " + version.UserName + " --password " + version.Password;

            Assert.AreEqual(expect, actual);

        }
    }
}