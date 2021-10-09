namespace Masiv.Roulette.Api
{
    public class DatabaseSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool AllowUserVariables { get; set; }
    }
}
