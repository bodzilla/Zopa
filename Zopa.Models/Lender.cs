namespace Zopa.Models
{
    public sealed class Lender
    {
        public Lender(string name, double annualInterestRateDecimal, int cashAvailable)
        {
            Name = name;
            AnnualInterestRateDecimal = annualInterestRateDecimal;
            CashAvailable = cashAvailable;
        }

        public string Name { get; }

        public double AnnualInterestRateDecimal { get; }

        public int CashAvailable { get; }
    }
}
