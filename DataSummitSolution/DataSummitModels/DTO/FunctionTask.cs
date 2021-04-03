using DataSummitModels.DB;
using System;

namespace DataSummitModels.DTO
{
    public class FunctionTask : DB.FunctionTask
    {
        public FunctionTask()
        { ; }

        public FunctionTask(string name, DateTime previous)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            Duration = TimeStamp - previous;
        }

        public DB.FunctionTask ToModel(FunctionTask functionTask)
        {
            DB.FunctionTask t = new DB.FunctionTask()
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
