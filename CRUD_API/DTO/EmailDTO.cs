namespace CRUD_API.DTO
{
    public class EmailDTO
    {
        public int orderId { get; set; }
        public List<EmailData> products { get; set; }

        public string custormerName { get; set; }
    }

    public class EmailData
    {
        public string productName { get; set; }

        public decimal productPrice { get; set; }

        public string productDes { get; set; }
        public string ImageUrl { get; set; }
        // Các trường khác của sản phẩm có thể được thêm vào tùy thuộc vào yêu cầu của bạn
    }
}
