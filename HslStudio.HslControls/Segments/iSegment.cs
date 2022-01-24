using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    public interface iSegment
    {
        /// <summary>
        /// A brush for not selected elements
        /// </summary>
        Color FillBrush { get; set; }

        /// <summary>
        /// A brush for selected elements
        /// </summary>
        Color SelectedFillBrush { get; set; }

        /// <summary>
        /// A brush for not selected elements
        /// </summary>
        Color PenColor { get; set; }

        /// <summary>
        /// A pen color for selected elements
        /// </summary>
        Color SelectedPenColor { get; set; }

        /// <summary>
        /// A pen thickness of elements
        /// </summary>
        double PenThickness { get; set; }

        /// <summary>
        /// A value for segments control
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// A gap  between segments 
        /// </summary>
        double GapWidth { get; set; }

        /// <summary>
        /// Checks whether or not the corners are rounded 
        /// </summary>
        bool RoundedCorners { get; set; }

        /// <summary>
        /// A tilt angl (in degrees)
        /// </summary>
        double TiltAngle { get; set; }

        /// <summary>
        /// Shows/Hides dot for segments control
        /// </summary>
        bool ShowDot { get; set; }

        /// <summary>
        /// Shows/Hides colon for segments control
        /// </summary>
        bool ShowColon { get; set; }

        /// <summary>
        /// On/Off dot for segments control
        /// </summary>
        bool OnDot { get; set; }

        /// <summary>
        /// On/Off colon for segments control
        /// </summary>
        bool OnColon { get; set; }

        /// <summary>
        /// A divider for vert. segments width
        /// </summary>
        double VertSegDivider { get; set; }

        /// <summary>
        /// A divider for horiz. segments height
        /// </summary>
        double HorizSegDivider { get; set; }
    }
}
