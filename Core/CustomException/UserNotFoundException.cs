namespace Common.CustomException;

public class UserNotFoundException(string message) : System.Exception(message);