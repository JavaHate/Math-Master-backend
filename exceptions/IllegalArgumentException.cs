namespace JavaHateBE.exceptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IllegalArgumentException"/> class with a specified 
    /// argument name and error message.
    /// Error for when an argument is invalid.
    /// </summary>
    /// <param name="argument">The name of the argument that caused the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public class IllegalArgumentException(string argument, string message) : Exception(message)
    {
        public string Argument { get; } = argument;
    }
}