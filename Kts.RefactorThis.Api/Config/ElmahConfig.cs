namespace Kts.RefactorThis.Api.Config
{
    /// <summary>
    /// Provides functionality to build connection strings
    /// TODO: Configuration is just the basic
    /// </summary>
    public class ElmahConfig
    {
        // Path to elmah page
        public string Path { get; set; }

        // Path to file system
        public string LogPath { get; set; }
    }
}
