
using backend.Infrastructure;

namespace backend.Application
{
    public class PaymentQuery
    {
        private readonly PaymentHandler _paymentHandler;

        public PaymentQuery(PaymentHandler paymentHandler)
        {
            _paymentHandler = paymentHandler;
        }
        public bool OutOfChange()
        {
            return _paymentHandler.OutOfChange();
        }
        public Dictionary<int, int> GetExistingChange()
        {
            return _paymentHandler.GetExistingChange();
        }
    }
}
