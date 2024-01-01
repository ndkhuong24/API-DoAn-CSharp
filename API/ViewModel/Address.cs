namespace API.ViewModel
{
    public class Address
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string DetailAddress { get; set; }
        public string ProvinceID { get; set; }
        public string DistrictID { get; set; }
        public string CommuneID { get; set; }
        public int Status { get; set; }
    }
}
