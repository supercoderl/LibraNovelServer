using LibraNovel.Application.ViewModels.Card;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface ICardService
    {
        Task<Response<List<CardResponse>>> GetCards(Guid userID);
        Task<Response<string>> CreateCard(CreateCardViewModel request);
    }
}
