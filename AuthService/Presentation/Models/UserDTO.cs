namespace AuthService.Presentation.Models;

public record UserDTO
{
    public string Email { get; init; }
    public bool IsEmailConfirmed { get; init; }

    public UserDTO() { }

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

        public UserDTO Build()
        {
            return new UserDTO
            {
                Email = Email,
                IsEmailConfirmed = IsEmailConfirmed
            };
        }
    }

    public static UserDTOBuilder Builder => new UserDTOBuilder();
}
