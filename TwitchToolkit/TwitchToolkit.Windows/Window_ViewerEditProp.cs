using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_ViewerEditProp : Window
{
	private Viewer viewer = null;

	private EditPropsActions action;

	private EditProp prop;

	private bool allViewers = false;

	private int amount = 0;

	private string amountBuffer = "";

	private string actionBuffer = "";

	private string viewerBuffer = "";

	public override Vector2 InitialSize => new Vector2(300f, 178f);

	public Window_ViewerEditProp(EditPropsActions action, EditProp prop, Viewer viewer = null)
	{
		this.action = action;
		this.prop = prop;
		base.doCloseButton = true;
		Viewers.RefreshViewers();
		if (viewer == null)
		{
			allViewers = true;
			viewerBuffer = "<b>All Viewers</b>";
		}
		else
		{
			allViewers = false;
			this.viewer = viewer;
			viewerBuffer = "<b>" + viewer.username + "</b>";
		}
		if (viewer != null && action == EditPropsActions.Set)
		{
			switch (prop)
			{
			case EditProp.Coins:
				amount = viewer.GetViewerCoins();
				break;
			case EditProp.Karma:
				amount = viewer.GetViewerKarma();
				break;
			}
		}
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0173: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing erences)
		switch (action)
		{
		case EditPropsActions.Give:
			actionBuffer = "Giving " + viewerBuffer + " " + amount + " " + prop.ToString();
			break;
		case EditPropsActions.Set:
			actionBuffer = "Setting " + viewerBuffer + "'s " + prop.ToString() + " to " + amount;
			break;
		case EditPropsActions.Take:
			actionBuffer = "Taking " + amount + " " + prop.ToString() + " from " + viewerBuffer;
			break;
		}
		Rect label = new Rect(0f, 0f, ((Rect)(inRect)).width, 28f);
		Widgets.Label(label, actionBuffer);
		label.y =(((Rect)( label)).y + 28f);
		amountBuffer = amount.ToString();
		Widgets.TextFieldNumeric<int>(label, ref amount, ref amountBuffer, 0f, 1E+09f);
		label.y =(((Rect)( label)).y + 38f);
		if (Widgets.ButtonText(label, action.ToString(), true, true, true))
		{
			UpdateViewers();
		}
	}

	private void UpdateViewers()
	{
		List<Viewer> viewersToUpdate = new List<Viewer>();
		if (allViewers)
		{
			if (action == EditPropsActions.Give)
			{
				foreach (string viewer in Viewers.ParseViewersFromJsonAndFindActiveViewers())
				{
					viewersToUpdate.Add(Viewers.GetViewer(viewer));
				}
			}
			else
			{
				viewersToUpdate = Viewers.All;
			}
		}
		else
		{
			viewersToUpdate.Add(this.viewer);
		}
		switch (action)
		{
		case EditPropsActions.Give:
			if (prop == EditProp.Coins)
			{
				Viewers.GiveAllViewersCoins(amount, viewersToUpdate);
			}
			else if (prop == EditProp.Karma)
			{
				Viewers.GiveAllViewersKarma(amount, viewersToUpdate);
			}
			break;
		case EditPropsActions.Take:
			if (prop == EditProp.Coins)
			{
				Viewers.GiveAllViewersCoins(-amount, viewersToUpdate);
			}
			else if (prop == EditProp.Karma)
			{
				Viewers.GiveAllViewersKarma(-amount, viewersToUpdate);
			}
			break;
		case EditPropsActions.Set:
			if (prop == EditProp.Coins)
			{
				Viewers.SetAllViewersCoins(amount, viewersToUpdate);
			}
			else if (prop == EditProp.Karma)
			{
				Viewers.SetAllViewersKarma(amount, viewersToUpdate);
			}
			break;
		}
		((Window)this).Close(true);
	}
}
