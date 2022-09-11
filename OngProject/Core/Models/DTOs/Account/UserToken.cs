using System;

namespace OngProject.Core.Models.DTOs.Account
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}