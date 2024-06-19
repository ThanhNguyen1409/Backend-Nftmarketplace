namespace NFTMarketPlace_Backend.DTO
{
    public class OfferDTO
    {

        public int NftId { get; set; }

        public string Bidder { get; set; }

        public float Amount { get; set; }

        public long Duration { get; set; }
    }
}
