using DataSummitModels.DB;
using System;

namespace DataSummitModels.DTO
{
    // TODO - explain this class to me
    public class FunctionTaskDto : FunctionTask
    {
        public FunctionTaskDto()
        { ; }

        public FunctionTaskDto(string name, DateTime previous)
        {
            Name = name;
            TimeStamp = DateTime.Now;
            Duration = TimeStamp - previous;
        }

        public DB.FunctionTask ToModel(FunctionTaskDto functionTask)
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
