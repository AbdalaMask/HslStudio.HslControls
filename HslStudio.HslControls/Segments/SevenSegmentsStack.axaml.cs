using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HslStudio.HslControls.Segments
{
    /// <summary>
    /// Interaction logic for SevenSegmentsStack.xaml
    /// </summary>
    [DesignTimeVisible(false)]
    public partial class SevenSegmentsStack : SegmentsStackBase
    {
        private ItemsControl SegmentsArray;
        /// <summary>
        /// Stores chars from the splitted value string
        /// </summary>
        private ObservableCollection<CharItem> ValueChars;

        public SevenSegmentsStack()
        {
            InitializeComponent();
            SegmentsArray = this.FindControl<ItemsControl>("SegmentsArray");
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
