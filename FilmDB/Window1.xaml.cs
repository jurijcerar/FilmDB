using System.Windows;
using System.Windows.Controls;

namespace Filmska_Baza
{
    /// <summary>
    /// Settings dialog — genre management and autosave configuration.
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            LoadGenres();
        }

        // ── Genre management ─────────────────────────────────────────────────

        private void LoadGenres()
        {
            foreach (string g in Properties.Settings.Default.genres)
                Genres_List.Items.Add(g);
        }

        private void Add_Genre(object sender, RoutedEventArgs e)
        {
            string g = Add_Genre_Textbox.Text.Trim();
            if (g == "") return;
            Properties.Settings.Default.genres.Add(g);
            Properties.Settings.Default.Save();
            Genres_List.Items.Add(g);
        }

        private void Delete_Genre(object sender, RoutedEventArgs e)
        {
            if (Genres_List.SelectedIndex < 0) return;
            Properties.Settings.Default.genres.Remove(Genres_List.SelectedItem.ToString());
            Properties.Settings.Default.Save();
            Genres_List.Items.RemoveAt(Genres_List.SelectedIndex);
        }

        private void Edit_Genre(object sender, RoutedEventArgs e)
        {
            string g = Add_Genre_Textbox.Text.Trim();
            if (g == "" || Genres_List.SelectedIndex < 0) return;

            int idx = Genres_List.SelectedIndex;
            Properties.Settings.Default.genres.Insert(idx, g);
            Properties.Settings.Default.genres.RemoveAt(idx + 1);
            Properties.Settings.Default.Save();

            Genres_List.Items.Insert(idx, g);
            Genres_List.Items.RemoveAt(idx + 1);
        }

        // ── Accept ───────────────────────────────────────────────────────────

        private void Accept_Button(object sender, RoutedEventArgs e)
        {
            // Validate autosave time if autosave is enabled
            if (AS_bool.IsChecked == true)
            {
                if (!int.TryParse(AS_time.Text, out int minutes) || minutes <= 0)
                {
                    MessageBox.Show("Please enter a valid autosave interval (minutes > 0).",
                                    "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            DialogResult = true;
        }
    }
}
