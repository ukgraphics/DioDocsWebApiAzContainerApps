using GrapeCity.Documents.Excel;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/diodocsexcelexport", ([FromQuery(Name = "firstname")] string? firstname, [FromQuery(Name = "lastname")] string? lastname, HttpRequest request, HttpResponse response) =>
{
    var workbook = new Workbook();
    workbook.Worksheets[0].Range["A1"].Value = $"Ç±ÇÒÇ…ÇøÇÕÅA{firstname} {lastname}ÅI";

    using var ms = new MemoryStream();
    workbook.Save(ms, SaveFileFormat.Xlsx);

    response.Headers.Add("Content-Disposition", "attachment;filename=Result.xlsx");
    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    response.Body.WriteAsync(ms.ToArray());
}).WithName("GetDioDocsExcelExport");

app.Run();