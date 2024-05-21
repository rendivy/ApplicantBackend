using EnrollmentService.Presentation.Model;

namespace EnrollmentService.Presentation.Util;

public static class AdmissionRequestValidator
{
    public static bool HasUniquePriorities(AdmissionRequest request)
    {
        var priorities = request.Programs.Select(p => p.Priority);
        return priorities.Distinct().Count() == request.Programs.Count;
    }
}