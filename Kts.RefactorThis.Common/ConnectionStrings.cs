namespace Kts.RefactorThis.Common
{
    /// <summary>
    /// Holds connection string value retrieved from configuration during application startup.
    /// Instance of class is registered as a singleton during app startup process
    /// </summary>
    public class ConnectionStrings
    {
        public string Default { get; set; }
    }
}
