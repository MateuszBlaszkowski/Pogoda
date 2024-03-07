namespace MauiApp4;


public partial class MainPage : ContentPage
{

    public class GraphicsDrawable : IDrawable
    {
        public Color color;
        public double point = 0;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            
            canvas.Rotate(-(int)point+90, dirtyRect.Center.X, dirtyRect.Center.Y);
            canvas.StrokeColor = color;
            canvas.StrokeSize = 6;
            canvas.DrawLine(0, 50, 70, 0);
            canvas.StrokeColor = color;
            canvas.DrawLine(0, 50, 70, 100);
            canvas.DrawLine(0, 50, 140, 50);
            
        }
    }
    GraphicsDrawable drawable = new GraphicsDrawable();
    public MainPage()
	{
		InitializeComponent();
		graphicsView.Drawable = drawable;
	}

    private async void Btn_Clicked(object sender, EventArgs e)
    {
        /* drawable.color = Colors.Red;
         drawable.point = Int16.Parse(pointEntry.Text);
         graphicsView.Invalidate();*/
        
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                DisplayAlert("OK", $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}", "OK");
        
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
        compassData.TextColor = Colors.Green;
        drawable.point = e.Reading.HeadingMagneticNorth;
        compassData.Text = $"Compass: {e.Reading.HeadingMagneticNorth}";
        graphicsView.Invalidate();
    }
}

