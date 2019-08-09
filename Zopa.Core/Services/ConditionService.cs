using Microsoft.Extensions.Configuration;
using Zopa.Core.Contracts;

namespace Zopa.Core.Services
{
    public class ConditionService : IConditionService
    {
        private readonly int _divisibleValue;
        private readonly int _acceptanceRangeBottom;
        private readonly int _acceptanceRangeTop;

        public ConditionService(IConfiguration configuration)
        {
            _divisibleValue = configuration.GetValue<int>("DivisibleValue");
            _acceptanceRangeBottom = configuration.GetValue<int>("AcceptanceRangeBottom");
            _acceptanceRangeTop = configuration.GetValue<int>("AcceptanceRangeTop");
        }

        /// <inheritdoc />
        public bool CheckAmountRequestedValid(int amountRequested)
        {
            bool isDivisible = IsDivisible(amountRequested);
            bool isWithinRange = IsWithinRange(amountRequested);
            return isDivisible && isWithinRange;
        }

        /// <inheritdoc />
        public bool IsDivisible(int amountRequested) => amountRequested % _divisibleValue == 0;

        /// <inheritdoc />
        public bool IsWithinRange(int amountRequested)
            => amountRequested >= _acceptanceRangeBottom && amountRequested <= _acceptanceRangeTop;
    }
}
