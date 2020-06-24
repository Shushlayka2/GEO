using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MapApp
{
    enum State { Developer, User, lineDrawer, shapeDrawer };

    public partial class MainWindow : Window
    {
        List<ObjectItem> Items = new List<ObjectItem>();
        List<Point> Points = new List<Point>();

        private State State = State.User;

        public MainWindow()
        {
            InitializeComponent();
            using (GEOContext db = new GEOContext())
                Items.AddRange(db.ObjectItems.Include(oi => oi.Points).ToList());
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.F2)
            {
                if (State == State.Developer)
                {
                    ValidatePage validatePage = new ValidatePage(Points, ref Items);
                    bool? result = validatePage.ShowDialog();
                    if (result == true)
                    {
                        Points.Clear();
                        State = State.User;
                    }
                }
                else
                {
                    var result = MessageBox.Show("Do you want to change state priorety?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        State = State.Developer;
                }
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.F3)
            {
                if (State == State.User)
                {
                    var result = MessageBox.Show("Do you want to change state priorety?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        lineCanvas.Visibility = Visibility.Visible;
                        mainImg.Visibility = Visibility.Collapsed;
                        State = State.lineDrawer;
                    }
                }
                else if (State == State.lineDrawer)
                {
                    lineCanvas.Visibility = Visibility.Collapsed;
                    mainImg.Visibility = Visibility.Visible;
                    State = State.User;
                }
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.F4)
            {
                if (State == State.User)
                {
                    var result = MessageBox.Show("Do you want to change state priorety?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        shapeCanvas.Visibility = Visibility.Visible;
                        mainImg.Visibility = Visibility.Collapsed;
                        State = State.shapeDrawer;
                    }
                }
                else if (State == State.shapeDrawer)
                {
                    shapeCanvas.Visibility = Visibility.Collapsed;
                    mainImg.Visibility = Visibility.Visible;
                    State = State.User;
                }
            }
            else if (e.Key == Key.Escape && State == State.shapeDrawer)
            {
                if (shapeCanvas.Children.Count != 0 && curentLine != null)
                {
                    finalLine.X2 = curentLine.X1;
                    finalLine.Y2 = curentLine.Y1;
                    shapeCanvas.Children.Remove(curentLine);
                    curentLine = null;
                    MessageBox.Show(getShapeArea().ToString());
                }
                else if (shapeCanvas.Children.Count != 0 && curentLine == null)
                {
                    shapeCanvas.Children.Clear();
                    finalLine = null;
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point clickedPoint = new Point { X = e.MouseDevice.GetPosition(null).X, Y = e.MouseDevice.GetPosition(null).Y };
            if (State == State.User)
            {
                foreach (ObjectItem item in Items)
                {
                    if (item.IsPointInside(clickedPoint))
                        MessageBox.Show(item.Name);
                }
            }
            else
            {
                Points.Add(clickedPoint);
            }
        }

        //lineCanvas Handler

        Line line = new Line { Stroke = Brushes.Black, StrokeThickness = 3 };
        bool clicked = false;
        private void lineCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lineCanvas.Children.Clear();
            line.X1 = e.MouseDevice.GetPosition(null).X;
            line.Y1 = e.MouseDevice.GetPosition(null).Y;
            clicked = true;
        }

        private void lineCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (clicked)
            {
                lineCanvas.Children.Clear();
                line.X2 = e.MouseDevice.GetPosition(null).X;
                line.Y2 = e.MouseDevice.GetPosition(null).Y;
                lineCanvas.Children.Add(line);
            }
        }

        private void lineCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clicked = false;
            MessageBox.Show(getLineLength().ToString() + " м");
        }

        private double getLineLength()
        {
            return Math.Round(Math.Sqrt(Math.Pow((line.Y2 - line.Y1), 2) + Math.Pow((line.X2 - line.X1), 2)) * 0.785);
        }

        //shapeCanvas Handler
        Line finalLine = null;
        Line curentLine = null;
        private void shapeCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (finalLine == null)
            {
                finalLine = new Line { X1 = e.MouseDevice.GetPosition(null).X, Y1 = e.MouseDevice.GetPosition(null).Y, Stroke = Brushes.Black, StrokeThickness = 3 };
                curentLine = new Line { X1 = e.MouseDevice.GetPosition(null).X, Y1 = e.MouseDevice.GetPosition(null).Y, Stroke = Brushes.Black, StrokeThickness = 3 };
                shapeCanvas.Children.Add(curentLine);
                shapeCanvas.Children.Add(finalLine);
            }
            else
            {
                curentLine.X2 = e.MouseDevice.GetPosition(null).X;
                curentLine.Y2 = e.MouseDevice.GetPosition(null).Y;
                curentLine = new Line { X1 = e.MouseDevice.GetPosition(null).X, Y1 = e.MouseDevice.GetPosition(null).Y, Stroke = Brushes.Black, StrokeThickness = 3 };
                shapeCanvas.Children.Add(curentLine);
            }
        }

        private void shapeCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (curentLine != null)
            {
                curentLine.X2 = e.MouseDevice.GetPosition(null).X;
                curentLine.Y2 = e.MouseDevice.GetPosition(null).Y;
                finalLine.X2 = e.MouseDevice.GetPosition(null).X;
                finalLine.Y2 = e.MouseDevice.GetPosition(null).Y;
            }
        }

        private double getShapeArea()
        {
            double result = 0.5 * 1.18;
            double temp = 0;
            for (int i = 0; i < shapeCanvas.Children.Count; i++)
            {
                if (i == 0)
                {
                    temp += ((Line)shapeCanvas.Children[i]).X1 * (((Line)shapeCanvas.Children[i + 1]).Y1 - ((Line)shapeCanvas.Children[shapeCanvas.Children.Count - 1]).Y1);
                }
                else if (i == shapeCanvas.Children.Count - 1)
                {
                    temp += ((Line)shapeCanvas.Children[i]).X1 * (((Line)shapeCanvas.Children[0]).Y1 - ((Line)shapeCanvas.Children[i - 1]).Y1);
                }
                else
                {
                    temp += ((Line)shapeCanvas.Children[i]).X1 * (((Line)shapeCanvas.Children[i + 1]).Y1 - ((Line)shapeCanvas.Children[i - 1]).Y1);
                }
            }
            return result * Math.Abs(temp);
        }
    }
}
