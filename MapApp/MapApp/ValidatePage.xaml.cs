using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MapApp
{
    /// <summary>
    /// Логика взаимодействия для ValidatePAge.xaml
    /// </summary>
    public partial class ValidatePage : Window
    {
        List<Point> Points { get; set; }
        List<ObjectItem> Items { get; set; }

        public ValidatePage(List<Point> points, ref List<ObjectItem> items)
        {
            InitializeComponent();
            Points = points;
            Items = items;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            ObjectItem item = new ObjectItem { Name = ObjectNameTB.Text };
            using (GEOContext db = new GEOContext())
            {
                Points.ForEach(p => p.ObjectItem = item);
                db.Points.AddRange(Points);
                db.ObjectItems.Add(item);
                db.SaveChanges();
                Items.Add(db.ObjectItems.Last());
            }
            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
