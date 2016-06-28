using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ViewModels;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvorakTrainer
{
    public sealed partial class WordControl : UserControl
    {
        public WordViewModel ViewModel
        {
            get
            {
                return (WordViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty, value);

                ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(ViewModel.Active))
            {
                AdjustScrollPosition();
            }
        }

        public static readonly DependencyProperty ViewModelProperty =
           DependencyProperty.Register("ViewModel", typeof(WordViewModel), typeof(WordControl),
                new PropertyMetadata(new WordViewModel()));


        public WordControl()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            AdjustScrollPosition();
        }

        private async void AdjustScrollPosition()
        {
            if (ViewModel.Active)
            {
                var sv = this.GetFirstAncestorOfType<ScrollViewer>();
                if (sv != null)
                {
                    var wp = this.GetFirstAncestorOfType<WrapPanel>();
                    var t = TransformToVisual(wp);
                    Point point = t.TransformPoint(new Point(0, 0));
                    await sv.ScrollToVerticalOffsetWithAnimationAsync(point.Y, 0.4);
                }
            }
        }
    }


}
