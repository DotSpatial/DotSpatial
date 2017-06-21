namespace $safeprojectname$
{
    using System;
    using System.Collections.Generic;
    $if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
    $endif$using System.Text;

    using DotSpatial.Controls;
    using DotSpatial.Controls.Header;

    public class MyPlugin1 : Extension
    {
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("My Button Caption", ButtonClick));
            base.Activate();
        }

          public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            // Your logic goes here.
        }
    }
}
