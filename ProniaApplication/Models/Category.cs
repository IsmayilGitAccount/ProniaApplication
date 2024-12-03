using System.ComponentModel.DataAnnotations;

namespace ProniaApplication.Models
{
    public class Category:BaseEntity
    {
        [MaxLength(30, ErrorMessage = "The name must have a maximum of 30 characters")]
        public string Name { get; set; }

        //relational
        public List<Product>? Products { get; set; }
    }
}
