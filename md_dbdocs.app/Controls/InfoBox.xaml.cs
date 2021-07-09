using System.Windows;
using System.Windows.Controls;

namespace md_dbdocs.app.Controls
{

    /// <summary>
    /// Interaction logic for InfoBox.xaml
    /// </summary>
    public partial class InfoBox : UserControl
    {
        public InfoBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SetValueProperty = DependencyProperty.Register("SetBoxValue", typeof(string), typeof(InfoBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSetValueChanged)));
        public string SetBoxValue
        {
            get { return (string)GetValue(SetValueProperty); }
            set { SetValue(SetValueProperty, value); }
        }

        private static void OnSetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoBox infoBox = d as InfoBox;
            infoBox.OnSetValueChanged(e);
        }

        private void OnSetValueChanged(DependencyPropertyChangedEventArgs e)
        {
            string v = e.NewValue != null ? e.NewValue.ToString() : "";
            iBoxValue.Text = v;
        }


        public static readonly DependencyProperty SetLabelProperty = DependencyProperty.Register("SetBoxLabel", typeof(string), typeof(InfoBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSetLabelChanged)));
        public string SetBoxLabel
        {
            get { return (string)GetValue(SetLabelProperty); }
            set { SetValue(SetLabelProperty, value); }
        }

        private static void OnSetLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoBox infoBox = d as InfoBox;
            infoBox.OnSetLabelChanged(e);
        }

        private void OnSetLabelChanged(DependencyPropertyChangedEventArgs e)
        {
            iBoxLabel.Text = e.NewValue.ToString();
        }

        public static readonly DependencyProperty SetWidthProperty = DependencyProperty.Register("SetBoxWidth", typeof(double), typeof(InfoBox), new PropertyMetadata(90D, new PropertyChangedCallback(OnSetWidthChanged)));
        public double SetBoxWidth
        {
            get { return (double)GetValue(SetWidthProperty); }
            set { SetValue(SetWidthProperty, value); }
        }

        private static void OnSetWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoBox infoBox = d as InfoBox;
            infoBox.OnSetWidthChanged(e);
        }

        private void OnSetWidthChanged(DependencyPropertyChangedEventArgs e)
        {
            double othersW = (iBoxLabel.ActualWidth + iBoxLabel.Margin.Left + iBoxLabel.Margin.Right) +
                            (iSeparate.ActualWidth + iSeparate.Margin.Left + iSeparate.Margin.Right) +
                            (iBoxValue.Margin.Left + iBoxValue.Margin.Right);
            double newWidth = (double)e.NewValue - othersW;
            iBoxValue.MaxWidth = newWidth < 0 ? 0 : newWidth;
        }
    }
}
