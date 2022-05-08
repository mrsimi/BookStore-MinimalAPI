using bookstoreapi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapGet("/", () => "Hello World");
app.MapPost("/books", async (Book bookrequest) =>
{
    var result = await BookStoreService.Add(bookrequest);
    if (result == null) return Results.Problem("An error as occured while trying to add the book");
    return Results.Ok(result);
});

app.MapGet("/books/{bookId}", async (string bookId) =>
{
    var result = await BookStoreService.GetById(bookId);
    if (result == null) return Results.NotFound("Book Not Found");
    return Results.Ok(result);
});

app.MapGet("/books", async () =>
{
    var result = await BookStoreService.GetAll();
    return Results.Ok(result);
});

app.MapDelete("/books/{bookId}", async (string bookId) =>
{
    var result = await BookStoreService.DeleteById(bookId);
    if (result == null) return Results.Problem("An error as occured while trying to add the book");
    return Results.Ok(result);
});

app.MapPut("/books/{bookId}", async ([FromRoute]string bookId, 
                        [FromBody]Book book) =>
{
    var result = await BookStoreService.Update(book, bookId);
    if (result == null) return Results.Problem("An error as occured while trying to add the book");
    return Results.Ok(result);
});

app.Run();