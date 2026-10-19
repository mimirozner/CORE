# מערכת הספרים

API פשוט ב־ASP.NET Core להצגת ספרים, חיפוש והוספת תובנות.

## הרצה

```powershell
dotnet run
```

פתחו את `http://localhost:5108/swagger` כדי לבדוק את ה־API.

## נקודות קצה

- `GET /api/books` — הצגת כל הספרים.
- `GET /api/books/search?query=...` — חיפוש לפי שם ספר או מחבר.
- `POST /api/books/{id}/insights` — הוספת תובנה; גוף הבקשה: `{"insight":"..."}`.

הנתונים נשמרים בזיכרון ומתאפסים עם הפעלה מחדש של השרת.
