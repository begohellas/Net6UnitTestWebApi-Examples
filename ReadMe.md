##Example Unit Test in .NET6 with Xunit

Two types of tests case:
- test service without shared objects between tests
- test service with shared objects between tests, [IClassFixture](https://xunit.net/docs/shared-context)

For both cases, setted log to view, [ITestOutputHelper](https://xunit.net/docs/capturing-output), [Serilog-Sinks-Xunit](https://www.nuget.org/packages/Serilog.Sinks.XUnit/)

## Running Tests

To run tests, run the following command

```bash
  dotnet test
```

## API Reference

#### Get all items todo

```http
  GET /todos
```

#### Get item todo by id

```http
  GET /todos/id
```

#### Register item todo

```http
  POST /todos
```
