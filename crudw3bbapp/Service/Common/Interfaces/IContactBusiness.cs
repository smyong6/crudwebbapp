using Domain.Entities;
using Service.Common.Models;

namespace Service.Common.Interfaces
{
    public interface IContactBusiness
    {
        Task<ContactResult> CreateContact(CreateContact contact);
        Task<DeleteResult> DeleteContact(Guid id);
        Task<GetResult> GetContacts();
        Task<ContactResult> UpdateContact(Guid id, CreateContact contact);
    }
}