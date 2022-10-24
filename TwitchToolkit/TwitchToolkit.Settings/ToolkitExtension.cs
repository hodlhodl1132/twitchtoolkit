using System;
using Verse;

namespace TwitchToolkit.Settings;

public class ToolkitExtension
{
	public Mod mod;

	public Type windowType;

	public ToolkitExtension(Mod mod, Type windowType)
	{
		this.mod = mod;
		this.windowType = windowType;
	}
}
