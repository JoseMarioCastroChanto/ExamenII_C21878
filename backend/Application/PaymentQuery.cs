
using backend.Infrastructure;

namespace backend.Application
{
    public interface IPaymentQuery
    {
        bool OutOfChange();
        Dictionary<int, int> GetExistingChange();
    }

    public class PaymentQuery: IPaymentQuery
    {
        private readonly IPaymentHandler _paymentHandler;

        public PaymentQuery(IPaymentHandler paymentHandler)
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
