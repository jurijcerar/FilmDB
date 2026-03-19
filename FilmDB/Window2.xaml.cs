using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Filmska_Baza
{
    /// <summary>
    /// Add / Edit film dialog
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            Load_Genres();
        }

        // ── Image ────────────────────────────────────────────────────────────

        private void Image_Click(object sender, MouseButtonEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                // Bug fix: *.GIF,*.PNG → *.GIF;*.PNG (comma was breaking the filter)
                Filter = "Image Files (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG",
                Multiselect = false
            };
            if (ofd.ShowDialog() == true)
            {
                img.Source = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));
                Image_Path.Content = ofd.FileName;
            }
        }

        // ── Genres ──────────────────────────────────────────────────────────

        private void Load_Genres()
        {
            foreach (string genre in Properties.Settings.Default.genres)
                ListView_Available_Genres.Items.Add(genre);
        }

        private void Add_Genre(object sender, RoutedEventArgs e)
        {
            if (ListView_Available_Genres.SelectedItem != null)
                ListView_Genres.Items.Add(ListView_Available_Genres.SelectedItem);
        }

        private void Delete_Genre(object sender, RoutedEventArgs e)
        {
            // Bug fix: was checking ListView_Directors.SelectedIndex
            if (ListView_Genres.SelectedIndex > -1)
                ListView_Genres.Items.RemoveAt(ListView_Genres.SelectedIndex);
        }

        // ── Directors ────────────────────────────────────────────────────────

        private void Add_Director(object sender, RoutedEventArgs e)
        {
            string g = Directors_Textbox.Text.Trim();
            if (g != "") ListView_Directors.Items.Add(g);
        }

        private void Delete_Director(object sender, RoutedEventArgs e)
        {
            if (ListView_Directors.SelectedIndex > -1)
                ListView_Directors.Items.RemoveAt(ListView_Directors.SelectedIndex);
        }

        private void Edit_Director(object sender, RoutedEventArgs e)
        {
            string g = Directors_Textbox.Text.Trim();
            if (g != "" && ListView_Directors.SelectedIndex > -1)
            {
                int idx = ListView_Directors.SelectedIndex;
                ListView_Directors.Items.Insert(idx, g);
                ListView_Directors.Items.RemoveAt(idx + 1);
            }
        }

        // ── Writers ──────────────────────────────────────────────────────────

        private void Add_Writer(object sender, RoutedEventArgs e)
        {
            string g = Writers_Textbox.Text.Trim();
            if (g != "") ListView_Writers.Items.Add(g);
        }

        private void Delete_Writer(object sender, RoutedEventArgs e)
        {
            // Bug fix: was checking ListView_Directors.SelectedIndex
            if (ListView_Writers.SelectedIndex > -1)
                ListView_Writers.Items.RemoveAt(ListView_Writers.SelectedIndex);
        }

        private void Edit_Writer(object sender, RoutedEventArgs e)
        {
            string g = Writers_Textbox.Text.Trim();
            if (g != "" && ListView_Writers.SelectedIndex > -1)
            {
                int idx = ListView_Writers.SelectedIndex;
                ListView_Writers.Items.Insert(idx, g);
                ListView_Writers.Items.RemoveAt(idx + 1);
            }
        }

        // ── Actors ───────────────────────────────────────────────────────────

        private void Add_Actor(object sender, RoutedEventArgs e)
        {
            string g = Actors_Textbox.Text.Trim();
            if (g != "") ListView_Actors.Items.Add(g);
        }

        private void Delete_Actor(object sender, RoutedEventArgs e)
        {
            // Bug fix: was checking ListView_Directors.SelectedIndex
            if (ListView_Actors.SelectedIndex > -1)
                ListView_Actors.Items.RemoveAt(ListView_Actors.SelectedIndex);
        }

        private void Edit_Actor(object sender, RoutedEventArgs e)
        {
            string g = Actors_Textbox.Text.Trim();
            if (g != "" && ListView_Actors.SelectedIndex > -1)
            {
                int idx = ListView_Actors.SelectedIndex;
                ListView_Actors.Items.Insert(idx, g);
                ListView_Actors.Items.RemoveAt(idx + 1);
            }
        }

        // ── Favorite ─────────────────────────────────────────────────────────

        private void Fav_Button_Click(object sender, RoutedEventArgs e)
        {
            Fav_or_No.Content = Fav_or_No.Content.ToString() == "Favorite"
                ? "Not Favorite"
                : "Favorite";
        }

        // ── Rating ───────────────────────────────────────────────────────────

        private void UC_OnGetRating(object sender, int rating)
        {
            Rating_UC.UCText.Text = "Ocena: " + rating;
        }

        // ── Accept / Cancel ──────────────────────────────────────────────────

        private void Accept_Button(object sender, RoutedEventArgs e)
        {
            if (Image_Path.Content.ToString() == "a" ||
                string.IsNullOrWhiteSpace(Film_Title.Text) ||
                string.IsNullOrWhiteSpace(Rating_UC.UCText.Text))
            {
                // Bug fix: was showing two MessageBoxes (standard + Xceed) for the same error
                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Please add a title, poster image, and rating before saving.",
                    "Missing Fields",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }
    }
}
