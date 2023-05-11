# MyWeather

1) Création d'un compte sur "https://openweathermap.org/" pour récuperer l'Api du  weather for free.
2) chercher l'offre gratuite pour s'inscrire.
3) Récuperer le code de l'api ce qui est "e2a9a1fbef788251baf0686f081f86a6" pour mon propre compte.
4) Tester l'api et faire des petites modification pour la langue "fr" et pour l'unité en metric, le code postale "75000" et le pays "fr"; 
on obtient ce lien "https://api.openweathermap.org/data/2.5/weather?q=75000,fr&APPID=e2a9a1fbef788251baf0686f081f86a6&units=metric&lang=fr"
5)  MainPage.xaml.cs : Création d'un nouveau projet sur Visual Studio 2022
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
                    var meteo = OpenWeatherMap.FromJson(jsonString);  
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

6) OpenWeatherMap.cs : On va créer un objet C# qaui va etre une recopie de l'objet JSon qu'on va récupérer par le client lors de la requette; donc on pourra interroger directements le propriétes qui composent le fichier JSon
    // on utilise ce site pour transformer un objet Json en sa classe en C# "https://app.quicktype.io"
    ou on peut utiliser un autre lien... https://jsonformatter.curiousconcept.com/
7) On doit parcourir Newtonsoft.Json pour avoir accées aux librairies...
8) On lisant le code dans "OpenWeatheMaps.cs" On a une méthode qui permet de changer un objet Json sous forme de chaine de caractère en un objet de type OpenWeatheMap ==>  
    {
    public static OpenWeatherMap FromJson(string json) => JsonConvert.DeserializeObject<OpenWeatherMap>(json, MyWeather.Converter.Settings);
    }

9) Pour transformer Unix to DateTime en C#
"https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa"

public static string UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            var heure = dtDateTime.ToShortTimeString();
            return heure;
        }

10) Pour intégrer l'image  : 

    {
        https://www.reddit.com/r/FreeCodeCamp/comments/4con5s/how_do_i_use_the_icon_given_in_the_open_weather/

        var iconUrl = "http://openweathermap.org/img/w/" + iconCode + ".png";
    }

10) Géstion d'erreur Exception : 

    * Try and Catch
    * pour que l'application fonctionne avec les liens Https : 
        On doit ajouter dans le fichier "AssemblyInfo.cs"
{
    // Activation du traffic en HTTP et HTTPS
    [assembly: Application(UsesCleartextTraffic = true)]        
}
        






