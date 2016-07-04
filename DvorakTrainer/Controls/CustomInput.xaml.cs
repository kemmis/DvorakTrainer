using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Services;
using ViewModels.Annotations;
using WinRTXamlToolkit.Controls.Extensions;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvorakTrainer.Controls
{
    public sealed partial class CustomInput : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty WordToMatchProperty = DependencyProperty.Register(
            "WordToMatch", typeof(string), typeof(CustomInput), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty MapToDvorakProperty = DependencyProperty.Register(
            "MapToDvorak", typeof(bool), typeof(CustomInput), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(CustomInput), new PropertyMetadata(default(string)));

        private string _cursorMargin = "0,0,0,0";
        private Visibility _cursorVisibility = Visibility.Collapsed;

        public ObservableCollection<BufferCharacter> Buffer = new ObservableCollection<BufferCharacter>();

        private bool focused;

        public CustomInput()
        {
            InitializeComponent();
            var cw = CoreWindow.GetForCurrentThread();
            cw.CharacterReceived += CwOnCharacterReceived;
            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;
            Tapped += OnTapped;
            Storyboard1.Begin();
        }

        public string WordToMatch
        {
            get { return (string) GetValue(WordToMatchProperty); }
            set { SetValue(WordToMatchProperty, value); }
        }

        public bool MapToDvorak
        {
            get { return (bool) GetValue(MapToDvorakProperty); }
            set { SetValue(MapToDvorakProperty, value); }
        }

        public Visibility CursorVisibility
        {
            get { return _cursorVisibility; }
            set
            {
                if (value == _cursorVisibility) return;
                _cursorVisibility = value;
                OnPropertyChanged(nameof(CursorVisibility));
            }
        }

        public string CursorMargin
        {
            get { return _cursorMargin; }
            set
            {
                if (value == _cursorMargin) return;
                _cursorMargin = value;
                OnPropertyChanged(nameof(CursorMargin));
            }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                if (string.IsNullOrEmpty(value))
                {
                    Buffer.Clear();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LettersStackPanelOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            PositionCursor();
        }

        public event EventHandler TextChanged;
        public event EventHandler EnterKeyPressed;
        public event EventHandler SpaceKeyPressed;

        private void OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            Focus(FocusState.Programmatic);
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            focused = false;
            CursorVisibility = Visibility.Collapsed;
        }

        private void OnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            focused = true;
            CursorVisibility = Visibility.Visible;
        }


        private void CwOnCharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (!focused) return;

            var c = Convert.ToChar(args.KeyCode);

            if (MapToDvorak)
            {
                c = DvorakConverter.Convert(c);
            }

            if (char.IsLetter(c))
            {
                var bc = new BufferCharacter
                {
                    Character = c.ToString()
                };

                if (Buffer.Count < WordToMatch.Length)
                {
                    bc.CharacterToMatch = WordToMatch[Buffer.Count].ToString();
                }

                Buffer.Add(bc);
                Text += c;
                TextChanged?.Invoke(this, new EventArgs());
            }
            else if (c == '\b')
            {
                if (Buffer.Any())
                {
                    Buffer.Remove(Buffer.Last());
                }
                if (!string.IsNullOrEmpty(Text))
                {
                    if (Text.Length > 0)
                    {
                        Text = Text.Substring(0, Text.Length - 1);
                        TextChanged?.Invoke(this, new EventArgs());
                    }
                }
            }
            else if (c == '\r')
            {
                EnterKeyPressed?.Invoke(this, new EventArgs());
            }
            else if (c == ' ')
            {
                SpaceKeyPressed?.Invoke(this, new EventArgs());
            }
        }

        private void PositionCursor()
        {
            var pos = 0;

            var last = LettersStackPanel.GetDescendantsOfType<StackPanel>().LastOrDefault();
            if (last != null)
            {
                pos = Convert.ToInt32(Math.Floor(last.GetPosition(relativeTo: this).X + last.ActualWidth));
            }
            CursorMargin = pos + ",0,0,0";
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PositionCursor();
        }
    }

    public class BufferCharacter
    {
        public string Character { get; set; }
        public string CharacterToMatch { get; set; }

        public SolidColorBrush Color
        {
            get { return new SolidColorBrush(CharacterToMatch == Character ? Colors.Black : Colors.Red); }
        }
    }
}