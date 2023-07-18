using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Group7WebApp.Models
{
    public class Post
    {
        
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
       
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        [Required]
        public string Author { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;

        public DateTime DateUpdate { get; set; } = DateTime.Now;

        public ICollection<Category> Categories { get; set;}
    }
}
