using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseIt.Commands.Windows.VersionControls;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class SvnTest
    {
        [TestMethod]
        public void svn_Checkout_Test()
        {
            var version = new SvnSetting()
            {
                Url = "http://www.svnchina.com/svn/release_it "
            };

            var svn = new SvnCommand(version);
            var actual = svn.BuildArguments(new ExecuteSetting("./"));
            var expect = "checkout " + version.Url + " ./release_it";


            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void Svn_Checkout_withAuth()
        {
            var version = new SvnSetting
            {
                Url = "http://www.svnchina.com/svn/release_it",
                UserName = "username",
                Password = "wd"
            };

            var svn = new SvnCommand(version);
            var actual = svn.BuildArguments(new ExecuteSetting("./"));
            var expect = "checkout " + version.Url + " ./release_it --username " + version.UserName + " --password " +
                         version.Password;

            Assert.AreEqual(expect, actual);
        }
    }
}