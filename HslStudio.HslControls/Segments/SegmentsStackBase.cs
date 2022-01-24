using Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// A base classs for stack of segment controls
    /// </summary>
    [DesignTimeVisible(false)]
    public class SegmentsStackBase : SegmentBase
    {
        public static StyledProperty<int> ElementsCountProperty;

        /// <summary>
        /// Number of segment elements to show
        /// </summary>
        public int ElementsCount
        {
            get => (int)GetValue(ElementsCountProperty);
            set => SetValue(ElementsCountProperty, value);
        }

        static SegmentsStackBase()
        {
           
            ElementsCountProperty = AvaloniaProperty.Register<SegmentsStackBase, int>(nameof(ElementsCount), 1);
            ElementsCountProperty.Changed.AddClassHandler<SegmentsStackBase>(x => x.CountChanged);

        }

        public SegmentsStackBase()
        {
            PropertyChanged += OnPropertyChanged;
        }

        public virtual void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
        public void CountChanged(AvaloniaPropertyChangedEventArgs e)
        {
            SegmentsStackBase segments = (SegmentsStackBase)e.Sender;
            segments.OnPropertyChanged(e.Sender, e);
        }
      

        public ObservableCollection<CharItem> GetCharsArray()
        {
            if (Value == null)
            {
                Value = "9999";
            }
            // converts value to char array
            char[] charArray = Value.ToCharArray();
            // the dots count
            int dotCount = charArray.Where(c => c == '.').Count();
            // the colons count
            int colonCount = charArray.Where(c => c == ':').Count();

            // the chars count without dots and colons
            int charCount = charArray.Count() - dotCount;

            ObservableCollection<CharItem> valueChars = new ObservableCollection<CharItem>();
            int index = 0;

            if (charArray.Count() > 0)
            {
                for (int i = 0; i < ElementsCount; i++)
                {
                    // sets properties for the each seven segment item
                    CharItem item = new CharItem
                    {
                        ShowDot = ShowDot,
                        ShowColon = ShowColon,
                        FillBrush = FillBrush,
                        SelectedFillBrush = SelectedFillBrush,
                        PenColor = PenColor,
                        SelectedPenColor = SelectedPenColor,
                        PenThickness = PenThickness,
                        GapWidth = GapWidth,
                        RoundedCorners = RoundedCorners,
                        TiltAngle = TiltAngle,
                        VertSegDivider = VertSegDivider,
                        HorizSegDivider = HorizSegDivider
                    };

                    valueChars.Add(item);

                    if (i >= ElementsCount - charCount)
                    {
                        if (index <= charArray.Count() - 1)
                        {
                            // sets char for the element
                            if (charArray[index] != '.' && charArray[index] != ':')
                            {
                                valueChars[i].Item = charArray[index];
                            }

                            // sets ":" for the element
                            if (charArray[index] == ':')
                            {
                                valueChars[i].OnColon = true;
                            }

                            // sets dot for the element
                            if (charArray[index] == '.')
                            {
                                valueChars[i - 1].OnDot = true;
                                valueChars[i].Item = charArray[index + 1];
                                index++;
                            }
                        }

                        index++;
                    }
                }


                // sets dot for the last element if required
                if (ElementsCount >= charCount)
                {
                    if (charArray[charArray.Count() - 1] == '.')
                    {
                        CharItem item = valueChars.Last();
                        item.OnDot = true;
                    }
                }
                else
                {
                    if (charArray[index] == '.')
                    {
                        CharItem item = valueChars[ElementsCount - 1];
                        item.OnDot = true;
                    }
                }

            }

            return valueChars;
        }

    }
}
