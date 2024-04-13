namespace Common.CustomException;

public class InvalidPasswordException(string message) : System.Exception(message);