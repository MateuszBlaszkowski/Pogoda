
using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using System.Windows.Markup;

namespace MauiApp4;

public partial class NewPage1 : ContentPage
{
	public string test = "";
	public string station;
    static double x = 51.196216;
    static double y = 21.126904;
	List<Stations> stations = new List<Stations>();
    string html
    {
        get
        {
            return @"
	            <!DOCTYPE html>
	            <html>
	            <head>
		            <base target='_top'>
		            <meta charset='utf-8'>
		            <meta name='viewport' content='width=device-width, initial-scale=1'>
		            <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' integrity='sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY=' crossorigin=''/>
		            <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js' integrity='sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo=' crossorigin=''></script>
		            <style>
			            body{
				            margin: 0;
			            }
		            </style>
	            </head>
	            <body>
	            <div id='map' style='width: 100%; height: 300px;'></div>
	            <script>
		            const map = L.map('map').setView([" + x + @", " + y + @"], 14);
		            var marker = L.marker([" + x + @", " + y + @"]).addTo(map);
		            const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
			            maxZoom: 19,
			            attribution: `&copy; <a href='http://www.openstreetmap.org/copyright'>OpenStreetMap</a>`
		            }).addTo(map);
	            </script>
	            </body>
	            </html>
	            ";
        }
    }
	public class GraphicsDrawable : IDrawable
	{
        public int[] values = { 5,2,5,4,3,10,20};
		public int count = 20;
		
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
			double stepX = (double)300/(values.Length-1);
			double stepY = (double)180 / (values.Max() - values.Min());
			double step = (Math.Ceiling((double)values.Max()/10)*10)/count;
            canvas.StrokeSize = 2;
            canvas.FillColor = Colors.DodgerBlue;
			canvas.StrokeLineCap = LineCap.Round;
            PathF path = new PathF();
			for(int i=0; i<count+1; i++)
			{
                canvas.StrokeColor = Colors.Gainsboro;
                canvas.DrawString(Math.Round(i * step).ToString(), dirtyRect.Left,dirtyRect.Bottom- (float)(i*(200/count)), HorizontalAlignment.Center);
				canvas.DrawLine(dirtyRect.Left + 10, dirtyRect.Bottom - (float)(i * (200/count)), dirtyRect.Right, dirtyRect.Bottom - (float)(i * (200/count)) );
			}
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawLine(dirtyRect.Left + 10, dirtyRect.Top, dirtyRect.Left + 10, dirtyRect.Bottom + 10);
            canvas.DrawLine(dirtyRect.Left + 10, dirtyRect.Bottom + 10, dirtyRect.Right, dirtyRect.Bottom + 10);
            canvas.StrokeColor = Colors.DodgerBlue;
            for (int i=0; i<values.Length; i++)
			{
				//canvas.DrawString(values[i].ToString(), 10 + (float)(i * stepX), dirtyRect.Bottom, HorizontalAlignment.Center);
				if (i == 0)
				{
					path.MoveTo(10 + (float)(i * stepX), (dirtyRect.Bottom - 20) - (float)(values[i] * stepY) + (float)(values.Min() * stepY));
					canvas.FillCircle(10+(float)(i * stepX), (dirtyRect.Bottom - 20) - (float)(values[i] * stepY) + (float)(values.Min() * stepY), 4);
				}
				else
				{
					path.LineTo(10+(float)(i * stepX), (dirtyRect.Bottom - 20) - (float)(values[i] * stepY) + (float)(values.Min() * stepY));
                    canvas.FillCircle(10+(float)(i * stepX), (dirtyRect.Bottom - 20) - (float)(values[i] * stepY) + (float)(values.Min() * stepY), 4);
                }
            }
            canvas.DrawPath(path);
        }
    }
	GraphicsDrawable drawable = new GraphicsDrawable();
    public NewPage1(Cities c)
	{
		InitializeComponent();
		x = 52;//double.Parse(c.lat);
		y = 21;//double.Parse(c.lon);
		station = c.miejscowosc;
		webview.Source = new HtmlWebViewSource { Html = html };
		gView.Drawable = drawable;
        

}

    private async void Button_Clicked(object sender, EventArgs e)
	{
        

        /*string json = "{\"dateFrom\":\""+ dateFrom.Date.ToString("yyyy-MM-dd") + "\",\"dateTo\":\""+ dateTo.Date.ToString("yyyy-MM-dd") + "\", \"timeFrom\":\""+timeFrom.Time.Hours.ToString()+"\", \"timeTo\":\""+timeTo.Time.Hours.ToString()+"\", \"station\":\""+station+"\"}";
		using(var client = new HttpClient())
		{
            HttpContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage result = await client.PostAsync("https://srv50655.seohost.com.pl/n3/getHistoricalData", httpContent);
            string content = await result.Content.ReadAsStringAsync();
			stations = JsonConvert.DeserializeObject<List<Stations>>(content);
			if (double.Parse(stations[0].cisnienie) > double.Parse(stations[stations.Count-1].cisnienie))
			{
                arrow.Source = "arrow2.png";
                arrow.IsVisible = true;
			}
            else
            {
				arrow.Source = "arrow3.png";
				arrow.IsVisible = true;
            }
        }*/
    }


    private void slider_ValueChanged_1(object sender, ValueChangedEventArgs e)
    {
        drawable.count = (int)Math.Round(slider.Value)*2;
        gView.Invalidate();
    }
}