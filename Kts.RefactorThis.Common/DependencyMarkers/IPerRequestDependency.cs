namespace Kts.RefactorThis.Common.DependencyMarkers
{
    /// <summary>
    /// Hints DI service configuration that type implementing this interface
    /// should be as registered as instance per request (scoped)
    /// </summary>
    public interface IPerRequestDependency
    {
    }
}
