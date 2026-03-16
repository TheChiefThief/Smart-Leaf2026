using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PlantSearchController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public PlantSearchController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetPlantImage(string name)
    {
        var encodedName = System.Net.WebUtility.UrlEncode(name);
        var url = $"https://api.inaturalist.org/v1/search?q={encodedName}&sources=taxa";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error consultando la API de iNaturalist");

        var content = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(content);

        var results = json["results"];
        if (results == null || !results.HasValues)
            return NotFound("Planta no encontrada");

        var photoUrl = results[0]["record"]?["default_photo"]?["medium_url"]?.ToString();
        if (string.IsNullOrEmpty(photoUrl))
            return NotFound("No se encontró una imagen de la planta.");

        return Ok(photoUrl);
    }
}
