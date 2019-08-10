namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service repayments.
    /// </summary>
    public interface IRepaymentService
    {
        /// <summary>
        /// Gets the monthly repayment amount for a requested loan.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <param name="annualInterestRateDecimal"></param>
        /// <param name="repaymentLengthMonths"></param>
        /// <returns></returns>
        double GetMonthlyRepaymentAmount(int amountRequested, double annualInterestRateDecimal, int repaymentLengthMonths);
    }
}
