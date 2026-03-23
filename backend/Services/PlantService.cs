using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Services
{
    public class PlantService : IPlantService
    {
        private readonly IPlantRepository _repo;

        public PlantService(IPlantRepository repo) => _repo = repo;

        public async Task<IEnumerable<PlantResponse>> GetByGardenIdAsync(int gardenId)
        {
            var plants = await _repo.GetByGardenIdAsync(gardenId);
            return plants.Select(Map);
        }

        public async Task<PlantResponse> AddPlantAsync(CreatePlantRequest request)
        {
            var plant = new Plant
            {
                GardenId = request.GardenId,
                SpeciesId = request.SpeciesId,
                Nickname = request.Nickname,
                InitialQuantity = request.InitialQuantity,
                PlantingDate = request.PlantingDate,
                Status = request.Status,
                Notes = request.Notes
            };
            var created = await _repo.CreateAsync(plant);
            return Map(created);
        }

        public async Task<PlantResponse?> GetPlantByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return Map(p);
        }

        public async Task UpdateStatusAsync(int id, string status) =>
            await _repo.UpdateStatusAsync(id, status);

        public async Task UpdatePlantAsync(int id, UpdatePlantRequest request)
        {
            var plant = await _repo.GetByIdAsync(id);
            if (plant == null) return;

            if (request.Nickname != null) plant.Nickname = request.Nickname;
            if (request.InitialQuantity.HasValue) plant.InitialQuantity = request.InitialQuantity.Value;
            if (request.Notes != null) plant.Notes = request.Notes;

            await _repo.UpdateAsync(plant);
        }

        public async Task DeletePlantAsync(int id) => await _repo.DeleteAsync(id);

        private static PlantResponse Map(Plant p) => new()
        {
            Id = p.Id,
            GardenId = p.GardenId,
            SpeciesId = p.SpeciesId,
            Nickname = p.Nickname,
            InitialQuantity = p.InitialQuantity,
            PlantingDate = p.PlantingDate,
            Status = p.Status,
            EndDate = p.EndDate,
            Notes = p.Notes
        };
    }
}
