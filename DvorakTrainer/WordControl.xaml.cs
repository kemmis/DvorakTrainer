using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvorakTrainer
{
    public sealed partial class WordControl : UserControl
    {
        public WordViewModel ViewModel
        {
            get
            {
                return (WordViewModel) GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);
            }
        }

        public static readonly DependencyProperty ViewModelProperty =
           DependencyProperty.Register("ViewModel", typeof(WordViewModel), typeof(WordControl),
                new PropertyMetadata(new WordViewModel()));


        public WordControl()
        {
            this.InitializeComponent();
        }
    }
}
