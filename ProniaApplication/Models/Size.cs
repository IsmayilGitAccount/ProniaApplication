namespace ProniaApplication.Models
{
    public class Size:BaseEntity
    {
        public string SizeName { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
    }
}
