using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace ReleaseIt.Log
{
    public class MsBuildLog : Logger, IDisposable
    {
        private readonly StreamWriter sw;

        public MsBuildLog()
        {
            sw = new StreamWriter(DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
        }

        public void Dispose()
        {
            sw.Dispose();
        }


        public override void Initialize(IEventSource eventSource)
        {
            eventSource.ProjectStarted += eventSource_ProjectStarted;
            eventSource.ErrorRaised += eventSource_ErrorRaised;
            eventSource.MessageRaised += eventSource_MessageRaised;
            eventSource.AnyEventRaised += eventSource_AnyEventRaised;
            eventSource.WarningRaised += eventSource_WarningRaised;
            eventSource.StatusEventRaised += eventSource_StatusEventRaised;
            eventSource.TaskStarted += eventSource_TaskStarted;
        }

        private void eventSource_ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            var name = Path.GetFileNameWithoutExtension(e.ProjectFile);
        }

        private void eventSource_TaskStarted(object sender, TaskStartedEventArgs e)
        {
            sw.Write(e.Message);
            Console.WriteLine(e.Message);
        }

        private void eventSource_StatusEventRaised(object sender, BuildStatusEventArgs e)
        {
            sw.Write(e.Message);
            Console.WriteLine(e.Message);
        }

        private void eventSource_WarningRaised(object sender, BuildWarningEventArgs e)
        {
            sw.Write(e.Message);
            Console.WriteLine(e.Message);
        }

        private void eventSource_AnyEventRaised(object sender, BuildEventArgs e)
        {
            sw.Write(e.Message);
            Console.WriteLine(e.Message);
        }

        private void eventSource_MessageRaised(object sender, BuildMessageEventArgs e)
        {
            sw.Write(e.Message);
            Console.WriteLine(e.Message);
        }

        private void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            sw.Write(e.Message);
            Console.WriteLine(e.Message);
        }
    }
}