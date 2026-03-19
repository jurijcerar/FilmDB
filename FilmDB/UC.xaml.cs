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

namespace Filmska_Baza
{
    /// <summary>
    /// Interaction logic for UC.xaml
    /// </summary>
    public partial class UC : UserControl
    {

        public string Title
        {
            get
            {
                return UCTitle.Text;
            }

            set
            {
                UCTitle.Text = value;
            }
        }

        public UC()
        {
            InitializeComponent();
        }

        public delegate void GetRating(object sender, int rating);
        public event GetRating OnGetRating;

        private void Send_Data(object sender, RoutedEventArgs e)
        {
            OnGetRating?.Invoke(this, UCSelect.SelectedIndex);
            UCSelect.SelectedIndex = -1;
        }
    }
}
