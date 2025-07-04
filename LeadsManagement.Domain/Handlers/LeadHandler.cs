using LeadsManagement.Domain.Commands;
using LeadsManagement.Domain.Commands.Lead;
using LeadsManagement.Domain.Entities;
using LeadsManagement.Domain.Entities.Contracts;
using LeadsManagement.Domain.Enums;
using LeadsManagement.Domain.Exceptions;
using LeadsManagement.Domain.Handlers.Contracts;
using LeadsManagement.Domain.Service;
using LeadsManagement.Domain.Repositories;

namespace LeadsManagement.Domain.Handlers;

public class LeadHandler:IHandler<CreateLeadCommand>
{
    private readonly ILeadRepository  _repository;
    private readonly IEmailService _emailService;

    public LeadHandler(ILeadRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }
    
    public async Task<ICommandResult> HandleAsync(CreateLeadCommand command)
    {
        try
        {
            command.Validate();
        }
        catch (InvalidLeadExceptions ex)
        {
            return new GenericCommandResult(false, ex.Message, string.Empty);
        }

        var lead = new Lead(command.FirstName, command.LastName, command.Email, command.PhoneNumber, command.Description, command.Suburb, command.Price);
        await _repository.CreateAsync(lead);
        return new GenericCommandResult(true, "Lead criado com sucesso", lead);
    }

    public async Task<ICommandResult> HandleAsync(UpdateLeadCommand command)
    {
        try
        {
            command.Validate();
        }
        catch (Exception ex)
        {
            return new GenericCommandResult(false, ex.Message, string.Empty);
        }

        var lead = await _repository.GetByIdAsync(command.Id!.Value);
        
        if (lead == null)
        {
            return new GenericCommandResult(false, "Lead não encontrado", string.Empty);
        }

        if(lead.Category != ECategoryType.Waiting)   
        {
            return new GenericCommandResult(false, "Lead já foi aceito ou recusado", lead);
        }

        if (command.Category == ECategoryType.Accepted && lead.Price > 500)
        {
            lead.ApplyDiscount(0.10m);
        }


        lead.UpdateCategory(command.Category);

        await _repository.UpdateAsync(lead);
        _emailService.SendEmail(lead.Email, "Lead atualizado", "Seu lead foi atualizado com sucesso");
        
        return new GenericCommandResult(true, "Lead atualizado com sucesso", lead);
    }
}