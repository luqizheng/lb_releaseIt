using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReleaseIt.UnitTest
{
    [TestClass]
    public class UnitTestCommandSet
    {
        [TestMethod]
        public void From_complex_un_sortId_and_dependy()
        {
            var actual = new List<string>();
            var commands = new List<ICommand>
            {
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "1"
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "2",
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "1-1",
                        Dependency="1"
                    }
                },
                 new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "3"
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "1-1-1",
                        Dependency="1-1"
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "1-2",
                        Dependency="1"
                    }
                }
            };
            var st = new CommandSet(new ExecuteSetting("./"), commands);
            st.Skip.Add("1");
            st.Invoke();

            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual("2", actual[0]);
            Assert.AreEqual("3", actual[1]);
        }
        [TestMethod]
        public void From_Skip_step()
        {
            var actual = new List<string>();
            var commands = new List<ICommand>
            {
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "1"
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "2",
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "3"
                    }
                }
            };
            var st = new CommandSet(new ExecuteSetting("./"), commands);
            st.Skip.Add("1");
            st.Invoke();

            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual("2", actual[0]);
            Assert.AreEqual("3", actual[1]);
        }
        [TestMethod]
        public void From_Run_Auto_Step()
        {
            var result = new List<string>();
            var commands = new List<ICommand>
            {
                new CommandMock(result)
                {
                    Setting = new SettingMock
                    {
                        Id = "1"
                    }
                },
                new CommandMock(result)
                {
                    Setting = new SettingMock
                    {
                        Id = "2",
                        Dependency="1"
                    }
                },
                new CommandMock(result)
                {
                    Setting = new SettingMock
                    {
                        Dependency = "2",
                        Id = "3"
                    }
                }
            };
            var st = new CommandSet(new ExecuteSetting("./"), commands);

            st.Include.Add("3");
            st.Invoke();

            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual("1", result[0]);
            Assert.AreEqual("2", result[1]);
            Assert.AreEqual("3", result[2]);

        }

        [TestMethod]
        public void Test_Tags_Order()
        {
            var actual = new List<string>();
            var commands = new List<ICommand>
            {
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "1",
                        Tags = new[]
                        {
                            "Run"
                        }
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "2",
                    }
                },
                new CommandMock(actual)
                {
                    Setting = new SettingMock
                    {
                        Id = "3",
                          Tags = new[]
                        {
                            "Run"
                        }
                    }
                }
            };
            var st = new CommandSet(new ExecuteSetting("./"), commands);
            st.IncludeTags.Add("Run");
            st.Invoke();

            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual("1", actual[0]);
            Assert.AreEqual("3", actual[1]);
        }

    }
}