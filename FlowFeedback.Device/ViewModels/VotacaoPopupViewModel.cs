using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FlowFeedback.Device.Messages;
using FlowFeedback.Device.Models;
using FlowFeedback.Device.Services;
using FlowFeedback.Device.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class VotacaoPopupViewModel : ObservableObject
{
    private readonly AlvoDto _alvo;
    private readonly DatabaseService _dbService;
    private readonly CommunityToolkit.Maui.Views.Popup _popup;
    private readonly CancellationTokenSource _cts = new();
    private Dictionary<string, CategoriaTags> _mapaTags;
    private int _notaSelecionada;

    public string Titulo => _alvo.Titulo;
    public List<NpsOption> OpcoesNps { get; }

    [ObservableProperty]
    private bool _mostrarNotas = true;

    [ObservableProperty]
    private bool _mostrarTags = false;

    [ObservableProperty]
    private List<FeedbackTag> _tagsAtuais;

    public VotacaoPopupViewModel(AlvoDto alvo, DatabaseService dbService, CommunityToolkit.Maui.Views.Popup popup)
    {
        _alvo = alvo;
        _dbService = dbService;
        _popup = popup;
        OpcoesNps = GerarOpcoesNps();

        _ = CarregarTagsDoJson();
        IniciarTimerAutoFechamento();
    }

    private async Task CarregarTagsDoJson()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("tags.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            _mapaTags = JsonSerializer.Deserialize<Dictionary<string, CategoriaTags>>(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar JSON: {ex.Message}");
        }
    }

    [RelayCommand]
    private void SelecionarNota(NpsOption opcao)
    {
        _notaSelecionada = opcao.Nota;

        bool isPositivo = _notaSelecionada >= 7;
        string tipoChave = ((int)_alvo.Tipo).ToString();

        if (_mapaTags != null && _mapaTags.TryGetValue(tipoChave, out var categoria))
        {
            var nomesTags = isPositivo ? categoria.Positivas : categoria.Negativas;
            TagsAtuais = nomesTags.Select(nome => new FeedbackTag(nome, isPositivo)).ToList();
        }

        MostrarNotas = false;
        MostrarTags = true;
    }

    [RelayCommand]
    private async Task FinalizarComTag(FeedbackTag tag)
    {
        await ProcessarVoto(_notaSelecionada, tag?.Nome ?? "");
    }

    [RelayCommand]
    private async Task PularTags()
    {
        await ProcessarVoto(_notaSelecionada, "");
    }

    private async Task ProcessarVoto(int nota, string tag)
    {
        try
        {
            await _dbService.SalvarVotoAsync(new VotoLocal
            {
                AlvoId = _alvo.Id,
                Nota = nota,
                TagMotivo = tag,
                DataHora = DateTime.Now
            });

            await FecharPopup();
            WeakReferenceMessenger.Default.Send(new VotoRegistradoMessage());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao votar: {ex.Message}");
            await FecharPopup();
        }
    }

    private List<NpsOption> GerarOpcoesNps()
    {
        var lista = new List<NpsOption>();
        for (int i = 0; i <= 10; i++)
        {
            string hexColor = i switch
            {
                <= 6 => "#E74C3C",
                <= 8 => "#F1C40F",
                _ => "#27AE60"
            };
            lista.Add(new NpsOption(i, Color.FromArgb(hexColor)));
        }
        return lista;
    }

    [RelayCommand]
    private async Task Fechar() => await FecharPopup();

    private async Task FecharPopup()
    {
        _cts.Cancel();
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                if (_popup != null) await _popup.CloseAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Popup CloseAsync error: {ex.Message}");
            }
        });
    }

    private void IniciarTimerAutoFechamento()
    {
        Task.Delay(TimeSpan.FromSeconds(20), _cts.Token).ContinueWith(async t =>
        {
            if (!t.IsCanceled) await FecharPopup();
        });
    }
}