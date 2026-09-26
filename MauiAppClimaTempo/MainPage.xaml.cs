using MauiAppClimaTempo.Models;
using MauiAppClimaTempo.Services;
using Microsoft.Extensions.FileProviders;

namespace MauiAppClimaTempo
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
          
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Sem Conexão", "Você está sem conexão com a internet. Verifique sua rede e tente novamente.", "OK");
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(cidadeEntry.Text))
                {
                    Tempo? previsao = await DataService.GetPrevisao(cidadeEntry.Text);
                    if (previsao != null)
                    {
                        string dadosPrevisao = "";

                        dadosPrevisao = $"Cidade: {cidadeEntry.Text}\n" +
                                         $"Temperatura mínima: {previsao.temp_min}°C\n" +
                                         $"Temperatura máxima: {previsao.temp_max}°C\n" +
                                         $"Visibilidade: {previsao.visibility} metros\n" +
                                         $"Nascer do sol: {previsao.sunrise}\n" +
                                         $"Pôr do sol: {previsao.sunset}\n" +
                                         $"Condição: {previsao.main}\n" +
                                         $"Descrição: {previsao.description}\n" +
                                         $"Velocidade do vento: {previsao.speed} m/s";

                        resultadoLabel.Text = dadosPrevisao;
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Previsão não encontrada.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Digite o nome de uma cidade.", "OK");
                }
            }
            catch (Exception ex)
            {
                
                if (ex.Message == "CIDADE_NAO_ENCONTRADA" || ex.InnerException?.Message == "CIDADE_NAO_ENCONTRADA")
                {
                    await DisplayAlert("Cidade Não Encontrada", "Não foi possível encontrar uma cidade com esse nome, tente novamente.", "OK");
                }
                else if (ex.Message == "SEM_INTERNET" || ex is HttpRequestException)
                {
                    await DisplayAlert("Sem Conexão", "Não foi possível conectar ao servidor. Verifique sua conexão com a internet.", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
                }
            }
        }
    }
}
