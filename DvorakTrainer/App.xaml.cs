using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvorakTrainer.Helpers;
using Prism.Mvvm;
using Prism.Windows;
using ViewModels;
using MainPageViewModel = DvorakTrainer.ViewModels.MainPageViewModel;

namespace DvorakTrainer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() : base()
        {
            this.InitializeComponent();

#if DEBUG

            bool CLEAR_SETTINGS = false;

            if (CLEAR_SETTINGS)
            {
                StorageHelper.ClearAllSettings();
            }

#endif
            
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {

            NavigationService.Navigate("Main", null);
            
            return Task.FromResult<object>(null);
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // Register factory methods for the ViewModelLocator for each view model that takes dependencies so that you can pass in the
            // dependent services from the factory method here.
            ViewModelLocationProvider.Register(typeof(Views.MainPage).ToString(), () => new MainPageViewModel(NavigationService));

            return base.OnInitializeAsync(args);
        }

        

    }
}
