using conservation_backend.Config;
using conservation_backend.Features.LessLivestockConfig.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessLivestockConfig;

public class LessLivestockConfigRepository(AppDBContext context, IUserContext userContext)
    : ILessLivestockConfigRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<LessLivestockConfigResponseDto> GetConfig()
    {
        var types = await _context.LessLivestockTypes
            .AsNoTracking()
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Label)
            .Select(x => new LessLivestockTypeDto
            {
                Id = x.Id,
                Key = x.Key,
                Label = x.Label,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
            })
            .ToListAsync();

        var actionOptions = await _context.LessLivestockActionOptions
            .AsNoTracking()
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Label)
            .Select(x => new LessLivestockActionOptionDto
            {
                Id = x.Id,
                Key = x.Key,
                Label = x.Label,
                SortOrder = x.SortOrder,
                RequiresControlNumber = x.RequiresControlNumber,
                RequiresCaseNumber = x.RequiresCaseNumber,
                IsRevenue = x.IsRevenue,
                IsActive = x.IsActive,
            })
            .ToListAsync();

        return new LessLivestockConfigResponseDto
        {
            LivestockTypes = types,
            ActionOptions = actionOptions,
        };
    }

    public async Task<bool> UpdateConfig(LessLivestockConfigUpdateRequest request)
    {
        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var existingTypes = await _context.LessLivestockTypes
            .ToListAsync();

        var incomingTypeIds = request.LivestockTypes
            .Where(x => x.Id.HasValue)
            .Select(x => x.Id!.Value)
            .ToHashSet();

        foreach (var existing in existingTypes)
        {
            if (!incomingTypeIds.Contains(existing.Id))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var item in request.LivestockTypes)
        {
            if (item.Id.HasValue)
            {
                var existing = existingTypes.FirstOrDefault(x => x.Id == item.Id.Value);
                if (existing == null)
                    continue;

                existing.Key = (item.Key ?? string.Empty).Trim();
                existing.Label = (item.Label ?? string.Empty).Trim();
                existing.SortOrder = item.SortOrder;
                existing.IsActive = item.IsActive;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
                existing.DeletedAt = null;
                continue;
            }

            _context.LessLivestockTypes.Add(new LessLivestockType
            {
                Key = (item.Key ?? string.Empty).Trim(),
                Label = (item.Label ?? string.Empty).Trim(),
                SortOrder = item.SortOrder,
                IsActive = item.IsActive,
                CreatedAt = now,
                CreatedBy = userId,
            });
        }

        var existingActions = await _context.LessLivestockActionOptions
            .ToListAsync();

        var incomingActionIds = request.ActionOptions
            .Where(x => x.Id.HasValue)
            .Select(x => x.Id!.Value)
            .ToHashSet();

        foreach (var existing in existingActions)
        {
            if (!incomingActionIds.Contains(existing.Id))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var item in request.ActionOptions)
        {
            if (item.Id.HasValue)
            {
                var existing = existingActions.FirstOrDefault(x => x.Id == item.Id.Value);
                if (existing == null)
                    continue;

                existing.Key = (item.Key ?? string.Empty).Trim();
                existing.Label = (item.Label ?? string.Empty).Trim();
                existing.SortOrder = item.SortOrder;
                existing.RequiresControlNumber = item.RequiresControlNumber;
                existing.RequiresCaseNumber = item.RequiresCaseNumber;
                existing.IsRevenue = item.IsRevenue;
                existing.IsActive = item.IsActive;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
                existing.DeletedAt = null;
                continue;
            }

            _context.LessLivestockActionOptions.Add(new LessLivestockActionOption
            {
                Key = (item.Key ?? string.Empty).Trim(),
                Label = (item.Label ?? string.Empty).Trim(),
                SortOrder = item.SortOrder,
                RequiresControlNumber = item.RequiresControlNumber,
                RequiresCaseNumber = item.RequiresCaseNumber,
                IsRevenue = item.IsRevenue,
                IsActive = item.IsActive,
                CreatedAt = now,
                CreatedBy = userId,
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
