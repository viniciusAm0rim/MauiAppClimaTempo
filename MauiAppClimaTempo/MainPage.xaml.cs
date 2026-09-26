using MauiAppClimaTempo.Models;
using MauiAppClimaTempo.Services;

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
            try
            {
                if (!string.IsNullOrEmpty(cidadeEntry.Text))
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
                        DisplayAlert("Erro", "Previsão não encontrada.", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Erro", "Digite o nome de uma cidade.", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }
    }
}
