using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ViewModels.Annotations;

namespace ViewModels
{
    public class MainPageViewModel
    {

        public MainPageViewModel()
        {
            var wordViewModels = _wordsToMatch.Select((w, i) => new WordViewModel()
            {
                Active = i == 0,
                Text = new StringTupple()
                {
                    Display = w,
                    Compare = ""
                }
            });

            _wordsToType = new ObservableCollection<WordViewModel>(wordViewModels);
        }

        private int _currentWordIndex = 0;
        private List<string> _wordsToMatch = new List<string>()
        {
            "derp","wowee","wicked","shitstorm","buttlick","dickfuck","teabag","assclown","exhorated","green","dirty","dicky"
        };

        private ObservableCollection<WordViewModel> _wordsToType;

        public ObservableCollection<WordViewModel> WordsToType
        {
            get { return _wordsToType; }
            set { _wordsToType = value; }
        }

        public void OnInputKeyUp(object sender, KeyRoutedEventArgs args)
        {
            var reb = sender as RichEditBox;
            if (args.Key == VirtualKey.Space)
            {
                reb.Document.SetText(TextSetOptions.None, "");
                return;
            }

            string textEntered = null;
            reb.Document.GetText(TextGetOptions.NoHidden, out textEntered);
            var curWordTupple = WordsToType[_currentWordIndex].Text;
            curWordTupple.Compare = textEntered;
            WordsToType[_currentWordIndex].Text = curWordTupple;
        }

        public void OnInputKeyDown(object sender, KeyRoutedEventArgs args)
        {
            var reb = sender as RichEditBox;
            string textEntered = null;
            reb.Document.GetText(TextGetOptions.NoHidden, out textEntered);

            var wordToMatch = _wordsToMatch[_currentWordIndex];

            if (args.Key == VirtualKey.Space)
            {
                if (wordToMatch == textEntered.Trim())
                {
                    WordsToType[_currentWordIndex].Active = false;
                    WordsToType[_currentWordIndex].Completed = true;

                    var curWordTupple = WordsToType[_currentWordIndex].Text;
                    curWordTupple.Compare = "";
                    WordsToType[_currentWordIndex].Text = curWordTupple;

                    _currentWordIndex++;

                    if (_currentWordIndex < WordsToType.Count)
                    {
                        WordsToType[_currentWordIndex].Active = true;
                    }
                }
                reb.Document.SetText(TextSetOptions.None, "");
                //HighlightCurrentWord();
            }
        }
    }

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

    public class StringTupple
    {
        public string Display { get; set; }
        public string Compare { get; set; }

        public WordViewModel WordViewModel { get; set; }
        public SolidColorBrush ForegroundColor
        {
            get
            {
                if (WordViewModel.Completed)
                {
                    return new SolidColorBrush(Colors.DimGray);
                }

                if (!WordViewModel.Active)
                {
                    return new SolidColorBrush(Colors.Black);
                }

                if (string.IsNullOrWhiteSpace(Compare))
                {
                    return new SolidColorBrush(Colors.Black);
                }

                return new SolidColorBrush(Display != Compare ? Colors.Red : Colors.Bisque);
            }
        }

    }
}
