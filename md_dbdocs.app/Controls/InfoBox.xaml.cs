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
            string v;

            if (e.NewValue != null)
            {
                v = e.NewValue.ToString();
            }
            else
            {
                v = "";
            }

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
    }
}
