using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ViewModels;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvorakTrainer.Controls
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
