namespace JavaHateBE.exceptions
{
    /// <summary>
    /// Exception thrown when an object is not found.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public class ObjectNotFoundException(string model, string message) : Exception(message) 
    {
        public string Object { get; } = model;
    }
}