using Domain.Entities;

namespace Service.Common.Models;

public class ContactResult : BaseResult
{
    public Contact? Contact { get; set; }
}
