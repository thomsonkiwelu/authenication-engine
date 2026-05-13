using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.Weathers.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.Weathers;

public class WeatherService(IWeatherRepository repository, IFileService fileService, IMapper mapper): IWeatherService
{
    private readonly IWeatherRepository _weatherRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<WeatherResponseDto>> GetPagedWeather(WeatherPaginationDto dto)
    {
        var pagedData = await _weatherRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<WeatherResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<WeatherResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<WeatherDto> CreateWeather(WeatherRequest dto)
    {
        var weather = _mapper.Map<Weather>(dto);
        var created = await _weatherRepository.Create(weather);
        
        if(!string.IsNullOrWhiteSpace(dto.AttachmentId))
            await _fileService.UpdateUploadedFile(dto.AttachmentId, AppEntities.WeatherEntity, created.Id);
     
        var responseDto = _mapper.Map<WeatherDto>(created);
        return responseDto;
    }

    public async Task<WeatherDto> GetWeatherById(Guid id)
    {
        var result = await _weatherRepository.GetById(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.WeatherEntity
        );

        return result;
    }

    public async Task<WeatherDto> UpdateWeather(Guid id, WeatherRequest dto)
    {
        var weatherId= await _weatherRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(weatherId))
            throw new ArgumentNullException("Failure to update weather data");
        
        return await _weatherRepository.GetById(id);
    }

    public async Task<bool> DeleteWeather(Guid id)
    {
        return await _weatherRepository.Delete(id);
    }
}