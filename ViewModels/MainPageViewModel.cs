using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            SelectedLevel = Levels[0];
        }

        private int _currentWordIndex = 0;
        private List<string> _wordsToMatch = new List<string>()
        {
            "derp","wowee","wicked","shitstorm","buttlick","dickfuck","teabag","assclown","exhorated","green","dirty","dicky"
        };

        public Level SelectedLevel { get; set; }
        public List<Level> Levels = new List<Level>()
        {
            new Level()
            {
                Name = "1 - Home Row 8 Keys",
                Characters = "a,o,e,u,h,t,n,s"
            },
            new Level()
            {
                Name = "2 - Full Home Row",
                Characters = "a,o,e,u,i,d,h,t,n,s"
            },
            new Level()
            {
                Name = "3 - Home Row + c, f, k, l, m, p, r, v",
                Characters = "a,o,e,u,i,d,h,t,n,s,c,f,k,l,m,p,r,v"
            }
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

    public class Level
    {
        public string Characters { get; set; }
        public string Name { get; set; }
    }
}
