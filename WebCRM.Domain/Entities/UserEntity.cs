using System;
using Microsoft.AspNetCore.Identity;

namespace WebCRM.Domain.Entities;

public class UserEntity : IdentityUser<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginDate { get; set; }
    public long? MerchantId { get; set; }
    public MerchantEntity? Merchant { get; set; }
}