using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// A char item with segment properties 
    /// </summary>
    public class CharItem : iSegment
    {
        public char Item { get; set; }
        public Color FillBrush { get; set; }
        public Color SelectedFillBrush { get; set; }
        public Color PenColor { get; set; }
        public Color SelectedPenColor { get; set; }
        public double PenThickness { get; set; }
        public string Value { get; set; }
        public double GapWidth { get; set; }
        public bool RoundedCorners { get; set; }
        public double TiltAngle { get; set; }
        public bool ShowDot { get; set; }
        public bool OnDot { get; set; }
        public bool ShowColon { get; set; }
        public bool OnColon { get; set; }
        public double VertSegDivider { get; set; }
        public double HorizSegDivider { get; set; }

    }
}
