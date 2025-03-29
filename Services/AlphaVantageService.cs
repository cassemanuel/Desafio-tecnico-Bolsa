using System.Globalization;
using System.Text.Json;

namespace Desafio_INOA.Services
{
    public class AlphaVantageService
    {
        private readonly string _apiKey = "SUA SENHA AQUI"; //chave gerada lá no alphavantage
        private readonly HttpClient _httpClient;

        public AlphaVantageService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<decimal?> ObterCotacaoAsync(string symbol)
        {
            string function = "TIME_SERIES_DAILY";
            string outputsize = "compact"; //compact ou full, compact traz apenas 100 pontos na serie. full traz 30 dias
            string url =
                $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&outputsize={outputsize}&datatype=json&apikey={_apiKey}";
            //url modificada do exemplo da documentação para que eu possa passar meus parametros como o ativo e o intervalo

            // Console.WriteLine($"URL da API: {url}\n"); // Para depuração. desativei para execução clean do programa

            try //se mexe com requisicao coloca um bloco try-catch
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url); //requisicao assincrona
                response.EnsureSuccessStatusCode(); //garante que o http tenha sucesso na resposta
                string jsonString = await response.Content.ReadAsStringAsync();

                JsonDocument document = JsonDocument.Parse(jsonString); // trabalhar em cima do JSON que a api gera de resposta

                if (
                    document.RootElement.TryGetProperty(
                        "Time Series (Daily)",
                        out JsonElement timeSeriesElement
                    )
                )
                {
                    //dados diários são organizados por data (YYYY-MM-DD). Pegamos a data mais recente (a primeira chave no JSON)
                    string latestDate = timeSeriesElement.EnumerateObject().FirstOrDefault().Name;

                    if (
                        !string.IsNullOrEmpty(latestDate)
                        && timeSeriesElement.TryGetProperty(latestDate, out JsonElement latestData)
                    )
                    {
                        if (
                            latestData.TryGetProperty("4. close", out JsonElement closePriceElement) //4. close = preço de fechamento
                        )
                        {
                            if (
                                decimal.TryParse(
                                    closePriceElement.GetString(),
                                    NumberStyles.Float,
                                    CultureInfo.InvariantCulture,
                                    out decimal closePrice
                                )
                            )
                            {
                                return closePrice;
                            }
                        }
                    }
                }
                else if (
                    document.RootElement.TryGetProperty(
                        "Error Message",
                        out JsonElement errorMessageElement
                    )
                )
                {
                    Console.WriteLine(
                        $"Erro ao analisar a resposta da Alpha Vantage: {errorMessageElement.GetString()}"
                    );
                }

                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na requisição para a Alpha Vantage: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao deserializar JSON: {ex.Message}");
                return null;
            }
        }
    }
}
