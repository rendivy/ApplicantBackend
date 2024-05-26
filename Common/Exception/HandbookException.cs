namespace Common.Exception;

public class ProgramNotFoundException(string message) : System.Exception(message);

public class UnauthorizedRoleException(string message) : System.Exception(message);