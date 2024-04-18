
using Newtonsoft.Json;

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

    public NewPage1(Cities c)
	{
		InitializeComponent();
		x = double.Parse(c.lat);
		y = double.Parse(c.lon);
		station = c.miejscowosc;
		webview.Source = new HtmlWebViewSource { Html = html };
		
    }

    private async void Button_Clicked(object sender, EventArgs e)
	{

		string json = "{\"dateFrom\":\""+ dateFrom.Date.ToString("yyyy-MM-dd") + "\",\"dateTo\":\""+ dateTo.Date.ToString("yyyy-MM-dd") + "\", \"timeFrom\":\""+timeFrom.Time.Hours.ToString()+"\", \"timeTo\":\""+timeTo.Time.Hours.ToString()+"\", \"station\":\""+station+"\"}";
		using(var client = new HttpClient())
		{
            HttpContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage result = await client.PostAsync("http://10.0.2.2:3000/getHistoricalData", httpContent);
            string content = await result.Content.ReadAsStringAsync();
			stations = JsonConvert.DeserializeObject<List<Stations>>(content);
			await DisplayAlert("oK", stations[0].stacja, "OK");
        }
    }
}