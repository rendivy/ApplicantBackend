namespace Common.RabbitModel.Email;

public class EmailResponse
{
    public string To { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    
}