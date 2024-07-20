using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Card;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraNovel.WebAPI.Controllers
{
    public class CardController : BaseApiController
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet("/get-cards")]
        public async Task<IActionResult> GetCards()
        {
            var userID = User.FindFirstValue("UserID");
            if(userID == null)
            {
                return Unauthorized();
            }

            return (Ok(await _cardService.GetCards(Guid.Parse(userID))));
        }

        [HttpPost("/create-card")]
        public async Task<IActionResult> CreateCard(CreateCardViewModel request)
        {
            var userID = User.FindFirstValue("UserID");
            if (userID == null)
            {
                return Unauthorized();
            }

            request.UserID = Guid.Parse(userID);

            return Ok(await _cardService.CreateCard(request));
        }
    }
}
