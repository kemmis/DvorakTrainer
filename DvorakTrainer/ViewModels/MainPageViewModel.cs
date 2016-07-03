﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using ViewModels;
using ViewModels.Annotations;
using WinRTXamlToolkit.Controls.Extensions;
using Services;

namespace DvorakTrainer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectedLevel = Levels[0];
            //Start();
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
                }
            }
        }

        public string CursorMargin
        {
            get { return _cursorMargin; }
            set
            {
                if (_cursorMargin != value)
                {
                    _cursorMargin = value;
                    OnPropertyChanged(nameof(CursorMargin));
                }
            }
        }

        public Visibility CursorVisibility
        {
            get { return _cursorVisibility; }
            set
            {
                if (_cursorVisibility != value)
                {
                    _cursorVisibility = value;
                    OnPropertyChanged(nameof(CursorVisibility));
                }
            }
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

        private int _currentWordIndex = 0;
        private List<string> _wordsToMatch;

        public Object SelectedLevel { get; set; }
        public List<Level> Levels = new List<Level>()
        {
            new Level()
            {
                Name = "1 - Home Row 8 Keys",
                Characters = "a,o,e,u,h,t,n,s",
                LevelIndex = 1
            },
            new Level()
            {
                Name = "2 - Full Home Row",
                Characters = "a,o,e,u,i,d,h,t,n,s",
                LevelIndex = 2
            },
            new Level()
            {
                Name = "3 - Home Row + c, f, k, l, m, p, r, v",
                Characters = "a,o,e,u,i,d,h,t,n,s,c,f,k,l,m,p,r,v",
                LevelIndex = 3
            },
            new Level()
            {
                Name = "4 - Level 3 + b, g, j, q, w, x, y, z",
                Characters = "a,o,e,u,i,d,h,t,n,s,c,f,k,l,m,p,r,v,b,g,j,q,w,x,y,z",
                LevelIndex = 4
            }
        };

        private ObservableCollection<WordViewModel> _wordsToType;
        private bool _enabled;
        private bool _isMainInputFocused;
        private bool _mapToDvorak;
        private string _cursorMargin = "0,0,0,0";
        private bool _showKeyboardLayout = true;
        private Visibility _cursorVisibility;
        private bool _resetScroll;

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

        public void OnInputKeyUp(object sender, KeyRoutedEventArgs args)
        {
            var tb = sender as TextBox;
            var reb = tb.GetFirstAncestorOfType<Grid>().GetFirstDescendantOfType<RichEditBox>();

            // always keep cursor at end of textbox
            tb.SelectionStart = tb.Text.Length;
            tb.SelectionLength = 0;

            var dvTxt = MapToDvorak ? DvorakConverter.Convert(tb.Text) : tb.Text;
            reb.Document.SetText(TextSetOptions.None, dvTxt);

            if (args.Key == VirtualKey.Space)
            {
                reb.Document.SetText(TextSetOptions.None, "");
                tb.Text = "";
                var r2 = reb.Document.GetRange(0, 0);
                Rect rect2;
                int hit2;
                r2.GetRect(PointOptions.ClientCoordinates, out rect2, out hit2);

                CursorMargin = (rect2.Right + 10) + ",0,0,0";
                return;
            }

            string textEntered = null;
            reb.Document.GetText(TextGetOptions.NoHidden, out textEntered);
            var curWordTupple = WordsToType[_currentWordIndex].Text;
            curWordTupple.Compare = textEntered;
            WordsToType[_currentWordIndex].Text = curWordTupple;

            var r1 = reb.Document.GetRange(0, textEntered.Length);
            Rect rect;
            int hit;
            r1.GetRect(PointOptions.ClientCoordinates, out rect, out hit);
            CursorMargin = (rect.Right + 14) + ",0,0,0";

            var wordToMatch = _wordsToMatch[_currentWordIndex];
            for (int i = 0; i < textEntered.Length; i++)
            {
                var highlightCharacter = false;
                // if text to match isn't as long as i, then highlight character i
                if (wordToMatch.Length <= i)
                {
                    highlightCharacter = true;
                }
                else if (wordToMatch[i] != textEntered[i])
                {
                    highlightCharacter = true;
                }

                var range = reb.Document.GetRange(i, i + 1);
                if (range != null)
                {
                    range.CharacterFormat.ForegroundColor = highlightCharacter ? Colors.Red : Colors.Black;
                    reb.Document.ApplyDisplayUpdates();
                }
            }
        }

        public void OnInputTapped(object sender, TappedRoutedEventArgs args)
        {
            var tb = sender as TextBox;

            // if input is tapped, put cursor at end of line so typing doesn't get interupted
            // always keep cursor at end of textbox
            tb.SelectionStart = tb.Text.Length;
            tb.SelectionLength = 0;

        }

        public void OnInputDoubleTapped(object sender, DoubleTappedRoutedEventArgs args)
        {
            var tb = sender as TextBox;

            // if input is tapped, put cursor at end of line so typing doesn't get interupted
            // always keep cursor at end of textbox
            tb.SelectionStart = tb.Text.Length;
            tb.SelectionLength = 0;

        }

        public void OnInputFocus(object sender, RoutedEventArgs args)
        {
            var tb = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                CursorMargin = (Convert.ToInt32(Math.Floor(tb.ActualWidth / 2))) + ",0,0,0";
            }

            CursorVisibility = Visibility.Visible;
        }

        public void OnInputLostFocus(object sender, RoutedEventArgs args)
        {
            CursorVisibility = Visibility.Collapsed;
        }

        public void OnInputKeyDown(object sender, KeyRoutedEventArgs args)
        {

            var tb = sender as TextBox;
            var reb = tb.GetFirstAncestorOfType<Grid>().GetFirstDescendantOfType<RichEditBox>();
            //reb.Document.SetText(TextSetOptions.None, tb.Text);

            string textEntered = MapToDvorak ? DvorakConverter.Convert(tb.Text) : tb.Text;
            //reb.Document.GetText(TextGetOptions.NoHidden, out textEntered);

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

                    CurrentWordIndex++;

                    if (_currentWordIndex < WordsToType.Count)
                    {
                        WordsToType[_currentWordIndex].Active = true;
                    }
                }
                reb.Document.SetText(TextSetOptions.None, "");
                tb.Text = "";
            }
        }

        public async Task OnLevelChange()
        {
            await Start();
        }

        private async Task Start()
        {
            Disable();
            var levelIndex = ((Level)SelectedLevel).LevelIndex;
            CurrentWordIndex = 0;
            var wls = new WordListService();
            _wordsToMatch = (await wls.GetWordsAsync(100, levelIndex)).ToList();
            var wordViewModels = _wordsToMatch.Select((w, i) => new WordViewModel()
            {
                Active = i == 0,
                Text = new StringTupple()
                {
                    Display = w,
                    Compare = ""
                }
            });

            WordsToType = new ObservableCollection<WordViewModel>(wordViewModels);

            IsMainInputFocused = true;
            ResetScroll = true;
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
    }
}
