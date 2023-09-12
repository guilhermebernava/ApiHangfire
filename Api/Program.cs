using Api.Injections;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
//add hangfire
builder.Services.AddConfiguredHangfire(builder.Configuration.GetConnectionString("hangfireDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
//uses the dashboard from hangfire
app.UseHangfireDashboard();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    //map the dashboard as an endpoint
    endpoints.MapHangfireDashboard();
});
app.Run();
