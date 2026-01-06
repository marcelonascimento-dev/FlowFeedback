using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FlowFeedback.Device.Messages;
using FlowFeedback.Device.Models;
using FlowFeedback.Device.Services;
using FlowFeedback.Domain.Entities;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace FlowFeedback.Device.ViewModels;

public partial class MainViewModel :
    ObservableObject,
    IRecipient<VotoRegistradoMessage>
{
    private readonly DatabaseService _dbService;
    private readonly HttpClient _httpClient;
    private readonly ConfigurationService _configuration;

    [ObservableProperty]
    private ObservableCollection<AlvoDto> alvos = [];

    [ObservableProperty]
    private bool estaCarregando;

    [ObservableProperty]
    private string? logoUrl;

    public MainViewModel(
        DatabaseService dbService,
        ConfigurationService configuration,
        HttpClient httpClient)
    {
        _dbService = dbService;
        _configuration = configuration;
        _httpClient = httpClient;

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    [RelayCommand]
    private async Task SelecionarAlvo(AlvoDto alvo)
    {
        if (alvo == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "Alvo", alvo }
        };

        await Shell.Current.GoToAsync(nameof(Views.VotacaoPage), navigationParameter);
    }

    // ==========================
    // CONFIGURAÇÃO INICIAL
    // ==========================
    [RelayCommand]
    private async Task CarregarConfiguracaoInicial()
    {
        if (EstaCarregando)
            return;

        if (_configuration.IsLoaded)
        {
            AplicarConfiguracao(_configuration.Configuracao!);
            return;
        }

        try
        {
            EstaCarregando = true;

            var config = await _httpClient
                .GetFromJsonAsync<ConfigResponse>(
                    $"config/dispositivo/{_configuration.DeviceId}");

            if (config is null)
                throw new Exception("Configuração inválida.");

            _configuration.SetConfig(config);

            AplicarConfiguracao(config);
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync(
                "Erro",
                "Não foi possível carregar as configurações.",
                "OK");
        }
        finally
        {
            EstaCarregando = false;
        }
    }

    // ==========================
    // APLICA CONFIGURAÇÃO
    // ==========================
    private void AplicarConfiguracao(ConfigResponse config)
    {
        Alvos.Clear();

        if (config.Cards != null)
        {
            foreach (var card in config.Cards)
                Alvos.Add(card);
        }

        LogoUrl = config.LogoUrl;

        WeakReferenceMessenger.Default.Send(
            new UpdateThemeMessage(
                config.CorPrimaria,
                config.CorSecundaria));
    }

    // ==========================
    // ABRIR VOTAÇÃO
    // ==========================
    [RelayCommand]
    private async Task AbrirVotacao(AlvoDto alvo)
    {
        var popup = new Views.VotacaoPopup();

        var popupVm = new VotacaoPopupViewModel(
            alvo,
            _dbService,
            popup);

        popup.BindingContext = popupVm;

        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }

    // ==========================
    // RECEBE EVENTO DE VOTO
    // ==========================
    public void Receive(VotoRegistradoMessage message)
    {
        _ = SincronizarDados();
    }

    // ==========================
    // SYNC DE VOTOS
    // ==========================
    private async Task SincronizarDados()
    {
        try
        {
            var votosPendentes =
                await _dbService.ObterVotosPendentesAsync();

            if (votosPendentes == null || !votosPendentes.Any())
                return;

            var request = new SyncVotosRequest(
                _configuration.TenantId,
                _configuration.DeviceId,
                votosPendentes.Select(v => new VotoItemDto(
                    v.AlvoId,
                    v.Nota,
                    v.DataHora,
                    v.TagMotivo
                )).ToList()
            );

            var response =
                await _httpClient.PostAsJsonAsync(
                    "sync/votos", request);

            if (response.IsSuccessStatusCode)
            {
                await _dbService
                    .MarcarComoSincronizadoAsync(
                        votosPendentes.Select(v => v.Id));

                System.Diagnostics.Debug.WriteLine(
                    $"[SYNC] {votosPendentes.Count} votos enviados.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[SYNC ERROR]: {ex.Message}");
        }
    }
}


public record ConfigResponse(Guid TenantId, string NomeUnidade, List<AlvoDto> Cards, string? CorPrimaria, string? CorSecundaria, string LogoUrl);
public record UpdateThemeMessage(string? Primaria, string? Secundaria);
public record AlvoDto(
    Guid Id,
    string Titulo,
    string Subtitulo,
    string? ImagemUrl,
    int Tipo)
{
    public string TipoFormatado => Tipo switch
    {
        1 => "Pessoa",
        2 => "Serviço",
        3 => "Ambiente",
        4 => "Produto",
        _ => "Geral"
    };

    public string IconeTipo => Tipo switch
    {
        1 => "person",
        2 => "settings",
        3 => "location_on",
        4 => "shopping_bag",
        _ => "info"
    };
}