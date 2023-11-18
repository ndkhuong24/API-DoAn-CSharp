namespace API.ViewModel
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public double Value { get; set; }
        public int MaximumValue { get; set; }
        public int ConditionValue { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
