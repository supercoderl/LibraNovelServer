using AutoMapper;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Transaction;
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
    public class TransactionService : ITransactionService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public TransactionService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<string>> StoreTransaction(CreateTransactionViewModel request)
        {
            var transaction = _mapper.Map<Transaction>(request);
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return new Response<string>("Lưu giao dịch thành công", null);
        }

        public async Task<Response<List<TransactionResponse>>> GetTransactions(int cardID)
        {
            var query = _context.Transactions.AsNoTracking();

            var transactions = await query.Where(t => t.CardID == cardID).ToListAsync();

            return new Response<List<TransactionResponse>>
            {
                Succeeded = true,
                Data = transactions.Select(t => _mapper.Map<TransactionResponse>(t)).ToList()
            };
        }
    }
}
