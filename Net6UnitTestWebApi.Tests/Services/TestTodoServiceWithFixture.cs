using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Net6UnitTestWebApi.API.Services;
using Net6UnitTestWebApi.Tests.Helper;
using Net6UnitTestWebApi.Tests.MockData;
using Xunit;

namespace Net6UnitTestWebApi.Tests.Services;

/// <summary>
/// Test TodoService with provide shared object instances across the tests in the class
/// IClassFixture -> https://xunit.net/docs/shared-context
/// </summary>
public class TestTodoServiceWithFixture : IClassFixture<InMemoryDbContextFixture>
{
    private readonly InMemoryDbContextFixture _dbContextFixture;

    public TestTodoServiceWithFixture(
        InMemoryDbContextFixture dbContextFixture)
    {
        _dbContextFixture = dbContextFixture;
    }
    
    [Fact]
    public async Task GetAll_ReturnTodoCollectionAsync()
    {
        _dbContextFixture.DbContext.Todos.AddRange(TodoMockData.GetTodos());
        await _dbContextFixture.DbContext.SaveChangesAsync();

        var sut = new TodoService(_dbContextFixture.DbContext);

        var result = await sut.GetAllAsync();

        result.Should().HaveCount(TodoMockData.GetTodos().Count);
    }

    
    [Fact]
    public async Task SaveAsync_AddNewTodoAsync()
    {
        /* if disable comment for the next 2 rows you get error, this show how work IClassFixture */
        // _dbContextFixture.DbContext.Todos.AddRange(TodoMockData.GetTodos());
        // await _dbContextFixture.DbContext.SaveChangesAsync();

        var newTodo = TodoMockData.AddTodo();

        var sut = new TodoService(_dbContextFixture.DbContext);

        await sut.SaveAsync(newTodo);

        int exptedRecordCount = TodoMockData.GetTodos().Count + 1;
        _dbContextFixture.DbContext.Todos.Count().Should().Be(exptedRecordCount);
    }
}