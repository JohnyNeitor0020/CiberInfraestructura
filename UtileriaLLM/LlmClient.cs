using System.Text;
using System.Text.Json;
namespace UtileriaLLM;

/// Utilería que envía un prompt a la API de Gemini (HTTP POST + JSON) y devuelve el texto.
public class LlmClient
{
    private static readonly HttpClient http = new() { Timeout = TimeSpan.FromSeconds(60) };

    public string UltimaPeticion { get; private set; } = "";
    public string UltimaRespuesta { get; private set; } = "";
    public int UltimoCodigo { get; private set; }

    public async Task<string> PreguntarAsync(string apiKey, string modelo, string prompt)
    {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent";

        // 1) Armar el JSON de la petición
        UltimaPeticion = JsonSerializer.Serialize(new
        {
            contents = new[] { new { parts = new[] { new { text = prompt } } } }
        });

        // 2) Enviar la petición HTTP POST (la llave va en un encabezado)
        using var req = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(UltimaPeticion, Encoding.UTF8, "application/json")
        };
        req.Headers.Add("x-goog-api-key", apiKey);

        using var res = await http.SendAsync(req);
        UltimoCodigo = (int)res.StatusCode;
        UltimaRespuesta = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new Exception($"Error HTTP {UltimoCodigo}. Revisa tu llave, el modelo o tu límite de uso.");

        // 3) Extraer solo el texto de la respuesta JSON
        using var doc = JsonDocument.Parse(UltimaRespuesta);
        return doc.RootElement.GetProperty("candidates")[0].GetProperty("content")
                  .GetProperty("parts")[0].GetProperty("text").GetString();
    }
}
