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
        /// <summary>
        /// Stores chars from the splitted value string
        /// </summary>
        private ObservableCollection<CharItem> ValueChars;

        public SevenSegmentsStack()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnPropertyChanged(IAvaloniaObject sender, AvaloniaPropertyChangedEventArgs e)
        {
            ValueChars = GetCharsArray();
            SegmentsArray.ItemsSource = ValueChars;
        }
    }
}
