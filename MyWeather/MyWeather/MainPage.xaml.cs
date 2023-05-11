using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;

namespace MyWeather
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            getMeteo();
        }

        // c'est une méthode asynchrone : (complètement indépendante de l'application) à partir du moment ou on va appeler un serveur web; on sort de l'application donc il y'aura des temps d'attentes pour que la requette parte et le temps pour obtenir la réponse 
        async void getMeteo()
        {

            try
            {
                // création d'un client (Instensier Objet) : c'est ce qui nous permet de faire des requêtes vers un serveur (OpenWeatherMap)
                using (HttpClient client = new HttpClient()) // using : elle détruit l'objet une fois qu'on a fini de l'utiliser
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/weather"); // le client web pointra sur l'adresse de base ici 
                    string request = $"?q={cp.Text},fr&APPID=e2a9a1fbef788251baf0686f081f86a6&units=metric&lang=fr"; // c'est la requette qui s'effectuera par le client
                    string jsonString = await client.GetStringAsync(request); // On va récuperer toute la requette dans une chaine de caractère "jsonString"
                    var meteo = OpenWeatherMap.FromJson(jsonString); // 
                    temp.Text = meteo.Main.Temp.ToString() + " °C";
                    vent.Text = meteo.Wind.Speed.ToString() + " km/h";
                    hum.Text = meteo.Main.Humidity.ToString() + " %";
                    visibility.Text = meteo.Weather[0].Description;
                    lever.Text = UnixTimeStampToDateTime(meteo.Sys.Sunrise);
                    coucher.Text = UnixTimeStampToDateTime(meteo.Sys.Sunset);
                    img.Source = $"http://openweathermap.org/img/w/{meteo.Weather[0].Icon}.png";
                    ville.Text = meteo.Name;
                    result.IsVisible = true;
                    ville.IsVisible = true;
                }
            }
            catch (Exception ex)
            {

                cp.Text = "";
                result.IsVisible = false;
                ville.IsVisible = false;
            }
            
        }

        public static string UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            var heure = dtDateTime.ToShortTimeString();
            return heure;
        }
    }
}
