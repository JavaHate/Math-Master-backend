namespace JavaHateBE.Model.DTOs
{
    /// <summary>
    /// Data transfer object for user update (Id should be passed).
    /// </summary>
    public record UpdateUserInput(Guid Id, string Username, string Password, string Email);
}