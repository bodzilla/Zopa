namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service repayments.
    /// </summary>
    public interface IRepaymentService
    {
        /// <summary>
        /// Gets the repayment amount for a requested loan.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <param name="interestRateDecimal"></param>
        /// <param name="repaymentLengthMonths"></param>
        /// <returns></returns>
        double GetRepaymentAmount(int amountRequested, double interestRateDecimal, int repaymentLengthMonths);
    }
}
