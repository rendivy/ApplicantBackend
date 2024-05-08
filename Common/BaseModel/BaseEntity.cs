using System.ComponentModel.DataAnnotations;

namespace Common.BaseModel;

public class BaseEntity
{
    [Key] public Guid Id { get; set; }
}