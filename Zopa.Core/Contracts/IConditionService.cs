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
        bool CheckAmountRequestedValid(int amountRequested);

        /// <summary>
        /// Checks if the amount requested is disivible by the given number.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <param name="divisible"></param>
        /// <returns></returns>
        bool IsDivisible(int amountRequested, int divisible);

        /// <summary>
        /// Checks if the amount requested is within the given range.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <param name="rangeBottom"></param>
        /// <param name="rangeTop"></param>
        /// <returns></returns>
        bool IsWithinRange(int amountRequested, int rangeBottom, int rangeTop);
    }
}
