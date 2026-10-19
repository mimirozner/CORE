using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "images")),
    RequestPath = "/images"
});

var books = new List<Book>
{
    new()
    {
        Id = 1,
        Title = "איסתרק",
        Author = "מיה קינן",
        Summary = "בלב לבה של אתיל, בירת הממלכה היהודית האגדית, מתרחש אסון. כוח נסתר, בעל עוצמה זדונית, מכה פעם- אחר פעם ומאיים על חייו של איסטרק יורש העצר... כעת, כאשר מופיעים בקיעים בעולמו המוגן, נדרש איסטרק להוכיח לעצמו ולעמו, כי הנו ראוי למעמד אליו נולד.",
        Heroes = new() { "איסתרק", "לאונרד", "מרדכי" },
        Quotes = new() { "לפעמים הצעד הקשה ביותר הוא פשוט להיות נאמן לעצמך." },
        ImageUrl = "/images/isterak.jpg"
    },
    new()
    {
        Id = 2,
        Title = "אל תסתר",
        Author = "ליבי קליין",
        Summary = "לייבי ואסתי מתעוררים למציאות חדשה שלא היו יכולים להעלות על דעתם... כשהקושי מאיים לטרוף את כל הקלפים, כל אחד מהם, בדרכו שלו, מוצא את ה' יתברך דווקא בתוך החושך, בתוך ההסתר.",
        Heroes = new() { "לייבי", "אסתי", "גולדי", "שאולי" },
        Quotes = new() { "מוצא את ה' יתברך דווקא בתוך החושך, בתוך ההסתר." },
        ImageUrl = "/images/altaster.jpg"
    }
};

app.MapGet("/api/books", () => books);

app.MapGet("/api/books/search", (string? query) =>
{
    if (string.IsNullOrWhiteSpace(query))
    {
        return Results.Ok(books);
    }

    var matchingBooks = books
        .Where(book =>
            book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            book.Author.Contains(query, StringComparison.OrdinalIgnoreCase))
        .ToList();

    return Results.Ok(matchingBooks);
});

app.MapPost("/api/books/{id:int}/insights", (int id, AddInsightRequest request) =>
{
    var book = books.FirstOrDefault(book => book.Id == id);
    if (book is null)
    {
        return Results.NotFound(new { message = "הספר לא נמצא." });
    }

    var insight = request.Insight?.Trim();
    if (string.IsNullOrWhiteSpace(insight))
    {
        return Results.BadRequest(new { message = "יש להזין תובנה לפני השליחה." });
    }

    book.UserInsights.Add(insight);
    return Results.Ok(book);
});

app.Run();

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Summary { get; set; } = "";
    public List<string> Heroes { get; set; } = new();
    public List<string> Quotes { get; set; } = new();
    public string ImageUrl { get; set; } = "";
    public List<string> UserInsights { get; set; } = new();
}

public class AddInsightRequest
{
    public string? Insight { get; set; }
}
