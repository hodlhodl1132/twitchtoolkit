using System;
using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Settings;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows;

public class Window_HodlBotSettings : Window
{
	private int totalWeightsForCategories = 1;

	private int totalWeightsForKarma = 1;

	public override Vector2 InitialSize => new Vector2(500f, 700f);

	public Window_HodlBotSettings()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0022: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0053: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0204: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		Text.Font =((GameFont)2);
		listing.Label("<color=#4BB543>HodlBot</color> Settings", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)listing).Gap(12f);
		((Listing)listing).GapLine(12f);
		listing.Label("HodlBot chooses events from a random category or type. The chance of one of these categories/types being picked is based on weights below. Setting to 0% disables it.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		string hodlbotMTBDays = Math.Truncate((double)ToolkitSettings.HodlBotMTBDays * 100.0 / 100.0).ToString();
		listing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.HodlBotMTBDays, ref hodlbotMTBDays, 0.5f, 10f);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("Default HodlBot Weights", "Reset Weights"))
		{
			Settings_Storyteller.NewVoteCategoryWeightsHodlBot();
			Settings_Storyteller.NewVoteTypeWeightsHodlBot();
		}
		((Listing)listing).Gap(12f);
		listing.Label("Random Category Weights", -1f, (string)null);
		((Listing)listing).Gap(12f);
		List<string> VoteCategoryList = ToolkitSettings.VoteCategoryWeights.Keys.ToList();
		List<float> VoteCategoryFloatList = ToolkitSettings.VoteCategoryWeights.Values.ToList();
		int newWeights = 0;
		for (int j = 0; j < VoteCategoryList.Count(); j++)
		{
			string buffer = VoteCategoryFloatList[j].ToString();
			float newValue = VoteCategoryFloatList[j];
			float percentage = (float)Math.Round(newValue / (float)totalWeightsForCategories * 100f, 2);
			listing.TextFieldNumericLabeled<float>(VoteCategoryList[j] + " " + percentage + "% - ", ref newValue, ref buffer, 0f, 1E+09f);
			ToolkitSettings.VoteCategoryWeights[VoteCategoryList[j]] = newValue;
			newWeights += (int)newValue;
		}
		totalWeightsForCategories = newWeights;
		((Listing)listing).Gap(12f);
		listing.Label("Random Type Weights", -1f, (string)null);
		((Listing)listing).Gap(12f);
		List<string> VoteTypeList = ToolkitSettings.VoteTypeWeights.Keys.ToList();
		List<float> VoteTypeFloatList = ToolkitSettings.VoteTypeWeights.Values.ToList();
		newWeights = 0;
		for (int i = 0; i < VoteTypeList.Count(); i++)
		{
			string buffer2 = VoteTypeFloatList[i].ToString();
			float newValue2 = VoteTypeFloatList[i];
			float percentage2 = (float)Math.Round(newValue2 / (float)totalWeightsForKarma * 100f, 2);
			listing.TextFieldNumericLabeled<float>(VoteTypeList[i] + " " + percentage2 + "% - ", ref newValue2, ref buffer2, 0f, 1E+09f);
			ToolkitSettings.VoteTypeWeights[VoteTypeList[i]] = newValue2;
			newWeights += (int)newValue2;
		}
		totalWeightsForKarma = newWeights;
		((Listing)listing).End();
	}
}
