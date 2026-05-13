using conservation_backend.Config;
using conservation_backend.Features.LessRangerDivisionConfig.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessRangerDivisionConfig;

public class LessRangerDivisionConfigRepository(AppDBContext context, IUserContext userContext)
    : ILessRangerDivisionConfigRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<LessRangerDivisionConfigResponseDto> GetConfig()
    {
        var rankCategories = await _context.LessRangerRankCategories
            .AsNoTracking()
            .Select(x => new LessRangerRankCategoryItemDto
            {
                RankId = x.RankId,
                Category = x.Category,
            })
            .ToListAsync();

        var dutyFields = await _context.LessRangerDutyFieldDefinitions
            .AsNoTracking()
            .OrderBy(x => x.Category)
            .ThenBy(x => x.SortOrder)
            .Select(x => new LessRangerDutyFieldDefinitionDto
            {
                Id = x.Id,
                Category = x.Category,
                Key = x.Key,
                Label = x.Label,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
            })
            .ToListAsync();

        return new LessRangerDivisionConfigResponseDto
        {
            RankCategories = rankCategories,
            DutyFields = dutyFields,
        };
    }

    public async Task<bool> UpdateConfig(LessRangerDivisionConfigUpdateRequest request)
    {
        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var existingRankCategories = await _context.LessRangerRankCategories
            .ToListAsync();

        var incomingRankIds = request.RankCategories.Select(x => x.RankId).ToHashSet();

        foreach (var existing in existingRankCategories)
        {
            if (!incomingRankIds.Contains(existing.RankId))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var item in request.RankCategories)
        {
            var existing = existingRankCategories.FirstOrDefault(x => x.RankId == item.RankId);
            if (existing == null)
            {
                _context.LessRangerRankCategories.Add(new LessRangerRankCategory
                {
                    RankId = item.RankId,
                    Category = item.Category,
                    CreatedAt = now,
                    CreatedBy = userId,
                });
                continue;
            }

            existing.Category = item.Category;
            existing.UpdatedAt = now;
            existing.UpdatedBy = userId;
            existing.DeletedAt = null;
        }

        var existingDutyFields = await _context.LessRangerDutyFieldDefinitions
            .ToListAsync();

        var incomingIds = request.DutyFields
            .Where(x => x.Id.HasValue)
            .Select(x => x.Id!.Value)
            .ToHashSet();

        foreach (var existing in existingDutyFields)
        {
            if (!incomingIds.Contains(existing.Id))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var item in request.DutyFields)
        {
            if (item.Id.HasValue)
            {
                var existing = existingDutyFields.FirstOrDefault(x => x.Id == item.Id.Value);
                if (existing == null)
                    continue;

                existing.Category = item.Category;
                existing.Key = item.Key;
                existing.Label = item.Label;
                existing.SortOrder = item.SortOrder;
                existing.IsActive = item.IsActive;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
                existing.DeletedAt = null;
                continue;
            }

            _context.LessRangerDutyFieldDefinitions.Add(new LessRangerDutyFieldDefinition
            {
                Category = item.Category,
                Key = item.Key,
                Label = item.Label,
                SortOrder = item.SortOrder,
                IsActive = item.IsActive,
                CreatedAt = now,
                CreatedBy = userId,
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
