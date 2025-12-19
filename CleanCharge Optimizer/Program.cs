using CleanCharge_Optimizer.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<DownloadDataService>();
builder.Services.AddHttpClient<EnergyMixService>();
builder.Services.AddHttpClient<ChargingWindowService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost5173");
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseStaticFiles();

app.UseDefaultFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
