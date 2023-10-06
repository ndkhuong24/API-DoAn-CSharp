namespace API.ViewModel
{
    public class ProductAndImagesDto
    {
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public int ColorID { get; set; }
        public int SoleID { get; set; }
        public int MaterialID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public List<ImageDto> Images { get; set; }
    }
}
