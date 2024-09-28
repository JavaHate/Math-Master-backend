namespace JavaHateBE.model.DTOs
{
    /// <summary>
    /// Data transfer object for user login.
    /// </summary>
    public struct UserLogin
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