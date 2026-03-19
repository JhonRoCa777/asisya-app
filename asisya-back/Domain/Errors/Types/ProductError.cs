
namespace Domain.Errors.Types
{
    public class ProductNotFoundError() : GeneralError("Producto NO Registrado") { }
    public class ProductNotActiveError() : GeneralError("Producto Inactivo") { }
}
