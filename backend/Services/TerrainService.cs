using SmartLeaf.Application.DTOs;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Domain;
using System.Text.Json;

namespace SmartLeaf.Services
{
    public class TerrainService : ITerrainService
    {
        private readonly ITerrainRepository _terrainRepo;
        private readonly ISproutRepository  _sproutRepo;

        public TerrainService(ITerrainRepository terrainRepo, ISproutRepository sproutRepo)
        {
            _terrainRepo = terrainRepo;
            _sproutRepo  = sproutRepo;
        }

        public async Task<TerrainResponse?> GetByGardenIdAsync(int gardenId)
        {
            var terrain = await _terrainRepo.GetByGardenIdAsync(gardenId);
            if (terrain == null) return null;
            return await MapWithSprouts(terrain);
        }

        public async Task<TerrainResponse> CreateAsync(CreateTerrainRequest request)
        {
            var terrain = new Terrain
            {
                GardenId  = request.GardenId,
                ShapeType = request.ShapeType,
                Dimensions = request.Dimensions
            };
            var created = await _terrainRepo.CreateAsync(terrain);
            return await MapWithSprouts(created);
        }

        public async Task UpdateAsync(int id, UpdateTerrainRequest request)
        {
            var terrain = await _terrainRepo.GetByIdAsync(id);
            if (terrain == null) return;

            if (request.ShapeType != null)  terrain.ShapeType  = request.ShapeType;
            if (request.Dimensions != null) terrain.Dimensions = request.Dimensions;

            await _terrainRepo.UpdateAsync(terrain);
        }

        public async Task DeleteAsync(int id) => await _terrainRepo.DeleteAsync(id);

        // ── Sprouts ──────────────────────────────────────────────────────────

        public async Task<SproutResponse> AddSproutAsync(CreateSproutRequest request)
        {
            var sprout = new Sprout
            {
                TerrainId = request.TerrainId,
                PlantId   = request.PlantId,
                Label     = request.Label,
                Color     = request.Color,
                Form      = request.Form,
                X         = request.X,
                Y         = request.Y,
                Z         = request.Z
            };
            var created = await _sproutRepo.CreateAsync(sprout);
            return MapSprout(created);
        }

        public async Task UpdateSproutAsync(int sproutId, UpdateSproutRequest request)
        {
            var sprout = await _sproutRepo.GetByIdAsync(sproutId);
            if (sprout == null) return;

            if (request.Label != null) sprout.Label = request.Label;
            if (request.Color != null) sprout.Color = request.Color;
            if (request.Form  != null) sprout.Form  = request.Form;
            if (request.X.HasValue)    sprout.X     = request.X.Value;
            if (request.Y.HasValue)    sprout.Y     = request.Y.Value;
            if (request.Z.HasValue)    sprout.Z     = request.Z.Value;

            await _sproutRepo.UpdateAsync(sprout);
        }

        public async Task DeleteSproutAsync(int sproutId) => await _sproutRepo.DeleteAsync(sproutId);

        // ── Helpers ──────────────────────────────────────────────────────────

        private async Task<TerrainResponse> MapWithSprouts(Terrain t)
        {
            var sprouts = await _sproutRepo.GetByTerrainIdAsync(t.Id);
            return new TerrainResponse
            {
                Id         = t.Id,
                GardenId   = t.GardenId,
                ShapeType  = t.ShapeType,
                Dimensions = t.Dimensions?.RootElement,
                CreatedAt  = t.CreatedAt,
                Sprouts    = sprouts.Select(MapSprout).ToList()
            };
        }

        private static SproutResponse MapSprout(Sprout s) => new()
        {
            Id        = s.Id,
            TerrainId = s.TerrainId,
            PlantId   = s.PlantId,
            Label     = s.Label,
            Color     = s.Color,
            Form      = s.Form,
            X         = s.X,
            Y         = s.Y,
            Z         = s.Z
        };
    }
}
