# Dto Generator

Stop writing boilerplate code for your DTOs. This package will generate them for you.

# Introduction

Since its generator library, and it works on compile time you have to set the output type, for the dependency,

```xml
<ProjectReference Include="TenJames.DtoGenerator" Version="x.x.x" OutputItemType="Analyzer" />
```

# Example

```csharp

[GenerateDto(DtoType.All)]
class User 
{
    [MapTo(typeof(string), "src.ToString()")]
    [DtoVisibility(DtoType.AllRead | DtoType.Update)]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    [DtoIgnore]
    public string Password { get; set; }
    
}


// will generate the following DTOs

class UserReadDto
{
    [Required] public string Id { get; set; }
    [Required] public string Name { get; set; }
}

class UserReadDetailDto
{
    [Required] public string Id { get; set; }
    [Required] public string Name { get; set; }
}

class UserCreateDto
{
    [Required] public string Name { get; set; }
}

class UserUpdateDto
{
    [Required] public string Name { get; set; }
}

 

```

# Attributes

Main usage ate defined with attrubutes. The following attributes are available:

- `GenerateDtoAttribute`: Entry point for the generator. It will generate a DTO for the class where it is defined.
- `DtoIgnoreAttribute`: Ignore a property from the DTO generation.
- `MapToAttribute`: Mapping a property with function to the DTO (read and detail)
- `MapFromAttribute`: Mapping a property with function from the DTO (write)
