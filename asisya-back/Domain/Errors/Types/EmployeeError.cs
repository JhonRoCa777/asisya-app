
namespace Domain.Errors.Types
{
    public class DocumentTypeNotFoundError() : GeneralError("Tipo de Documento NO Registrado") { }
    public class EmployeeNotFoundError() : GeneralError("Empleado NO Registrado") { }
    public class EmployeeNotActiveError() : GeneralError("Empleado Inactivo") { }
}
