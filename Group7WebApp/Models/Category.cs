using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Group7WebApp.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Category Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Category Code")]
        public string Code { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;

        public ICollection<Post>? Posts { get; set; }



    }
}
