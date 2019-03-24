using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ColourPicker
{
    /// <summary>
    /// This class exists only to have a reference type for Color.
    /// </summary>
    public class ColourWrapper
    {
        public Color Color { get; set; }

        public ColourWrapper( Color color )
        {
            Color = color;
        }
    }
}