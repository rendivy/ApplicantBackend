using AuthService.Presentation.Models.Admin;
using AuthService.Presentation.Models.Token;

namespace AuthService.Domain.Interfaces;

public interface IAdminService
{
    public Task CreateManager(CreateManagerRequest createManagerRequest, string userRole);
}