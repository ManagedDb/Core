using ManagedDb.Core.Features.SchemaConvertors;
using ManagedDb.Core.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Xml.Serialization;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Mvc;

namespace ManagedDb.Core.Features.DataProxyCreators
{
    public class DataProxyCreator
    {
        private const string Namespace = "ManagedDb.Core.Features.DataProxyCreators";

        public void GenerateProject(
            string pathToSchemas, 
            string pathToSave) 
        {
            var entitySchema = pathToSchemas
                .Split(",")
                .Select(x => 
                    JsonSerializer.Deserialize<EntitySchema>(
                        x, 
                        MdbHelper.GetJsonSerializerOptions));

            foreach (var entity in entitySchema) 
            {
                // save entity
                var entityScript = GenerateClass(entity);
                var pathToSaveEntity = $"{pathToSave}/{entity.EntityName}.cs";
                File.WriteAllText(pathToSaveEntity, entityScript);
                // save controller
                var controllerScript = GenerateController(entity);
                var pathToSaveController = $"{pathToSave}/{entity.EntityName}Controller.cs";
            }
        }

        private string GenerateClass(EntitySchema entitySchema)
        {
            var compileUnit = new CodeCompileUnit();
            var ns = new CodeNamespace(Namespace);
            ns.Imports.Add(new CodeNamespaceImport("System.ComponentModel.DataAnnotations"));
            compileUnit.Namespaces.Add(ns);

            var classType = new CodeTypeDeclaration(entitySchema.EntityName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public,
                BaseTypes = { new CodeTypeReference(typeof(MdbBaseEntity)) }
            };
            classType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(TableAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(entityName))));

            foreach (var field in entitySchema.Fields)
            {
                var property = new CodeMemberField
                {
                    Attributes = MemberAttributes.Public,
                    Name = field.Key,
                    Type = new CodeTypeReference(field.Value.FieldType)
                };

                var isRequired = field.Value.IsRequired == true;
                var isPk = field.Value.IsPk == true;

                if (isRequired)
                {
                    property.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(RequiredAttribute))));
                }
                if (isPk)
                {
                    property.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(KeyAttribute))));
                }
                classType.Members.Add(property);
            }

            ns.Types.Add(classType);

            var provider = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions { BracingStyle = "C" };
            using var writer = new StringWriter();
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
            
            return writer.ToString();
        }
    
        private string GenerateController(EntitySchema entitySchema)
        {
            var compileUnit = new CodeCompileUnit();
            var ns = new CodeNamespace(Namespace);
            ns.Imports.Add(new CodeNamespaceImport("System.ComponentModel.DataAnnotations"));
            compileUnit.Namespaces.Add(ns);

            var genericParentType = $"ODataBaseController<{Namespace}.{entitySchema.EntityName}>";
            var classType = new CodeTypeDeclaration($"{entitySchema.EntityName}Controller")
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public,
                BaseTypes = { new CodeTypeReference(genericParentType) }
            };

            // Create a constructor that takes ApplicationDbContext as a parameter
            var constructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
            };
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(ApplicationDbContext), 
                "context"));

            // Add a statement in this constructor to call the base constructor with ApplicationDbContext
            constructor.BaseConstructorArgs.Add(new CodeSnippetExpression("context"));

            // Add the constructor to the class
            classType.Members.Add(constructor);

            // add attribute
            classType.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(RouteAttribute)),
                    new CodeAttributeArgument(new CodePrimitiveExpression("odata/[controller]"))));

            ns.Types.Add(classType);

            var provider = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions { BracingStyle = "C" };
            using var writer = new StringWriter();
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);

            return writer.ToString();
        }
    }
}
