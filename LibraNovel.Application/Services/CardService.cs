using AutoMapper;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Card;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class CardService : ICardService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public CardService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateCard(CreateCardViewModel request)
        {
            var card = _mapper.Map<Card>(request);
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo thẻ thành công", null);
        }

        public async Task<Response<List<CardResponse>>> GetCards(Guid userID)
        {
            var cards = await _context.Cards.AsNoTracking().Where(c => c.UserID == userID).ToListAsync();
            return new Response<List<CardResponse>>
            {
                Succeeded = true,
                Data = cards.Select(c => _mapper.Map<CardResponse>(c)).ToList()
            };
        }
    }
}
