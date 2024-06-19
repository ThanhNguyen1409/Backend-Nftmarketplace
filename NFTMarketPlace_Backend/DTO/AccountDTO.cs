namespace NFTMarketPlace_Backend.DTO
{
    public class AccountDTO
    {
        public string AccountAddress { get; set; }

        public string AccountName { get; set; }

        public string AccountEmail { get; set; }

        public IFormFile Avatar { get; set; }

        public IFormFile BannerImage { get; set; }
        public IFormFile BannerVideo { get; set; }
    }
}
