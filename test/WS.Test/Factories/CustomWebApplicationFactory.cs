using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Integration.Models.Dtos;
using WS.Core.Interfaces.Integration;
using WS.Infrastructure.Data;
using Moq;

namespace WS.Test.Factories;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Find the service descriptor that registers the DbContext.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<WarningSentenceContext>));

            if (descriptor != null)
            {
                // Remove registration.
                services.Remove(descriptor);
            }

            // Add DbContext using an in-memory database for testing.
            services.AddDbContext<WarningSentenceContext>(options =>
            {
                options.UseInMemoryDatabase("ShwWarningSentencesInMemory");
            });

            // Replace ProductHttpService with a mock
            var catalogueServiceDescriptor = services.FirstOrDefault(
                d => d.ServiceType == typeof(IProductHttpService));

            if (catalogueServiceDescriptor != null)
            {
                services.Remove(catalogueServiceDescriptor);
            }

            var mockProductHttpService = new Mock<IProductHttpService>();
            // Mock ProductHttpService 
            mockProductHttpService.Setup(service => service.GetInUseWarningSentences())
                .ReturnsAsync(new SharedProductWsDto
                {
                    WarningSentenceIds = new List<int> { 1, 2, 3 }
                });

            // Replace ISyncProducer with a mock
            var syncProducerDescriptor = services.FirstOrDefault(
                d => d.ServiceType == typeof(ISyncProducer));

            if (syncProducerDescriptor != null)
            {
                services.Remove(syncProducerDescriptor);
            }

            var mockSyncProducer = new Mock<ISyncProducer>();

            services.AddSingleton(mockProductHttpService.Object);
            services.AddSingleton(mockSyncProducer.Object);
        });
    }
}