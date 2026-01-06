using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FlowFeedback.Device.Messages;
using FlowFeedback.Device.Models;
using FlowFeedback.Device.Services;
using FlowFeedback.Device.Views;
using FlowFeedback.Domain.Enums;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace FlowFeedback.Device.ViewModels;

public partial class VotacaoViewModel : ObservableObject, IQueryAttributable
{
    private readonly DatabaseService _dbService;
    private Dictionary<string, CategoriaTags> _mapaTags;
    private int _notaSelecionada;

    // Propriedade que será preenchida via navegação
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Titulo))]
    [NotifyPropertyChangedFor(nameof(ImagemUrl))]
    private AlvoDto _alvo;

    // Wrappers para a View
    public string Titulo => Alvo?.Titulo;
    public string ImagemUrl => Alvo?.ImagemUrl ?? string.Empty;

    public ObservableCollection<OpcaoAvaliacao> OpcoesAvaliacao { get; } = new();

    [ObservableProperty]
    private bool _mostrarNotas = true;

    [ObservableProperty]
    private bool _mostrarTags = false;

    [ObservableProperty]
    private List<FeedbackTag> _tagsAtuais;

    public VotacaoViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
        CarregarOpcoesAvaliacao();
        _ = CarregarTagsDoJson();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Alvo", out var alvoObj) && alvoObj is AlvoDto alvo)
        {
            Alvo = alvo;
        }
    }

    private void CarregarOpcoesAvaliacao()
    {
        OpcoesAvaliacao.Clear();
        OpcoesAvaliacao.Add(new OpcaoAvaliacao { Nivel = NivelSatisfacao.MuitoInsatisfeito, Emoji = "😡", Descricao = "Péssimo", Cor = Colors.Red });
        OpcoesAvaliacao.Add(new OpcaoAvaliacao { Nivel = NivelSatisfacao.Insatisfeito, Emoji = "🙁", Descricao = "Ruim", Cor = Colors.Orange });
        OpcoesAvaliacao.Add(new OpcaoAvaliacao { Nivel = NivelSatisfacao.Neutro, Emoji = "😐", Descricao = "Regular", Cor = Color.FromArgb("#FFD700") });
        OpcoesAvaliacao.Add(new OpcaoAvaliacao { Nivel = NivelSatisfacao.Satisfeito, Emoji = "🙂", Descricao = "Bom", Cor = Colors.LightGreen });
        OpcoesAvaliacao.Add(new OpcaoAvaliacao { Nivel = NivelSatisfacao.MuitoSatisfeito, Emoji = "😍", Descricao = "Ótimo", Cor = Colors.Green });
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
            System.Diagnostics.Debug.WriteLine($"Erro tags: {ex.Message}");
        }
    }

    [RelayCommand]
    private void RegistrarVoto(OpcaoAvaliacao opcao)
    {
        if (opcao == null) return;
        _notaSelecionada = opcao.Valor;

        bool isPositivo = _notaSelecionada >= 4;

        string tipoChave = ((int)Alvo.Tipo).ToString();

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
                AlvoId = Alvo.Id,
                Nota = nota,
                TagMotivo = tag,
                DataHora = DateTime.Now
            });

            WeakReferenceMessenger.Default.Send(new VotoRegistradoMessage());

            await Shell.Current.GoToAsync(nameof(AgradecimentoPage));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao votar: {ex.Message}");
            await Voltar();
        }
    }

    [RelayCommand]
    private async Task Voltar()
    {
        await Shell.Current.GoToAsync("..");
    }
}