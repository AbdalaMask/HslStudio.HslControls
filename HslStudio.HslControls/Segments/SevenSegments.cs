using Avalonia;
using Avalonia.Collections;
using Avalonia.Media;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// A seven segments control
    /// </summary>
    [DesignTimeVisible(false)]
    public class SevenSegments : SegmentBase
    {
        #region Protected variables

        protected bool isPropertyCahnged = true;
        protected double startPointThickness;

        protected double vertRoundCoef = 0;
        protected double horizRoundCoef = 0;

        /// <summary>
        /// The width of the vert. segm
        /// </summary>
        protected double VertSegW { get; private set; }

        /// <summary>
        /// The width of the vert. segment's part
        /// </summary>
        protected double VertSegPartW { get; private set; }

        /// <summary>
        /// The height of the vert. segment's part
        /// </summary>
        protected double VertSegSmallPartH { get; private set; }

        /// <summary>
        /// The height of the horiz. segment
        /// </summary>
        protected double HorizSegH { get; private set; }

        /// <summary>
        /// The height of the horiz. segment's part
        /// </summary>
        protected double HorizSegSmallPartH { get; private set; }

        /// <summary>
        /// The width of the horiz. segment's part
        /// </summary>
        protected double HorizSegSmallPartW { get; private set; }

        /// <summary>
        /// The horizontal midlle point
        /// </summary>
        protected double MidPoint { get; private set; }

        /// <summary>
        /// The gap between segments
        /// </summary>
        protected double GapW { get; private set; }


        /// <summary>
        /// The diameter of the dot
        /// </summary>
        protected double DotDiameter { get; private set; }

        /// <summary>
        /// The diameter of the colon
        /// </summary>
        protected double ColonDiameter { get; private set; }

        /// <summary>
        /// The height depending on the decimal dot
        /// </summary>
        protected double VirtualWidth { get; private set; }

        /// <summary>
        /// The width depending on the decimal dot
        /// </summary>
        protected double VirtualHeight { get; private set; }


        /// <summary>
        /// The list of geometries to detect selected segments
        /// </summary>
        protected List<GeometryWithSegm> GeometryFigures;

        /// <summary>
        /// The width of the vert. segment's bottom part
        /// </summary>
        protected double VertSegBotPartW { get; private set; }

        /// <summary>
        /// Points collection for the left bottom segment
        /// </summary>
        protected AvaloniaList<Point> LeftBottomSegmPoints { get; private set; }

        /// <summary>
        /// Points collection for the left top segment
        /// </summary>
        protected AvaloniaList<Point> LeftTopSegmPoints { get; private set; }

        /// <summary>
        /// Points collection for the top segment
        /// </summary>
        protected AvaloniaList<Point> TopSegmPoints { get; set; }

        /// <summary>
        /// Points collection for the bottom segment
        /// </summary>
        protected AvaloniaList<Point> BottomSegmPoints { get; set; }

        /// <summary>
        /// Points collection for the middle segment
        /// </summary>
        protected AvaloniaList<Point> MiddleSegmPoints { get; set; }

        /// <summary>
        /// Points collection for the right top segment
        /// </summary>
        protected AvaloniaList<Point> RightTopSegmPoints { get; private set; }

        /// <summary>
        /// Points collection for the right bottom segment
        /// </summary>
        protected AvaloniaList<Point> RightBottomSegmPoints { get; private set; }

        protected double figureStartPointY;

        #endregion

        #region Constructor

        public SevenSegments()
        {
            PropertyChanged += OnPropertyChanged;
            vertRoundCoef = 5.5;
            horizRoundCoef = 15;
        }

        private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            SegmentBase segments = (SegmentBase)sender;
            isPropertyCahnged = true;

            segments.InvalidateVisual();
        }

        #endregion

        #region Drawing
        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            CalculateMeasures();

            AssignSegments();
            ClearSegmentsSelection();
            SetSegments();

            // Draws segments
            foreach (GeometryWithSegm entry in GeometryFigures)
            {

                if (SegmentsBrush.Any())
                {
                    Tuple<int, Brush, Color> brush = SegmentsBrush.SingleOrDefault(s => s.Item1 == entry.SegmentNumber);
                    Pen figurePen = new Pen(new SolidColorBrush(brush != null ? brush.Item3 : PenColor),
                        PenThickness);

                    SolidColorBrush sc2 = new SolidColorBrush(Color.FromArgb(FillBrush.A, FillBrush.R, FillBrush.G, FillBrush.B));

                    drawingContext.DrawGeometry(brush != null ? brush.Item2 : sc2,
                        figurePen, entry.Geometry);

                }
                else
                {
                    Pen figurePen = new Pen(new SolidColorBrush(entry.IsSelected ? SelectedPenColor : PenColor), PenThickness);
                    SolidColorBrush sc = new SolidColorBrush(Color.FromArgb(SelectedFillBrush.A, SelectedFillBrush.R, SelectedFillBrush.G, SelectedFillBrush.B));
                    SolidColorBrush sc2 = new SolidColorBrush(Color.FromArgb(FillBrush.A, FillBrush.R, FillBrush.G, FillBrush.B));

                    drawingContext.DrawGeometry(entry.IsSelected ? sc : sc2,
                        figurePen, entry.Geometry);
                }
            }

            // Draws decimal dot
            DrawDot(drawingContext);

            // Draws colon
            DrawColon(drawingContext);
        }

       

        /// <summary>
        /// Clear selected segments and value
        /// </summary>
        public void ClearSegments()
        {
            Value = string.Empty;
            SelectedSegments = new List<int>();
            SegmentsBrush = new List<Tuple<int, Brush, Color>>();
        }

        /// <summary>
        /// Assigns a segment number to required path geometry. Order is important!
        /// </summary>
        protected virtual void AssignSegments()
        {
            GeometryFigures = new List<GeometryWithSegm>
            {
                new GeometryWithSegm(LeftBottomSegement(), (int)SevenSegmentsNumbers.LeftBottom),
                new GeometryWithSegm(LeftTopSegement(), (int)SevenSegmentsNumbers.LeftTop),
                new GeometryWithSegm(RightTopSegement(), (int)SevenSegmentsNumbers.RightTop),
                new GeometryWithSegm(RightBottomSegement(), (int)SevenSegmentsNumbers.RightBottom),
                new GeometryWithSegm(MiddleSegement(), (int)SevenSegmentsNumbers.Middle),
                new GeometryWithSegm(TopSegement(), (int)SevenSegmentsNumbers.Top),
                new GeometryWithSegm(BottomSegement(), (int)SevenSegmentsNumbers.Bottom)
            };
        }

        /// <summary>
        /// Selects required segments
        /// </summary>
        protected void SetSegments()
        {
            if (SelectedSegments.Any())
            {
                for (int i = 0; i < SelectedSegments.Count; i++)
                {
                    GeometryFigures.Single(t => t.SegmentNumber == SelectedSegments[i]).IsSelected = true;
                }
            }
            else
            {
                ValueSegmentsSelection();
            }
        }


        /// <summary>
        /// Calculates required points and measures
        /// </summary>
        private void CalculateMeasures()
        {
            //Horiz. figure
            HorizSegH = Height / HorizSegDivider;
            HorizSegSmallPartH = HorizSegH / 4;

            //Vert. figure
            VertSegW = Width / VertSegDivider;
            VertSegPartW = (VertSegW / 3.5);
            VertSegSmallPartH = VertSegW / 3.5;
            VertSegBotPartW = VertSegW / 2;

            HorizSegSmallPartW = VertSegW / 4;

            //The points calculation
            MidPoint = Height / 2;
            GapW = GapWidth;

            DotDiameter = HorizSegH;
            ColonDiameter = HorizSegH;

            VirtualHeight = ShowDot ? Height - DotDiameter / 1.5 : Height;
            VirtualWidth = ShowDot ? Width - DotDiameter / 1.5 : Width;

            figureStartPointY = VirtualHeight - (HorizSegSmallPartH + GapW + VertSegSmallPartH);
            startPointThickness = PenThickness / 2;

        }

        /// <summary>
        /// Selects segments depending on the value 
        /// </summary>
        protected virtual void ValueSegmentsSelection()
        {
            if (int.TryParse(Value, out int tempValue))
            {
                if (tempValue > 9)
                {
                    tempValue = 9;
                }

                if (tempValue < 0)
                {
                    tempValue = 0;
                }

                switch (tempValue)
                {
                    case 0:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.RightBottom,
                            (int)SevenSegmentsNumbers.Bottom,
                            (int)SevenSegmentsNumbers.LeftBottom);
                        break;
                    case 1:
                        SelectSegments((int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.RightBottom);
                        break;
                    case 2:
                        SelectSegments((int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.Middle,
                            (int)SevenSegmentsNumbers.LeftBottom,
                            (int)SevenSegmentsNumbers.Bottom);
                        break;
                    case 3:
                        SelectSegments((int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.Middle,
                            (int)SevenSegmentsNumbers.RightBottom,
                            (int)SevenSegmentsNumbers.Bottom);
                        break;
                    case 4:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.Middle,
                            (int)SevenSegmentsNumbers.RightBottom);
                        break;
                    case 5:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.Middle,
                            (int)SevenSegmentsNumbers.RightBottom,
                            (int)SevenSegmentsNumbers.Bottom);
                        break;
                    case 6:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.Middle,
                            (int)SevenSegmentsNumbers.RightBottom,
                            (int)SevenSegmentsNumbers.LeftBottom,
                            (int)SevenSegmentsNumbers.Bottom);
                        break;
                    case 7:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.RightBottom);
                        break;
                    case 8:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.RightTop,
                            (int)SevenSegmentsNumbers.Middle,
                            (int)SevenSegmentsNumbers.LeftBottom,
                            (int)SevenSegmentsNumbers.RightBottom,
                            (int)SevenSegmentsNumbers.Bottom);
                        break;
                    case 9:
                        SelectSegments((int)SevenSegmentsNumbers.LeftTop,
                            (int)SevenSegmentsNumbers.Top,
                            (int)SevenSegmentsNumbers.RightTop,
                             (int)SevenSegmentsNumbers.Middle,
                             (int)SevenSegmentsNumbers.RightBottom,
                             (int)SevenSegmentsNumbers.Bottom);
                        break;
                }
            }

            // Selects segment for the minus sign 
            if (Value == "-")
            {
                SelectSegments((int)SevenSegmentsNumbers.Middle);
            }

        }


        /// <summary>
        /// Draws decimal dot separator
        /// </summary>
        protected void DrawDot(DrawingContext drawingContext)
        {
            if (ShowDot)
            {
                PathGeometry pathGeometry = new PathGeometry();
                Pen dotPen = new Pen(new SolidColorBrush(OnDot ? SelectedPenColor : PenColor), PenThickness);
                Point centerPoint = new Point(Width - DotDiameter / 2, Height - DotDiameter / 2);
                pathGeometry = CreateEllipseGeometry(centerPoint, pathGeometry, DotDiameter / 2);
                SolidColorBrush sc = new SolidColorBrush(Color.FromArgb(SelectedFillBrush.A, SelectedFillBrush.R, SelectedFillBrush.G, SelectedFillBrush.B));
                SolidColorBrush sc2 = new SolidColorBrush(Color.FromArgb(FillBrush.A, FillBrush.R, FillBrush.G, FillBrush.B));

                drawingContext.DrawGeometry(OnDot ? sc : sc2,
                    dotPen, pathGeometry);
            }
        }

        /// <summary>
        /// Draws colon
        /// </summary>
        private void DrawColon(DrawingContext drawingContext)
        {
            if (ShowColon)
            {
                PathGeometry pathGeometry = new PathGeometry();

                double hUpper = (MiddleSegmPoints[2].Y - GapW - HorizSegH) - (HorizSegH + GapW);
                double yTop = HorizSegH + GapW + hUpper / 2 + ColonDiameter / 2;
                double xTop = XByAngle(yTop) + VertSegW;

                double hLower = (BottomSegmPoints[2].Y - GapW) - (MiddleSegmPoints[0].Y + GapW + HorizSegH);
                double yBottom = MiddleSegmPoints[0].Y + GapW + HorizSegH + hLower / 2 - ColonDiameter / 2;
                double xBottom = XByAngle(yBottom) + VertSegW;

                double xTopMiddle = xTop + (((VirtualWidth - xBottom) - xTop) / 2);
                double xBottomMiddle = xBottom + (((VirtualWidth - xTop) - xBottom) / 2);

                Pen colonPen = new Pen(new SolidColorBrush(OnColon ? SelectedPenColor : PenColor), PenThickness);

                // the top ellipse
                Point centerPoint = new Point(xTopMiddle, yTop);
                pathGeometry = CreateEllipseGeometry(centerPoint, pathGeometry, ColonDiameter / 2);
                SolidColorBrush sc = new SolidColorBrush(Color.FromArgb(SelectedFillBrush.A, SelectedFillBrush.R, SelectedFillBrush.G, SelectedFillBrush.B));
                SolidColorBrush sc2 = new SolidColorBrush(Color.FromArgb(FillBrush.A, FillBrush.R, FillBrush.G, FillBrush.B));

                drawingContext.DrawGeometry(OnColon ? sc : sc2,
                    colonPen, pathGeometry);

                //the bottom ellipse
                centerPoint = new Point(xBottomMiddle, yBottom);
                pathGeometry = CreateEllipseGeometry(centerPoint, pathGeometry, ColonDiameter / 2);
                SolidColorBrush sc1 = new SolidColorBrush(Color.FromArgb(SelectedFillBrush.A, SelectedFillBrush.R, SelectedFillBrush.G, SelectedFillBrush.B));
                SolidColorBrush sc3 = new SolidColorBrush(Color.FromArgb(FillBrush.A, FillBrush.R, FillBrush.G, FillBrush.B));

                drawingContext.DrawGeometry(OnColon ? sc1 : sc3,
                    colonPen, pathGeometry);

            }
        }

        private PathGeometry CreateEllipseGeometry(Point centerPoint,
            PathGeometry pathGeometry,
            double diameter)
        {
            EllipseGeometry ellipseGeometry;
            SkewTransform transform;
            ellipseGeometry = new EllipseGeometry
            {
                Center = centerPoint,
                RadiusX = diameter,
                RadiusY = diameter
            };

            pathGeometry = PathGeometry.CreateFromGeometry(ellipseGeometry);

            transform = new SkewTransform(-TiltAngle,
                0, centerPoint.X, centerPoint.Y);
            pathGeometry.Transform = transform;
            return pathGeometry;
        }


        /// <summary>
        /// Sets required geometry figures as selected
        /// </summary>
        protected void SelectSegments(params int[] segmNumbers)
        {
            for (int i = 0; i < segmNumbers.Length; i++)
            {
                GeometryFigures.Single(t => t.SegmentNumber == segmNumbers[i]).IsSelected = true;
            }

        }

        /// <summary>
        /// Clears selection for all geometry figures 
        /// </summary>
        protected void ClearSegmentsSelection()
        {
            GeometryFigures.ForEach(c => c.IsSelected = false);
        }


        /// <summary>
        /// Draws custom path geometry
        /// </summary>
        protected PathGeometry SegmentPathGeometry(Point startPoint, PolyLineSegment polyLineSegment)
        {
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(polyLineSegment);
            return pathGeometry;
        }

        /// <summary>
        /// Required segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry SegmentGeometry(AvaloniaList<Point> assignPoints, AvaloniaList<Point> drawnPoints)
        {
            assignPoints = drawnPoints;
            Point startPoint = assignPoints[0];
            PolyLineSegment segment = new PolyLineSegment { Points = assignPoints };
            return SegmentPathGeometry(startPoint, segment);
        }

        /// <summary>
        /// Returns X-coord by the angle and height
        /// </summary>
        /// <param name="y">Y-coordinate to calculate height</param>
        protected double XByAngle(double y)
        {
            double h = figureStartPointY - y;
            return (TanAngle() * h);
        }

        /// <summary>
        /// Returns tangent of the tilt angle in degrees
        /// </summary>
        protected double TanAngle()
        {
            return Math.Tan(TiltAngle * (Math.PI / 180.0));
        }

        /// <summary>
        /// Returns gap shift for the top and bottom segments
        /// </summary>
        private double GapShift()
        {
            return GapW * 0.75;
        }


        #endregion

        #region Points' locations

        /// <summary>
        /// Calulates points  for the left top segment
        /// </summary>
        /// <returns></returns>
        protected AvaloniaList<Point> GetLeftTopSegmPoints()
        {
            AvaloniaList<Point> points = new AvaloniaList<Point>();

            double intermPoint = VirtualHeight / 2 - HorizSegH / 2;
            double startTopY = HorizSegSmallPartH + GapW + VertSegSmallPartH + startPointThickness;
            double x1 = XByAngle(startTopY);

            // the bezier point
            Point bezPoint;
            if (RoundedCorners)
            {
                double yBezier = (VirtualHeight - startPointThickness) / vertRoundCoef;
                double xBezier = RoundedCorners ? XByAngle(yBezier) : 0;
                bezPoint = new Point(xBezier + startPointThickness, yBezier);
            }
            else
            {
                bezPoint = new Point(x1 + startPointThickness, HorizSegSmallPartH + startPointThickness + GapW + VertSegSmallPartH);
            }


            startTopY = HorizSegSmallPartH + GapShift();
            double x2 = XByAngle(startTopY);

            startTopY = HorizSegH + GapW / 2;
            double x3 = XByAngle(startTopY);

            startTopY = intermPoint - (GapW / 2);
            double x4 = XByAngle(startTopY - startPointThickness);

            startTopY = (VirtualHeight / 2) - GapW / 2;
            double x5 = XByAngle(startTopY - startPointThickness);


            // three top points, starting from the left point
            points.Add(new Point(x1 + startPointThickness, HorizSegSmallPartH + GapW + VertSegSmallPartH + startPointThickness));
            points.Add(new Point(x2 + VertSegPartW + startPointThickness, HorizSegSmallPartH + startPointThickness + GapShift()));
            points.Add(new Point(x3 + VertSegW + startPointThickness, HorizSegH + startPointThickness + GapW / 2));

            // three bottom points, starting from the right point
            points.Add(new Point(x4 + VertSegW + startPointThickness, intermPoint - (GapW / 2)));
            points.Add(new Point(x5 + VertSegBotPartW + startPointThickness, (VirtualHeight / 2) - GapW / 2));
            points.Add(new Point(x5 + startPointThickness, (VirtualHeight / 2) - GapW / 2));


            // the point for rounded Bezier curve
            points.Add(bezPoint);

            return points;
        }

        /// <summary>
        /// Calulates points for the left bottom segment
        /// </summary>
        /// <returns></returns>
        protected AvaloniaList<Point> GetLeftBottomSegmPoints()
        {
            AvaloniaList<Point> points = new AvaloniaList<Point>();

            double startBottomY = (VirtualHeight / 2) + HorizSegH / 2 + (GapW / 2);
            double startBottomY2 = VirtualHeight - (HorizSegH + GapW / 2) - startPointThickness;

            double x1 = XByAngle((VirtualHeight / 2) + GapW / 2);
            double x = XByAngle(startBottomY);
            double x2 = XByAngle(startBottomY2);

            // the bezier point
            Point bezPoint;
            if (RoundedCorners)
            {
                double yBezier = VirtualHeight - startPointThickness - VirtualHeight / vertRoundCoef;
                double xBezier = RoundedCorners ? XByAngle(yBezier) : 0;
                bezPoint = new Point(xBezier + startPointThickness, yBezier);
            }
            else
            {
                bezPoint = new Point(startPointThickness, figureStartPointY - startPointThickness);
            }

            // three top points, starting from left top point
            points.Add(new Point(x1 + startPointThickness, (VirtualHeight / 2) + GapW / 2));
            points.Add(new Point(x1 + VertSegBotPartW + startPointThickness, (VirtualHeight / 2) + GapW / 2));
            points.Add(new Point(x + VertSegW + startPointThickness, startBottomY));

            // three bottom points, starting from right
            points.Add(new Point(x2 + VertSegW + startPointThickness, startBottomY2));
            points.Add(new Point(VertSegPartW + startPointThickness, VirtualHeight - startPointThickness - (HorizSegSmallPartH + GapShift())));
            points.Add(new Point(startPointThickness, figureStartPointY - startPointThickness));

            // the point for rounded Bezier curve
            points.Add(bezPoint);

            return points;
        }

        /// <summary>
        /// Calulates points for the right bottom segment
        /// </summary>
        /// <returns></returns>
        protected AvaloniaList<Point> GetRightBottomSegmPoints()
        {
            AvaloniaList<Point> points = new AvaloniaList<Point>
            {

                // three top points, starting from the left point
                new Point(VirtualWidth - LeftTopSegmPoints[3].X, VirtualHeight - LeftTopSegmPoints[3].Y),
                new Point(VirtualWidth - LeftTopSegmPoints[4].X, VirtualHeight - LeftTopSegmPoints[4].Y),
                new Point(VirtualWidth - LeftTopSegmPoints[5].X, VirtualHeight - LeftTopSegmPoints[5].Y),

                // the point for rounded Bezier curve
                new Point(VirtualWidth - LeftTopSegmPoints[6].X, VirtualHeight - LeftTopSegmPoints[6].Y),


                // three bottom points, starting from the right point
                new Point(VirtualWidth - LeftTopSegmPoints[0].X, VirtualHeight - LeftTopSegmPoints[0].Y),
                new Point(VirtualWidth - LeftTopSegmPoints[1].X, VirtualHeight - LeftTopSegmPoints[1].Y),
                new Point(VirtualWidth - LeftTopSegmPoints[2].X, VirtualHeight - LeftTopSegmPoints[2].Y)
            };

            return points;
        }


        /// <summary>
        /// Calulates points  for the right top segment
        /// </summary>
        protected AvaloniaList<Point> GetRightTopSegmPoints()
        {
            AvaloniaList<Point> points = new AvaloniaList<Point>
            {

                // three top points, starting from the left point
                new Point(VirtualWidth - LeftBottomSegmPoints[3].X, VirtualHeight - LeftBottomSegmPoints[3].Y),
                new Point(VirtualWidth - LeftBottomSegmPoints[4].X, VirtualHeight - LeftBottomSegmPoints[4].Y),
                new Point(VirtualWidth - LeftBottomSegmPoints[5].X, VirtualHeight - LeftBottomSegmPoints[5].Y),

                // the point for rounded Bezier curve
                new Point(VirtualWidth - LeftBottomSegmPoints[6].X, VirtualHeight - LeftBottomSegmPoints[6].Y),

                // three bottom points, starting from the right point
                new Point(VirtualWidth - LeftBottomSegmPoints[0].X, VirtualHeight - LeftBottomSegmPoints[0].Y),
                new Point(VirtualWidth - LeftBottomSegmPoints[1].X, VirtualHeight - LeftBottomSegmPoints[1].Y),
                new Point(VirtualWidth - LeftBottomSegmPoints[2].X, VirtualHeight - LeftBottomSegmPoints[2].Y)
            };

            return points;
        }

        /// <summary>
        /// Calculates points collection for the middle segment
        /// </summary>
        /// <returns></returns>
        protected AvaloniaList<Point> GetMiddleSegmPoints()
        {
            double x = XByAngle((VirtualHeight / 2) + HorizSegH / 2) + (VertSegW + GapW);
            double x1 = XByAngle(VirtualHeight / 2) + VertSegBotPartW + GapW;
            double x2 = XByAngle(VirtualHeight / 2 - HorizSegH / 2) + VertSegW + GapW;

            AvaloniaList<Point> points = new AvaloniaList<Point>
            {

                // three left points, starting from the bottom point
                new Point(x, (VirtualHeight / 2) + HorizSegH / 2),
                new Point(x1, (VirtualHeight / 2)),
                new Point(x2, (VirtualHeight / 2) - HorizSegH / 2),

                // three right points, starting from the top point
                new Point(VirtualWidth - x, RightTopSegmPoints[6].Y + GapW / 2),
                new Point(VirtualWidth - x1, VirtualHeight / 2),
                new Point(VirtualWidth - x2, RightBottomSegmPoints[0].Y - GapW / 2)
            };
            return points;
        }


        /// <summary>
        /// Calulates points for the top segment
        /// </summary>
        /// <returns></returns>
        protected AvaloniaList<Point> GetTopSegmPoints()
        {
            AvaloniaList<Point> points = new AvaloniaList<Point>();
            double topLeftX = LeftTopSegmPoints[1].X + HorizSegSmallPartW;
            double topRightX = RightTopSegmPoints[1].X - HorizSegSmallPartW;
            double coefRound = RoundedCorners ? VirtualWidth / horizRoundCoef : 0;

            // three left points, starting from the bottom point
            points.Add(new Point(LeftTopSegmPoints[2].X + GapW, HorizSegH + startPointThickness));
            points.Add(new Point(LeftTopSegmPoints[1].X + GapShift(), HorizSegSmallPartH + startPointThickness));
            points.Add(new Point(topLeftX, startPointThickness));

            // two top Bezier points starting from the left point
            points.Add(new Point(topLeftX + coefRound, startPointThickness));
            points.Add(new Point(topRightX - coefRound, startPointThickness));

            // three right points, starting from the top left point
            points.Add(new Point(topRightX, startPointThickness));
            points.Add(new Point(RightTopSegmPoints[1].X - GapShift(), HorizSegSmallPartH + startPointThickness));
            points.Add(new Point(RightTopSegmPoints[0].X - GapW, HorizSegH + startPointThickness));

            return points;
        }


        /// <summary>
        /// Calulates points for the bottom segment
        /// </summary>
        /// <returns></returns>
        protected AvaloniaList<Point> GetBottomSegmPoints()
        {
            AvaloniaList<Point> points = new AvaloniaList<Point>();
            double botLeftX = LeftBottomSegmPoints[4].X + HorizSegSmallPartW;
            double botRightX = RightBottomSegmPoints[5].X - HorizSegSmallPartW;
            double coefRound = RoundedCorners ? VirtualWidth / horizRoundCoef : 0;

            // three left points, starting from the bottom point
            points.Add(new Point(botLeftX, VirtualHeight - startPointThickness));
            points.Add(new Point(LeftBottomSegmPoints[4].X + GapShift(), VirtualHeight - HorizSegSmallPartH - startPointThickness));
            points.Add(new Point(LeftBottomSegmPoints[3].X + GapW, VirtualHeight - HorizSegH - startPointThickness));

            // three right points, starting from the top left point
            points.Add(new Point(RightBottomSegmPoints[6].X - GapW, VirtualHeight - HorizSegH - startPointThickness));
            points.Add(new Point(RightBottomSegmPoints[5].X - GapShift(), VirtualHeight - HorizSegSmallPartH - startPointThickness));
            points.Add(new Point(botRightX, VirtualHeight - startPointThickness));

            // two bottom Bezier points starting from the right point
            points.Add(new Point(botRightX - coefRound, VirtualHeight - startPointThickness));
            points.Add(new Point(botLeftX + coefRound, VirtualHeight - startPointThickness));

            return points;
        }


        #endregion

        #region Segments' geometries

        /// <summary>
        /// Right top segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry RightTopSegement()
        {

            RightTopSegmPoints = GetRightTopSegmPoints();
            Point startPoint = RightTopSegmPoints[0];
            LineSegment line0 = new LineSegment { Point = RightTopSegmPoints[0] };
            LineSegment line1 = new LineSegment { Point = RightTopSegmPoints[1] };
            LineSegment line4 = new LineSegment { Point = RightTopSegmPoints[4] };
            LineSegment line5 = new LineSegment { Point = RightTopSegmPoints[5] };
            LineSegment line6 = new LineSegment { Point = RightTopSegmPoints[6] };

            // The Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezier = new AvaloniaList<Point>
            {
                RightTopSegmPoints[1],
                RightTopSegmPoints[2],
                RightTopSegmPoints[3]
            };

            BezierSegment bez = new BezierSegment
            {
                Point1 = RightTopSegmPoints[1],Point2=RightTopSegmPoints[2],Point3= RightTopSegmPoints[3],
            };

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(line0);
            pathFigure.Segments.Add(line1);
            pathFigure.Segments.Add(bez);
            pathFigure.Segments.Add(line4);
            pathFigure.Segments.Add(line5);
            pathFigure.Segments.Add(line6);

            return pathGeometry;
        }


        /// <summary>
        /// Middle segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry MiddleSegement()
        {
            MiddleSegmPoints = GetMiddleSegmPoints();

            Point startPoint = MiddleSegmPoints[0];
            PolyLineSegment segment = new PolyLineSegment { Points = MiddleSegmPoints };
            return SegmentPathGeometry(startPoint, segment);
        }


        /// <summary>
        /// Right bottom segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry RightBottomSegement()
        {

            RightBottomSegmPoints = GetRightBottomSegmPoints();
            Point startPoint = RightBottomSegmPoints[0];
            LineSegment line0 = new LineSegment { Point = RightBottomSegmPoints[0] };
            LineSegment line1 = new LineSegment { Point = RightBottomSegmPoints[1] };
            LineSegment line2 = new LineSegment { Point = RightBottomSegmPoints[2] };
            LineSegment line3 = new LineSegment { Point = RightBottomSegmPoints[3] };
            LineSegment line6 = new LineSegment { Point = RightBottomSegmPoints[6] };

            // The Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezier = new AvaloniaList<Point>
            {
                RightBottomSegmPoints[3],
                RightBottomSegmPoints[4],
                RightBottomSegmPoints[5]
            };
            BezierSegment bez = new BezierSegment
            {
                Point1 = RightBottomSegmPoints[3],
                Point2 = RightBottomSegmPoints[4],
                Point3 = RightBottomSegmPoints[5],
            };
          

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(line0);
            pathFigure.Segments.Add(line1);
            pathFigure.Segments.Add(line2);
            pathFigure.Segments.Add(line3);
            pathFigure.Segments.Add(bez);

            pathFigure.Segments.Add(line6);

            return pathGeometry;
        }


        /// <summary>
        /// Top segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry TopSegement()
        {
            TopSegmPoints = GetTopSegmPoints();
            Point startPoint = TopSegmPoints[0];
            LineSegment line0 = new LineSegment { Point = TopSegmPoints[0] };
            LineSegment line1 = new LineSegment { Point = TopSegmPoints[1] };
            LineSegment line3 = new LineSegment { Point = TopSegmPoints[3] };
            LineSegment line4 = new LineSegment { Point = TopSegmPoints[4] };
            LineSegment line6 = new LineSegment { Point = TopSegmPoints[6] };
            LineSegment line7 = new LineSegment { Point = TopSegmPoints[7] };

            // The left Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezierLeft = new AvaloniaList<Point>
            {
                TopSegmPoints[1], TopSegmPoints[2], TopSegmPoints[3]
            };
            BezierSegment bezLeft = new BezierSegment
            {
                Point1 = TopSegmPoints[1],
                Point2 = TopSegmPoints[2],
                Point3 = TopSegmPoints[3],
            };

           


            // The right Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezierRight = new AvaloniaList<Point>
            {
                TopSegmPoints[4], TopSegmPoints[5], TopSegmPoints[6]
            };
            BezierSegment bezRight = new BezierSegment
            {
                Point1 = TopSegmPoints[4],
                Point2 = TopSegmPoints[5],
                Point3 = TopSegmPoints[6],
            };
           

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(line0);
            pathFigure.Segments.Add(line1);
            pathFigure.Segments.Add(bezLeft);
            pathFigure.Segments.Add(line3);
            pathFigure.Segments.Add(line4);
            pathFigure.Segments.Add(bezRight);
            pathFigure.Segments.Add(line6);
            pathFigure.Segments.Add(line7);

            return pathGeometry;
        }



        /// <summary>
        /// Left top segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry LeftTopSegement()
        {
            LeftTopSegmPoints = GetLeftTopSegmPoints();
            Point startPoint = LeftTopSegmPoints[6];
            LineSegment line0 = new LineSegment { Point = LeftTopSegmPoints[6] };
            LineSegment line1 = new LineSegment { Point = LeftTopSegmPoints[1] };
            LineSegment line2 = new LineSegment { Point = LeftTopSegmPoints[2] };
            LineSegment line3 = new LineSegment { Point = LeftTopSegmPoints[3] };
            LineSegment line4 = new LineSegment { Point = LeftTopSegmPoints[4] };
            LineSegment line5 = new LineSegment { Point = LeftTopSegmPoints[5] };

            // The Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezier = new AvaloniaList<Point>
            {
                LeftTopSegmPoints[6],
                LeftTopSegmPoints[0],
                LeftTopSegmPoints[1]
            };
            BezierSegment bez = new BezierSegment
            {
                Point1 = LeftTopSegmPoints[6],
                Point2 = LeftTopSegmPoints[0],
                Point3 = LeftTopSegmPoints[1]
            };
           
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(bez);
            pathFigure.Segments.Add(line2);
            pathFigure.Segments.Add(line3);
            pathFigure.Segments.Add(line4);
            pathFigure.Segments.Add(line5);


            return pathGeometry;
        }




        /// <summary>
        /// Left Bottom segment drawing
        /// </summary>
        /// <returns></returns>
        protected PathGeometry LeftBottomSegement()
        {
            LeftBottomSegmPoints = GetLeftBottomSegmPoints();
            Point startPoint = LeftBottomSegmPoints[0];
            LineSegment line0 = new LineSegment { Point = LeftBottomSegmPoints[0] };
            LineSegment line1 = new LineSegment { Point = LeftBottomSegmPoints[1] };
            LineSegment line2 = new LineSegment { Point = LeftBottomSegmPoints[2] };
            LineSegment line3 = new LineSegment { Point = LeftBottomSegmPoints[3] };
            LineSegment line4 = new LineSegment { Point = LeftBottomSegmPoints[4] };

            // The Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezier = new AvaloniaList<Point>
            {
                LeftBottomSegmPoints[4],
                LeftBottomSegmPoints[5],
                LeftBottomSegmPoints[6]
            };
            BezierSegment bez = new BezierSegment
            {
                Point1 = LeftBottomSegmPoints[4],
                Point2 = LeftBottomSegmPoints[5],
                Point3 = LeftBottomSegmPoints[6]
            };
            

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(line0);
            pathFigure.Segments.Add(line1);
            pathFigure.Segments.Add(line2);
            pathFigure.Segments.Add(line3);
            pathFigure.Segments.Add(line4);
            pathFigure.Segments.Add(bez);

            return pathGeometry;
        }




        /// <summary>
        /// Bottom segment drawing
        /// </summary>
        protected PathGeometry BottomSegement()
        {
            BottomSegmPoints = GetBottomSegmPoints();
            Point startPoint = BottomSegmPoints[1];

            LineSegment line0 = new LineSegment { Point = BottomSegmPoints[0] };
            LineSegment line1 = new LineSegment { Point = BottomSegmPoints[1] };
            LineSegment line2 = new LineSegment { Point = BottomSegmPoints[2] };
            LineSegment line3 = new LineSegment { Point = BottomSegmPoints[3] };
            LineSegment line4 = new LineSegment { Point = BottomSegmPoints[4] };
            LineSegment line6 = new LineSegment { Point = BottomSegmPoints[6] };
            LineSegment line7 = new LineSegment { Point = BottomSegmPoints[7] };


            // The right Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezierRight = new AvaloniaList<Point>
            {
                BottomSegmPoints[4], BottomSegmPoints[5], BottomSegmPoints[6]
            };
            BezierSegment bezRight = new BezierSegment
            {
                Point1 = BottomSegmPoints[4],
                Point2 = BottomSegmPoints[5],
                Point3 = BottomSegmPoints[6]
            };
            

            // The left Bezier curve for rounded corners
            AvaloniaList<Point> pointsBezierLeft = new AvaloniaList<Point>
            {
                BottomSegmPoints[7], BottomSegmPoints[0], BottomSegmPoints[1]
            };
            BezierSegment bezLeft = new BezierSegment
            {
                Point1 = BottomSegmPoints[7],
                Point2 = BottomSegmPoints[0],
                Point3 = BottomSegmPoints[1]
            };
            

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };
            pathGeometry.Figures.Add(pathFigure);

            pathFigure.Segments.Add(line1);
            pathFigure.Segments.Add(line2);
            pathFigure.Segments.Add(line3);
            pathFigure.Segments.Add(line4);
            pathFigure.Segments.Add(bezRight);
            pathFigure.Segments.Add(line6);
            pathFigure.Segments.Add(line7);
            pathFigure.Segments.Add(bezLeft);

            return pathGeometry;
        }

        #endregion

    }
}
