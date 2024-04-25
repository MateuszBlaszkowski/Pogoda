using Newtonsoft.Json;

namespace MauiApp4;

public partial class Page2 : ContentPage
{
	List<Cities> list = new List<Cities>();
	public Page2()
	{
		InitializeComponent();
		ok();
        
		
	}
	public async void ok()
	{
        using (var client = new HttpClient())
        {
			HttpContent httpContent = new StringContent(""/*"{\"lat\":52, \"lon\":21}"*/, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage result = await client.PostAsync("https://srv50655.seohost.com.pl/n3/getAllStations", httpContent);
			string content = await result.Content.ReadAsStringAsync();
			list = JsonConvert.DeserializeObject<List<Cities>>(content);
            listView.BindingContext = this;
            listView.ItemsSource = list;
        }
    }

   

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {

        Cities city = list.Find(x => x.miejscowosc == (e.Item as Cities).miejscowosc);

        
        /*Cities city = new Cities()
        {
            miejscowosc = (e.Item as Cities).miejscowosc
        };*/
        NewPage1 NewPage1 = new NewPage1(city) { BindingContext = city };

        await Navigation.PushAsync(NewPage1);
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        listView.ItemsSource = list.FindAll(x=>x.miejscowosc.StartsWith(searchBar.Text, StringComparison.OrdinalIgnoreCase));
    }
}