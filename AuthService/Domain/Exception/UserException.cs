namespace AuthService.Domain.Exception;

public class UserAlreadyHaveRoleException(string message) : System.Exception(message);

public class UserDoesntHavePermissionException(string message) : System.Exception(message);

