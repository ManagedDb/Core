using ManagedDb.Core.Features.PullRequests;
using ManagedDb.Core.Features.SchemaConverters;
using ManagedDb.Core.Features.SchemaConvertors;
using ManagedDb.Core.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManagedDb.Core.Units.SchemaConverters;

[TestClass]
public class SchemaConverterTests
{
    private const string schemaPath = "data\\mdb.entity.schema.json";
    private const string dataCsvPath = "data\\testtodo.csv";

    private readonly Mock<ILogger<SchemaProvider>> schemaProviderLoggerMock;
    private readonly Mock<IOptions<ManagedDbOptions>> optionsMock;
    private readonly Mock<SchemaProvider> providerMock;
    private readonly Mock<ILogger<SchemaConvertor>> loggerMock;

    private readonly SchemaConvertor schemaConvertor;

    public SchemaConverterTests()
    {
        this.schemaProviderLoggerMock = new Mock<ILogger<SchemaProvider>>();
        this.optionsMock = new Mock<IOptions<ManagedDbOptions>>();

        this.providerMock = new Mock<SchemaProvider>(
            schemaProviderLoggerMock.Object,
            optionsMock.Object);

        this.loggerMock = new Mock<ILogger<SchemaConvertor>>();

        this.schemaConvertor = new SchemaConvertor(
            this.providerMock.Object,
            this.loggerMock.Object);
    }

    [TestMethod]
    public async Task Happy_Flow() 
    {
        // setup
        var schemaContent = await File.ReadAllTextAsync(schemaPath);
        var schema = JsonSerializer.Deserialize<EntitySchema>(
            schemaContent,
            MdbHelper.GetJsonSerializerOptions);

        this.providerMock
            .Setup(s => s
                .GetSchemaAsync(It.IsAny<string>()))
            .ReturnsAsync(schema);

        var data = new List<EntityChange>();
        data.Add(new EntityChange(
            "todo",
            "data\\todos\\todos.csv",
            1, 
            EntityChangeTypeEnum.Updated,
            UpdatedFields: new Dictionary<string, string>() 
            {
                ["rank"] = "2"
            }));

        data.Add(new EntityChange(
            "todo",
            "data\\todos\\todos.csv",
            10,
            EntityChangeTypeEnum.Added,
            OriginalFields: new Dictionary<string, string>() 
            {
                ["id"] = "10",
                ["title"] = "newTodo",
                ["rank"] = "5",
                ["isDone"] = "false"
            }));

        // act
        var res = await this.schemaConvertor.ConvertBySchemaAsync(
            data.ToArray());

        // assert
        Assert.AreEqual(res.Count(), data.Count);
    }

    [TestMethod]
    public async Task IsRequiredFieldNotProvided() 
    {
        // setup
        var schemaContent = await File.ReadAllTextAsync(schemaPath);
        var schema = JsonSerializer.Deserialize<EntitySchema>(
            schemaContent,
            MdbHelper.GetJsonSerializerOptions);

        this.providerMock
            .Setup(s => s
                .GetSchemaAsync(It.IsAny<string>()))
            .ReturnsAsync(schema);

        var data = new List<EntityChange>();
        data.Add(new EntityChange(
            "todo",
            "data\\todos\\todos.csv",
            1,
            EntityChangeTypeEnum.Added,
            OriginalFields: new Dictionary<string, string>()
            {
                // we are missing rank field
                ["id"] = "10",
                ["title"] = "str",
                ["isDone"] = "true"
            }));

        // act
        Func<Task<EntityChange[]>> act = 
            async() => 
                await this.schemaConvertor.ConvertBySchemaAsync(
                    data.ToArray());

        // assert
        await Assert.ThrowsExceptionAsync<MissingRequiredFieldException>(act);
    }

    [TestMethod]
    public async Task IsFieldTypeNotCorrect()
    {
        // setup
        var schemaContent = await File.ReadAllTextAsync(schemaPath);
        var schema = JsonSerializer.Deserialize<EntitySchema>(
            schemaContent,
            MdbHelper.GetJsonSerializerOptions);

        this.providerMock
            .Setup(s => s
                .GetSchemaAsync(It.IsAny<string>()))
            .ReturnsAsync(schema);

        var data = new List<EntityChange>();
        data.Add(new EntityChange(
            "todo",
            "data\\todos\\todos.csv",
            1,
            EntityChangeTypeEnum.Updated,
            UpdatedFields: new Dictionary<string, string>()
            {
                // id should be int
                ["id"] = "not_int"
            }));

        // act
        Func<Task<EntityChange[]>> act =
            async () =>
                await this.schemaConvertor.ConvertBySchemaAsync(
                    data.ToArray());

        // assert
        await Assert.ThrowsExceptionAsync<FieldTypeIsNotCorrect>(act);
    }
}
