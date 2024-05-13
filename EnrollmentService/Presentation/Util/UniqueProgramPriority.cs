using System.ComponentModel.DataAnnotations;
using EnrollmentService.Application.Model;

namespace EnrollmentService.Presentation.Util;

public class UniquePrioritiesAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var list = value as IEnumerable<EnrollmentProgramRequest>;
        if (list == null)
            return true;

        return list.GroupBy(x => x.EnrollmentPriority).All(g => g.Count() == 1);
    }

    public override string FormatErrorMessage(string name)
    {
        return "Priorities in request should be unique.";
    }
}