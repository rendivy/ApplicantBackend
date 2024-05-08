using Common.BaseModel;

namespace Common.RabbitModel.User;

public class ApplicantResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }
    public required Gender Gender { get; set; }
    public required string Citizenship { get; init; }
}


public class ApplicantResponseBuilder
{
    private Guid _id;
    private string _email;
    private DateOnly _dateOfBirth;
    private string _fullName;
    private string _phoneNumber;
    private Gender _gender;
    private string _citizenship;

    public ApplicantResponseBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ApplicantResponseBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public ApplicantResponseBuilder WithDateOfBirth(DateOnly dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }

    public ApplicantResponseBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public ApplicantResponseBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public ApplicantResponseBuilder WithGender(Gender gender)
    {
        _gender = gender;
        return this;
    }

    public ApplicantResponseBuilder WithCitizenship(string citizenship)
    {
        _citizenship = citizenship;
        return this;
    }

    public ApplicantResponse Build()
    {
        return new ApplicantResponse
        {
            Id = _id,
            Email = _email,
            DateOfBirth = _dateOfBirth,
            FullName = _fullName,
            PhoneNumber = _phoneNumber,
            Gender = _gender,
            Citizenship = _citizenship
        };
    }
}