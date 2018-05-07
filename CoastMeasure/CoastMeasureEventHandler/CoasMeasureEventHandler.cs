using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace CoastMeasureEventHandler
{
    public class CoasMeasureEventHandler
    {
		WaterPlace WaterPlace = new WaterPlace();
		Bitmap MainImage;
		Dictionary<Ellipse, int> ellipseLayerNums = new Dictionary<Ellipse, int>();

		public void InitWaterPlace(BitmapImage bitmapImage)
		{
			Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);
			MainImage = bitmap;
			WaterPlace.FillWaterPlace(bitmap);
		}

		public async void FillWaterAsync(double level, Window window, Window canvasWindow)
		{
			try
			{
				Bitmap result = await WaterPlace.FillWater(MainImage.Size.Height - MainImage.Size.Height / 100 * level, MainImage);
				window.Resources.Remove("bmapImg");
				window.Resources.Add("bmapImg", Bitmap2BitmapImage(result));
				DrawNewEllipse(canvasWindow, MainImage.Size.Height - MainImage.Size.Height / 100 * level, result);
			}
			catch { }
		}

		public Canvas GetCoastReflection(double width, double height)
		{
			return WaterPlace.GetCoastReflection(MainImage, width, height, ref ellipseLayerNums);
		}

		private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
		{

			using (MemoryStream outStream = new MemoryStream())
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				Bitmap bitmap = new Bitmap(outStream);
				return new Bitmap(bitmap);
			}
		}

		private void DrawNewEllipse(Window canvasWindow, double level, Bitmap bitmap)
		{
			int layerNum = Convert.ToInt32(Math.Round(level)) / Math.Abs(bitmap.Height / 8) + 1;
			if (level < 20)
				layerNum--;
			foreach (KeyValuePair<Ellipse, int> pair in ellipseLayerNums)
			{
				if (pair.Value <= layerNum)
					pair.Key.Fill = new SolidColorBrush { Color = System.Windows.Media.Color.FromRgb(255, 255, 255) };
			}
			foreach (KeyValuePair<Ellipse, int> pair in ellipseLayerNums)
			{
				if (pair.Value > layerNum)
					pair.Key.Fill = new SolidColorBrush { Color = System.Windows.Media.Color.FromRgb(0, 0, 255) };
			}
		}

		private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
		{
			BitmapImage bitmapImage = new BitmapImage();
			using (MemoryStream memory = new MemoryStream())
			{
				bitmap.Save(memory, ImageFormat.Png);
				memory.Position = 0;
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = memory;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
			}
			return bitmapImage;
		}
	}
}
