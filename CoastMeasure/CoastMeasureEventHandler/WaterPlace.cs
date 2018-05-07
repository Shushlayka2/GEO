using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CoastMeasureEventHandler
{
	class WaterPlace
	{
		List<VerticalLine> waterPlace = new List<VerticalLine>();

		internal void FillWaterPlace(Bitmap bitmap)
		{
			for (int x = 0; x < bitmap.Size.Width; x++)
			{
				VerticalLine currentLine = new VerticalLine();
				waterPlace.Add(currentLine);
				for (int y = 0; y < bitmap.Size.Height; y++)
				{
					if (bitmap.GetPixel(x, y) == Color.Black)
						break;
					else if (bitmap.GetPixel(x, y).ToArgb() == Color.White.ToArgb())
					{
						currentLine.FillLine(x, y);
					}
				}
			}
		}

		internal Task<Bitmap> FillWater(double level, Bitmap bitmap)
		{
			return Task.Run(() =>
			{
				try
				{
					foreach (VerticalLine line in waterPlace)
					{
						foreach (Pixel p in line.GetPaintingPixels(level))
						{
							if (p.Color == Color.White)
							{
								bitmap.SetPixel(p.X, p.Y, Color.Blue);
								p.Color = Color.Blue;
							}
							else if (p.Color == Color.Blue)
							{
								bitmap.SetPixel(p.X, p.Y, Color.White);
								p.Color = Color.White;
							}
						}
					}
					return bitmap;
				}
				catch
				{
					return bitmap;
				}
			});
		}

		internal Canvas GetCoastReflection(Bitmap bitmap, double width, double height, ref Dictionary<Ellipse, int> ellipseLayerNums)
		{
			Canvas result = new Canvas();
			result.Width = width;
			result.Height = height;
			int layerNum = 1;
			for (int y = 0; y < bitmap.Height; y += Math.Abs(bitmap.Height / 8))
			{
				int leftEdge = 0;
				for (int x = 0; x < bitmap.Width; x++)
				{
					if (leftEdge == 0 && bitmap.GetPixel(x, y).ToArgb() == Color.White.ToArgb())
						leftEdge = x;
					else if (leftEdge != 0 && bitmap.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
					{
						Ellipse ellipse = new Ellipse();
						ellipse.StrokeThickness = 2;
						ellipse.Stroke = System.Windows.Media.Brushes.Black;
						ellipse.Width = (width / bitmap.Width) *  (x - 1 - leftEdge);
						ellipse.Height = ellipse.Width / 3;
						ellipse.Margin = new System.Windows.Thickness((width / bitmap.Width) * leftEdge, (height - ellipse.Height) / 2, 0, 0);
						leftEdge = 0;
						result.Children.Add(ellipse);
						ellipseLayerNums.Add(ellipse, layerNum);
					}
				}
				layerNum++;
			}
			return result;
		}
	}
}
