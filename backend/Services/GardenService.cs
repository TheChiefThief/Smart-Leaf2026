using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;

namespace SmartLeaf.Services
{
    public class GardenService : IGardenService
    {
        private readonly IGardenRepository _repo;

        public GardenService(IGardenRepository repo) => _repo = repo;

        public async Task<IEnumerable<GardenResponse>> GetUserGardensAsync(string userId)
        {
            var gardens = await _repo.GetByUserIdAsync(userId);
            return gardens.Select(g => new GardenResponse
            {
                Id = g.Id,
                Name = g.Name,
                ClimateZoneId = g.ClimateZoneId,
                SoilTypeId = g.SoilTypeId,
                SunExposure = g.SunExposure
            });
        }

        public async Task<GardenResponse> CreateGardenAsync(string userId, CreateGardenRequest request)
        {
            var garden = new Garden
            {
                UserId = userId,
                Name = request.Name,
                ClimateZoneId = request.ClimateZoneId,
                SoilTypeId = request.SoilTypeId,
                SunExposure = request.SunExposure
            };
            var created = await _repo.CreateAsync(garden);
            return new GardenResponse
            {
                Id = created.Id,
                Name = created.Name,
                ClimateZoneId = created.ClimateZoneId,
                SoilTypeId = created.SoilTypeId,
                SunExposure = created.SunExposure
            };
        }

        public async Task DeleteGardenAsync(int id) => await _repo.DeleteAsync(id);
    }
}
