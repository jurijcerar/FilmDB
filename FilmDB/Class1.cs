using System.Collections.Generic;
using System.ComponentModel;

namespace Filmska_Baza
{
    /// <summary>
    /// Represents a film entry in the database.
    /// </summary>
    public class Film : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _title = "";
        private string _poster = "";
        private string _description = "";
        private int    _rating = 0;
        private bool   _favorite = false;

        public string title
        {
            get => _title;
            set { _title = value; Notify(nameof(title)); }
        }

        public string poster
        {
            get => _poster;
            set { _poster = value; Notify(nameof(poster)); }
        }

        public string description
        {
            get => _description;
            set { _description = value; Notify(nameof(description)); }
        }

        public int rating
        {
            get => _rating;
            set { _rating = value; Notify(nameof(rating)); }
        }

        public bool favorite
        {
            get => _favorite;
            set { _favorite = value; Notify(nameof(favorite)); }
        }

        public List<string> directors = new List<string>();
        public List<string> writers   = new List<string>();
        public List<string> actors    = new List<string>();
        public List<string> genres    = new List<string>();

        public override string ToString() => title;
    }
}
