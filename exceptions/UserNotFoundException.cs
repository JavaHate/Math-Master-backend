namespace JavaHateBE.exceptions
{
    /// <summary>
    /// Exception thrown when a user is not found.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public class UserNotFoundException(string message) : Exception(message)
    {
    }
}