namespace ProniaApplication.Models
{
    public class ProductsImage:BaseEntity
    {
        public string ImageURL { get; set; }
        public bool? IsPrimary { get; set; }

        //relational
        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
