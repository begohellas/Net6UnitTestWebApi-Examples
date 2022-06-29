using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Net6UnitTestWebApi.API.Controllers;
using Net6UnitTestWebApi.API.Models;
using Net6UnitTestWebApi.API.Services;
using Net6UnitTestWebApi.Tests.MockData;
using Xunit;

namespace Net6UnitTestWebApi.Tests.Controllers;

/// <summary>
/// Test TodoController
/// </summary>
public class TestTodoController
{
    private Mock<ITodoService> MockTodoService;

    public TestTodoController()
    {
        MockTodoService = new Mock<ITodoService>();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturn200Status()
    {
        MockTodoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetTodos);
        var sut = new TodosController(MockTodoService.Object);

        var result = await sut.GetAllAsync();

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturn204Status()
    {
        MockTodoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetEmptyTodos);
        var sut = new TodosController(MockTodoService.Object);

        var result = await sut.GetAllAsync();

        result.GetType().Should().Be(typeof(NoContentResult));
        (result as NoContentResult)?.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task GetAsync_ShouldReturn200Status()
    {
        MockTodoService.Setup(_ => _.GetById(It.IsAny<int>())).ReturnsAsync(TodoMockData.GetTodo);
        var sut = new TodosController(MockTodoService.Object);
        
        var result = await sut.GetAsync(It.IsAny<int>());
        result.GetType().Should().Be(typeof(OkObjectResult));
        (result as OkObjectResult)?.Value.Should().BeOfType<Todo>();
    }
    
    [Fact]
    public async Task GetAsync_ShouldReturn404Status()
    {
        int id = 5;
        MockTodoService.Setup(_ => _.GetById(It.IsAny<int>())).ReturnsAsync(() => null);
        var sut = new TodosController(MockTodoService.Object);

        var result = await sut.GetAsync(id);
        result.GetType().Should().Be(typeof(NotFoundObjectResult));
        (result as NotFoundObjectResult)?.Value.Should().Be($"not found todo by Id {id.ToString()}");
    }

    [Fact]
    public async Task SaveAsync_ShouldCallTodoSaveAsyncOnce()
    {
        var newTodo = TodoMockData.AddTodo();
        var sut = new TodosController(MockTodoService.Object);

        var result = await sut.SaveAsync(newTodo);
        result.Should().BeOfType<CreatedResult>();
        (result as CreatedResult)?.Location.Should().Be($"/{newTodo.Id}");

        MockTodoService.Verify(_ => _.SaveAsync(newTodo), Times.Exactly(1));
    }
}