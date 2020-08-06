using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRPA.PMTech.SSH;
using System;
using System.Activities;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSHActivityTest
{
    [TestClass]
    public class UnitTest1
    {

        private static string YOUR_HOST     = "sshservername";
        private static int    YOUR_PORT     = 22;
        private static string YOUR_USER     = "yourusername";
        private static string YOUR_PASSWORD = "yourpassword";

        [TestMethod]
        public void SSHExecuterTest()
        {
            RunCommandActivity activity = new RunCommandActivity();
            activity.Host = YOUR_HOST;
            activity.Port = YOUR_PORT;
            activity.User = YOUR_USER;
            activity.Password = YOUR_PASSWORD;
            activity.Command = "ls -l";

            var output = WorkflowInvoker.Invoke(activity);

            Assert.AreEqual(Convert.ToInt32(output["ExitStatus"]), 0);
        }
    }
}
