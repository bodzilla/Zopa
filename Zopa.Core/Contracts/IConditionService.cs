namespace Zopa.Core.Contracts
{
    /// <summary>
    /// Constraints for requests.
    /// </summary>
    public interface IConditionService
    {
        /// <summary>
        /// Checks if the amount requested is valid.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <returns></returns>
        bool IsAmountRequestedValid(int amountRequested);
    }
}
