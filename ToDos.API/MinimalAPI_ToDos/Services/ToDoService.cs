namespace MinimalAPI_ToDos.Services;
public interface IToDoService
{
    ToDo Create(string toDoName);

    void Delete(Guid id);

    List<ToDo> GetAll();
    List<ToDo> GetAllLong(int signsAmount);

    ToDo GetById(Guid id);

    void Update(Guid id, ReplaceToDoDto toDo);
}

public class ToDoService : IToDoService
{
    public ToDoService()
    {
        var sampleToDoList = new List<ToDo>()
        {
            new ToDo { Value = "Learn MinimalAPI" },
            new ToDo { Value = "Wash the dishes" },
            new ToDo { Value = "Plan the family trip to Norway" },
            new ToDo { Value = "Clean the kitchen" },
            new ToDo { Value = "Vacuum the apartment" },
            new ToDo { Value = "Reorganize the spice cabinet for Mary" },
            new ToDo { Value = "Send the presentation to Jeff" },
            new ToDo { Value = "Fix Dad's tablet" },
            new ToDo { Value = "Rub the dust" }
        }; 
        for (int i = 0; i < sampleToDoList.Count; i++)
        {
            _toDos[sampleToDoList[i].Id] = sampleToDoList[i];
        }
    }

    private readonly Dictionary<Guid, ToDo> _toDos = new();

    public ToDo GetById(Guid id)
    {
        return _toDos.GetValueOrDefault(id);
    }

    public List<ToDo> GetAll()
    {
        return _toDos.Values.ToList();
    }

    public List<ToDo> GetAllLong(int signsAmount)
    {
        List<ToDo> longList = new List<ToDo>();
        foreach (var task in _toDos.Values)
        {
            if (task.Value.Length > signsAmount)
            {
                longList.Add(task);
            }
        }
        return longList;
    }

    public ToDo Create(string toDoValue)
    {
        if (toDoValue is null)
        {
            return null;
        }

        var toDo = new ToDo {
            Value = toDoValue,
            IsCompleted = false
        };
        
        _toDos[toDo.Id] = toDo;

        return toDo;
    }

    public void Update(Guid id, [FromBody] ReplaceToDoDto toDo)
    {
        var existingToDo = GetById(id);
        if (existingToDo is null)
        {
            return;
        }
        if(toDo.Value != null)
        { 
        _toDos[id].Value = toDo.Value;
        }
        if(_toDos[id].IsCompleted != toDo.IsCompleted)
        { 
        _toDos[id].IsCompleted = toDo.IsCompleted;
        }
    }

    public void Delete(Guid id)
    {
        _toDos.Remove(id);
    }
}