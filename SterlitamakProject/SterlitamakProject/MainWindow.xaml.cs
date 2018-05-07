using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SterlitamakProject
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			((Slider)sender).SelectionEnd = e.NewValue;
			PathGeometry pathGeom = (PathGeometry)PossibleArea.Data;
			PathFigure ellipseHalfFigure = pathGeom.Figures.FirstOrDefault();
			ArcSegment arcSegmentMain = (ArcSegment)ellipseHalfFigure.Segments.ElementAtOrDefault(0);
			LineSegment lineSegmentHorizontal = (LineSegment)ellipseHalfFigure.Segments.ElementAtOrDefault(1);
			pathGeom = (PathGeometry)ActualArea.Data;
			PathFigure innerEllipse = pathGeom.Figures.FirstOrDefault();
			LineSegment lineSegmentVertical = (LineSegment)innerEllipse.Segments.ElementAtOrDefault(0);
			ArcSegment arcSegmentInnerFirst = (ArcSegment)innerEllipse.Segments.ElementAtOrDefault(1);
			ArcSegment arcSegmentInnerSecond = (ArcSegment)innerEllipse.Segments.ElementAtOrDefault(2);

			if (e.NewValue > e.OldValue)
			{
				ellipseHalfFigure.StartPoint = new Point(ellipseHalfFigure.StartPoint.X, ellipseHalfFigure.StartPoint.Y - 20);
				arcSegmentMain.Point = new Point(arcSegmentMain.Point.X, arcSegmentMain.Point.Y - 20);
				arcSegmentMain.Size = new Size(arcSegmentMain.Size.Width, arcSegmentMain.Size.Height + 5);
				lineSegmentHorizontal.Point = new Point(lineSegmentHorizontal.Point.X, lineSegmentHorizontal.Point.Y - 20);
				innerEllipse.StartPoint = new Point(innerEllipse.StartPoint.X, innerEllipse.StartPoint.Y - 20);
				lineSegmentVertical.Point = new Point(lineSegmentVertical.Point.X, lineSegmentVertical.Point.Y + 17.5);
				arcSegmentInnerFirst.Point = new Point(arcSegmentInnerFirst.Point.X, arcSegmentInnerFirst.Point.Y - 20);
				arcSegmentInnerFirst.Size = new Size(arcSegmentInnerFirst.Size.Width - 1, arcSegmentInnerFirst.Size.Height);
				arcSegmentInnerSecond.Point = new Point(arcSegmentInnerSecond.Point.X, arcSegmentInnerSecond.Point.Y + 17.5);
				arcSegmentInnerSecond.Size = new Size(arcSegmentInnerSecond.Size.Width - 1, arcSegmentInnerSecond.Size.Height);
			}
			else
			{
				ellipseHalfFigure.StartPoint = new Point(ellipseHalfFigure.StartPoint.X, ellipseHalfFigure.StartPoint.Y + 20);
				arcSegmentMain.Point = new Point(arcSegmentMain.Point.X, arcSegmentMain.Point.Y + 20);
				arcSegmentMain.Size = new Size(arcSegmentMain.Size.Width, arcSegmentMain.Size.Height - 5);
				lineSegmentHorizontal.Point = new Point(lineSegmentHorizontal.Point.X, lineSegmentHorizontal.Point.Y + 20);
				innerEllipse.StartPoint = new Point(innerEllipse.StartPoint.X, innerEllipse.StartPoint.Y + 20);
				lineSegmentVertical.Point = new Point(lineSegmentVertical.Point.X, lineSegmentVertical.Point.Y - 17.5);
				arcSegmentInnerFirst.Point = new Point(arcSegmentInnerFirst.Point.X, arcSegmentInnerFirst.Point.Y + 20);
				arcSegmentInnerFirst.Size = new Size(arcSegmentInnerFirst.Size.Width + 1, arcSegmentInnerFirst.Size.Height);
				arcSegmentInnerSecond.Point = new Point(arcSegmentInnerSecond.Point.X, arcSegmentInnerSecond.Point.Y - 17.5);
				arcSegmentInnerSecond.Size = new Size(arcSegmentInnerSecond.Size.Width + 1, arcSegmentInnerSecond.Size.Height);
			}

		}
	}
}
