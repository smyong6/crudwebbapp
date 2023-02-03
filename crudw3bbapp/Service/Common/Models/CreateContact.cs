﻿namespace Service.Common.Models;

public class CreateContact
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Company { get; set; }
}
