using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int style_id { get; set; }
        public string description { get; set; }
        public DateTime create_date { get; set; }
        public int status { get; set; }
    }
}
