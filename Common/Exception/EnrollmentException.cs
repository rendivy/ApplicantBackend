namespace Common.Exception;

public class EnrollmentException(string message = "Enrollment for this applicant already exist")
    : System.Exception(message);

public class EnrollmentNotFound(string message) : System.Exception(message);

public class EnrollmentProgrammNotFound(string message) : System.Exception(message);

public class EnrollmentProgramStatusException(string message) : System.Exception(message);