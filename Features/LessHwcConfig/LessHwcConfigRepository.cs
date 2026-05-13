using conservation_backend.Config;
using conservation_backend.Features.LessHwcConfig.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessHwcConfig;

public class LessHwcConfigRepository(AppDBContext context, IUserContext userContext) : ILessHwcConfigRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<LessHwcConfigResponseDto> GetConfig()
    {
        var tabs = await _context.LessHwcTabDefinitions
            .AsNoTracking()
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Label)
            .Select(x => new LessHwcTabDto
            {
                Id = x.Id,
                Key = x.Key,
                Label = x.Label,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
            })
            .ToListAsync();

        var fields = await _context.LessHwcFieldDefinitions
            .AsNoTracking()
            .Include(x => x.TabDefinition)
            .Include(x => x.Options)
            .OrderBy(x => x.TabDefinition.SortOrder)
            .ThenBy(x => x.SortOrder)
            .ThenBy(x => x.Label)
            .Select(x => new LessHwcFieldDto
            {
                Id = x.Id,
                TabDefinitionId = x.TabDefinitionId,
                TabKey = x.TabDefinition.Key,
                Key = x.Key,
                Label = x.Label,
                FieldType = x.FieldType,
                Placeholder = x.Placeholder,
                IsRequired = x.IsRequired,
                IsComputed = x.IsComputed,
                ComputeFromKeys = x.ComputeFromKeys,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                Options = x.Options
                    .OrderBy(o => o.SortOrder)
                    .ThenBy(o => o.Label)
                    .Select(o => new LessHwcFieldOptionDto
                    {
                        Id = o.Id,
                        Value = o.Value,
                        Label = o.Label,
                        SortOrder = o.SortOrder,
                        IsActive = o.IsActive,
                    })
                    .ToList(),
            })
            .ToListAsync();

        return new LessHwcConfigResponseDto
        {
            Tabs = tabs,
            Fields = fields,
        };
    }

    public async Task<bool> UpdateConfig(LessHwcConfigUpdateRequest request)
    {
        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var existingTabs = await _context.LessHwcTabDefinitions
            .ToListAsync();

        var existingFields = await _context.LessHwcFieldDefinitions
            .ToListAsync();

        var existingOptions = await _context.LessHwcFieldOptions
            .ToListAsync();

        var incomingTabIds = request.Tabs
            .Where(x => x.Id.HasValue)
            .Select(x => x.Id!.Value)
            .ToHashSet();

        foreach (var existing in existingTabs)
        {
            if (!incomingTabIds.Contains(existing.Id))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        var tabIdByKey = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        foreach (var (item, index) in request.Tabs.Select((t, i) => (t, i)))
        {
            var key = (item.Key ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key))
                continue;

            var label = (item.Label ?? string.Empty).Trim();
            var sort = item.SortOrder > 0 ? item.SortOrder : index + 1;

            if (item.Id.HasValue)
            {
                var existing = existingTabs.FirstOrDefault(x => x.Id == item.Id.Value);
                if (existing == null) continue;

                existing.Key = key;
                existing.Label = label;
                existing.SortOrder = sort;
                existing.IsActive = item.IsActive;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
                existing.DeletedAt = null;

                tabIdByKey[key] = existing.Id;
                continue;
            }

            var created = new LessHwcTabDefinition
            {
                Id = Guid.NewGuid(),
                Key = key,
                Label = label,
                SortOrder = sort,
                IsActive = item.IsActive,
                CreatedAt = now,
                CreatedBy = userId,
            };

            _context.LessHwcTabDefinitions.Add(created);
            tabIdByKey[key] = created.Id;
            existingTabs.Add(created);
        }

        var incomingFieldIds = request.Fields
            .Where(x => x.Id.HasValue)
            .Select(x => x.Id!.Value)
            .ToHashSet();

        foreach (var existing in existingFields)
        {
            if (!incomingFieldIds.Contains(existing.Id))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var (item, index) in request.Fields.Select((f, i) => (f, i)))
        {
            var key = (item.Key ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key))
                continue;

            var label = (item.Label ?? string.Empty).Trim();

            var tabId = item.TabDefinitionId;
            if (!tabId.HasValue)
            {
                var tabKey = (item.TabKey ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(tabKey) && tabIdByKey.TryGetValue(tabKey, out var t))
                    tabId = t;
            }

            if (!tabId.HasValue)
                throw new InvalidOperationException($"Tab is required for field: {key}");

            var sort = item.SortOrder > 0 ? item.SortOrder : index + 1;

            if (item.Id.HasValue)
            {
                var existing = existingFields.FirstOrDefault(x => x.Id == item.Id.Value);
                if (existing == null) continue;

                existing.TabDefinitionId = tabId.Value;
                existing.Key = key;
                existing.Label = label;
                existing.FieldType = (item.FieldType ?? "text").Trim();
                existing.Placeholder = (item.Placeholder ?? string.Empty).Trim();
                existing.IsRequired = item.IsRequired;
                existing.IsComputed = item.IsComputed;
                existing.ComputeFromKeys = (item.ComputeFromKeys ?? string.Empty).Trim();
                existing.SortOrder = sort;
                existing.IsActive = item.IsActive;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
                existing.DeletedAt = null;
                continue;
            }

            var created = new LessHwcFieldDefinition
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = tabId.Value,
                Key = key,
                Label = label,
                FieldType = (item.FieldType ?? "text").Trim(),
                Placeholder = (item.Placeholder ?? string.Empty).Trim(),
                IsRequired = item.IsRequired,
                IsComputed = item.IsComputed,
                ComputeFromKeys = (item.ComputeFromKeys ?? string.Empty).Trim(),
                SortOrder = sort,
                IsActive = item.IsActive,
                CreatedAt = now,
                CreatedBy = userId,
            };

            _context.LessHwcFieldDefinitions.Add(created);
            existingFields.Add(created);
        }

        var incomingOptionIds = request.Fields
            .SelectMany(f => f.Options)
            .Where(o => o.Id.HasValue)
            .Select(o => o.Id!.Value)
            .ToHashSet();

        foreach (var existing in existingOptions)
        {
            if (!incomingOptionIds.Contains(existing.Id))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var field in request.Fields)
        {
            var fieldKey = (field.Key ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(fieldKey))
                continue;

            Guid? tabId = field.TabDefinitionId;
            if (!tabId.HasValue)
            {
                var tabKey = (field.TabKey ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(tabKey) && tabIdByKey.TryGetValue(tabKey, out var t))
                    tabId = t;
            }

            if (!tabId.HasValue)
                continue;

            Guid? resolvedFieldId = field.Id;

            if (!resolvedFieldId.HasValue)
            {
                resolvedFieldId = existingFields
                    .FirstOrDefault(x => x.TabDefinitionId == tabId.Value && x.Key.Equals(fieldKey, StringComparison.OrdinalIgnoreCase))
                    ?.Id;
            }

            if (!resolvedFieldId.HasValue)
                continue;

            var relevantExisting = existingOptions.Where(x => x.FieldDefinitionId == resolvedFieldId.Value).ToList();

            foreach (var (opt, index) in field.Options.Select((o, i) => (o, i)))
            {
                var value = (opt.Value ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                var label = (opt.Label ?? string.Empty).Trim();
                var sort = opt.SortOrder > 0 ? opt.SortOrder : index + 1;

                if (opt.Id.HasValue)
                {
                    var existing = relevantExisting.FirstOrDefault(x => x.Id == opt.Id.Value);
                    if (existing == null) continue;

                    existing.Value = value;
                    existing.Label = label;
                    existing.SortOrder = sort;
                    existing.IsActive = opt.IsActive;
                    existing.UpdatedAt = now;
                    existing.UpdatedBy = userId;
                    existing.DeletedAt = null;
                    continue;
                }

                _context.LessHwcFieldOptions.Add(new LessHwcFieldOption
                {
                    Id = Guid.NewGuid(),
                    FieldDefinitionId = resolvedFieldId.Value,
                    Value = value,
                    Label = label,
                    SortOrder = sort,
                    IsActive = opt.IsActive,
                    CreatedAt = now,
                    CreatedBy = userId,
                });
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
