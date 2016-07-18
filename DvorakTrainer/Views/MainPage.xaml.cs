using System;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using DvorakTrainer.Helpers;
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
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            //TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(MainTitleBar);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;

            var showWelcome = StorageHelper.GetSetting("show-welcome", true);
            if (showWelcome)
            {
                var val = await WelcomeDialog.ShowAsync();
                if (dontShowCb.IsChecked ?? false)
                {
                    StorageHelper.StoreSetting("show-welcome", false, true);
                }
            }
            CustomInput.Focus(FocusState.Programmatic);
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //TitleBar.Height = sender.Height;
            RightMask.Width = sender.SystemOverlayRightInset;
        }

        private async void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(ViewModel.ResetScroll))
            {
                await WordListScroll.ScrollToVerticalOffsetWithAnimationAsync(0d);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ProgramMenu.Open();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ProgramMenu.Open();
        }
    }
}
