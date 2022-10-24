using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.PawnQueue;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_Viewers : Window
{
	private GameComponentPawns component = null;

	private Viewer selectedViewer = null;

	private string viewerBuffer = "";

	private bool allViewers = false;

	private string searchQuery = "";

	private bool resetWarning = false;

	private bool resetAllWarning = false;

	private bool resetCoinWarning = false;

	private bool resetKarmaWarning = false;

	public Window_Viewers()
	{
		Helper.Log("constructing viewers window");
		Viewers.RefreshViewers();
		if (Current.Game != null)
		{
			component = Current.Game.GetComponent<GameComponentPawns>();
		}
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0074: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_019f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0214: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0265: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0272: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0321: Unknown result type (might be due to invalid IL or missing erences)
		//IL_032e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_035e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_038e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0405: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0433: Unknown result type (might be due to invalid IL or missing erences)
		//IL_048f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0599: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0603: Unknown result type (might be due to invalid IL or missing erences)
		//IL_062e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0748: Unknown result type (might be due to invalid IL or missing erences)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing erences)
		//IL_082a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_086f: Unknown result type (might be due to invalid IL or missing erences)
		float firstColumn = ((Rect)( inRect)).width * 0.67f;
		float cellHeight = 28f;
		Rect searchBar = new Rect(0f,0f, firstColumn * 0.66f, cellHeight);
		viewerBuffer = searchQuery;
		searchQuery = Widgets.TextEntryLabeled(searchBar, "Search:", searchQuery);
		searchBar.x =(((Rect)( searchBar)).x + ((Rect)( searchBar)).width);
		searchBar.width = (20f);
		if (Widgets.ButtonText(searchBar, "X", true, true, true) || searchQuery == "" || (searchQuery != viewerBuffer && (selectedViewer != null || allViewers)))
		{
			ClearViewer();
		}
		searchBar.x =(((Rect)( searchBar)).x + ((Rect)( searchBar)).width);
		searchBar.width = (firstColumn / 3f);
		if (Widgets.ButtonText(searchBar, "All Viewers", true, true, true))
		{
			SelectAllViewers();
		}
		if (selectedViewer == null && !allViewers)
		{
			if (searchQuery == "")
			{
				return;
			}
			List<Viewer> searchViewers = Viewers.All.Where((Viewer s) => s.username.Contains(searchQuery.ToLower()) || s.username == searchQuery.ToLower()).Take(6).ToList();
			Rect viewerButton = new Rect(0f, ((Rect)(searchBar)).y + cellHeight, 200f, cellHeight);
			{
				foreach (Viewer viewer in searchViewers)
				{
					if (Widgets.ButtonText(viewerButton, viewer.username, true, true, true))
					{
						SelectViewer(viewer);
					}
					viewerButton.y =(((Rect)( viewerButton)).y + ((Rect)( viewerButton)).height);
				}
				return;
			}
		}
		Rect editLabel = new Rect(0f, ((Rect)(searchBar)).y + cellHeight + 10f, firstColumn, cellHeight);
		Widgets.Label(editLabel, "Editing: " + viewerBuffer);
		Rect smallLabel = new Rect(0f, ((Rect)(editLabel)).y + cellHeight, firstColumn * 0.33f, cellHeight);
		Rect smallButton = new Rect(0f, ((Rect)(smallLabel)).y + cellHeight, firstColumn * 0.33f, cellHeight);
		Widgets.Label(smallLabel, "Coins");
		if (Widgets.ButtonText(smallButton, "Give", true, true, true))
		{
			OpenEditProp(EditPropsActions.Give, EditProp.Coins);
		}
		smallButton.y =(((Rect)( smallButton)).y + cellHeight);
		if (Widgets.ButtonText(smallButton, "Take", true, true, true))
		{
			OpenEditProp(EditPropsActions.Take, EditProp.Coins);
		}
		smallButton.y =(((Rect)( smallButton)).y + cellHeight);
		if (Widgets.ButtonText(smallButton, "Set", true, true, true))
		{
			OpenEditProp(EditPropsActions.Set, EditProp.Coins);
		}
		smallButton.y =(((Rect)( smallLabel)).y + cellHeight);
		smallButton.x =(firstColumn * 0.45f);
		smallLabel.x =(((Rect)( smallButton)).x);
		Widgets.Label(smallLabel, "Karma");
		if (Widgets.ButtonText(smallButton, "Give", true, true, true))
		{
			OpenEditProp(EditPropsActions.Give, EditProp.Karma);
		}
		smallButton.y =(((Rect)( smallButton)).y + cellHeight);
		if (Widgets.ButtonText(smallButton, "Take", true, true, true))
		{
			OpenEditProp(EditPropsActions.Take, EditProp.Karma);
		}
		smallButton.y =(((Rect)( smallButton)).y + cellHeight);
		if (Widgets.ButtonText(smallButton, "Set", true, true, true))
		{
			OpenEditProp(EditPropsActions.Set, EditProp.Karma);
		}
		float viewerInfoHeight = ((Rect)( smallButton)).y + cellHeight;
		if (allViewers)
		{
			smallButton.y =(((Rect)( smallLabel)).y + cellHeight);
			smallButton.x =(firstColumn * 1f);
			smallLabel.x =(((Rect)( smallButton)).x);
			smallLabel.width = (400f);
			if (Widgets.ButtonText(smallButton, "Karma round", true, true, true))
			{
				Viewers.AwardViewersCoins();
			}
			smallButton.y =(((Rect)( smallButton)).y + cellHeight);
			if (Widgets.ButtonText(smallButton, resetAllWarning ? "Are you sure?" : "Reset All", true, true, true))
			{
				if (resetAllWarning)
				{
					Viewers.ResetViewers();
					resetAllWarning = false;
				}
				else
				{
					resetAllWarning = true;
				}
			}
			smallButton.y =(((Rect)( smallButton)).y + cellHeight);
			if (Widgets.ButtonText(smallButton, resetCoinWarning ? "Are you sure?" : "Reset All Coins", true, true, true))
			{
				if (resetCoinWarning)
				{
					Viewers.ResetViewersCoins();
					resetCoinWarning = false;
				}
				else
				{
					resetCoinWarning = true;
				}
			}
			smallButton.y =(((Rect)( smallButton)).y + cellHeight);
			if (Widgets.ButtonText(smallButton, resetKarmaWarning ? "Are you sure?" : "Reset All Karma", true, true, true))
			{
				if (resetKarmaWarning)
				{
					Viewers.ResetViewersKarma();
					resetKarmaWarning = false;
				}
				else
				{
					resetKarmaWarning = true;
				}
			}
			smallButton.y = smallButton.y + cellHeight;
		}
		if (selectedViewer == null)
		{
			return;
		}
		smallLabel.x =(0f);
		smallLabel.width = (200f);
		smallLabel.y =(viewerInfoHeight + 20f);
		string colorCode = Viewer.GetViewerColorCode(selectedViewer.username);
		Widgets.Label(smallLabel, "<b>Viewer:</b> <color=#" + colorCode + ">" + selectedViewer.username + "</color>");
		smallLabel.y =(((Rect)( smallLabel)).y + cellHeight);
		smallButton.y =(((Rect)( smallLabel)).y);
		smallButton.x =(250f);
		Widgets.Label(smallLabel, "Banned: " + (selectedViewer.IsBanned ? "Yes" : "No"));
		if (Widgets.ButtonText(smallButton, selectedViewer.IsBanned ? "Unban" : "Ban", true, true, true))
		{
			if (selectedViewer.IsBanned)
			{
				selectedViewer.UnBanViewer();
			}
			else
			{
				selectedViewer.BanViewer();
			}
		}
		smallLabel.y =(((Rect)( smallLabel)).y + cellHeight);
		smallButton.y =(((Rect)( smallLabel)).y);
		Widgets.Label(smallLabel, "Toolkit Mod: " + (selectedViewer.mod ? "Yes" : "No"));
		if (Widgets.ButtonText(smallButton, selectedViewer.mod ? "Unmod" : "Mod", true, true, true))
		{
			selectedViewer.mod = !selectedViewer.mod;
		}
		if (component != null)
		{
			smallLabel.y =(((Rect)( smallLabel)).y + cellHeight);
			smallButton.y =(((Rect)( smallLabel)).y);
			Widgets.Label(smallLabel, "Colonist: " + (component.HasUserBeenNamed(selectedViewer.username) ? component.PawnAssignedToUser(selectedViewer.username).Name.ToStringShort : "None"));
			if (component.HasUserBeenNamed(selectedViewer.username) && Widgets.ButtonText(smallButton, "Unassign", true, true, true))
			{
				component.pawnHistory.Remove(selectedViewer.username);
			}
		}
		smallLabel.y =(((Rect)( smallLabel)).y + cellHeight);
		Widgets.Label(smallLabel, "Coins: " + selectedViewer.GetViewerCoins());
		smallLabel.y =(((Rect)( smallLabel)).y + cellHeight);
		Widgets.Label(smallLabel, "Karma: " + selectedViewer.GetViewerKarma() + "%");
		smallButton.y =(((Rect)( smallLabel)).y + cellHeight);
		smallButton.x =(0f);
		if (!Widgets.ButtonText(smallButton, resetWarning ? "Are you sure?" : "Reset Viewer", true, true, true))
		{
			return;
		}
		if (resetWarning)
		{
			Viewers.All = Viewers.All.Where((Viewer s) => s != selectedViewer).ToList();
			string username = selectedViewer.username;
			resetWarning = false;
			SelectViewer(Viewers.GetViewer(username));
		}
		else
		{
			resetWarning = true;
		}
	}

	public void OpenEditProp(EditPropsActions action, EditProp prop)
	{
		Find.WindowStack.TryRemove(typeof(Window_ViewerEditProp), true);
		Window_ViewerEditProp window = ((selectedViewer == null) ? new Window_ViewerEditProp(action, prop) : new Window_ViewerEditProp(action, prop, selectedViewer));
		Find.WindowStack.Add((Window)(object)window);
	}

	public void SelectViewer(Viewer viewer)
	{
		selectedViewer = viewer;
		viewerBuffer = viewer.username;
		searchQuery = viewerBuffer;
		allViewers = false;
	}

	public void SelectAllViewers()
	{
		selectedViewer = null;
		allViewers = true;
		viewerBuffer = "All Viewers";
		searchQuery = viewerBuffer;
	}

	public void ClearViewer()
	{
		selectedViewer = null;
		allViewers = false;
		viewerBuffer = "";
		searchQuery = viewerBuffer;
	}
}
