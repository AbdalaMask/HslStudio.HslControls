﻿using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// The class to detect selected segment
    /// </summary>
    public class GeometryWithSegm
    {
        public PathGeometry Geometry { get; set; }
        public int SegmentNumber { get; set; }
        public bool IsSelected { get; set; }

        public GeometryWithSegm(PathGeometry geometry, int segm, bool isSelected = false)
        {
            Geometry = geometry;
            SegmentNumber = segm;
            IsSelected = isSelected;
        }
    }
}
