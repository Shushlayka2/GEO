using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

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
