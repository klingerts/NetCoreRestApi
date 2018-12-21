namespace Kts.RefactorThis.Application.Abstractions
{
    /// <summary>
    /// Used to create units of work instances.
    /// Allows creation of multi-level unit of work (sub transaction).
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        // Returns a new instance of a unit of work object
        IUnitOfWork GetNewUnitOfWork();
    }
}
