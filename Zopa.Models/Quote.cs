namespace Zopa.Models
{
    public sealed class Quote
    {
        public Quote(int amountRequested, double annualInterestRateDecimal, double monthlyRepayment, int repaymentLengthMonths)
        {
            AmountRequested = amountRequested;
            AnnualInterestRateDecimal = annualInterestRateDecimal;
            MonthlyRepayment = monthlyRepayment;
            TotalRepayment = MonthlyRepayment * repaymentLengthMonths;

            // Convert to percentage.
            AnnualInterestRatePercentage = AnnualInterestRateDecimal * 100;
        }

        public int AmountRequested { get; }

        public double AnnualInterestRateDecimal { get; }

        public double AnnualInterestRatePercentage { get; }

        public double MonthlyRepayment { get; }

        public double TotalRepayment { get; }
    }
}
