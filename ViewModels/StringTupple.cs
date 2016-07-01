using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ViewModels.Annotations;

namespace ViewModels
{
    public class StringTupple:INotifyPropertyChanged
    {
        private WordViewModel _wordViewModel;
        public string Display { get; set; }
        public string Compare { get; set; }

        public WordViewModel WordViewModel
        {
            get { return _wordViewModel; }
            set
            {
                _wordViewModel = value; 
                _wordViewModel.PropertyChanged += WordViewModelOnPropertyChanged;
            }
        }

        private void WordViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(_wordViewModel.Active))
            {
                OnPropertyChanged(nameof(ForegroundColor));
            }
        }

        public SolidColorBrush ForegroundColor
        {
            get
            {
                if (WordViewModel.Completed)
                {
                    return Application.Current.Resources["SystemControlForegroundBaseMediumBrush"] as SolidColorBrush;
                }

                if (!WordViewModel.Active)
                {
                    //SystemColorWindowTextColor
                    return Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush;
                }

                if (string.IsNullOrWhiteSpace(Compare))
                {
                    // SystemControlForegroundBaseMediumLowBrush
                    return Application.Current.Resources["SystemControlForegroundAltHighBrush"] as SolidColorBrush;
                }

                // SystemControlForegroundAltHighBrush
                //SystemControlForegroundBaseMediumHighBrush
                return Display != Compare
                    ? new SolidColorBrush(Colors.Red)
                    : Application.Current.Resources["SystemControlForegroundBaseMediumHighBrush"] as SolidColorBrush;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}