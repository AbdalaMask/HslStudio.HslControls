using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;

namespace HslStudio.HslControls.Segments
{
    public partial class SixteenSegmentsStack : SegmentsStackBase
    {    /// <summary>
         /// Stores chars from the splitted value string
         /// </summary>
        private ObservableCollection<CharItem> ValueChars;

        private ItemsControl SegmentsArray;

        public SixteenSegmentsStack()
        {
            InitializeComponent();
            SegmentsArray = this.FindControl<ItemsControl>("SegmentsArray");
            VertSegDivider = defVertDividerSixteen;
            HorizSegDivider = defHorizDividerSixteen;
           
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            ValueChars = GetCharsArray();
            SegmentsArray.Items = ValueChars;
        }
    }
}
