using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml.Media;
using ViewModels.Annotations;

namespace ViewModels
{
    public class WordViewModel : INotifyPropertyChanged
    {
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged(nameof(Active));
                    OnPropertyChanged(nameof(BorderColor));
                }
            }
        }

        private bool _completed = false;

        public bool Completed
        {
            get
            {
                return _completed;
            }
            set
            {
                if (_completed != value)
                {
                    _completed = value;
                    OnPropertyChanged(nameof(Completed));
                }
            }
        }
        private ObservableCollection<StringTupple> _letters;

        private StringTupple _text;
        private bool _active;

        public StringTupple Text
        {
            set
            {
                _text = value;

                Letters.Clear();

                for (var i = 0; i < Math.Max(value.Display.Length, value.Compare.Length); i++)
                {
                    var tupple = new StringTupple()
                    {
                        Display = i < value.Display.Length ? value.Display[i].ToString() : "",
                        Compare = i < value.Compare.Length ? value.Compare[i].ToString() : "",
                        WordViewModel = this
                    };
                    Letters.Add(tupple);
                }
            }

            get { return _text; }
        }

        public ObservableCollection<StringTupple> Letters
        {
            get
            {
                if (_letters == null)
                {
                    _letters = new ObservableCollection<StringTupple>();
                }
                return _letters;
            }
            set { _letters = value; }
        }

        public SolidColorBrush BorderColor
        {
            get
            {
                return new SolidColorBrush(Active ? Colors.DarkSeaGreen : Colors.Transparent);
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