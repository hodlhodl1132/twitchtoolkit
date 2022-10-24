using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TwitchToolkit;

internal static class SettingsHelper
{
	public class LabeledRadioValue<T>
	{
		private string label;

		private T val;

		public string Label
		{
			get
			{
				return label;
			}
			set
			{
				label = value;
			}
		}

		public T Value
		{
			get
			{
				return val;
			}
			set
			{
				val = value;
			}
		}

		public LabeledRadioValue(string label, T val)
		{
			Label = label;
			Value = val;
		}
	}

	public static void SliderLabeled(this Listing_Standard ls, string label,  int val, string format, float min = 0f, float max = 100f, string tooltip = null)
	{
		float fVal = val;
		ls.SliderLabeled(label,  fVal, format, min, max);
		val = (int)fVal;
	}

	public static void SliderLabeled(this Listing_Standard ls, string label,  float val, string format, float min = 0f, float max = 1f, string tooltip = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0013: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0018: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0024: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0029: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0033: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0038: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0044: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0049: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0054: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0065: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing erences)
		Rect rect = ((Listing)ls).GetRect(Text.LineHeight);
		Rect rect2 = GenUI.Rounded(GenUI.LeftPart(rect, 0.7f));
		Rect rect3 = GenUI.Rounded(GenUI.LeftPart(GenUI.Rounded(GenUI.RightPart(rect, 0.3f)), 0.67f));
		Rect rect4 = GenUI.Rounded(GenUI.RightPart(rect, 0.1f));
		TextAnchor anchor = Text.Anchor;
		Text.Anchor =((TextAnchor)3);
		Widgets.Label(rect2, label);
		float result = (val = Widgets.HorizontalSlider(rect3, val, min, max, true, (string)null, (string)null, (string)null, -1f));
		Text.Anchor =((TextAnchor)5);
		string buffer = val.ToString();
		Widgets.TextFieldNumeric<float>(rect4,ref val, ref buffer, 0f, 1E+09f);
		if (!GenText.NullOrEmpty(tooltip))
		{
			TooltipHandler.TipRegion(rect, (TipSignal)(tooltip));
		}
		Text.Anchor =(anchor);
		((Listing)ls).Gap(((Listing)ls).verticalSpacing);
	}

	public static void FloatRange(this Listing_Standard ls, string label,  FloatRange range, float min = 0f, float max = 1f, string tooltip = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0013: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0018: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0024: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0029: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0033: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0038: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0052: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0057: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0087: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00af: Unknown result type (might be due to invalid IL or missing erences)
		Rect rect = ((Listing)ls).GetRect(Text.LineHeight);
		Rect rect2 = GenUI.Rounded(GenUI.LeftPart(rect, 0.7f));
		Rect rect3 = GenUI.Rounded(GenUI.LeftPart(GenUI.Rounded(GenUI.RightPart(rect, 0.3f)), 0.9f));
		rect3.yMin = rect3.yMin - 5f;
		TextAnchor anchor = Text.Anchor;
		Text.Anchor =((TextAnchor)3);
		Widgets.Label(rect2, label);
		Text.Anchor =((TextAnchor)5);
		int id = ((Listing)ls).CurHeight.GetHashCode();
		Widgets.FloatRange(rect3, id, ref range, min, max, (string)null, valueStyle);
		if (!GenText.NullOrEmpty(tooltip))
		{
			TooltipHandler.TipRegion(rect, (TipSignal)(tooltip));
		}
		Text.Anchor =(anchor);
		((Listing)ls).Gap(((Listing)ls).verticalSpacing);
	}

	public static bool CenteredButton(this Listing_Standard ls, string label, float buttonWidth = 250f, float buttonHeight = 30f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002c: Unknown result type (might be due to invalid IL or missing erences)
		Rect rect = ((Listing)ls).GetRect(Text.LineHeight);
		Rect rect2 = new Rect(rect.width - buttonWidth / 2f, rect.y, buttonWidth, buttonHeight);
		bool result = Widgets.ButtonText(rect2, label, true, false, true);
		((Listing)ls).Gap(((Listing)ls).verticalSpacing * 2f);
		return result;
	}

	public static Rect GetRect(this Listing_Standard listing_Standard, float? height = null)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0020: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		return ((Listing)listing_Standard).GetRect(height ?? Text.LineHeight);
	}

	public static void AddLabeledRadioList(this Listing_Standard listing_Standard, string header, string[] labels,  string val, float? headerHeight = null)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing erences)
		if (header != string.Empty)
		{
			Widgets.Label(listing_Standard.GetRect(headerHeight), header);
		}
		listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels), val);
	}

	private static void AddRadioList<T>(this Listing_Standard listing_Standard, List<LabeledRadioValue<T>> items,  T val, float? height = null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001c: Unknown result type (might be due to invalid IL or missing erences)
		foreach (LabeledRadioValue<T> item in items)
		{
			Rect lineRect = listing_Standard.GetRect(height);
			if (Widgets.RadioButtonLabeled(lineRect, item.Label, EqualityComparer<T>.Default.Equals(item.Value, val)))
			{
				val = item.Value;
			}
		}
	}

	private static List<LabeledRadioValue<string>> GenerateLabeledRadioValues(string[] labels)
	{
		List<LabeledRadioValue<string>> list = new List<LabeledRadioValue<string>>();
		foreach (string label in labels)
		{
			list.Add(new LabeledRadioValue<string>(label, label));
		}
		return list;
	}

	public static void AddLabeledTextField(this Listing_Standard listing_Standard, string label,  string settingsValue, float leftPartPct = 0.5f)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0016: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0027: Unknown result type (might be due to invalid IL or missing erences)
		listing_Standard.LineRectSpilter(out var leftHalf, out var rightHalf, leftPartPct);
		Widgets.Label(leftHalf, label);
		string buffer = settingsValue.ToString();
		settingsValue = Widgets.TextField(rightHalf, buffer);
	}

	public static void AddLabeledNumericalTextField<T>(this Listing_Standard listing_Standard, string label,  T settingsValue, float leftPartPct = 0.5f, float minValue = 1f, float maxValue = 100000f) where T : struct
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0016: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002b: Unknown result type (might be due to invalid IL or missing erences)
		listing_Standard.LineRectSpilter(out var leftHalf, out var rightHalf, leftPartPct);
		Widgets.Label(leftHalf, label);
		string buffer = settingsValue.ToString();
		Widgets.TextFieldNumeric<T>(rightHalf, ref settingsValue, ref buffer, minValue, maxValue);
	}

	public static Rect LineRectSpilter(this Listing_Standard listing_Standard, out Rect leftHalf, float leftPartPct = 0.5f, float? height = null)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0011: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0016: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001f: Unknown result type (might be due to invalid IL or missing erences)
		Rect lineRect = listing_Standard.GetRect(height);
		leftHalf = GenUI.Rounded(GenUI.LeftPart(lineRect, leftPartPct));
		return lineRect;
	}

	public static Rect LineRectSpilter(this Listing_Standard listing_Standard, out Rect leftHalf, out Rect rightHalf, float leftPartPct = 0.5f, float? height = null)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0015: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0024: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0025: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0028: Unknown result type (might be due to invalid IL or missing erences)
		Rect lineRect = listing_Standard.LineRectSpilter(out leftHalf, leftPartPct, height);
		rightHalf = GenUI.Rounded(GenUI.RightPart(lineRect, 1f - leftPartPct));
		return lineRect;
	}

	public static void DrawTexturedLineHorizontal(float x, float y, float length, Texture texture)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing erences)
		Rect position = new(x,y, length, 1f);
		GUI.DrawTexture(position, texture);
	}
}
