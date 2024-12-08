namespace JavaHateBE.Exceptions
{
    /// <summary>
    /// Exception thrown when an object is not found.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="model">The name of the model that was not found.</param>
    public class ObjectNotFoundException(string model, string message) : Exception(message)
    {
        public string Object { get; } = model;
    }
}