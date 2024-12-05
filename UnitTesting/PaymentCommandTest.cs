using Moq;
using backend.Application;
using backend.Domain;
using backend.Infrastructure;

namespace backend.Tests
{
    public class PaymentCommandTests
    {
        private readonly Mock<IPaymentQuery> _mockPaymentQuery;
        private readonly Mock<ICoffeeCommand> _mockCoffeeCommand;
        private readonly Mock<IPaymentHandler> _mockPaymentHandler;
        private readonly PaymentCommand _paymentCommand;

       
        private const string OutOfStockError = "Error: No hay suficiente stock de café para completar la orden.";
        private const string OutOfChangeError = "Fallo al realizar la compra.";
        private const int CoffeeId = 1;
        private const int CoffeeQuantity = 2;
        private static readonly int[] DefaultPaymentWay = new int[] { 10, 20, 0, 0, 0 };
        private const int CashChangeRequired = 100;
        private const int CoffeeQuantitySingle = 1;
        private const int MaxAvailableChange1000 = 2;
        private const int MaxAvailableChange500 = 3;
        private const int MaxAvailableChange100 = 5;

  
        private static readonly int AvailableChange1000 = 1000;
        private static readonly int AvailableChange500 = 500;
        private static readonly int AvailableChange100 = 100;

        private static readonly Dictionary<int, int> ExistingChange = new Dictionary<int, int>
        {
            { AvailableChange1000, MaxAvailableChange1000 },
            { AvailableChange500, MaxAvailableChange500 },
            { AvailableChange100, MaxAvailableChange100 }
        };

        public PaymentCommandTests()
        {
            _mockPaymentQuery = new Mock<IPaymentQuery>();
            _mockCoffeeCommand = new Mock<ICoffeeCommand>();
            _mockPaymentHandler = new Mock<IPaymentHandler>();
            _paymentCommand = new PaymentCommand(_mockPaymentQuery.Object, _mockCoffeeCommand.Object, _mockPaymentHandler.Object);
        }

        [Test]
        public void PaymentTransaction_ShouldReturnChange_WhenPaymentIsValid()
        {
            // Arrange
            var payment = new PaymentModel
            {
                CashChange = CashChangeRequired,
                CoffeeId = new[] { CoffeeId },
                CoffeeQuantity = new[] { CoffeeQuantity },
                PaymentWay = DefaultPaymentWay
            };

            _mockPaymentQuery.Setup(q => q.GetExistingChange()).Returns(ExistingChange);
            _mockCoffeeCommand.Setup(c => c.UpdateCoffeeAvailability(It.IsAny<int[]>(), It.IsAny<int[]>())).Returns(true);

            // Act
            var result = _paymentCommand.PaymentTransaction(payment);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.AreEqual(CashChangeRequired, result.MoneyValue[0]);
            Assert.AreEqual(1, result.Quantity[0]);
        }

        [Test]
        public void PaymentTransaction_ShouldThrowException_WhenOutOfChange()
        {
            // Arrange
            var payment = new PaymentModel
            {
                CashChange = AvailableChange1000,
                CoffeeId = new[] { CoffeeId },
                CoffeeQuantity = new[] { CoffeeQuantitySingle },
                PaymentWay = new int[] { 0, 0, 0, 0, 0 }
            };

            var noChangeAvailable = new Dictionary<int, int>
            {
                { AvailableChange1000, 0 },
                { AvailableChange500, 0 },
                { AvailableChange100, 0 }
            };

            _mockPaymentQuery.Setup(q => q.GetExistingChange()).Returns(noChangeAvailable);
            _mockCoffeeCommand.Setup(c => c.UpdateCoffeeAvailability(It.IsAny<int[]>(), It.IsAny<int[]>())).Returns(true);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _paymentCommand.PaymentTransaction(payment));
            Assert.AreEqual(OutOfChangeError, exception.Message);
        }

        [Test]
        public void PaymentTransaction_ShouldThrowException_WhenOutOfStock()
        {
            // Arrange
            var payment = new PaymentModel
            {
                CashChange = 500,
                CoffeeId = new[] { CoffeeId },
                CoffeeQuantity = new[] { 2 },
                PaymentWay = DefaultPaymentWay
            };

            _mockPaymentQuery.Setup(q => q.GetExistingChange()).Returns(ExistingChange);
            _mockCoffeeCommand.Setup(c => c.UpdateCoffeeAvailability(It.IsAny<int[]>(), It.IsAny<int[]>())).Returns(false);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _paymentCommand.PaymentTransaction(payment));
            Assert.AreEqual(OutOfStockError, exception.Message);
        }

        [Test]
        public void PaymentTransaction_ShouldReturnNoChange_WhenNoChangeIsRequired()
        {
            // Arrange
            var payment = new PaymentModel
            {
                CashChange = 0,
                CoffeeId = new[] { CoffeeId },
                CoffeeQuantity = new[] { CoffeeQuantitySingle },
                PaymentWay = new int[] { 1000, 0, 0, 0, 0 }
            };

            _mockPaymentQuery.Setup(q => q.GetExistingChange()).Returns(ExistingChange);
            _mockCoffeeCommand.Setup(c => c.UpdateCoffeeAvailability(It.IsAny<int[]>(), It.IsAny<int[]>())).Returns(true);

            // Act
            var result = _paymentCommand.PaymentTransaction(payment);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.AreEqual(0, result.MoneyValue[0]);
            Assert.AreEqual(0, result.Quantity[0]);
        }
    }
}