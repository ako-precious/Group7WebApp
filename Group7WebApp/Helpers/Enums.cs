using System.ComponentModel.DataAnnotations;

namespace Group7WebApp.Helpers
{
    public enum Status
    {
        [Display(Name = "Pending")]
        Pending = 0,
        [Display(Name = "Approved")]
        Approved = 1,
        [Display(Name = "Rejected")]
        Rejected = 2

    }
}
