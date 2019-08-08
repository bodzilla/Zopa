namespace Zopa.Core.Contracts
{
    /// <summary>
    /// Constraints for requests.
    /// </summary>
    public interface ICondition
    {
        bool IsValidRequest(int amountRequested);
    }
}
