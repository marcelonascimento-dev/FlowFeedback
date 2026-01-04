using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowFeedback.Device.Models;
using FlowFeedback.Device.Services;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace FlowFeedback.Device.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly HttpClient _httpClient;

    [ObservableProperty]
    private ObservableCollection<AlvoDto> alvos = new();

    [ObservableProperty]
    private bool estaCarregando;

    public MainViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7274/api/") };
    }

    [RelayCommand]
    private async Task CarregarConfiguracaoInicial()
    {
        if (EstaCarregando) return;

        try
        {
            EstaCarregando = true;

            // Substituir pelo ID real do dispositivo configurado no seu banco
            var deviceId = Guid.Parse("16053cfe-e2fa-47b6-b2b5-66f51efd319f");

            var config = await _httpClient.GetFromJsonAsync<ConfigResponse>($"config/dispositivo/{deviceId}");

            if (config?.Cards != null)
            {
                Alvos.Clear();
                foreach (var card in config.Cards)
                {
                    Alvos.Add(card);
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", "Não foi possível carregar as configurações.", "OK");
        }
        finally
        {
            EstaCarregando = false;
        }
    }

    [RelayCommand]
    private async Task AbrirVotacao(AlvoDto alvo)
    {
        // Lógica para abrir o popup de notas que faremos a seguir
    }
}

// DTOs temporários para bater com a API (Pode mover para ficheiros separados)
public record ConfigResponse(Guid TenantId, string NomeUnidade, List<AlvoDto> Cards);
public record AlvoDto(Guid Id, string Titulo, string Subtitulo, string ImagemUrl);