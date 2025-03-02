using BookingService;
using BookingService.Grpc.Clients;
using BookingService.Grpc.Servers;
using BookingService.Helpers;
using BookingService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
GlobalSingletonProperties globalSingletonProperties = new GlobalSingletonProperties
{
    mySqlConfiguration = builder.Configuration.GetSection("MySqlConfiguration").Get<MySqlConfiguration>()?? throw new ArgumentNullException()
};
builder.Services.AddSingleton(globalSingletonProperties);
builder.Services.AddDbContext<BookingDbContext>();
builder.Services.AddScoped<IBookingService, BookingService.Services.BookingService>();
builder.Services.AddScoped(s => new FlightServiceClient("http://localhost:7071"));

builder.Services.AddGrpc();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// setting up http2 for gRPC and http1 for REST communications
builder.WebHost.ConfigureKestrel(o =>
{
    
    o.ListenAnyIP(7073, listenOptions =>{listenOptions.Protocols = HttpProtocols.Http2;}); // for gRPC communication
    o.ListenAnyIP(7072, listenOptions =>{listenOptions.Protocols = HttpProtocols.Http1;}); // for REST communication
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.MapGrpcService<BookingServiceGrpcServer>();

using (IServiceScope serviceScope = app.Services.CreateScope())
{
    BookingDbContext bookingDbContext = serviceScope.ServiceProvider.GetRequiredService<BookingDbContext>();
    bookingDbContext.Database.EnsureCreated();
}

app.Run();