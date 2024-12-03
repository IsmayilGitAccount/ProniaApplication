using ProniaApplication.Models;

namespace ProniaApplication.ViewModels
{
    public class DetailsVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
