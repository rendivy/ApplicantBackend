namespace Common.RabbitModel.Manager;

public class ManagerAccountCreateResponse
{
    public required string From { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Password { get; set; }
}