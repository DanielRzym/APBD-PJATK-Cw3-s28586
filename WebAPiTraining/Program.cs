var builder = WebApplication.CreateBuilder(args);

// Dodajemy obsługę kontrolerów do kontenera DI
builder.Services.AddControllers(); 

var app = builder.Build();

// Mapujemy ścieżki (routing) do naszych plików w folderze Controllers
app.MapControllers(); 

app.Run();