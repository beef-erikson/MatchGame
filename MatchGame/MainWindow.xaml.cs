using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Match variables
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        int matchesFound;

        // Timer support
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
 
        /// <summary>
        /// Sets game up
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        /// <summary>
        /// Updates timer textblock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10f).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again? (Click Here)";
            }
        }

        /// <summary>
        /// Sets a list of 8 matching pair of animal emojis and randomly assigns each textblock an emoji
        /// </summary>
        private void SetUpGame()
        {
            // Creates list of emojis
            List<string> animalEmoji = new List<string>()
            {
                "🦊", "🦊",
                "🦔", "🦔",
                "🦝", "🦝",
                "🦒", "🦒",
                "🦁", "🦁",
                "🐷", "🐷",
                "🐀", "🐀",
                "🐰", "🐰",
            };

            Random random = new Random();

            // Sets each textBlock that isn't the time status to an random emoji from list
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            // Starts timer and sets variables to 0
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }


        /// <summary>
        /// Checks for match and changes visibilities of emojis as necessary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            // First click
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            // It's a match
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            // It's not a match
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        /// <summary>
        /// Resets the game if all matches have been found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
