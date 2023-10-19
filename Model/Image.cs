using System.ComponentModel.DataAnnotations.Schema;

namespace trying.Model
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
    }
}