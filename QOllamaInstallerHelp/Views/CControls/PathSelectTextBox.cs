using Microsoft.Win32;
using QOllamaInstallerHelp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class PathSelectTextBox : TextBox
    {
        public class PathSelectTextBoxViewModel
        {
            public ICommand PathSelectCommand { get; set; }
        }
        public enum PathSelectType
        {
            File, Fold
        }


        public bool IsSuccessInput
        {
            get { return (bool)GetValue(IsSuccessInputProperty); }
            set { SetValue(IsSuccessInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSuccessInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSuccessInputProperty =
            DependencyProperty.Register(nameof(IsSuccessInput), typeof(bool), typeof(PathSelectTextBox), new PropertyMetadata(false));



        public PathSelectType PathType
        {
            get { return (PathSelectType)GetValue(PathTypeProperty); }
            set { SetValue(PathTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathTypeProperty =
            DependencyProperty.Register(nameof(PathType), typeof(PathSelectType), typeof(PathSelectTextBox), new PropertyMetadata(PathSelectType.File));


        public ICommand PathSelectCommand { get; private set; }

        static PathSelectTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathSelectTextBox), new FrameworkPropertyMetadata(typeof(PathSelectTextBox)));
        }
        public PathSelectTextBox()
        {
            PathSelectCommand = new ActionCommand((obj) =>
            {
                this.Text = CreatePathSelect(this.PathType);
            }, (o) => true);
        }
        private static bool IsSuccessTextInput(string Text, PathSelectType Type)
        {
            switch (Type)
            {
                case PathSelectType.File:
                    return File.Exists(Text);
                case PathSelectType.Fold:
                    return Directory.Exists(Text);
                default:
                    throw new("未实现的类型判断 " + Type);
            }
        }
        private static string CreatePathSelect(PathSelectType Type)
        {
            switch (Type)
            {
                case PathSelectType.File:
                    {
                        var open = new OpenFileDialog();
                        open.ShowDialog();
                        return open.FileName;
                    }
                case PathSelectType.Fold:
                    {
                        var open = new OpenFolderDialog();
                        open.ShowDialog();
                        return open.FolderName;
                    }
                default:
                    throw new("未实现的类型判断 " + Type);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            IsSuccessInput = IsSuccessTextInput(Text, this.PathType);
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
    }
}
