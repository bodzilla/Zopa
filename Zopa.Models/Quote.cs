namespace Zopa.Models
{
    public sealed class Quote
    {
        public Quote(int amountRequested, double annualInterestRate, double monthlyRepayment, int repaymentLengthMonths)
        {
            AmountRequested = amountRequested;
            AnnualInterestRate = annualInterestRate;
            MonthlyRepayment = monthlyRepayment;
            TotalRepayment = MonthlyRepayment * repaymentLengthMonths;
        }

        public int AmountRequested { get; }

        public double AnnualInterestRate { get; }

        public double MonthlyRepayment { get; }

        public double TotalRepayment { get; }
    }
}
