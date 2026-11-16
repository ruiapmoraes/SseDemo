using System.Net.ServerSentEvents;

using var httpClient = new HttpClient();
Console.WriteLine("Conectando ao servidor SSE...");

await using var stream = await httpClient.GetStreamAsync("http://localhost:5000/sse");

await foreach (var item in SseParser.Create(stream).EnumerateAsync())
{
    Console.WriteLine($"[{item.EventType} | ID: {item.EventId}] {item.Data}");
}