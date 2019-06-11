using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows.Installation
{
    public class InstallStep : Def
    {
        public virtual bool Completed { get; set; } = false;

        public Type windowContentsDriver = typeof(WindowContentsDriver);

        private WindowContentsDriver driver = null;

        public WindowContentsDriver Driver
        {
            get
            {
                if (driver == null)
                {
                    driver = (WindowContentsDriver)Activator.CreateInstance(this.windowContentsDriver);
                }

                return driver;
            }
        }

        public virtual void DoWindowContents(Listing_Standard listing)
        {
            Driver.DoWindowContents(listing);
        }
    }

    public class WindowContentsDriver
    {
        public virtual void DoWindowContents(Listing_Standard listing)
        {

        }

        public virtual void PostInstall()
        {

        }
    }

}
