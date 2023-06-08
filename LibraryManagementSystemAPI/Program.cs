using BusinessLogic.Implementation;
using BusinessLogic.Interfaces;
using Common.Constants;
using Global.AppSettings;
using IRepository;
using LibraryManagementSystemAPI.Extensions;
using MongoDB.Driver;
using POCO.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.LoadKeys(builder.Configuration);
builder = DependencyInjection(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




static WebApplicationBuilder DependencyInjection(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IStudentService, StudentService>();
    switch (AppSettingKeys.ConnType)
    {
        case DBConstants.OracleOption:
            // Oracle option selected
            builder.Services.AddScoped<Oracle.DBContext.IDatabaseContext>(provider => new Oracle.DBContext.OracleDatabaseContext(AppSettingKeys.OracleDBConnection));
            builder.Services.AddScoped<IStudentRepository, Oracle.StudentRepository>();
            break;
        case DBConstants.SqlServerOption:
            // SQL Server option selected
            // Register the SqlConnectionContext with the connection string
            builder.Services.AddScoped<SQLServer.DBContext.IDatabaseContext>(provider => new SQLServer.DBContext.SqlConnectionContext(AppSettingKeys.SQLServerConnection));
            builder.Services.AddScoped<IStudentRepository, SQLServer.StudentRepository>();
    
            break;
        case DBConstants.PosgresOption:
            // PosGres option selected
            builder.Services.AddScoped<IStudentRepository, Posgres.StudentRepository>();
            break;
        case DBConstants.MongoOption:
            // MongoDB option selected
            // Register the MongoDB client and database
            var mongoClient = new MongoClient(AppSettingKeys.MongoDBConnection);
            var mongoDatabase = mongoClient.GetDatabase(AppSettingKeys.MongoDBDatabase);
            // Register the StudentRepository class
            builder.Services.AddSingleton<IMongoCollection<Student>>(mongoDatabase.GetCollection<Student>(AppSettingKeys.MongoDBCollection));
            builder.Services.AddSingleton<IStudentRepository, Mongo.StudentRepository>();
            break;
    }

    return builder;
}
