using Group7WebApp.Areas.Identity.Data;

namespace Group7WebApp.Models
{
    public class PostInformation
    {
        internal Task<WebAppUser?> user;

        public Post Post { get; set; }
        public Category Category { get; set; }
        public WebAppUser WebAppUser { get; set; }
    }
}
