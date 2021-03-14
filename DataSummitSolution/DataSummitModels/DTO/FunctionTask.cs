using DataSummitModels.DB;
using System;

namespace DataSummitModels.DTO
{
    public class FunctionTask : Task
    {
        public FunctionTask()
        { ; }

        public FunctionTask(string name, DateTime previous)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            Duration = TimeStamp - previous;
        }

        public Task ToModel(FunctionTask functionTask)
        {
            Task t = new Task()
            {
                Document = functionTask.Document,
                DocumentId = functionTask.DocumentId,
                Duration = functionTask.Duration,
                Name = functionTask.Name,
                TaskId = functionTask.TaskId,
                TimeStamp = functionTask.TimeStamp
            };
            return t;
        }
    }
}
