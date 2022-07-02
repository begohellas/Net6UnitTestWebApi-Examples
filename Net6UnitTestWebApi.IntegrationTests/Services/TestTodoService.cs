using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net6UnitTestWebApi.API.Data;
using Net6UnitTestWebApi.API.Services;
using Net6UnitTestWebApi.IntegrationTests.MockData;
using Xunit;
using Xunit.Abstractions;

namespace Net6UnitTestWebApi.IntegrationTests.Services;

/// <summary>
/// Test TodoService without provide shared object instances across the tests in the class
/// Create a new instance of the test class per test
/// </summary>
public class TestTodoService : IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly MyWorldDbContext _dbContext;

    public TestTodoService(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

        var dbName = $"{nameof(TestTodoService)}_{DateTime.Now.ToFileTime()}";
        var dbcontextOptions = new DbContextOptionsBuilder<MyWorldDbContext>()
            .UseInMemoryDatabase(dbName)
            .LogTo(outputHelper.WriteLine, LogLevel.Information) // view log ef core in unit test
            .Options;

        _dbContext = new MyWorldDbContext(dbcontextOptions);
        _dbContext.Database.EnsureCreated();

        // show that in every test execution, the constructor is new istance
        _outputHelper.WriteLine("{0} CTOR: {1}", nameof(TestTodoService), dbName);
    }

    [Fact]
    public async Task GetAll_ReturnTodoCollectionAsync()
    {
        _dbContext.Todos.AddRange(TodoMockData.GetTodos());
        await _dbContext.SaveChangesAsync();

        var sut = new TodoService(_dbContext);

        var result = await sut.GetAllAsync();

        result.Should().HaveCount(TodoMockData.GetTodos().Count);
    }

    [Fact]
    public async Task SaveAsync_AddNewTodoAsync()
    {
        _dbContext.Todos.AddRange(TodoMockData.GetTodos());
        await _dbContext.SaveChangesAsync();

        var (newTodo, _) = TodoMockData.AddTodo();

        var sut = new TodoService(_dbContext);

        await sut.SaveAsync(newTodo);

        int exptedRecordCount = TodoMockData.GetTodos().Count + 1;
        _dbContext.Todos.Count().Should().Be(exptedRecordCount);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();

        _outputHelper.WriteLine("{0} is disposed", nameof(MyWorldDbContext));
    }
}