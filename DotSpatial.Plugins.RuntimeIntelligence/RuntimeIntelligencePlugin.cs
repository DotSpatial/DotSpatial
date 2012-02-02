using System;
using System.Threading;
using DotSpatial.Controls;
using PreEmptive.Attributes;

namespace DotSpatial.Plugins.RuntimeIntelligence
{
    public class RuntimeIntelligencePlugin : Extension
    {
        // See http://runtimeintelligence.codeplex.com/
        [Setup(CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx")]
        public override void Activate()
        {
            //App.HeaderControl.Add(new SimpleActionItem("My Button Caption", ButtonClick));

            // AppDomain.CurrentDomain.DomainUnload += (sender, e) => Teardown();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => Teardown();

            base.Activate();
        }

        [Teardown]
        private static void Teardown()
        {
            Thread.Sleep(0);
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        //[Feature("Button Click")]
        //public void ButtonClick(object sender, EventArgs e)
        //{
        //    // Your logic goes here.
        //}
    }
}