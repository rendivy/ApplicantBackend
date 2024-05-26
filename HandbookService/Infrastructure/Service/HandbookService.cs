using System.Net.Http.Headers;
using System.Text;
using Common.Exception;
using HandbookService.Domain.Model;
using HandbookService.Domain.Model.Education;
using HandbookService.Domain.Model.Pagination;
using HandbookService.Domain.Service;
using HandbookService.Infrastructure.Data;
using HandbookService.Presentation.Mappers;
using HandbookService.Presentation.Model;
using Newtonsoft.Json;

namespace HandbookService.Infrastructure.Service;

public class HandbookService : IHandbookService
{
    private readonly HttpClient _httpClient;
    private readonly HandbookDbContext _handbookDbContext;
    private readonly IConfiguration _configuration;

    public HandbookService(IHttpClientFactory httpClientFactory, HandbookDbContext handbookDbContext,
        IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _handbookDbContext = handbookDbContext;
        _configuration = configuration;
        ConfigureRemoteService();
    }

    private void ConfigureRemoteService()
    {
        var baseUrl = _configuration.GetConnectionString("RemoteHandbookUrl");
        var username = _configuration.GetConnectionString("RemoteHandbookUsername");
        var password = _configuration.GetConnectionString("RemoteHandbookPassword");
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
    }

    private async Task<string> GetExternalResponseJson(string uri)
    {
        var remoteResponse = await _httpClient.GetAsync(uri);
        remoteResponse.EnsureSuccessStatusCode();
        return await remoteResponse.Content.ReadAsStringAsync();
    }

    private async Task ClearFaculties()
    {
        _handbookDbContext.Faculty.RemoveRange(_handbookDbContext.Faculty);
        await _handbookDbContext.SaveChangesAsync();
    }

    private async Task ClearEducationLevels()
    {
        _handbookDbContext.EducationLevel.RemoveRange(_handbookDbContext.EducationLevel);
        await _handbookDbContext.SaveChangesAsync();
    }

    private async Task ClearDocumentTypes()
    {
        _handbookDbContext.DocumentType.RemoveRange(_handbookDbContext.DocumentType);
        await _handbookDbContext.SaveChangesAsync();
    }

    private async Task ClearPrograms()
    {
        _handbookDbContext.Program.RemoveRange(_handbookDbContext.Program);
        await _handbookDbContext.SaveChangesAsync();
    }


    private async Task ImportExternalDictionary()
    {
        var newImport = new Import
        {
            Id = Guid.NewGuid(),
            Status = ImportStatus.InProcess,
        };
        await ClearFaculties();
        await ClearEducationLevels();
        await ClearDocumentTypes();
        await ClearPrograms();
        await _handbookDbContext.HandbookImport.AddAsync(newImport);
        await _handbookDbContext.SaveChangesAsync();
        await UpdateFaculties();
        await UpdateEducationLevels();
        await UpdateDocumentTypes();
        await UpdatePrograms();
        newImport.Status = ImportStatus.Finished;
        await _handbookDbContext.SaveChangesAsync();
    }

    private async Task UpdateData<TResponse, TEntity, TMapper, TId>(string endpoint)
        where TResponse : class
        where TEntity : class, IEntity<TId>, new()
        where TMapper : IMapper<TResponse, TEntity>, new()
    {
        var json = await GetExternalResponseJson(endpoint);
        var dtoList = JsonConvert.DeserializeObject<List<TResponse>>(json);
        var entities = dtoList.Select(it => new TMapper().Map(it));
        foreach (var entity in entities)
        {
            var existingEntity = await _handbookDbContext.Set<TEntity>().FindAsync(entity.Id);
            if (existingEntity != null)
            {
                _handbookDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                await _handbookDbContext.Set<TEntity>().AddAsync(entity);
            }
        }

        await _handbookDbContext.SaveChangesAsync();
    }

    private async Task UpdateFaculties()
    {
        await UpdateData<FacultyResponse, Faculty, FacultyMapper, Guid>("faculties");
    }

    private async Task UpdateEducationLevels()
    {
        await UpdateData<EducationLevelResponse, EducationLevel, EducationLevelMapper, int>("education_levels");
    }

    private async Task UpdateDocumentTypes()
    {
        await UpdateData<EducationDocumentTypeResponse, EducationDocumentType, EducationDocumentTypeMapper, Guid>(
            "document_types");
    }


    private async Task UpdatePrograms()
    {
        var currentPage = 1;
        const int pageSize = 50;
        var hasNextPage = true;
        var remotePrograms = new List<EducationProgram>();

        while (hasNextPage)
        {
            var educationProgramsRequestUri = $"programs?page={currentPage}&size={pageSize}";
            var educationProgramsJson = await GetExternalResponseJson(educationProgramsRequestUri);
            var educationProgramsDto = JsonConvert.DeserializeObject<PaginationResponse>(educationProgramsJson);

            if (educationProgramsDto?.Programs != null && currentPage < educationProgramsDto.Pagination.Count)
            {
                var remoteEducationProgramsPage = educationProgramsDto.Programs
                    .Select(it => new ProgramsMapper().Map(it));

                remotePrograms.AddRange(remoteEducationProgramsPage);
                currentPage++;
                hasNextPage = true;
            }
            else
            {
                hasNextPage = false;
            }
        }

        //save 
        await _handbookDbContext.Program.AddRangeAsync(remotePrograms);


        await _handbookDbContext.SaveChangesAsync();
    }


    public async Task ImportAllHandbookDataAsync()
    {
        await ImportExternalDictionary();
    }

    public async Task<EducationProgram?> GetProgramByIdAsync(Guid id)
    {
        var educationProgram = await _handbookDbContext.Program.FindAsync(id);
        if (educationProgram != null)
        {
            return educationProgram;
        }

        throw new ProgramNotFoundException("Program with this id not found");
    }

    public Task UpdateAllHandbookDataAsync()
    {
        throw new NotImplementedException();
    }
}

public interface IEntity<T>
{
    T Id { get; set; }
}