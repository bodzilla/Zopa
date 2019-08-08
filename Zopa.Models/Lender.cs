namespace Zopa.Models
{
    public sealed class Lender
    {
        public Lender(string name, double interestRateDecimal, double cashAvailable)
        {
            Name = name;
            InterestRateDecimal = interestRateDecimal;
            CashAvailable = cashAvailable;
        }

        public string Name { get; }

        public double InterestRateDecimal { get; }

        public double CashAvailable { get; }
    }
}
