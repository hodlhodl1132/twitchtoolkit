using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TwitchToolkit.PawnQueue;

public class GameComponentPawns : GameComponent
{
	public Dictionary<string, Pawn> pawnHistory = new Dictionary<string, Pawn>();

	public List<string> viewerNameQueue = new List<string>();

	public List<Pawn> listPawns = new List<Pawn>();

	public List<string> pawnNames = new List<string>();

	public List<string> ViewerNameQueue
	{
		get
		{
			if (viewerNameQueue == null)
			{
				viewerNameQueue = new List<string>();
			}
			return viewerNameQueue;
		}
	}

	public GameComponentPawns(Game game)
	{
	}

	public override void GameComponentTick()
	{
		if (Find.TickManager.TicksGame % 1000 != 0)
		{
			return;
		}
		List<Pawn> currentColonists = Find.ColonistBar.GetColonistsInOrder();
		foreach (KeyValuePair<string, Pawn> pair in pawnHistory)
		{
			if (!currentColonists.Contains(pair.Value))
			{
				pawnHistory.Remove(pair.Key);
			}
		}
	}

	public void AssignUserToPawn(string username, Pawn pawn)
	{
		pawnHistory.Add(username, pawn);
		if (ViewerNameQueue.Contains(username))
		{
			ViewerNameQueue.Remove(username);
		}
	}

	public bool HasUserBeenNamed(string username)
	{
		return pawnHistory.ContainsKey(username);
	}

	public bool HasPawnBeenNamed(Pawn pawn)
	{
		return pawnHistory.ContainsValue(pawn);
	}

	public string UserAssignedToPawn(Pawn pawn)
	{
		if (!HasPawnBeenNamed(pawn))
		{
			return null;
		}
		return pawnHistory.FirstOrDefault((KeyValuePair<string, Pawn> s) => s.Value == pawn).Key;
	}

	public Pawn PawnAssignedToUser(string username)
	{
		if (pawnHistory.ContainsKey(username))
		{
			return pawnHistory[username];
		}
		return null;
	}

	public bool UserInViewerQueue(string username)
	{
		return ViewerNameQueue.Contains(username);
	}

	public void AddViewerToViewerQueue(string username)
	{
		if (!UserInViewerQueue(username))
		{
			ViewerNameQueue.Add(username);
		}
	}

	public string GetNextViewerFromQueue()
	{
		if (ViewerNameQueue.Count < 1)
		{
			return null;
		}
		return ViewerNameQueue[0];
	}

	public string GetRandomViewerFromQueue()
	{
		if (ViewerNameQueue.Count < 1)
		{
			return null;
		}
		return ViewerNameQueue[Rand.Range(0, ViewerNameQueue.Count - 1)];
	}

	public int ViewersInQueue()
	{
		return ViewerNameQueue.Count;
	}

	public override void ExposeData()
	{
		((GameComponent)this).ExposeData();
		Scribe_Collections.Look<string, Pawn>(ref pawnHistory, "pawnHistory", (LookMode)1, (LookMode)3, ref pawnNames, ref listPawns);
		Scribe_Collections.Look<string>(ref viewerNameQueue, "viewerNameQueue", (LookMode)1, Array.Empty<object>());
	}
}
