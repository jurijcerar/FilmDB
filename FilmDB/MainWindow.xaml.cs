using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Filmska_Baza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Film> Filmi = new ObservableCollection<Film>();

        public MainWindow()
        {
            InitializeComponent();

            // Typewriter animation for footer label
            var anim = new StringAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromSeconds(11)),
                FillBehavior = FillBehavior.HoldEnd
            };
            string word = "FilmDB";
            for (int i = 1; i <= word.Length; i++)
                anim.KeyFrames.Add(new DiscreteStringKeyFrame(word.Substring(0, i), TimeSpan.FromSeconds(i)));

            var story = new Storyboard();
            Storyboard.SetTargetName(anim, Footer.Name);
            Storyboard.SetTargetProperty(anim, new PropertyPath(System.Windows.Controls.Label.ContentProperty));
            story.Children.Add(anim);
            story.Begin(this);

            // Session timeout: shut down after 60 minutes of idle
            var timeout = new DispatcherTimer { Interval = TimeSpan.FromMinutes(60) };
            timeout.Tick += (s, e) => Application.Current.Shutdown();
            timeout.Start();

            // Seed list
            ListView1.ItemsSource = Filmi;

            if (Properties.Settings.Default.genres == null ||
                Properties.Settings.Default.genres.Count == 0)
            {
                Properties.Settings.Default.genres = new System.Collections.Specialized.StringCollection();
            }

            // Sample data
            var t1 = new Film { title = "Taste of Cherry", description = "An Iranian man drives around Tehran searching for someone who will bury him after he takes his own life.", rating = 2, poster = "Slike/1.jpg", favorite = true };
            t1.directors.Add("Abbas Kiarostami");
            t1.writers.Add("Abbas Kiarostami");
            t1.actors.Add("Homayoun Ershadi");
            t1.genres.Add("Drama");
            Filmi.Add(t1);

            var t2 = new Film { title = "Stalker", description = "A guide leads two men through a forbidden zone that supposedly grants wishes.", rating = 4, poster = "Slike/2.jpg" };
            t2.directors.Add("Andrei Tarkovsky");
            t2.writers.Add("Arkady Strugatsky");
            t2.actors.Add("Alexander Kaidanovsky");
            t2.genres.Add("Sci-Fi");
            Filmi.Add(t2);

            var t3 = new Film { title = "Vertigo", description = "A retired detective with a fear of heights is hired to follow a man's wife.", rating = 5, poster = "Slike/3.jpg", favorite = true };
            t3.directors.Add("Alfred Hitchcock");
            t3.writers.Add("Alec Coppel");
            t3.actors.Add("James Stewart");
            t3.genres.Add("Thriller");
            Filmi.Add(t3);

            // Apply search filter
            var view = CollectionViewSource.GetDefaultView(ListView1.ItemsSource) as CollectionView;
            view.Filter = CustomFilter;
        }

        // ── Search Filter ────────────────────────────────────────────────────

        private bool CustomFilter(object obj)
        {
            if (string.IsNullOrEmpty(txtData.Text))
                return true;
            return obj.ToString().IndexOf(txtData.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void txtData_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListView1.ItemsSource).Refresh();
        }

        // ── Autosave ─────────────────────────────────────────────────────────

        private void Dt_Tick(object sender, EventArgs e)
        {
            // Bug fix: ensure Saves directory exists before writing
            Directory.CreateDirectory("Saves");
            using (TextWriter tw = new StreamWriter("Saves/autosave.xml"))
            {
                var xs = new XmlSerializer(typeof(ObservableCollection<Film>));
                xs.Serialize(tw, Filmi);
            }
        }

        // ── Menu: File ───────────────────────────────────────────────────────

        private void exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "(*.XML)|*.XML", Multiselect = false };
            if (ofd.ShowDialog() == true)
            {
                using (TextReader tr = new StreamReader(ofd.FileName))
                {
                    var xs = new XmlSerializer(typeof(ObservableCollection<Film>));
                    Filmi = (ObservableCollection<Film>)xs.Deserialize(tr);
                    ListView1.ItemsSource = Filmi;
                    // Bug fix: re-apply filter to the new collection view after import
                    var view = CollectionViewSource.GetDefaultView(ListView1.ItemsSource) as CollectionView;
                    view.Filter = CustomFilter;
                }
            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog { Filter = "(*.XML)|*.XML" };
            if (sfd.ShowDialog() == true)
            {
                using (TextWriter tw = new StreamWriter(sfd.FileName))
                {
                    var xs = new XmlSerializer(typeof(ObservableCollection<Film>));
                    xs.Serialize(tw, Filmi);
                }
            }
        }

        // ── Menu: Films ──────────────────────────────────────────────────────

        private void add_film(object sender, RoutedEventArgs e)
        {
            var window = new Window2();
            window.Fav_or_No.Content = "Not Favorite";
            if (window.ShowDialog() == true)
            {
                var f = new Film
                {
                    title = window.Film_Title.Text,
                    poster = window.Image_Path.Content.ToString(),
                    favorite = window.Fav_or_No.Content.ToString() == "Favorite",
                    description = window.Desc_Text.Text
                };

                foreach (var item in window.ListView_Directors.Items) f.directors.Add(item.ToString());
                foreach (var item in window.ListView_Writers.Items)   f.writers.Add(item.ToString());
                foreach (var item in window.ListView_Actors.Items)    f.actors.Add(item.ToString());
                foreach (var item in window.ListView_Genres.Items)    f.genres.Add(item.ToString());

                string ratingText = window.Rating_UC.UCText.Text;
                if (ratingText.Length > 0 && int.TryParse(ratingText[ratingText.Length - 1].ToString(), out int r))
                    f.rating = r;

                Filmi.Add(f);
            }
        }

        private void remove_film(object sender, RoutedEventArgs e)
        {
            if (ListView1.SelectedIndex > -1)
            {
                Filmi.RemoveAt(ListView1.SelectedIndex);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("No film selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void change_film(object sender, RoutedEventArgs e)
        {
            if (ListView1.SelectedIndex < 0) return;

            int j = ListView1.SelectedIndex;
            FilmTitle.Content = Filmi[j].title;
            Poster.Source = new BitmapImage(new Uri(Filmi[j].poster, UriKind.RelativeOrAbsolute));

            FilmData.Text = "Directors: " + string.Join(", ", Filmi[j].directors)
                          + "\nWriters: "  + string.Join(", ", Filmi[j].writers)
                          + "\nActors: "   + string.Join(", ", Filmi[j].actors);

            FilmDesc.Text = Filmi[j].description;
            Rating_UC.UCSelect.SelectedIndex = -1;

            Fav_or_No.Content = Filmi[j].favorite ? "Favorite" : "Not Favorite";
            Rating_UC.UCText.Text = "Ocena: " + Filmi[j].rating;
        }

        private void Change_Film(object sender, MouseButtonEventArgs e)
        {
            // Bug fix: guard against no selection before opening edit window
            if (ListView1.SelectedIndex < 0) return;

            int j = ListView1.SelectedIndex;
            var window = new Window2();

            window.Film_Title.Text = Filmi[j].title;
            window.img.Source = new BitmapImage(new Uri(Filmi[j].poster, UriKind.RelativeOrAbsolute));
            window.Image_Path.Content = Filmi[j].poster;
            window.Title = Filmi[j].title;
            window.Fav_or_No.Content = Filmi[j].favorite ? "Favorite" : "Not Favorite";

            foreach (var d in Filmi[j].directors) window.ListView_Directors.Items.Add(d);
            foreach (var w in Filmi[j].writers)   window.ListView_Writers.Items.Add(w);
            foreach (var a in Filmi[j].actors)     window.ListView_Actors.Items.Add(a);
            foreach (var g in Filmi[j].genres)     window.ListView_Genres.Items.Add(g);

            window.Desc_Text.Text = Filmi[j].description;
            window.Rating_UC.UCText.Text = "Ocena: " + Filmi[j].rating;

            if (window.ShowDialog() == true)
            {
                Filmi[j].title       = window.Film_Title.Text;
                Filmi[j].poster      = window.Image_Path.Content.ToString();
                Filmi[j].description = window.Desc_Text.Text;
                Filmi[j].favorite    = window.Fav_or_No.Content.ToString() == "Favorite";

                Filmi[j].directors.Clear();
                Filmi[j].writers.Clear();
                Filmi[j].actors.Clear();
                Filmi[j].genres.Clear();

                foreach (var item in window.ListView_Directors.Items) Filmi[j].directors.Add(item.ToString());
                foreach (var item in window.ListView_Writers.Items)   Filmi[j].writers.Add(item.ToString());
                foreach (var item in window.ListView_Actors.Items)    Filmi[j].actors.Add(item.ToString());
                foreach (var item in window.ListView_Genres.Items)    Filmi[j].genres.Add(item.ToString());

                string ratingText = window.Rating_UC.UCText.Text;
                if (ratingText.Length > 0 && int.TryParse(ratingText[ratingText.Length - 1].ToString(), out int r))
                    Filmi[j].rating = r;
            }
        }

        // ── Menu: Tools ──────────────────────────────────────────────────────

        private void settings_window(object sender, RoutedEventArgs e)
        {
            var window = new Window1();
            if (window.ShowDialog() == true)
            {
                if (window.AS_bool.IsChecked == true &&
                    int.TryParse(window.AS_time.Text, out int minutes) && minutes > 0)
                {
                    var dt = new DispatcherTimer { Interval = TimeSpan.FromMinutes(minutes) };
                    dt.Tick += Dt_Tick;
                    dt.Start();
                }
            }
        }

        // ── Rating / Favorite ────────────────────────────────────────────────

        private void UC_OnGetRating(object sender, int rating)
        {
            int j = ListView1.SelectedIndex;
            // Bug fix: guard against nothing selected
            if (j < 0) return;
            Rating_UC.UCText.Text = "Ocena: " + rating;
            Filmi[j].rating = rating;
        }

        private void Main_Favorite(object sender, RoutedEventArgs e)
        {
            int j = ListView1.SelectedIndex;
            // Bug fix: guard against nothing selected
            if (j < 0) return;

            if (Fav_or_No.Content.ToString() == "Favorite")
            {
                Filmi[j].favorite = false;
                Fav_or_No.Content = "Not Favorite";
            }
            else
            {
                Filmi[j].favorite = true;
                Fav_or_No.Content = "Favorite";
            }
        }

        // ── Layouts ──────────────────────────────────────────────────────────

        private void Default_Layout(object sender, RoutedEventArgs e)
        {
            ListView1.SetValue(Grid.RowProperty, 1);
            ListView1.SetValue(Grid.ColumnProperty, 0);

            C.SetValue(Grid.RowProperty, 1);
            C.SetValue(Grid.ColumnProperty, 1);
            C.SetValue(Grid.ColumnSpanProperty, 2);

            E.Visibility = Visibility.Visible;

            F.SetValue(Grid.ColumnProperty, 0);
            F.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void New_Layout(object sender, RoutedEventArgs e)
        {
            ListView1.SetValue(Grid.RowProperty, 1);
            ListView1.SetValue(Grid.ColumnProperty, 4);
            ListView1.SetValue(Grid.RowSpanProperty, 4);

            C.SetValue(Grid.RowProperty, 1);
            C.SetValue(Grid.ColumnProperty, 0);
            C.SetValue(Grid.ColumnSpanProperty, 2);

            E.Visibility = Visibility.Collapsed;

            F.SetValue(Grid.ColumnProperty, 2);
            F.HorizontalAlignment = HorizontalAlignment.Left;
        }
    }
}
