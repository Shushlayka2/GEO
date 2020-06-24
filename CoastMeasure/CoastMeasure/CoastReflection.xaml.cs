using System.Windows;
using System.Windows.Controls;

namespace CoastMeasure
{
    /// <summary>
    /// Логика взаимодействия для CoastReflection.xaml
    /// </summary>
    public partial class CoastReflection : Window
    {
        public CoastReflection(Canvas canvas)
        {
            InitializeComponent();
            this.Content = canvas;
            this.Show();
        }
    }
}
