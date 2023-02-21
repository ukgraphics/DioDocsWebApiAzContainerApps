using GrapeCity.Documents.Excel;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Globalization;

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

app.MapGet("/excelexport", ([FromQuery(Name = "name")] string? name, HttpRequest request, HttpResponse response) =>
{
    //Workbook.SetLicenseKey("");

    var workbook = new Workbook();
    workbook.Worksheets[0].Range["A1"].Value = $"こんにちは、{name}！";

    using var ms = new MemoryStream();
    workbook.Save(ms, SaveFileFormat.Xlsx);

    response.Headers.Add("Content-Disposition", "attachment;filename=Result.xlsx");
    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    response.Body.WriteAsync(ms.ToArray());

}).WithName("GetExcelExport");

app.MapGet("/pdfexport", ([FromQuery(Name = "name")] string? name, HttpRequest request, HttpResponse response) =>
{
    //GcPdfDocument.SetLicenseKey("");

    var doc = new GcPdfDocument();
    var g = doc.NewPage().Graphics;

    g.DrawString($"こんにちは、{name}！",
                 new TextFormat()
                 {
                     FontName = "IPAexゴシック",
                     FontSize = 12
                 },
                 new PointF(72, 72));

    using var ms = new MemoryStream();
    doc.Save(ms, false);

    response.Headers.Add("Content-Disposition", "attachment;filename=Result.pdf");
    response.ContentType = "application/pdf";
    response.Body.WriteAsync(ms.ToArray());

}).WithName("GetPdfExport");

app.Run();