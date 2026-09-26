using Newtonsoft.Json;
using MauiAppClimaTempo.Models;
using Newtonsoft.Json.Linq;
namespace MauiAppClimaTempo.Services;


    public class DataService
{
    public static async Task<Tempo?> GetPrevisao(string cidade)
    {
        Tempo? previsao = null;

        string chave = "351e4947a32b364638aff4fc6b3cd368"; // Substitua pela sua chave de API do OpenWeatherMap"

        string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={chave}&units=metric&lang=pt_br";

        using (HttpClient client = new HttpClient())// USA O HTTPCLIENT PARA FAZER A REQUISIÇÃO HTTP
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)// VERIFICA SE A RESPOSTA FOI BEM SUCEDIDA
            {
                string json = await response.Content.ReadAsStringAsync();

                var rascunho = JObject.Parse(json);// variavel que recebe o objeto em json e converte para um objeto JObject

                DateTime time = new();
                DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                //recebe valor do nascer do sol em segundos e converte para DateTime e depois para o horário local
                DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                previsao = new()// instanciando a classe Tempo e atribuindo os valores do objeto JObject para as propriedades da classe Tempo
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


            }// fechamento do if (response.IsSuccessStatusCode)
            else
            {
                // Tratar erro de requisição
                Console.WriteLine($"Erro ao obter previsão do tempo: {response.StatusCode}");
            }

            return previsao;

        }// fechamento do using (HttpClient client = new HttpClient())

    }
}