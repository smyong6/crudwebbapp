using Domain.Entities;

namespace Service.Common.Models;

public class GetResult : BaseResult
{
    public List<Contact>? Contacts { get; set; }
}
