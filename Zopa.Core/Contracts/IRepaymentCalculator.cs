namespace Zopa.Core.Contracts
{
    /// <summary>
    /// The service which calculates repayments on loans.
    /// </summary>
    public interface IRepaymentCalculator
    {
        /// <summary>
        /// Calculates the monthly repayment of a given loan.
        /// </summary>
        /// <param name="amountRequested"></param>
        /// <param name="interestRateDecimal"></param>
        /// <param name="repaymentLengthMonths"></param>
        /// <returns></returns>
        double CalculateRepayment(int amountRequested, double interestRateDecimal, int repaymentLengthMonths);
    }
}
