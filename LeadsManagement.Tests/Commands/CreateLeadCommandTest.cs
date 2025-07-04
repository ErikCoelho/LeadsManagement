using LeadsManagement.Domain.Commands.Lead;
using LeadsManagement.Domain.Exceptions;

namespace LeadsManagement.Tests.Commands;

[TestClass]
public class CreateLeadCommandTest
{
    [TestMethod]
    [TestCategory("Commands")]
    public void CreateLeadCommand_Should_Pass_Validation_When_Valid()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John Doe",
            LastName = "Silva",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test description",
            Category = "Test category",
            Suburb = "Test suburb",
            Price = 1000.00m,
            DataCriacao = DateTime.Now
        };

        try
        {
            command.Validate();
            Assert.IsTrue(true);
        }
        catch (InvalidLeadExceptions)
        {
            Assert.Fail("Não deveria lançar exceção para dados válidos");
        }
    }

    [TestMethod]
    [TestCategory("Commands")]
    public void CreateLeadCommand_Should_Throw_Exception_When_FirstName_Is_Empty()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "",
            LastName = "Silva",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test description",
            Category = "Test category",
            Suburb = "Test suburb",
            Price = 1000.00m,
            DataCriacao = DateTime.Now
        };

        var exception = Assert.ThrowsException<InvalidLeadExceptions>(() => command.Validate());
        Assert.AreEqual("O nome do lead não pode ser nulo", exception.Message);
    }

    [TestMethod]
    [TestCategory("Commands")]
    public void CreateLeadCommand_Should_Throw_Exception_When_Price_Is_Negative()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John Doe",
            LastName = "Silva",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test description",
            Category = "Test category",
            Suburb = "Test suburb",
            Price = -100.00m,
            DataCriacao = DateTime.Now
        };

        var exception = Assert.ThrowsException<InvalidLeadExceptions>(() => command.Validate());
        Assert.AreEqual("O preço do lead não pode ser menor que 0", exception.Message);
    }

    [TestMethod]
    [TestCategory("Commands")]
    public void CreateLeadCommand_Should_Throw_Exception_When_Email_Is_Invalid()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John Doe",
            LastName = "Silva",
            Email = "email-invalido",
            PhoneNumber = "1234567890",
            Description = "Test description",
            Category = "Test category",
            Suburb = "Test suburb",
            Price = 1000.00m,
            DataCriacao = DateTime.Now
        };

        var exception = Assert.ThrowsException<InvalidLeadExceptions>(() => command.Validate());
        Assert.AreEqual("O E-mail do lead não pode ser inválido", exception.Message);
    }

    [TestMethod]
    [TestCategory("Commands")]
    public void CreateLeadCommand_Should_Pass_Validation_When_Email_Is_Valid()
    {
        var command = new CreateLeadCommand
        {
            FirstName = "John Doe",
            LastName = "Silva",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Description = "Test description",
            Category = "Test category",
            Suburb = "Test suburb",
            Price = 1000.00m,
            DataCriacao = DateTime.Now
        };

        try
        {
            command.Validate();
            Assert.IsTrue(true);
        }
        catch (InvalidLeadExceptions)
        {
            Assert.Fail("Não deveria lançar exceção para email válido");
        }
    }
}