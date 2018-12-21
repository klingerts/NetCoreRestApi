namespace Kts.RefactorThis.Common
{
    /// <summary>
    /// Holds application specific configuration.
    /// Instance of class is registered as a singleton during app startup process.
    /// </summary>
    public class AppConfiguration
    {
        public static int MaxQueryRowsLimitDefault = 100;

        // Use secure defaults
        public int QueryRowsLimit { get; set; } = MaxQueryRowsLimitDefault;
        public bool ReturnsHumanReadableJson { get; set; } = false;
        public bool EnableSwagger { get; set; } = false;
        public string SwaggerEndpoint { get; set; } = "/swagger/{Version}/swagger.json";
        public bool EnableElmah { get; set; } = false;
        public bool UseDevPageInDevelopment { get; set; } = false;
        public bool DisableHttpsRedirection { get; set; } = false;
        public bool AcceptHttp { get; set; } = false;
        public bool DisableHsts { get; set; } = true;
    }
}
