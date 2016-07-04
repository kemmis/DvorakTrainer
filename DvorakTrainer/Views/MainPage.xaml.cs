using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ViewModels;
using WinRTXamlToolkit.Controls.Extensions;
using MainPageViewModel = DvorakTrainer.ViewModels.MainPageViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DvorakTrainer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel
        {
            get
            {
                return (MainPageViewModel)DataContext;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            ViewModel.LevelComplete += ViewModelOnLevelComplete;
            this.Loaded += OnLoaded;
        }

        private async void ViewModelOnLevelComplete(object sender, EventArgs eventArgs)
        {
            await LevelCompleteDialog.ShowAsync();
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var val = await WelcomeDialog.ShowAsync();
        }

        private async void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(ViewModel.ResetScroll))
            {
                await WordListScroll.ScrollToVerticalOffsetWithAnimationAsync(0d);
            }

        }
    }
}
