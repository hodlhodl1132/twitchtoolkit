using System;
using System.Linq;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class StoreIncidentEditor : Window
{
	public bool checkedForBackup = false;

	public bool haveBackup = false;

	public bool variableIncident = false;

	public StoreIncident storeIncident = null;

	public StoreIncidentVariables storeIncidentVariables = null;

	private string[] karmaTypeStrings = null;

	private string setKarmaType = "";

	public override Vector2 InitialSize => new Vector2(500f, 500f);

	public StoreIncidentEditor(StoreIncident storeIncident)
	{
		base.doCloseButton = true;
		this.storeIncident = storeIncident;
		if (storeIncident == null)
		{
			throw new ArgumentNullException();
		}
		MakeSureSaveExists();
		StoreIncidentVariables storeIncidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find((StoreIncidentVariables s) => ((Def)s).defName == ((Def)storeIncident).defName);
		if (storeIncidentVariables != null)
		{
			this.storeIncidentVariables = storeIncidentVariables;
			variableIncident = true;
		}
		karmaTypeStrings = Enum.GetNames(typeof(KarmaType));
		setKarmaType = storeIncident.karmaType.ToString();
	}

	public override void PostClose()
	{
		MakeSureSaveExists();
		Store_IncidentEditor.UpdatePriceSheet();
		((Mod)Toolkit.Mod).WriteSettings();
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004c: Unknown result type (might be due to invalid IL or missing erences)
		if (!checkedForBackup || !haveBackup)
		{
			MakeSureSaveExists();
			return;
		}
		Listing_Standard ls = new Listing_Standard();
		((Listing)ls).Begin(inRect);
		ls.Label(GenText.CapitalizeFirst(((Def)storeIncident).label), -1f, (string)null);
		((Listing)ls).Gap(12f);
		if (storeIncident.cost > 0)
		{
			storeIncident.abbreviation = ls.TextEntryLabeled("Purchase Code:", storeIncident.abbreviation, 1);
			((Listing)ls).Gap(12f);
			ls.AddLabeledNumericalTextField("Cost",  storeIncident.cost);
			ls.SliderLabeled("Max times per " + ToolkitSettings.EventCooldownInterval + " ingame day(s)",  storeIncident.eventCap, storeIncident.eventCap.ToString(), 0f, 15f);
			if (variableIncident && storeIncidentVariables.maxWager > 0)
			{
				((Listing)ls).Gap(12f);
				ls.SliderLabeled("Maximum coin wager",  storeIncidentVariables.maxWager, storeIncidentVariables.cost.ToString(), storeIncident.cost, 20000f);
				if (storeIncidentVariables.maxWager < storeIncidentVariables.cost)
				{
					storeIncidentVariables.maxWager = storeIncidentVariables.cost * 2;
				}
			}
			((Listing)ls).Gap(12f);
			ls.AddLabeledRadioList("Karma Type", karmaTypeStrings,  setKarmaType);
			storeIncident.karmaType = (KarmaType)Enum.Parse(typeof(KarmaType), setKarmaType);
			((Listing)ls).Gap(12f);
			if (ls.ButtonTextLabeled("Disable Store Incident", "Disable"))
			{
				storeIncident.cost = -10;
			}
		}
		((Listing)ls).Gap(12f);
		if (((Def)storeIncident).defName == "Item")
		{
			ls.SliderLabeled("Max times per " + ToolkitSettings.EventCooldownInterval + " ingame day(s)",  storeIncident.eventCap, storeIncident.eventCap.ToString(), 0f, 15f);
			((Listing)ls).Gap(12f);
		}
		if (variableIncident && storeIncidentVariables.customSettings)
		{
			((Listing)ls).Gap(12f);
			if (ls.ButtonTextLabeled("Edit Extra Settings", "Settings"))
			{
				storeIncidentVariables.settings.EditSettings();
			}
		}
		((Listing)ls).Gap(12f);
		if (((Def)storeIncident).defName != "Item" && ls.ButtonTextLabeled("Reset to Default", "Reset"))
		{
			Store_IncidentEditor.LoadBackup(storeIncident);
			if (storeIncident.cost < 1)
			{
				storeIncident.cost = 50;
			}
			setKarmaType = storeIncident.karmaType.ToString();
			MakeSureSaveExists();
		}
		if (((Def)storeIncident).defName == "Item" && ls.ButtonTextLabeled("Edit item prices", "Edit"))
		{
			Type type = typeof(StoreItemsWindow);
			Find.WindowStack.TryRemove(type, true);
			Window window = (Window)(object)new StoreItemsWindow();
			Find.WindowStack.Add(window);
		}
		((Listing)ls).End();
	}

	public void MakeSureSaveExists(bool forceSave = true)
	{
		checkedForBackup = true;
		Helper.Log("Checking if save exists");
		if (storeIncident == null)
		{
			Log.Error("incident is null");
		}
		haveBackup = Store_IncidentEditor.CopyExists(storeIncident);
		if (!haveBackup || forceSave)
		{
			Store_IncidentEditor.SaveCopy(storeIncident);
		}
	}
}
