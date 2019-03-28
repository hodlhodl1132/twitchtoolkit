using TwitchToolkitDev;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    class Settings_Integrations
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.CheckboxLabeled("TwitchToolkitStreamlabsSync".Translate(), ref ToolkitSettings.SyncStreamLabs);

            optionsListing.Gap();

            //optionsListing.CheckboxLabeled("TwitchToolkitStreamelementsSync".Translate(), ref ToolkitSettings.SyncStreamElements);

            optionsListing.Label("Temp Stream Elements - WILL UPDATE LATER");
            if (optionsListing.CenteredButton("TwitchToolkitImportStreamElements".Translate()))
                StreamElements.ImportPoints();

            if (optionsListing.CenteredButton("TwitchToolkitExportStreamElements".Translate()))
                StreamElements.SyncViewerStatsToWeb();
        }
    }
}
