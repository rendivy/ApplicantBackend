using HandbookService.Domain.Model;
using HandbookService.Domain.Model.Education;
using HandbookService.Presentation.Model;

namespace HandbookService.Presentation.Mappers;

public interface IMapper<TObjectFrom, TObjectTo>
{
    public TObjectTo Map(TObjectFrom objectFrom);
    public TObjectFrom Map(TObjectTo objectTo);
}

public class FacultyMapper : IMapper<FacultyResponse, Faculty>
{
    public Faculty Map(FacultyResponse objectFrom)
    {
        return new Faculty
        {
            Id = objectFrom.Id,
            Name = objectFrom.Name,
            CreateTime = objectFrom.CreateTime
        };
    }

    public FacultyResponse Map(Faculty objectTo)
    {
        throw new NotImplementedException();
    }
}

public class EducationMapper : IMapper<EducationProgramResponse, EducationProgram>
{
    public EducationProgram Map(EducationProgramResponse objectFrom)
    {
        return new EducationProgram
        {
            Id = objectFrom.Id,
            Name = objectFrom.Name,
            CreateTime = objectFrom.CreateTime,
            Code = objectFrom.Code,
            Language = objectFrom.Language,
            EducationForm = objectFrom.EducationForm,
            FacultyId = objectFrom.Faculty.Id,
            EducationLevelId = objectFrom.EducationLevel.Id
        };
    }

    public EducationProgramResponse Map(EducationProgram objectTo)
    {
        throw new NotImplementedException();
    }
}

public class EducationLevelMapper : IMapper<EducationLevelResponse, EducationLevel>
{
    public EducationLevel Map(EducationLevelResponse objectFrom)
    {
        return new EducationLevel
        {
            Id = objectFrom.Id,
            Name = objectFrom.Name,
        };
    }

    public EducationLevelResponse Map(EducationLevel objectTo)
    {
        throw new NotImplementedException();
    }
}

public class EducationDocumentTypeMapper : IMapper<EducationDocumentTypeResponse, EducationDocumentType>
{
    public EducationDocumentType Map(EducationDocumentTypeResponse objectFrom)
    {
        return new EducationDocumentType
        {
            Id = objectFrom.Id,
            Name = objectFrom.Name,
            CreateTime = objectFrom.CreateTime,
            EducationLevelId = objectFrom.EducationLevelResponse.Id,
            //тут надо вытащить id из всех объектов, которые пришли
            NextEducationLevelsIds = objectFrom.NextEducationLevels.Select(x => x.Id).ToList()
        };
    }

    public EducationDocumentTypeResponse Map(EducationDocumentType objectTo)
    {
        throw new NotImplementedException();
    }
}

public class ProgramsMapper : IMapper<EducationProgramResponse, EducationProgram>
{
    public EducationProgram Map(EducationProgramResponse objectFrom)
    {
        return new EducationProgram
        {
            Id = objectFrom.Id,
            Name = objectFrom.Name,
            CreateTime = objectFrom.CreateTime,
            Code = objectFrom.Code,
            Language = objectFrom.Language,
            EducationForm = objectFrom.EducationForm,
            FacultyId = objectFrom.Faculty.Id,
            EducationLevelId = objectFrom.EducationLevel.Id,
        };
    }

    public EducationProgramResponse Map(EducationProgram objectTo)
    {
        throw new NotImplementedException();
    }
}