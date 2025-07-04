using LeadsManagement.Domain.Commands;
using LeadsManagement.Domain.Commands.Lead;
using LeadsManagement.Domain.Entities;
using LeadsManagement.Domain.Enums;
using LeadsManagement.Domain.Handlers;
using LeadsManagement.Domain.Repositories;
using LeadsManagement.Domain.Service;
using Moq;

namespace LeadsManagement.Tests.Handlers;

[TestClass]
public class LeadHandlerTest
{
    private Mock<ILeadRepository> _mockRepository;
    private Mock<IEmailService> _mockEmailService;
    private LeadHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ILeadRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _handler = new LeadHandler(_mockRepository.Object, _mockEmailService.Object);
    }

    #region CreateLeadCommand Tests

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task CreateLeadCommand_Should_Return_Success_When_Valid()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test lead",
            Suburb = "Test suburb",
            Price = 1000.00m
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Lead>()))
                      .Returns(Task.CompletedTask);

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsTrue(genericResult!.Success);
        Assert.AreEqual("Lead criado com sucesso", genericResult.Message);
        Assert.IsNotNull(genericResult.Data);
        Assert.IsInstanceOfType(genericResult.Data, typeof(Lead));

        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Lead>()), Times.Once);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task CreateLeadCommand_Should_Return_Failure_When_FirstName_Is_Empty()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test lead",
            Suburb = "Test suburb",
            Price = 1000.00m
        };

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsFalse(genericResult!.Success);
        Assert.AreEqual("O nome do lead não pode ser nulo", genericResult.Message);
        Assert.AreEqual(string.Empty, genericResult.Data);

        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Lead>()), Times.Never);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task CreateLeadCommand_Should_Return_Failure_When_Price_Is_Negative()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test lead",
            Suburb = "Test suburb",
            Price = -100.00m
        };

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsFalse(genericResult!.Success);
        Assert.AreEqual("O preço do lead não pode ser menor que 0", genericResult.Message);
        Assert.AreEqual(string.Empty, genericResult.Data);

        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Lead>()), Times.Never);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task CreateLeadCommand_Should_Return_Failure_When_Email_Is_Invalid()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            PhoneNumber = "1234567890",
            Description = "Test lead",
            Suburb = "Test suburb",
            Price = 1000.00m
        };

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsFalse(genericResult!.Success);
        Assert.AreEqual("O E-mail do lead não pode ser inválido", genericResult.Message);
        Assert.AreEqual(string.Empty, genericResult.Data);

        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Lead>()), Times.Never);
    }

    #endregion

    #region UpdateLeadCommand Tests

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task UpdateLeadCommand_Should_Return_Success_When_Valid()
    {
        var leadId = Guid.NewGuid();
        var command = new UpdateLeadCommand
        {
            Id = leadId,
            Category = ECategoryType.Accepted
        };

        var existingLead = new Lead("John", "Doe", "john.doe@example.com", "1234567890", "Test lead", "Test suburb", 300.00m);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
                      .ReturnsAsync(existingLead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
                      .Returns(Task.CompletedTask);

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsTrue(genericResult!.Success);
        Assert.AreEqual("Lead atualizado com sucesso", genericResult.Message);
        Assert.IsNotNull(genericResult.Data);
        Assert.IsInstanceOfType(genericResult.Data, typeof(Lead));

        _mockRepository.Verify(r => r.GetByIdAsync(leadId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Lead>()), Times.Once);
        _mockEmailService.Verify(e => e.SendEmail(
            It.Is<string>(email => email == "john.doe@example.com"),
            It.Is<string>(subject => subject == "Lead atualizado"),
            It.Is<string>(body => body == "Seu lead foi atualizado com sucesso")
        ), Times.Once);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task UpdateLeadCommand_Should_Apply_Discount_When_Accepted_And_Price_Over_500()
    {
        var leadId = Guid.NewGuid();
        var command = new UpdateLeadCommand
        {
            Id = leadId,
            Category = ECategoryType.Accepted
        };

        var existingLead = new Lead("John", "Doe", "john.doe@example.com", "1234567890", "Test lead", "Test suburb", 1000.00m);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
                      .ReturnsAsync(existingLead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
                      .Returns(Task.CompletedTask);

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsTrue(genericResult!.Success);
        var updatedLead = genericResult.Data as Lead;
        Assert.AreEqual(900.00m, updatedLead.Price);
        Assert.AreEqual(ECategoryType.Accepted, updatedLead.Category);

        _mockRepository.Verify(r => r.GetByIdAsync(leadId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Lead>()), Times.Once);
        _mockEmailService.Verify(e => e.SendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        ), Times.Once);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task UpdateLeadCommand_Should_Not_Apply_Discount_When_Accepted_And_Price_Under_500()
    {
        var leadId = Guid.NewGuid();
        var command = new UpdateLeadCommand
        {
            Id = leadId,
            Category = ECategoryType.Accepted
        };

        var existingLead = new Lead("John", "Doe", "john.doe@example.com", "1234567890", "Test lead", "Test suburb", 300.00m);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
                      .ReturnsAsync(existingLead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
                      .Returns(Task.CompletedTask);

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsTrue(genericResult!.Success);
        var updatedLead = genericResult.Data as Lead;
        Assert.AreEqual(300.00m, updatedLead.Price);
        Assert.AreEqual(ECategoryType.Accepted, updatedLead.Category);

        _mockRepository.Verify(r => r.GetByIdAsync(leadId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Lead>()), Times.Once);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task UpdateLeadCommand_Should_Not_Apply_Discount_When_Rejected()
    {
        var leadId = Guid.NewGuid();
        var command = new UpdateLeadCommand
        {
            Id = leadId,
            Category = ECategoryType.Rejected
        };

        var existingLead = new Lead("John", "Doe", "john.doe@example.com", "1234567890", "Test lead", "Test suburb", 1000.00m);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
                      .ReturnsAsync(existingLead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
                      .Returns(Task.CompletedTask);

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsTrue(genericResult!.Success);
        var updatedLead = genericResult.Data as Lead;
        Assert.AreEqual(1000.00m, updatedLead.Price);
        Assert.AreEqual(ECategoryType.Rejected, updatedLead.Category);

        _mockRepository.Verify(r => r.GetByIdAsync(leadId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Lead>()), Times.Once);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task UpdateLeadCommand_Should_Return_Failure_When_Lead_Not_Found()
    {
        var leadId = Guid.NewGuid();
        var command = new UpdateLeadCommand
        {
            Id = leadId,
            Category = ECategoryType.Accepted
        };

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
                      .ReturnsAsync((Lead?)null);

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsFalse(genericResult!.Success);
        Assert.AreEqual("Lead não encontrado", genericResult.Message);
        Assert.AreEqual(string.Empty, genericResult.Data);

        _mockRepository.Verify(r => r.GetByIdAsync(leadId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Lead>()), Times.Never);
        _mockEmailService.Verify(e => e.SendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        ), Times.Never);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public async Task UpdateLeadCommand_Should_Return_Failure_When_Id_Is_Empty()
    {
        var command = new UpdateLeadCommand
        {
            Id = Guid.Empty,
            Category = ECategoryType.Accepted
        };

        var result = await _handler.HandleAsync(command);

        var genericResult = result as GenericCommandResult;
        Assert.IsFalse(genericResult!.Success);
        Assert.AreEqual("Id do lead não pode ser vazio", genericResult.Message);
        Assert.AreEqual(string.Empty, genericResult.Data);

        _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Lead>()), Times.Never);
    }

    #endregion
}