using HexagonalAdventure.Adapters.Out.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace HexagonalAdventure.Apdaters.IntegrationTests;

public class PostgresWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Testler başlamadan önce ve bittikten sonra yapmamız gereken kaynak yönetim işlemleri olacağından
    // bu sınıfa IAsyncLifetime arayüzünü de uyguladık.

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("DeppoTestDb")
        .WithUsername("postgres")
        .WithPassword("P@ssw0rd1234")
        .Build();
    public async Task InitializeAsync()
    {
        // Docker üzerinden postgresql konteynırını başlatır. Testler bu veritabanını kullanacak.
        await _container.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync(); // Testler tamamlandıktan sonra konteynırı durdurur ve kaynakları temizler.
    }

    // Burada da WebApplicationFactory'den gelen ConfigureWebHost metodunu eziyoruz(override)
    // Burada klasik olarak program sınıfındaki servislerin temizlenmesi ve container db'nin context'e eklenmesi gibi işlemler yapılıyor.
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<DeppoDbContext>)); // Program sınıfında AddDbContext'in kaydettiği Npgsql yapılandırma kaynağını kaldırır.
            services.RemoveAll(typeof(DbContextOptions<DeppoDbContext>)); // DbContext ile ilgili tüm servisleri kaldırır.
            services.RemoveAll(typeof(DbConnection)); // Varsa DbConnection ile ilgili tüm servisleri kaldırır. Örneğin veritabanı kayıtları silinir.

            services.AddDbContext<DeppoDbContext>(options =>
            {
                options.UseNpgsql(_container.GetConnectionString()); // Artık Npgsql, container'ın sağladığı db'ye bağlanacak
            });

            // Tabii şimdi bir konteynır kullanıyor olsa da gerçek veritabanına ihtiyacımız var.
            // Dolayısıyla migration prosedürünü yürütmemiz lazım ki gerekli tablolar da oluşsun.
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeppoDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }
}
