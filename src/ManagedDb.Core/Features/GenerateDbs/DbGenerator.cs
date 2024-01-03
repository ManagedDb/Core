using ManagedDb.Core.Features.SchemaConvertors;
using ManagedDb.Core.Helpers;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManagedDb.Core.Features.GenerateDbs;

public class DbGenerator
{
    public void Generate(
        string pathToDataFolder,
        string dbExportPath)
    {
        Batteries.Init();

        var connectionString = $"Data Source={dbExportPath}";
        using var conn = new SqliteConnection(connectionString);

        conn.Open();

        try 
        {
            this.GenerateTablesAndData(
                conn,
                pathToDataFolder);
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            conn.Close();
        }
    }

    private void GenerateTablesAndData(
        SqliteConnection conn,
        string pathToDataFolder) 
    {
        var entityFolders = Directory
            .GetDirectories(pathToDataFolder);

        foreach (var ef in entityFolders)
        {
            var dataCsv = Directory
                .GetFiles(ef, "*.csv")
                .FirstOrDefault();

            var dataJson = Directory
                .GetFiles(ef, "mdb.entity.schema.json")
                .FirstOrDefault();

            var schema = JsonSerializer.Deserialize<EntitySchema>(
                File.ReadAllText(dataJson),
                MdbHelper.GetJsonSerializerOptions);

            // generate table
            var createTableCommand = this.GenerateCreateTableCommand(schema);
            using var cmd = new SqliteCommand(createTableCommand, conn);
            cmd.ExecuteNonQuery();

            this.InsertData(
                conn, 
                dataCsv, 
                schema);
        }
    }

    private string GenerateCreateTableCommand(EntitySchema schema) 
    {
        var sb = new StringBuilder();

        sb.Append($"CREATE TABLE IF NOT EXISTS {schema.EntityName} (");

        foreach(var f in schema.Fields)
        {
            sb.Append($"{f.Key} {this.GetSQLiteType(f.Value.FieldType)}");

            if (f.Value.IsRequired == true)
            {
                sb.Append(" NOT NULL");
            }

            if (f.Value.IsPk == true)
            {
                sb.Append(" PRIMARY KEY");
            }

            sb.Append(",");
        }
        
        sb.Insert(sb.Length - 1, ")");
        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }

    private void InsertData(
        SqliteConnection conn,
        string pathToCsv,
        EntitySchema schema)
    {
        var lines = File.ReadAllLines(pathToCsv)
            .Skip(1);

        var sb = new StringBuilder();

        foreach (var l in lines)
        {
            var isnertCommand = this.GenerateInsertCommand(l, schema);
            using var cmd = new SqliteCommand(isnertCommand, conn);
            cmd.ExecuteNonQuery();
        }
    }

    private string GenerateInsertCommand(string rowData, EntitySchema schema)
    {
        var columnSb = new StringBuilder();
        var valsSb = new StringBuilder();

        var values = rowData.Split(',');

        for (int i = 0; i < schema.Fields.Count; i++)
        {
            var f = schema.Fields.ElementAt(i);
            var value = values[i];
            var strValue = f.Value.FieldType == "string"
                ? $"'{value}'"
                : value;

            columnSb.Append($"{f.Key},");
            valsSb.Append($"{strValue},");
        }
        columnSb.Remove(columnSb.Length - 1, 1);
        valsSb.Remove(valsSb.Length - 1, 1);

        var sb = new StringBuilder();

        sb.Append($"INSERT INTO {schema.EntityName} (");
        sb.Append(columnSb.ToString());
        sb.Append(") VALUES (");
        sb.Append(valsSb.ToString());
        sb.Append(")");

        return sb.ToString();
    }

    private string GetSQLiteType(string fieldType)
    {
        switch (fieldType.ToLower())
        {
            case "int":
                return "INTEGER";
            case "string":
                return "TEXT";
            case "bool":
                return "INTEGER";  // SQLite does not have a separate Boolean storage class. Instead, Boolean values are stored as integers 0 (false) and 1 (true).
            case "datetime":
                return "TEXT";
            default:
                throw new Exception($"Unsupported field type: {fieldType}");
        }
    }
}
