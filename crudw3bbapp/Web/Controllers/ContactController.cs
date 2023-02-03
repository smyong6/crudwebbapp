using Microsoft.AspNetCore.Mvc;
using Service.Common.Interfaces;
using Service.Common.Models;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactBusiness _contactBusiness;

    public ContactController(
        IContactBusiness contactBusiness)
    {
        _contactBusiness = contactBusiness;
    }

    [HttpGet(Name = "GetContacts")]
    public async Task<IActionResult> Get()
    {
        var result = await _contactBusiness.GetContacts();

        if(!result.Success)
            return NotFound(result?.Error);

        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateContact contact)
    {
        var result = await _contactBusiness.CreateContact(contact);

        if (!result.Success)
            return BadRequest(result?.Error);

        return Ok(result);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateContact contact)
    {
        var result = await _contactBusiness.UpdateContact(id, contact);

        if (!result.Success)
            return NotFound(result?.Error);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _contactBusiness.DeleteContact(id);

        if (!result.Success)
            return NotFound(result?.Error);

        return Ok(result);
    }
}