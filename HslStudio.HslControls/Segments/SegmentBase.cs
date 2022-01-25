using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// A base classs for segment controls
    /// </summary>
    [DesignTimeVisible(false)]
    public class SegmentBase : UserControl, iSegment
    {
        protected event EventHandler<AvaloniaPropertyChangedEventArgs>? PropertyChanged = (sender, e) => { };

        protected static double defVertDividerSixteen = 7.5;
        protected static double defHorizDividerSixteen = 11.5;

        public static StyledProperty<Color> PenColorProperty;
        public static StyledProperty<Color> SelectedPenColorProperty;
        public static StyledProperty<Color> FillBrushProperty;
        public static StyledProperty<double> PenThicknessProperty;
        public static StyledProperty<Color> SelectedFillBrushProperty;
        public static StyledProperty<string> ValueProperty;
        public static StyledProperty<double> GapWidthProperty;
        public static StyledProperty<bool> ShowDotProperty;
        public static StyledProperty<bool> OnDotProperty;
        public static StyledProperty<bool> ShowColonProperty;
        public static StyledProperty<bool> OnColonProperty;
        public static StyledProperty<double> TiltAngleProperty;
        public static StyledProperty<bool> RoundedCornersProperty;
        public static StyledProperty<List<int>> SelectedSegmentsProperty;
        public static StyledProperty<List<Tuple<int, Brush, Color>>> SegmentsBrushProperty;
        public static StyledProperty<double> VertSegDividerProperty;
        public static StyledProperty<double> HorizSegDividerProperty;



        static SegmentBase()
        {
            

            PenThicknessProperty = AvaloniaProperty.Register<SegmentBase, double>(nameof(PenThickness), 1.0);

            PenColorProperty = AvaloniaProperty.Register<SegmentBase, Color>(nameof(PenColor), Color.FromRgb(234, 234, 234));

            SelectedPenColorProperty = AvaloniaProperty.Register<SegmentBase, Color>(nameof(SelectedPenColor), Colors.Black);

            FillBrushProperty = AvaloniaProperty.Register<SegmentBase, Color>(nameof(FillBrush), Color.FromRgb(248, 248, 248));


            SelectedFillBrushProperty = AvaloniaProperty.Register<SegmentBase, Color>(nameof(SelectedFillBrush), Colors.Green) ;


            TiltAngleProperty = AvaloniaProperty.Register<SegmentBase, double>(nameof(TiltAngle), 10.0);


            GapWidthProperty = AvaloniaProperty.Register<SegmentBase, double>(nameof(GapWidth), 3.0);

            RoundedCornersProperty = AvaloniaProperty.Register<SegmentBase, bool>(nameof(RoundedCorners), false);

            ValueProperty = AvaloniaProperty.Register<SegmentBase, string>(nameof(Value), string.Empty);
            ShowDotProperty = AvaloniaProperty.Register<SegmentBase, bool>(nameof(ShowDot),false);

            OnDotProperty = AvaloniaProperty.Register<SegmentBase, bool>(nameof(OnDot), false);

            ShowColonProperty = AvaloniaProperty.Register<SegmentBase, bool>(nameof(ShowColon),false);
            OnColonProperty = AvaloniaProperty.Register<SegmentBase, bool>(nameof(OnColon), false);
            SelectedSegmentsProperty = AvaloniaProperty.Register<SegmentBase, List<int>>(nameof(SelectedSegments), new List<int>());
            SegmentsBrushProperty = AvaloniaProperty.Register<SegmentBase, List<Tuple<int, Brush, Color>>>(nameof(SegmentsBrush), new List<Tuple<int, Brush, Color>>());

            VertSegDividerProperty = AvaloniaProperty.Register<SegmentBase, double>(nameof(VertSegDivider), 5.0);

            HorizSegDividerProperty = AvaloniaProperty.Register<SegmentBase, double>(nameof(HorizSegDivider), 9.0);

            PenColorProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            SelectedPenColorProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            FillBrushProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            PenThicknessProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            SelectedFillBrushProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            ValueProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            GapWidthProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            ShowDotProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            OnDotProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            ShowColonProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            OnColonProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            TiltAngleProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            RoundedCornersProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            SelectedSegmentsProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            SegmentsBrushProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            VertSegDividerProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);
            HorizSegDividerProperty.Changed.AddClassHandler<SegmentBase>(x => x.VisualChanged);

        }

        /// <summary>
        /// A list of selected segments set by user
        /// </summary>
        public List<int> SelectedSegments
        {
            get => (List<int>)GetValue(SelectedSegmentsProperty);
            set => SetValue(SelectedSegmentsProperty, value);
        }

        /// <summary>
        /// A list of segments numbers, fill brushes and pen colors
        /// </summary>
        public List<Tuple<int, Brush, Color>> SegmentsBrush
        {
            get => (List<Tuple<int, Brush, Color>>)GetValue(SegmentsBrushProperty);
            set => SetValue(SegmentsBrushProperty, value);
        }

        /// <summary>
        /// A brush for not selected elements
        /// </summary>
        public Color FillBrush
        {
            get => (Color)GetValue(FillBrushProperty);
            set => SetValue(FillBrushProperty, value);
        }

        /// <summary>
        /// A brush for selected elements
        /// </summary>
        public Color SelectedFillBrush
        {
            get => (Color)GetValue(SelectedFillBrushProperty);
            set => SetValue(SelectedFillBrushProperty, value);
        }

        /// <summary>
        /// A pen color for not selected elements
        /// </summary>
        public Color PenColor
        {
            get => (Color)GetValue(PenColorProperty);
            set => SetValue(PenColorProperty, value);
        }

        /// <summary>
        /// A pen color for selected elements
        /// </summary>
        public Color SelectedPenColor
        {
            get => (Color)GetValue(SelectedPenColorProperty);
            set => SetValue(SelectedPenColorProperty, value);
        }

        /// <summary>
        /// A pen thickness of elements
        /// </summary>
        public double PenThickness
        {
            get => (double)GetValue(PenThicknessProperty);
            set => SetValue(PenThicknessProperty, value);
        }

        /// <summary>
        /// A value for segments control
        /// </summary>
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// A gap between segments (in pixels)
        /// </summary>
        public double GapWidth
        {
            get => (double)GetValue(GapWidthProperty);
            set => SetValue(GapWidthProperty, value);
        }


        /// <summary>
        /// Checks whether or not the corners are rounded 
        /// </summary>
        public bool RoundedCorners
        {
            get => (bool)GetValue(RoundedCornersProperty);
            set => SetValue(RoundedCornersProperty, value);
        }


        /// <summary>
        /// A tilt angle (in degrees)
        /// </summary>
        public double TiltAngle
        {
            get => (double)GetValue(TiltAngleProperty);
            set => SetValue(TiltAngleProperty, value);
        }


        /// <summary>
        /// Shows/Hides dot for segments control
        /// </summary>
        public bool ShowDot
        {
            get => (bool)GetValue(ShowDotProperty);
            set => SetValue(ShowDotProperty, value);
        }

        /// <summary>
        /// On/Off dot for segments control
        /// </summary>
        public bool OnDot
        {
            get => (bool)GetValue(OnDotProperty);
            set => SetValue(OnDotProperty, value);
        }

        /// <summary>
        /// Shows/Hides colon for segments control
        /// </summary>
        public bool ShowColon
        {
            get => (bool)GetValue(ShowColonProperty);
            set => SetValue(ShowColonProperty, value);
        }

        /// <summary>
        /// On/Off colon for segments control
        /// </summary>
        public bool OnColon
        {
            get => (bool)GetValue(OnColonProperty);
            set => SetValue(OnColonProperty, value);
        }

        /// <summary>
        /// A divider for vert. segments width
        /// </summary>
        public double VertSegDivider
        {
            get => (double)GetValue(VertSegDividerProperty);
            set => SetValue(VertSegDividerProperty, value);
        }


        /// <summary>
        /// A divider for horiz. segments height
        /// </summary>
        public double HorizSegDivider
        {
            get => (double)GetValue(HorizSegDividerProperty);
            set => SetValue(HorizSegDividerProperty, value);
        }


        private  void VisualChanged(AvaloniaPropertyChangedEventArgs e)
        {
            SegmentBase segments = (SegmentBase)e.Sender;
            segments.PropertyChanged(e.Sender, e);
        }


    }
}
