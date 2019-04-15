using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store
{
    public abstract class IncidentHelper
    {
        public abstract bool IsPossible();
        public abstract void TryExecute();
        public StoreIncident storeIncident = null;
    }

    public abstract class IncidentHelperVariables
    {
        public abstract bool IsPossible(string message, Viewer viewer, bool separateChannel = false);
        public abstract void TryExecute();
        public StoreIncidentVariables storeIncident = null;
        public abstract Viewer viewer { get; set; }
    }
}
