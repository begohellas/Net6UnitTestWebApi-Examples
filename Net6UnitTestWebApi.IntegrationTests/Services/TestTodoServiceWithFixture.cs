using System.Threading.Tasks;
using FluentAssertions;
using Net6UnitTestWebApi.API.Services;
using Net6UnitTestWebApi.IntegrationTests.Helper;
using Net6UnitTestWebApi.IntegrationTests.MockData;
using Xunit;

namespace Net6UnitTestWebApi.IntegrationTests.Services;

/// <summary>
/// Test TodoService with provide shared object instances across the tests in the class
/// IClassFixture -> https://xunit.net/docs/shared-context
/// </summary>
public class TestTodoServiceWithFixture : IClassFixture<InMemoryDbContextFixture>
{
    private readonly InMemoryDbContextFixture _dbContextFixture;
    private readonly TodoService _todoService;

    public TestTodoServiceWithFixture(
        InMemoryDbContextFixture dbContextFixture)
    {
        _dbContextFixture = dbContextFixture;

        _todoService = new TodoService(_dbContextFixture.DbContext);
    }

    [Fact]
    public async Task GetAll_ReturnTodoCollectionAsync()
    {
        var todos = await _todoService.GetAllAsync();

        todos.Should().NotBeNull();
        todos.Should().Contain(TodoMockData.GetTodos());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetById_ReturnTodoAsync(int idTodo)
    {
        var todo = await _todoService.GetById(idTodo);

        todo?.Should().NotBeNull();
        todo?.Id.Should().Be(idTodo);
    }

    [Fact]
    public async Task SaveAsync_AddNewTodoAsync()
    {
        /* if disable comment for the next 2 rows you get error, this show how work IClassFixture */
        // _dbContextFixture.DbContext.Todos.AddRange(TodoMockData.GetTodos());
        // await _dbContextFixture.DbContext.SaveChangesAsync();

        var (newTodo, idNewTodo) = TodoMockData.AddTodo();

        await _todoService.SaveAsync(newTodo);

        var todoAdded = await _todoService.GetById(idNewTodo);
 
        todoAdded?.Should().NotBeNull();
        todoAdded?.Id.Should().Be(idNewTodo);
    }
}