using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvorakTrainer
{
    public sealed partial class Key : UserControl
    {
        public Key()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Key),
                new PropertyMetadata("A"));

        public bool IndexKey
        {
            get { return (bool)GetValue(IndexKeyProperty); }
            set { SetValue(IndexKeyProperty, value); }
        }

        public static readonly DependencyProperty IndexKeyProperty =
            DependencyProperty.Register("IndexKey", typeof(bool), typeof(Key),
                new PropertyMetadata(false));

        public SolidColorBrush Color
        {
            get
            {
                return new SolidColorBrush(IndexKey ? Colors.Black : Colors.DimGray);
            }
        }

        
    }
}
