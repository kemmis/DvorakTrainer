using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using DvorakTrainer.Helpers;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Services;

namespace DvorakTrainer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private int _selectedLevelIndex;
        private string _completedMessage;
        private int _currentWordIndex;
        private bool _enabled;
        private DateTime _endTime;
        private string _enteredText;
        private bool _isMainInputFocused;
        private bool _mapToDvorak;
        private INavigationService _navigationService;
        private bool _resetScroll;
        private bool _showKeyboardLayout = true;

        private DateTime _startTime;
        private List<string> _wordsToMatch;

        private ObservableCollection<WordViewModel> _wordsToType;

        private string _wordToMatch = "";

        public List<Level> Levels = new List<Level>
        {
            new Level
            {
                Name = "1 - Home Row 8 Keys",
                Characters = "a,o,e,u,h,t,n,s",
                LevelIndex = 1
            },
            new Level
            {
                Name = "2 - Full Home Row",
                Characters = "a,o,e,u,i,d,h,t,n,s",
                LevelIndex = 2
            },
            new Level
            {
                Name = "3 - Home Row + c, f, k, l, m, p, r, v",
                Characters = "a,o,e,u,i,d,h,t,n,s,c,f,k,l,m,p,r,v",
                LevelIndex = 3
            },
            new Level
            {
                Name = "4 - Level 3 + b, g, j, q, w, x, y, z",
                Characters = "a,o,e,u,i,d,h,t,n,s,c,f,k,l,m,p,r,v,b,g,j,q,w,x,y,z",
                LevelIndex = 4
            }
        };

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _selectedLevelIndex = StorageHelper.GetSetting("selected-level", 0);
            _mapToDvorak = StorageHelper.GetSetting("map-to-dvorak", true);
            _showKeyboardLayout = StorageHelper.GetSetting("show-keyboard-layout", true);
            SelectedLevel = Levels[_selectedLevelIndex];
        }

        public string CompletedMessage
        {
            get { return _completedMessage; }
            set
            {
                if (_completedMessage != value)
                {
                    _completedMessage = value;
                    OnPropertyChanged(nameof(CompletedMessage));
                }
            }
        }

        public string WordToMatch
        {
            get { return _wordToMatch; }
            set
            {
                if (_wordToMatch != value)
                {
                    _wordToMatch = value;
                    OnPropertyChanged(nameof(WordToMatch));
                }
            }
        }

        public string EnteredText
        {
            get { return _enteredText; }
            set
            {
                if (_enteredText != value)
                {
                    _enteredText = value;
                    OnPropertyChanged(nameof(EnteredText));
                }
            }
        }

        public bool ResetScroll
        {
            get { return _resetScroll; }
            set
            {
                if (value == _resetScroll) return;
                _resetScroll = value;
                OnPropertyChanged(nameof(ResetScroll));
                _resetScroll = false;
            }
        }

        public bool ShowKeyboardLayout
        {
            get { return _showKeyboardLayout; }
            set
            {
                if (_showKeyboardLayout != value)
                {
                    _showKeyboardLayout = value;
                    OnPropertyChanged(nameof(ShowKeyboardLayout));
                    OnPropertyChanged(nameof(KeyboardVisibility));
                }
                IsMainInputFocused = true;
                StorageHelper.StoreSetting("show-keyboard-layout", value, true);
            }
        }

        public Visibility KeyboardVisibility
        {
            get { return ShowKeyboardLayout ? Visibility.Visible : Visibility.Collapsed; }
        }


        public bool MapToDvorak
        {
            get { return _mapToDvorak; }
            set
            {
                if (_mapToDvorak != value)
                {
                    _mapToDvorak = value;
                    OnPropertyChanged(nameof(MapToDvorak));
                }
                IsMainInputFocused = true;
                StorageHelper.StoreSetting("map-to-dvorak", value, true);
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnPropertyChanged(nameof(Enabled));
                    OnPropertyChanged(nameof(WordListOpacity));
                    OnPropertyChanged(nameof(SpinnerVisibility));
                }
            }
        }

        public Visibility SpinnerVisibility
        {
            get { return Enabled ? Visibility.Collapsed : Visibility.Visible; }
        }

        public double WordListOpacity
        {
            get { return Enabled ? 1 : 0.25; }
        }

        public bool IsMainInputFocused
        {
            get { return _isMainInputFocused; }
            set
            {
                if (true || _isMainInputFocused != value)
                {
                    _isMainInputFocused = value;
                    OnPropertyChanged(nameof(IsMainInputFocused));
                }
            }
        }

        public int CurrentWordIndex
        {
            get { return _currentWordIndex; }
            set
            {
                if (_currentWordIndex != value)
                {
                    _currentWordIndex = value;
                    OnPropertyChanged(nameof(CurrentWordIndex));
                }
            }
        }

        public object SelectedLevel { get; set; }

        public ObservableCollection<WordViewModel> WordsToType
        {
            get { return _wordsToType; }
            set
            {
                if (_wordsToType != value)
                {
                    _wordsToType = value;
                    OnPropertyChanged(nameof(WordsToType));
                }
            }
        }

        public event EventHandler LevelComplete;

        public void OnTextChanged()
        {
            var curWordTupple = WordsToType[_currentWordIndex].Text;
            curWordTupple.Compare = EnteredText;
            WordsToType[_currentWordIndex].Text = curWordTupple;
        }

        public void OnSpaceOrEnterPressed()
        {
            var wordToMatch = _wordsToMatch[_currentWordIndex];

            if (wordToMatch == EnteredText)
            {
                WordsToType[_currentWordIndex].Active = false;
                WordsToType[_currentWordIndex].Completed = true;

                var curWordTupple = WordsToType[_currentWordIndex].Text;
                curWordTupple.Compare = "";
                WordsToType[_currentWordIndex].Text = curWordTupple;

                CurrentWordIndex++;

                if (_currentWordIndex < WordsToType.Count)
                {
                    WordsToType[_currentWordIndex].Active = true;
                    WordToMatch = WordsToType[_currentWordIndex].Text.Display;
                }
                else
                {
                    _endTime = DateTime.Now;
                    UpdateCompletedMessage();
                    LevelComplete?.Invoke(this, new EventArgs());
                }
                EnteredText = "";
            }
            else
            {
                EnteredText = "";
                OnTextChanged();
            }
        }


        public async Task OnLevelChange()
        {
            _selectedLevelIndex = ((Level)SelectedLevel).LevelIndex - 1;
            StorageHelper.StoreSetting("selected-level", _selectedLevelIndex, true);
            await Start();
        }

        private async Task Start()
        {
            Disable();
            var levelIndex = ((Level)SelectedLevel).LevelIndex;
            CurrentWordIndex = 0;
            var wls = new WordListService();
            _wordsToMatch = (await wls.GetWordsAsync(100, levelIndex)).ToList();
            var wordViewModels = _wordsToMatch.Select((w, i) => new WordViewModel
            {
                Active = i == 0,
                Text = new StringTupple
                {
                    Display = w,
                    Compare = ""
                }
            });

            WordsToType = new ObservableCollection<WordViewModel>(wordViewModels);
            WordToMatch = WordsToType[0].Text.Display;

            IsMainInputFocused = true;
            ResetScroll = true;
            _startTime = DateTime.Now;
            Enable();
        }

        private void Disable()
        {
            Enabled = false;
        }

        private void Enable()
        {
            Enabled = true;
        }

        private void UpdateCompletedMessage()
        {
            var numWords = WordsToType.Count;
            var duration = Math.Max(1, (_endTime - _startTime).Minutes);
            var rate = (int)Math.Ceiling(numWords / (duration * 1.0));
            var msg = $"You typed {numWords} words in {duration} minutes. That's {rate} words per minute.";
            CompletedMessage = msg;
        }
    }
}