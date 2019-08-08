using Zopa.Core.Contracts;

namespace Zopa.Core.Common
{
    public class Conditions : IConditions
    {
        /// <inheritdoc />
        public bool CheckAmountRequestedValid(int amountRequested)
        {
            bool isDivisible = IsDivisible(amountRequested, 100);
            bool isWithinRange = IsWithinRange(amountRequested, 1000, 15000);
            return isDivisible && isWithinRange;
        }

        /// <inheritdoc />
        public bool IsDivisible(int amountRequested, int divisible) => amountRequested % divisible == 0;

        /// <inheritdoc />
        public bool IsWithinRange(int amountRequested, int rangeBottom, int rangeTop) => amountRequested >= rangeBottom && amountRequested <= rangeTop;

    }
}
