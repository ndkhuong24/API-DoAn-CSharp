namespace API.ViewModel
{
    public class UserAddress
    {
        public int? AddressID { get; set; }
        public string ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string CommuneID { get; set; }
        public string CommuneName { get; set; }
        public string DetailAddress { get; set; }
        public int? Status { get; set; }
    }
}
