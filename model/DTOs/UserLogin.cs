namespace JavaHateBE.Model.DTOs
{
    /// <summary>
    /// Data transfer object for user login.
    /// </summary>
    public class UserLogin
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public UserLogin(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}