using Data.DataBase;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using System.Data;

namespace DevTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SQLitePCL.Batteries.Init();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Set up SQLite inmemory connection
            builder.Services.AddScoped<IDbConnection>(sp =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                DataBaseInitializer.Initialize(connection);

                return connection;
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
