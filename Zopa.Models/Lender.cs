namespace Zopa.Models
{
    public sealed class Lender
    {
        public Lender(string name, double annualInterestRateDecimal, double cashAvailable)
        {
            Name = name;
            AnnualInterestRateDecimal = annualInterestRateDecimal;
            CashAvailable = cashAvailable;
        }

        public string Name { get; }

        public double AnnualInterestRateDecimal { get; }

        public double CashAvailable { get; }
    }
}
