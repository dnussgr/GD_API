namespace GD_API.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string ClaimNumber { get; set; }
        public string OwnerName { get; set; }
        public string Location { get; set; }
        public decimal GoldAmount { get; set; }
        public DateTime DateClaimed { get; set; }
    }
}
