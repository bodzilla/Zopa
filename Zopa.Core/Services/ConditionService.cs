using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;

namespace Zopa.Core.Services
{
    public class ConditionService : IConditionService
    {
        private readonly ILogger<ConditionService> _logger;
        private readonly int _divisibleValue;
        private readonly int _acceptanceRangeBottom;
        private readonly int _acceptanceRangeTop;

        public ConditionService(ILogger<ConditionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _divisibleValue = configuration.GetValue<int>("DivisibleValue");
            _acceptanceRangeBottom = configuration.GetValue<int>("AcceptanceRangeBottom");
            _acceptanceRangeTop = configuration.GetValue<int>("AcceptanceRangeTop");
        }

        /// <inheritdoc />
        public bool IsAmountRequestedValid(int amountRequested)
        {
            try
            {
                bool isDivisible = IsDivisible(amountRequested);
                bool isWithinRange = IsWithinRange(amountRequested);
                return isDivisible && isWithinRange;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not validate conditions for amount requested.", amountRequested, ex);
                throw;
            }
        }

        /// <inheritdoc />
        public bool IsDivisible(int amountRequested)
        {
            try
            {
                return amountRequested % _divisibleValue == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not verify divisible.", amountRequested, _divisibleValue, ex);
                throw;
            }
        }

        /// <inheritdoc />
        public bool IsWithinRange(int amountRequested)
        {
            try
            {
                return amountRequested >= _acceptanceRangeBottom && amountRequested <= _acceptanceRangeTop;
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not verify range.", amountRequested, _acceptanceRangeBottom, _acceptanceRangeTop, ex);
                throw;
            }
        }
    }
}
