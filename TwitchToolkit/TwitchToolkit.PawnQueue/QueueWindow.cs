using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace TwitchToolkit.PawnQueue;

internal class QueueWindow : Window
{
	private GameComponentPawns pawnComponent;

	public string selectedUsername = "";

	public Pawn selectedPawn = null;

	public int pawnIndex = -1;

	public List<Pawn> allColonists = new List<Pawn>();

	public List<Pawn> unnamedColonists = new List<Pawn>();

	public override Vector2 InitialSize => new Vector2(500f, 500f);

	public QueueWindow()
	{
		base.doCloseButton = true;
		pawnComponent = Current.Game.GetComponent<GameComponentPawns>();
		if (pawnComponent == null)
		{
			Log.Error("component null");
			((Window)this).Close(true);
		}
		GetPawn(PawnQueueSelector.FirstDefault);
	}

	public override void DoWindowContents(Rect inRect)
	{
        Rect unnamedCounter = new Rect(((Rect)(inRect)).x + 10f, 0f, 300f, 52f);
		Widgets.Label(unnamedCounter, "Unnamed Colonists: " + unnamedColonists.Count);
		Rect colonistPortrait = new Rect(((Rect)(inRect)).x + 10f, 60f, 100f, 140f);
		DrawColonist(colonistPortrait, selectedPawn);
		Rect rightSide = new Rect(130f, 60f, 300f, 26f);
		selectedUsername = Widgets.TextEntryLabeled(rightSide, "Username:", selectedUsername);
		rightSide.width = (120f);
		rightSide.y =(((Rect)( rightSide)).y + 26f);
		rightSide.x =(((Rect)( rightSide)).x + 150f);
		if (Widgets.ButtonText(rightSide, "Assign", true, true, true))
		{
			NameColonist(selectedUsername, selectedPawn);
		}
		Rect pawnSelectors = new Rect(26f, 210f, 40f, 26f);
		if (Widgets.ButtonText(pawnSelectors, "<", true, true, true))
		{
			GetPawn(PawnQueueSelector.Back);
		}
		pawnSelectors.x =(((Rect)( pawnSelectors)).x + 42f);
		if (Widgets.ButtonText(pawnSelectors, ">", true, true, true))
		{
			GetPawn(PawnQueueSelector.Next);
		}
		Rect namedStatus = new Rect(0f, 236f, 300f, 26f);
		bool viewerNamed = pawnComponent.HasPawnBeenNamed(selectedPawn);
		Widgets.Label(namedStatus, "Name: " + selectedPawn.Name);
		namedStatus.y =(((Rect)( namedStatus)).y + 26f);
		Widgets.Label(namedStatus, "Colonist " + (viewerNamed ? ("Named after <color=#4BB543>" + pawnComponent.UserAssignedToPawn(selectedPawn) + "</color>") : "<color=#B2180E>Unnamed</color>"));
		Rect queueButtons = new Rect(0f, 300f, 200f, 26f);
		Widgets.Label(queueButtons, "Viewers in Queue: " + pawnComponent.ViewersInQueue());
		queueButtons.y =(((Rect)( queueButtons)).y + 26f);
		if (Widgets.ButtonText(queueButtons, "Next Viewer from Queue", true, true, true))
		{
			selectedUsername = pawnComponent.GetNextViewerFromQueue();
		}
		queueButtons.y =(((Rect)( queueButtons)).y + 26f);
		if (Widgets.ButtonText(queueButtons, "Random Viewer from Queue", true, true, true))
		{
			selectedUsername = pawnComponent.GetRandomViewerFromQueue();
		}
		queueButtons.y =(((Rect)( queueButtons)).y + 26f);
		if (Widgets.ButtonText(queueButtons, "Ban Viewer from Queue", true, true, true))
		{
			Viewer viewer = Viewers.GetViewer(selectedUsername);
			viewer.BanViewer();
		}
	}

	public void GetPawn(PawnQueueSelector method)
	{
		switch (method)
		{
		case PawnQueueSelector.Next:
			if (pawnIndex + 1 == allColonists.Count)
			{
				selectedPawn = allColonists[0];
				pawnIndex = 0;
			}
			else
			{
				pawnIndex++;
				selectedPawn = allColonists[pawnIndex];
			}
			break;
		case PawnQueueSelector.Back:
			if (pawnIndex - 1 < 0)
			{
				selectedPawn = allColonists[allColonists.Count - 1];
				pawnIndex = allColonists.FindIndex((Pawn s) => s == selectedPawn);
			}
			else
			{
				pawnIndex--;
				selectedPawn = allColonists[pawnIndex];
			}
			break;
		case PawnQueueSelector.FirstDefault:
			Helper.Log("first or default");
			allColonists = Find.ColonistBar.GetColonistsInOrder();
			unnamedColonists = GetUnamedColonists();
			selectedUsername = "";
			GetPawn(PawnQueueSelector.New);
			break;
		case PawnQueueSelector.New:
			if (unnamedColonists.Count > 0)
			{
				selectedPawn = unnamedColonists[0];
			}
			else
			{
				selectedPawn = allColonists[0];
			}
			pawnIndex = allColonists.FindIndex((Pawn s) => s == selectedPawn);
			break;
		}
	}

	public void NextColonist()
	{
		List<Pawn> colonistsUnnamed = GetUnamedColonists();
		if (!GenList.NullOrEmpty<Pawn>((IList<Pawn>)colonistsUnnamed))
		{
		}
	}

	public List<Pawn> GetUnamedColonists()
	{
		List<Pawn> allColonists = Find.ColonistBar.GetColonistsInOrder();
		List<Pawn> colonistsUnnamed = new List<Pawn>();
		foreach (Pawn pawn in allColonists)
		{
			if (!pawnComponent.pawnHistory.ContainsValue(pawn))
			{
				colonistsUnnamed.Add(pawn);
			}
		}
		return colonistsUnnamed;
	}

	public void DrawColonist(Rect rect, Pawn colonist)
	{
        GUI.DrawTexture(rect, (Texture)(object)PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, new Rot4(0), ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, (Dictionary<Apparel, Color>)null, (Color?)null, false));
	}

	public void NameColonist(string username, Pawn pawn)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00c6: Expected O, but got Unknown
		if (pawnComponent.HasPawnBeenNamed(pawn) && pawnComponent.pawnHistory.ContainsValue(pawn))
		{
			string key = null;
			foreach (KeyValuePair<string, Pawn> pair in pawnComponent.pawnHistory)
			{
				if (pair.Value == pawn)
				{
					key = pair.Key;
				}
			}
			if (key != null)
			{
				pawnComponent.pawnHistory.Remove(key);
			}
		}
		Name name = pawn.Name;
		NameTriple currentName = (NameTriple)(object)((name is NameTriple) ? name : null);
		pawn.Name = ((Name)new NameTriple(currentName.First, username, currentName.Last));
		pawnComponent.AssignUserToPawn(selectedUsername.ToLower(), selectedPawn);
		GetPawn(PawnQueueSelector.FirstDefault);
	}
}
