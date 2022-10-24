using System.Text;
using System.Text.RegularExpressions;
using MoonSharp.Interpreter;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit;

public class CommandDriver
{
	public Command command = null;

	public virtual void RunCommand(ITwitchMessage twitchMessage)
	{
		Helper.Log("filtering command");
		string output = FilterTags(twitchMessage, command.outputMessage);
		Helper.Log("command filtered");
		if (!UserData.IsTypeRegistered<Functions>())
		{
			UserData.RegisterType<Functions>();
			UserData.RegisterType<Viewer>();
		}
		Helper.Log("creating script");
		Script script = new Script();
		script.DebuggerEnabled = true;
		DynValue functions = UserData.Create(new Functions());
		script.Globals.Set("functions", functions);
		Helper.Log("Parsing Script " + output);
		DynValue res = script.DoString(output);
		TwitchWrapper.SendChatMessage(res.CastToString());
		Log.Message(res.CastToString());
	}

	public string FilterTags(ITwitchMessage twitchMessage, string input)
	{
		Helper.Log("starting filter");
		Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
		StringBuilder output = new StringBuilder(input);
		output.Replace("{username}", viewer.username);
		output.Replace("{balance}", viewer.GetViewerCoins().ToString());
		output.Replace("{karma}", viewer.GetViewerKarma().ToString());
		output.Replace("{purchaselist}", ToolkitSettings.CustomPricingSheetLink);
		output.Replace("{coin-reward}", ToolkitSettings.CoinAmount.ToString());
		output.Replace("\n", "");
		Helper.Log("starting regex");
		Regex regex = new Regex("\\[(.*?)\\]");
		MatchCollection matches = regex.Matches(output.ToString());
		foreach (Match match in matches)
		{
			Helper.Log("found match " + match.Value);
			string code = match.Value;
			code = code.Replace("[", "");
			code = code.Replace("]", "");
			output.Replace(match.Value, MoonSharpString(code));
		}
		return output.ToString();
	}

	public string MoonSharpString(string function)
	{
		DynValue res = Script.RunString(function);
		return res.String;
	}

	public double MoonSharpDouble(string function)
	{
		DynValue res = Script.RunString(function);
		return res.Number;
	}

	private static string TokenizeObjects()
	{
		return "";
	}
}
