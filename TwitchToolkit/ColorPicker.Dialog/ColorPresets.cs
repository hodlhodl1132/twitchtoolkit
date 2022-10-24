using UnityEngine;

namespace ColorPicker.Dialog;

public class ColorPresets
{
	public int Count => Colors.Length;

	private Color[] Colors { get; set; }

	private int SelectedIndex { get; set; }

	public bool IsModified { get; set; }

	public Color this[int i]
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
			//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0010: Unknown result type (might be due to invalid IL or missing erences)
			return Colors[i];
		}
		set
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing erences)
			SetColor(i, value);
		}
	}

	public ColorPresets()
	{
		Colors = (Color[])(object)new Color[6];
		Deselect();
		IsModified = false;
	}

	public void Deselect()
	{
		SelectedIndex = -1;
	}

	public Color GetSelectedColor()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0012: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0015: Unknown result type (might be due to invalid IL or missing erences)
		return Colors[SelectedIndex];
	}

	public bool HasSelected()
	{
		return SelectedIndex != -1;
	}

	public bool IsSelected(int i)
	{
		return SelectedIndex == i;
	}

	public void SetColor(int i, Color c)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0022: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		if (!((Color)(Colors[i])).Equals(c))
		{
			Colors[i] = c;
			IsModified = true;
		}
	}

	public void SetSelected(int i)
	{
		SelectedIndex = i;
	}

	internal void SetSelectedColor(Color c)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000e: Unknown result type (might be due to invalid IL or missing erences)
		Colors[SelectedIndex] = c;
		IsModified = true;
	}
}
