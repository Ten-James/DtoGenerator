namespace SourceGenerator;

using System;

/// <summary>
/// Attribute that enables the generation of DTOs.
/// </summary>
/// <param name="dtoType">Targets of Dto</param>
[AttributeUsage(AttributeTargets.Class)]
public class GenerateDtoAttribute(DtoType dtoType) : Attribute { }

/// <summary>
/// Attribute to ignore a property in the generated DTO.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DtoIgnoreAttribute : Attribute { }

/// <summary>
/// Attribute to map a property to a specified type and expression.
/// </summary>
/// <param name="type">The target type to map to.</param>
/// <param name="mapExpression">The mapping expression. having base object as src</param>
[AttributeUsage(AttributeTargets.Property)]
public class MapToAttribute(Type type, string mapExpression) : Attribute { }

/// <summary>
/// Attribute to map a property from a specified type and expression.
/// </summary>
/// <param name="type">The source type to map from.</param>
/// <param name="mapExpression">The mapping expression. having base object as src</param>
[AttributeUsage(AttributeTargets.Property)]
public class MapFromAttribute(Type type, string mapExpression) : Attribute { }

/// <summary>
/// Determines the visibility of a property in a DTO.
/// </summary>
/// <param name="dtoType"></param>
[AttributeUsage(AttributeTargets.Property)]
public class DtoVisibilityAttribute(DtoType dtoType) : Attribute { }

/// <summary>
/// Enumeration of DTO types.
/// </summary>
[Flags]
public enum DtoType
{
    /// <summary>
    /// DTO for creating an entity.
    /// </summary>
    Create = 1 << 0,

    /// <summary>
    /// DTO for reading an entity.
    /// </summary>
    Read = 1 << 1,

    /// <summary>
    /// DTO for reading detailed information of an entity.
    /// </summary>
    ReadDetail = 1 << 2,

    /// <summary>
    /// DTO for updating an entity.
    /// </summary>
    Update = 1 << 3,

    /// <summary>
    /// DTO for all read operations.
    /// </summary>
    AllRead = Read | ReadDetail,

    /// <summary>
    /// DTO for all operations.
    /// </summary>
    All = Create | Read | Update | ReadDetail
}