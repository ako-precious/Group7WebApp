using Microsoft.EntityFrameworkCore;

namespace Group7WebApp.Models
{
    [Keyless]
    public class PostCategory
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

}
