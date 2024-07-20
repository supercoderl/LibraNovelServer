using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Card
{
    public class CardResponse
    {
        public int CardID { get; set; }
        public Guid UserID { get; set; }
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }  
        public string CardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV {  get; set; }
        public string Status { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
