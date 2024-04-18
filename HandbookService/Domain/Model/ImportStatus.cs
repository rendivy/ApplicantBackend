using System.Text.Json.Serialization;

namespace HandbookService.Domain.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImportStatus
{ 
    InProcess,
    Finished,
    Error
}