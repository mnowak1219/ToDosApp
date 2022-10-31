namespace MinimalAPI_ToDos.Requests;

public class ToDoRequest
{
    public static IResult GetAll(IToDoService service)
    {
        var toDos = service.GetAll();
        return Results.Ok(toDos);
    }
    [AllowAnonymous]
    public static IResult GetById(IToDoService service, Guid id)
    {
        var toDo = service.GetById(id);
        if (toDo == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(toDo);
    }
    public static IResult GetAllLong(IToDoService service, [FromQuery] int signsAmount)
    {
        var toDos = service.GetAllLong(signsAmount);
        return Results.Ok(toDos);
    }

    public static IResult Create(IToDoService service, [FromBody] CreateToDoDto dto)
    {
        if (string.IsNullOrEmpty(dto.Value))
        {
            return Results.BadRequest();
        }
        var toDo = service.Create(dto.Value);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    [Authorize/*(optional policies)*/]
    public static IResult Update(IToDoService service, [FromQuery] string id, ReplaceToDoDto toDo)
    {
        if (Guid.TryParse(id, out Guid guidID))
        {
            var toDoOriginal = service.GetById(guidID);
            if (toDoOriginal == null)
            {
                return Results.NotFound();
            }
            service.Update(guidID, toDo);
            return Results.Ok($"ToDo with ID: {id} updated succesfully.");
        }
        else
        {
            return Results.BadRequest(id);
        }
    }

    public static IResult Delete(IToDoService service, Guid id)
    {
        var toDo = service.GetById(id);
        if (toDo == null)
        {
            return Results.NotFound();
        }
        service.Delete(id);
        return Results.NoContent();       
    }
}
