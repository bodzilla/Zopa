using System;

namespace Zopa.Core.Common.Exceptions
{
    [Serializable]
    public class AmountRequestedNotRaisedException : ApplicationException
    {
        public AmountRequestedNotRaisedException()
            : base("Could not raise enough funds from lenders.")
        {
        }
    }
}
