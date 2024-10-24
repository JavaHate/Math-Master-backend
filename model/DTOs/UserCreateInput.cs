namespace JavaHateBE.Model.DTOs
{
    /// <summary>
    /// Data transfer object for user creation (Id should not be passed).
    /// </summary>
    public record UserCreateInput(string Username, string Password, string Email);
}