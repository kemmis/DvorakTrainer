using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ViewModels
{
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
                    return new SolidColorBrush(Colors.LightGray);
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