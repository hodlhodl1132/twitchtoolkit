using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
    public class Listing_TwitchToolkit : Listing_Standard
    {
        public new int Slider(float val, float min, float max)
        {
            Rect rect = GetRect(22f);
            float result = (int)Widgets.HorizontalSlider(rect, val, min, max);
            Gap(verticalSpacing);
            return (int)result;
        }
    }
}
