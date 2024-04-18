using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Graphics;
using Android.Text;
using Microsoft.Maui.Devices.Sensors;
using GoogleGson;
using Newtonsoft.Json;


namespace MauiApp4;


public partial class MainPage : ContentPage
{
    
    public class GraphicsDrawable : IDrawable
    {
        public Color color;
        public float point = 0;
        public float p2 = 0;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Color.FromHex("#97acad");
            canvas.StrokeSize = 5;
            canvas.DrawCircle(dirtyRect.Center.X,dirtyRect.Center.Y, 70);
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 3;
            canvas.FontSize = 26;
            canvas.FontColor = Colors.Red;
            canvas.DrawString("N", dirtyRect.Center.X, dirtyRect.Top+35, HorizontalAlignment.Center);
            canvas.FontColor = Colors.Black;
            canvas.DrawString("E", dirtyRect.Right-30, dirtyRect.Center.Y+10, HorizontalAlignment.Center);
            canvas.DrawString("S", dirtyRect.Center.X, dirtyRect.Bottom -15, HorizontalAlignment.Center);
            canvas.DrawString("W", dirtyRect.Left+25, dirtyRect.Center.Y+10, HorizontalAlignment.Center);
            canvas.DrawLine(dirtyRect.Center.X, dirtyRect.Bottom-40, dirtyRect.Center.X, dirtyRect.Bottom - 100);
            canvas.DrawLine(dirtyRect.Left+40, dirtyRect.Center.Y, dirtyRect.Right -40, dirtyRect.Center.Y);
            Microsoft.Maui.Graphics.IImage arrow;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("MauiApp4.Resources.Images.arrow.png"))
            {
                arrow = PlatformImage.FromStream(stream);
            }
            if (arrow != null)
            {
                canvas.Rotate(p2, dirtyRect.Center.X, dirtyRect.Center.Y);
                canvas.DrawImage(arrow, dirtyRect.Left + 62, dirtyRect.Top + 32, arrow.Width, arrow.Height);
            }
            
            Microsoft.Maui.Graphics.IImage image;
            using (Stream stream = assembly.GetManifestResourceStream("MauiApp4.Resources.Images.kompas.png"))
            {
                image = PlatformImage.FromStream(stream);
            }
            if(image != null)
            {
                canvas.Rotate(point-p2, dirtyRect.Center.X, dirtyRect.Center.Y);
                canvas.DrawImage(image, dirtyRect.Left+62, dirtyRect.Top+32, image.Width, image.Height);
            }
        }
    }
    GraphicsDrawable drawable = new GraphicsDrawable();
    public MainPage()
	{
		InitializeComponent();
        getNearestStation();
        
		graphicsView.Drawable = drawable;
	}
    private async void getNearestStation()
    {
        GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(3));
        Location location = await Geolocation.Default.GetLocationAsync(request, new CancellationTokenSource().Token);
        if (location != null)
        {
            using (var client = new HttpClient())
            {
                HttpContent httpContent = new StringContent("{\"lat\":\""+location.Latitude+"\", \"lon\":\""+location.Longitude+"\"}", System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync("http://10.0.2.2:3000/getNearestStation", httpContent);
                string content = await result.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<List<Stations>>(content);
                nearestStation.Text = json[0].stacja;
                drawable.p2 = float.Parse(json[0].kierunek_wiatru);
                graphicsView.Invalidate();
                stationData.Text = $"Kierunek wiatru: {json[0].kierunek_wiatru}\nTemperatura: {json[0].temperatura}\nPrędkośc wiatru: {json[0].predkosc_wiatru}\nWilgotność względna: {json[0].wilgotnosc_wzgledna}\nSuma opadu: {json[0].suma_opadu}\nCiśnienie: {json[0].cisnienie}";
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring)
            {
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.UI);
            }
            else
            {
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
            }
        }
    }

    private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        drawable.point = (float)e.Reading.HeadingMagneticNorth;
        graphicsView.Invalidate();
    }
}

