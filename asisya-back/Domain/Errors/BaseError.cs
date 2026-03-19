using System.Net;

namespace Domain.Errors
{
    public class BaseError(HttpStatusCode Code, string Message)
    {
        public HttpStatusCode Code { get; } = Code;
        public string Message { get; } = Message;
    }

    /* ERRORES DE FORMS STRING -> JSON */
    public class UnprocessableEntityError(string _Message)
        : BaseError(HttpStatusCode.UnprocessableEntity, _Message)
    { }

    /* SE MUESTRAN AL USUARIO, SIN ACCIÓN POSTERIOR */
    public class GeneralError(string _Message)
        : BaseError(HttpStatusCode.BadRequest, _Message)
    { }

    /* SE MUESTRAN Y REDIRIGEN AL HOME */
    public class UnauthorizedError(string _Message = "Sin Permisos")
        : BaseError(HttpStatusCode.Unauthorized, _Message)
    { }

    /* SE MUESTRAN Y REDIRIGEN AL LOGIN */
    public class ForbiddenError(string _Message = "Debe Realizar Login")
        : BaseError(HttpStatusCode.Forbidden, _Message)
    { }

    /* SE DEJA PASAR PARA QUE EL COMPONENTE LO MANEJE */
    public class ConcurrentError(string _Message)
        : BaseError(HttpStatusCode.Conflict, _Message)
    { }

    /* ERRORES NO CONTROLADOS (SOLO SE MUESTRA) */
    public class InternalServerError(string _Message = "Error del servidor, por favor comuníquese con el área TIC.")
        : BaseError(HttpStatusCode.Conflict, _Message)
    { }
}
