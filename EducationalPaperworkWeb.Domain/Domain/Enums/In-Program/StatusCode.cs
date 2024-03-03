using static System.Net.WebRequestMethods;

namespace EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums
{
    public enum StatusCode
    {
        OK = 200,
        Created = 201,
        NoContent = 204,

        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,

        InternalServerError = 500
    }
}
//1xx: Информационные
//2xx: Успех
//3xx: Перенаправление
//4xx: Ошибка клиента
//5xx: Ошибка сервера

//200 OK: Успешный запрос.
//201 Created: Запрос успешно создал новый ресурс.
//204 No Content: Запрос успешен, но ответа нет (обычно используется при удалении ресурса).
//400 Bad Request: Запрос некорректен или не может быть выполнен.
//401 Unauthorized: Недостаточно прав для доступа к ресурсу.
//403 Forbidden: Доступ к ресурсу запрещен.
//404 Not Found: Запрошенный ресурс не найден.
//500 Internal Server Error: Внутренняя ошибка сервера.