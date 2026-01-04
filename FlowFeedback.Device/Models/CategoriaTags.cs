using System.Text.Json.Serialization;

public record CategoriaTags(
    [property: JsonPropertyName("positivas")] List<string> Positivas,
    [property: JsonPropertyName("negativas")] List<string> Negativas);