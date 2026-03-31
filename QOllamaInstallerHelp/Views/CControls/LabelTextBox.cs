using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QOllamaInstallerHelp.Views.CControls
{
    public class LabelTextBox : TextBox
    {


        public object LabelTitle
        {
            get { return (object)GetValue(LabelTitleProperty); }
            set { SetValue(LabelTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTitleProperty =
            DependencyProperty.Register(nameof(LabelTitle), typeof(object), typeof(LabelTextBox), new PropertyMetadata(null));




        public object EmptyTextDisplay
        {
            get { return (object)GetValue(EmptyTextDisplayProperty); }
            set { SetValue(EmptyTextDisplayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmptyTextDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmptyTextDisplayProperty =
            DependencyProperty.Register(nameof(EmptyTextDisplay), typeof(object), typeof(LabelTextBox), new PropertyMetadata(null));



        static LabelTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelTextBox), new FrameworkPropertyMetadata(typeof(LabelTextBox)));
        }
    }
}
