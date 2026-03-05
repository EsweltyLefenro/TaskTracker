var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    var nodeId = Environment.GetEnvironmentVariable("NODE_ID") ?? "unknown";
    var host = Environment.GetEnvironmentVariable("HOSTNAME") ?? "unknown";
    var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss 'UTC'");

    var html = $$"""
    <!doctype html>
    <html lang="ru">
    <head>
      <meta charset="utf-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1" />
      <title>Node Info</title>
      <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .box { padding: 20px; border: 1px solid #ddd; border-radius: 12px; max-width: 720px; }
        .big { font-size: 32px; font-weight: 700; margin: 0 0 10px 0; }
        .muted { color: #666; }
        code { background: #f5f5f5; padding: 2px 6px; border-radius: 6px; }
      </style>
    </head>
    <body>
      <div class="box">
        <div class="big">Нода {{nodeId}}</div>
        <div class="muted">Container hostname: <code>{{host}}</code></div>
        <div class="muted">Time: <code>{{now}}</code></div>
        <p class="muted">Обнови страницу несколько раз — Nginx будет крутить ноды по round-robin.</p>
      </div>
    </body>
    </html>
    """;

    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
