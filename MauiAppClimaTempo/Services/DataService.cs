using Newtonsoft.Json;
using MauiAppClimaTempo.Models;
using Newtonsoft.Json.Linq;
using System.Net; 

namespace MauiAppClimaTempo.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? previsao = null;

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new HttpRequestException("SEM_INTERNET");
            }

            string chave = "351e4947a32b364638aff4fc6b3cd368"; 

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={chave}&units=metric&lang=pt_br";

            using (HttpClient client = new HttpClient()) 
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    previsao = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],

                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],

                        visibility = (int)rascunho["visibility"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        speed = (double)rascunho["wind"]["speed"],

                        sunrise = sunrise.ToString("HH:mm"),
                        sunset = sunset.ToString("HH:mm")
                    };
                }
                else
                {
                    
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new Exception("CIDADE_NAO_ENCONTRADA");
                    }

                    
                    throw new Exception($"Erro na requisição: {response.StatusCode}");
                }

                return previsao;
            }
        }
    }
}