namespace trying.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Rating { get; set; }
        public int Price { get; set; }
        public int DiscountPercentage { get; set; }
        public string Stock { get; set; }
        public string Brand { get; set; }
        public string Thumbnail { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}