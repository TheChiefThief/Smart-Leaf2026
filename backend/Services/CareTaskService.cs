using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Services
{
    public class CareTaskService : ICareTaskService
    {
        private readonly ICareTaskRepository _repo;

        public CareTaskService(ICareTaskRepository repo) => _repo = repo;

        public async Task<IEnumerable<CareTaskResponse>> GetByPlantIdAsync(int plantId)
        {
            var tasks = await _repo.GetByPlantIdAsync(plantId);
            return tasks.Select(t => new CareTaskResponse
            {
                Id = t.Id,
                PlantId = t.PlantId,
                TaskType = t.TaskType,
                ScheduledDate = t.ScheduledDate,
                Status = t.Status
            });
        }

        public async Task<CareTaskResponse> CreateAsync(CreateCareTaskRequest request)
        {
            var task = new CareTask
            {
                PlantId = request.PlantId,
                TaskType = request.TaskType,
                ScheduledDate = request.ScheduledDate,
                Status = request.Status
            };
            var created = await _repo.CreateAsync(task);
            return new CareTaskResponse
            {
                Id = created.Id,
                PlantId = created.PlantId,
                TaskType = created.TaskType,
                ScheduledDate = created.ScheduledDate,
                Status = created.Status
            };
        }

        public async Task<CareTaskResponse?> GetTaskByIdAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return null;
            return new CareTaskResponse
            {
                Id = t.Id,
                PlantId = t.PlantId,
                TaskType = t.TaskType,
                ScheduledDate = t.ScheduledDate,
                Status = t.Status
            };
        }

        public async Task UpdateStatusAsync(int id, string status) =>
            await _repo.UpdateStatusAsync(id, status);

        public async Task UpdateTaskAsync(int id, UpdateCareTaskRequest request)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return;

            if (request.TaskType != null) task.TaskType = request.TaskType;
            if (request.ScheduledDate.HasValue) task.ScheduledDate = request.ScheduledDate.Value;
            if (request.Status != null) task.Status = request.Status;

            await _repo.UpdateAsync(task);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
