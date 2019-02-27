using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace TwitchStories{

    public class ViewerCoins : TwitchSettings
    {
        public List<Viewer> viewers = new List<Viewer>();

        public override void ExposeData()
        {
            Scribe_Values.Look(ref viewers, "viewers", LookMode.Reference);
        }
    }

}


