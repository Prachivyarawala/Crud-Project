using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace API.Models{
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
}