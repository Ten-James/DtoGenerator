#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SourceGenerator;

internal static class ObjectSpecificationParser
{
    internal static ObjectSpecification Parse(INamedTypeSymbol symbol)
    {
        var dtoType = GetDtoType(symbol);
        var properties = symbol.GetAllProperties()
            .OfType<IPropertySymbol>()
            .Where(
                property => !property.GetAttributes().Any(a => a.AttributeClass?.Name == "DtoIgnoreAttribute")
            )
            .Select(property => new PropertySpecification
            {
                Name = property.Name,
                Summary = property.GetDocumentationCommentXmlStyled(),
                Type = property.Type.ToString(),
                IsRequired = property.GetAttributes().Any(a => a.AttributeClass?.Name == "RequiredAttribute"),
                IsNullable = property.NullableAnnotation == NullableAnnotation.Annotated,
                Visibility = GetVisibility(property),
                MapTo = GetMapping(property, "MapToAttribute"),
                MapFrom = GetMapping(property, "MapFromAttribute")
            })
            .ToList();

        return new ObjectSpecification
        {
            DtoType = dtoType,
            Name = symbol.Name,
            Properties = properties
        };
    }

    private static string GetDocumentationCommentXmlStyled(this ISymbol symbol)
    {
        var docComment = symbol.GetDocumentationCommentXml();
        if (string.IsNullOrWhiteSpace(docComment))
        {
            return string.Empty;
        }
        var lines = docComment!.Split('\n');
        var result = new  StringBuilder();
        //remove first and last
        for (var i = 1; i < lines.Length - 2; i++)
        {
            result.AppendLine("///"+lines[i].Trim());
        }
        return result.ToString().Trim();
    }

    private static DtoType GetDtoType(INamedTypeSymbol symbol)
    {
        return (DtoType)symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "GenerateDtoAttribute")
            .ConstructorArguments[0].Value!;
    }

    private static MappingSpecification? GetMapping(IPropertySymbol property, string name)
    {
        var attribute = property.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == name);
        if (attribute == null)
        {
            return null;
        }

        var type = attribute.ConstructorArguments[0].Value!.ToString();
        var function = attribute.ConstructorArguments[1].Value!.ToString();
        return new MappingSpecification
        {
            ToType = type,
            Function = function
        };
    }

    private static DtoType GetVisibility(IPropertySymbol property)
    {
        var attribute = property.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DtoVisibilityAttribute");
        if (attribute != null)
        {
            return (DtoType)attribute.ConstructorArguments[0].Value!;
        }
        return DtoType.All;
    }

    private static IEnumerable<IPropertySymbol> GetAllProperties(this INamedTypeSymbol? symbol)
    {
        while (symbol != null)
        {
            foreach (var member in symbol.GetMembers().OfType<IPropertySymbol>())
            {
                yield return member;
            }

            symbol = symbol.BaseType;
        }
    }
}