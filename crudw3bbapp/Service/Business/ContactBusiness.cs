using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Common.Exceptions;
using Service.Common.Interfaces;
using Service.Common.Models;

namespace Service.Business;

public class ContactBusiness : IContactBusiness
{
    private const string GetError = "No contacts found.";
    private const string AlreadyExistsError = "Contact {firstName} {lastName} already exists.";
    private const string NotFoundError = "Contact with Id:{id} not found.";

    private readonly IContactDbContext _contactDbContext;
    private readonly ILogger<ContactBusiness> _logger;

    public ContactBusiness(IContactDbContext contactDbContext,
        ILogger<ContactBusiness> logger)
    {
        _contactDbContext = contactDbContext;
        _logger = logger;
    }

    public async Task<GetResult> GetContacts()
    {
        var contacts = await _contactDbContext.Contacts.ToListAsync();

        if (!contacts.Any())
        {
            _logger.LogError(GetError);

            return new GetResult()
            {
                Success = false,
                Error = GetError,
            };
        }

        return new GetResult()
        {
            Success = true,
            Contacts = contacts
        };
    }

    public async Task<ContactResult> CreateContact(CreateContact contact)
    {
        var existingContact = await _contactDbContext.Contacts.FirstOrDefaultAsync(c => c.FirstName == contact.FirstName && c.LastName == contact.LastName);

        if (existingContact != null)
        {
            _logger.LogError(AlreadyExistsError, contact.FirstName, contact.LastName);

            return new ContactResult()
            {
                Success = false,
                Error = $"Contact {contact.FirstName} {contact.LastName} already exists.",
            };
        }

        var newContact = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = contact.FirstName,
            LastName = contact?.LastName,
            Email = contact?.Email,
            PhoneNumber = contact?.PhoneNumber,
            Company = contact?.Company
        };

        await _contactDbContext.Contacts.AddAsync(newContact);

        await _contactDbContext.SaveChangesAsync(default);

        return new ContactResult()
        {
            Success = true,
            Contact = newContact
        };
    }

    public async Task<ContactResult> UpdateContact(Guid id, CreateContact contact)
    {

        var existingContact = await _contactDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);

        if (existingContact == null)
        {
            _logger.LogError(NotFoundError, id);

            return new ContactResult()
            {
                Success = false,
                Error = $"Contact with Id:{id} not found.",
            };
        }

        existingContact.FirstName = contact.FirstName;
        existingContact.LastName = contact?.LastName;
        existingContact.Email = contact?.Email;
        existingContact.PhoneNumber = contact?.PhoneNumber;
        existingContact.Company = contact?.Company;

        _contactDbContext.Contacts.Update(existingContact);

        await _contactDbContext.SaveChangesAsync(default);

        return new ContactResult()
        {
            Success = true,
            Contact = existingContact
        };
    }

    public async Task<DeleteResult> DeleteContact(Guid id)
    {

        var existingContact = await _contactDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);

        if (existingContact == null)
        {
            _logger.LogError(NotFoundError, id);

            return new DeleteResult()
            {
                Success = false,
                Error = $"Contact with Id:{id} not found.",
            };
        }

        _contactDbContext.Contacts.Remove(existingContact);

        await _contactDbContext.SaveChangesAsync(default);

        return new DeleteResult()
        {
            Success = true,
            Id = id,
        };
    }
}
