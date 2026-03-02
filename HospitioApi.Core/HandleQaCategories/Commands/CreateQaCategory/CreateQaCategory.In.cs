namespace HospitioApi.Core.HandleQaCategories.Commands.CreateQaCategory;

public class CreateQaCategoryIn
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
