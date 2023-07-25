using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Group7WebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the WebAppUser class
public class WebAppUser : IdentityUser
{
    [ProtectedPersonalData]
    [Required]
    public string FirtName { get; set; }
    [ProtectedPersonalData]
    [Required]
    public string LastName { get; set; }

    [Required]
    public string Role { get; set; }
    public string Status { get; set; }
    public string? ContactId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    
}

