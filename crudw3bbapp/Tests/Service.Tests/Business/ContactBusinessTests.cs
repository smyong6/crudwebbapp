using Domain.Entities;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Service.Business;
using Service.Common.Models;

namespace Service.Tests.Business;

[TestClass]
public class ContactBusinessTests : TestBase
{
    private readonly Guid _testContactId = Guid.NewGuid();

    private readonly ContactDbContext _contactDbContext;
    private readonly ContactBusiness _sut;

    private readonly Mock<ILogger<ContactBusiness>> _mockLogger = new ();

    public ContactBusinessTests()
    {
        _contactDbContext = _serviceProvider.GetRequiredService<ContactDbContext>();

        _sut = new ContactBusiness(_contactDbContext, _mockLogger.Object);
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _contactDbContext.Database.EnsureDeletedAsync();
    }

    [TestMethod]
    public async Task GetContacts_ShouldReturnContacts_WhenContactsExist()
    {
        await _contactDbContext.Contacts.AddAsync(new Contact
        {
            Id = _testContactId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        });

        await _contactDbContext.SaveChangesAsync();

        var result = await _sut.GetContacts();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result?.Contacts?.Count);
        Assert.AreEqual(_testContactId, result?.Contacts?.First().Id);
    }

    [TestMethod]
    public async Task GetContacts_ShouldReturnError_WhenNoContactsExist()
    {
        var result = await _sut.GetContacts();

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual("No contacts found.", result.Error);
    }

    [TestMethod]
    public async Task CreateContact_ShouldAddContact_WhenContactDoesNotExist()
    {
        var newContact = new CreateContact()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        };

        var result = await _sut.CreateContact(newContact);

        var contacts = _contactDbContext.Contacts.ToList();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, contacts.Count);
        Assert.AreEqual(newContact.FirstName, contacts.First().FirstName);
    }

    [TestMethod]
    public async Task CreateContact_ShouldReturnError_WhenAContactWithSameNameExists()
    {
        await _contactDbContext.Contacts.AddAsync(new Contact
        {
            Id = _testContactId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        });

        var newContact = new CreateContact()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@email.com",
            PhoneNumber = "07123456711",
            Company = "Test"
        };

        await _contactDbContext.SaveChangesAsync();

        var result = await _sut.CreateContact(newContact);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual($"Contact {newContact.FirstName} {newContact.LastName} already exists.", result.Error);
    }

    [TestMethod]
    public async Task UpdateContact_ShouldAddUpdatedContact_WhenContactExists()
    {
        var existingContact = new Contact()
        {
            Id = _testContactId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        }; 

        await _contactDbContext.Contacts.AddAsync(existingContact);

        await _contactDbContext.SaveChangesAsync();

        var updatedContact = new CreateContact()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "newemail@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        };

        var result = await _sut.UpdateContact(_testContactId, updatedContact);

        var contacts = _contactDbContext.Contacts.ToList();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(updatedContact.Email, contacts.First().Email);
    }

    [TestMethod]
    public async Task UpdateContact_ShouldReturnError_WhenContactDoesNotExist()
    {
        await _contactDbContext.Contacts.AddAsync(new Contact
        {
            Id = _testContactId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        });

        await _contactDbContext.SaveChangesAsync();

        var updatedContact = new CreateContact()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "newemail@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        };

        var nonExistingId = Guid.NewGuid();

        var result = await _sut.UpdateContact(nonExistingId, updatedContact);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual($"Contact with Id:{nonExistingId} not found.", result.Error);
    }

    [TestMethod]
    public async Task DeleteContact_ShouldRemoveContact_WhenContactExists()
    {
        var existingContact = new Contact()
        {
            Id = _testContactId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        };

        await _contactDbContext.Contacts.AddAsync(existingContact);

        await _contactDbContext.SaveChangesAsync();

        var result = await _sut.DeleteContact(_testContactId);

        var contacts = _contactDbContext.Contacts.ToList();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(0, contacts.Count);
    }

    [TestMethod]
    public async Task DeleteContact_ShouldReturnError_WhenContactDoesNotExist()
    {
        await _contactDbContext.Contacts.AddAsync(new Contact
        {
            Id = _testContactId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            PhoneNumber = "07123456789",
            Company = "ACME"
        });

        await _contactDbContext.SaveChangesAsync();

        var nonExistingId = Guid.NewGuid();

        var result = await _sut.DeleteContact(nonExistingId);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.AreEqual($"Contact with Id:{nonExistingId} not found.", result.Error);
    }
}