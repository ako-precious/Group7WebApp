using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Group7WebApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Category Code")]
        public string Code { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
    }
}
