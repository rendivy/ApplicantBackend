namespace Common.CustomException;

public class UserAlreadyHaveRoleException(string message) : System.Exception(message);

public class UserDoesntHavePermissionException(string message) : System.Exception(message);

public class InvalidRefreshTokenException : System.Exception;

public class UserNotFoundException(string message) : System.Exception(message);

public class InvalidPasswordException(string message) : System.Exception(message);