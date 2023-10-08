namespace API.ViewModel
{
    public class OrderDTO
    {
        public int CustomerID { get; set; }
        public int UserID { get; set; }
        public int TotalPrice { get; set; }
        public int TranSportFee { get; set; }
        public string Description { get; set; }
        public int VoucherID { get; set; }
        public int DiscountPrice { get; set; }
        public int FinalPrice { get; set; }
        public int Status { get; set; }
        public List<OrderDetailDTO> orderDetail { get; set; }
    }
}
