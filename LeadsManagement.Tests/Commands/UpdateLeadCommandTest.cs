using LeadsManagement.Domain.Commands.Lead;
using LeadsManagement.Domain.Enums;

namespace LeadsManagement.Tests.Commands;

[TestClass]
public class UpdateLeadCommandTest
{
    [TestMethod]
    [TestCategory("Commands")]
    public void UpdateLeadCommand_Should_Return_Success_When_Category_Is_Accepted()
    {
        var command = new UpdateLeadCommand
        {
            Id = Guid.NewGuid(),
            Category = ECategoryType.Accepted
        };

        try
        {
            command.Validate();
            Assert.IsTrue(true);
        }
        catch (Exception)
        {
            Assert.Fail("Não deveria lançar exceção para dados válidos");
        }
    }

    [TestMethod]
    [TestCategory("Commands")]
    public void UpdateLeadCommand_Should_Throw_Exception_When_Id_Is_Empty()
    {
        var command = new UpdateLeadCommand {
            Id = Guid.Empty,
            Category = ECategoryType.Waiting
        };

        try
        {
            command.Validate();
            Assert.Fail("Deveria lançar exceção para dados inválidos");
        }
        catch (Exception ex)
        {
            Assert.AreEqual("Id do lead não pode ser vazio", ex.Message);
        }
    }
}