namespace AuthService.Presentation.Models;

public record UserRequest
{
    public string Email { get; init; }
    public bool IsEmailConfirmed { get; init; }

    public UserRequest() { }

    public class UserDTOBuilder
    {
        private string Email { get; set; }
        private bool IsEmailConfirmed { get; set; }

        public UserDTOBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public UserDTOBuilder WithIsEmailConfirmed(bool isEmailConfirmed)
        {
            IsEmailConfirmed = isEmailConfirmed;
            return this;
        }

        public UserRequest Build()
        {
            return new UserRequest
            {
                Email = Email,
                IsEmailConfirmed = IsEmailConfirmed
            };
        }
    }

    public static UserDTOBuilder Builder => new UserDTOBuilder();
}
