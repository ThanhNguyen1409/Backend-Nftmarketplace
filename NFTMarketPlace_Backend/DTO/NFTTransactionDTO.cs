using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFTMarketPlace_Backend.DTO
{
    public class NFTTransactionDTO
    {


        public string NftAddress { get; set; }

        public int tokenId { get; set; }

        public string Event { get; set; }


        public string Price { get; set; }



        public string From { get; set; }


        public string To { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }
    }
}
