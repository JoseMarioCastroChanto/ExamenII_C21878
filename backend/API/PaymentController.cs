using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Domain;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private  PaymentCommand _command;
        private PaymentQuery _query;


        public PaymentController(PaymentCommand command, PaymentQuery query)
        {
            _command = command;
            _query = query;
        }

        [HttpGet]
        public  bool OutOfChange()
        {
            var isOutOfChange = _query.OutOfChange();
            return isOutOfChange;
        }

        [HttpPut]
        public IActionResult PaymentTransaction([FromBody] PaymentModel payment)
        {
            try
            {
                var cashChange = _command.PaymentTransaction(payment);
                return Ok(cashChange); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
