
namespace Domain.Errors.Types
{
    public class CategoryNotFoundError() : GeneralError("Categoría NO Registrada") { }

    public class CategoryExistsError() : GeneralError("Categoría Ya Registrada") { }

    public class CategoryNotActiveError() : GeneralError("Categoría Inactiva") { }
}
