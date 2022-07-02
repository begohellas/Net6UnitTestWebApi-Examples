using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net6UnitTestWebApi.API.Data;
using Net6UnitTestWebApi.IntegrationTests.MockData;
using Net6UnitTestWebApi.IntegrationTests.Services;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;
using ILogger = Serilog.ILogger;

namespace Net6UnitTestWebApi.IntegrationTests.Helper;

public class InMemoryDbContextFixture : IDisposable
{
    private readonly ILogger _output;

    private const string OutputTemplate =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}";

    public MyWorldDbContext DbContext { get; }

    public InMemoryDbContextFixture(IMessageSink diagnosticMessageSink)
    {
        _output = ConfigureSerilogProvider(diagnosticMessageSink);

        var dbName = $"{nameof(TestTodoService)}_{DateTime.Now.ToFileTime()}";
        var dbcontextOptions = new DbContextOptionsBuilder<MyWorldDbContext>()
            .UseInMemoryDatabase(dbName)
            .EnableSensitiveDataLogging()
            .LogTo(action => { _output.Information(action); },
                LogLevel.Information)
            .Options;

        DbContext = new MyWorldDbContext(dbcontextOptions);

        SeedData();

        _output.Information("{DbContext} CTOR: {DbName}", nameof(MyWorldDbContext), dbName);
    }
    
    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();

        _output.Information("{DbContext} is disposed", nameof(MyWorldDbContext));
    }

    private void SeedData()
    {
        DbContext.Database.EnsureCreated();
        DbContext.Todos.AddRange(TodoMockData.GetTodos());
        DbContext.SaveChanges();
    }

    private static ILogger ConfigureSerilogProvider(IMessageSink diagnosticMessageSink)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            // use IMessageSink because "inject" ITestOutputHelper into fixtures is not available
            // https://github.com/xunit/xunit/issues/565
            .WriteTo.TestOutput(diagnosticMessageSink, LogEventLevel.Information, outputTemplate: OutputTemplate)
            .CreateLogger()
            .ForContext<InMemoryDbContextFixture>();
    }
}