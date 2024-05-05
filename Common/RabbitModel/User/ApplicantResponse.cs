namespace Common.RabbitModel.User;

public class ApplicantResponse
{
    public string Id { get; init; }
    public string Email { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public string? Role { get; init; }
    public string? FullName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Citizenship { get; init; }
}