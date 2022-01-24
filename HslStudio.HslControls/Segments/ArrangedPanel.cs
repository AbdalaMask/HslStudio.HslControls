using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// A panel children of which fill maximum space
    /// </summary>
    [DesignTimeVisible(false)]
    internal class ArrangedPanel : Panel
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0;
            double y = 0;
            double w = finalSize.Width / Children.Count;
            double h = finalSize.Height;

            foreach (var child in Children)
            {
                child.Arrange(new Rect(new Point(x, y), new Size(w, h)));
                x += w;
            }
            return finalSize;
        }
    }
}
