using backend.Application;
using backend.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;

public class CoffeeCommandTests
{
    private CoffeeCommand _coffeeCommand;
    private Mock<ICoffeeQuery> _mockCoffeeQuery;
    private Mock<ICoffeeHandler> _mockCoffeeHandler;

    private const string ValidParametersMessage = "Los identificadores de café y las cantidades deben ser válidos.";
    private const string EnoughStockMessage = "No hay suficiente stock de café para la cantidad solicitada.";


    private const int InvalidCoffeeId1 = -1;
    private const int CoffeeId1 = 1;
    private const int CoffeeId2 = 2;

    private const int InvalidQuantity = -5;
    private const int ValidQuantity1 = 5;
    private const int ValidQuantity2 = 10;
    private const int AvailableStock1 = 10;
    private const int AvailableStock2 = 10;
    private const int InsufficientStock1 = 3;
    private const int InsufficientStock2 = 5;

    public CoffeeCommandTests()
    {
        _mockCoffeeQuery = new Mock<ICoffeeQuery>();
        _mockCoffeeHandler = new Mock<ICoffeeHandler>();
        _coffeeCommand = new CoffeeCommand(_mockCoffeeQuery.Object, _mockCoffeeHandler.Object);
    }

    [Test]
    public void UpdateCoffeeAvailability_WithInvalidCoffeeIds_ThrowsArgumentException()
    {
        // Arrange
        int[] invalidCoffeeIds = { InvalidCoffeeId1, CoffeeId2 };
        int[] validQuantities = { ValidQuantity1, ValidQuantity2 };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _coffeeCommand.UpdateCoffeeAvailability(invalidCoffeeIds, validQuantities));
        Assert.AreEqual(ValidParametersMessage, exception.Message);
    }

    [Test]
    public void UpdateCoffeeAvailability_WithInvalidQuantities_ThrowsArgumentException()
    {
        // Arrange
        int[] validCoffeeIds = { CoffeeId1, CoffeeId2 };
        int[] invalidQuantities = { InvalidQuantity, ValidQuantity2 };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _coffeeCommand.UpdateCoffeeAvailability(validCoffeeIds, invalidQuantities));
        Assert.AreEqual(ValidParametersMessage, exception.Message);
    }

    [Test]
    public void UpdateCoffeeAvailability_WithInsufficientStock_ThrowsInvalidOperationException()
    {
        // Arrange
        int[] coffeeIds = { CoffeeId1, CoffeeId2 };
        int[] quantities = { ValidQuantity1, ValidQuantity2 };

        var availableCoffees = new List<(int CoffeeId, int Available)>
        {
            (CoffeeId1, InsufficientStock1),
            (CoffeeId2, InsufficientStock2)
        };

        _mockCoffeeQuery.Setup(q => q.CheckCoffeeExistence(coffeeIds)).Returns(availableCoffees);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _coffeeCommand.UpdateCoffeeAvailability(coffeeIds, quantities));
        Assert.AreEqual(EnoughStockMessage, exception.Message);
    }

    [Test]
    public void UpdateCoffeeAvailability_WithValidParameters_UpdatesAvailability()
    {
        // Arrange
        int[] coffeeIds = { CoffeeId1, CoffeeId2 };
        int[] quantities = { ValidQuantity1, ValidQuantity2 };

        var availableCoffees = new List<(int CoffeeId, int Available)>
        {
            (CoffeeId1, AvailableStock1),
            (CoffeeId2, AvailableStock2)
        };

        _mockCoffeeQuery.Setup(q => q.CheckCoffeeExistence(coffeeIds)).Returns(availableCoffees);
        _mockCoffeeHandler.Setup(h => h.UpdateCoffeeAvailability(coffeeIds, quantities)).Verifiable();

        // Act
        bool result = _coffeeCommand.UpdateCoffeeAvailability(coffeeIds, quantities);

        // Assert
        Assert.True(result);
        _mockCoffeeHandler.Verify(h => h.UpdateCoffeeAvailability(coffeeIds, quantities), Times.Once);
    }
}