using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Habilita servir o index.html da pasta wwwroot automaticamente
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/sse", (CancellationToken ct) =>
{
    async IAsyncEnumerable<SseItem<string>> StreamEvents(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var count = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            count++;
            yield return new SseItem<string>(
                data: $"Mensagem #{count} - {DateTime.Now:HH:mm:ss}",
                eventType: "notificacao"
            )
            {
                EventId = count.ToString()
            };

            await Task.Delay(2000, cancellationToken);
        }
    }

    return TypedResults.ServerSentEvents(StreamEvents(ct));
});

app.Run("http://localhost:5000");