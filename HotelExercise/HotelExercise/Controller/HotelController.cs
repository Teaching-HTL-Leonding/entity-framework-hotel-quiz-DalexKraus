using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelExercise.Controller
{
    public class HotelController
    {
        private readonly HotelContextFactory _contextFactory;
        
        public HotelController(string[] args)
        {
            _contextFactory = new HotelContextFactory(args);
        }

        public async Task AddHotel(Hotel hotel)
        {
            var dbContext = _contextFactory.GetNewDbContext();
            await dbContext.Hotels.AddAsync(hotel);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Price> GetPrice(RoomType roomType)
        {
            var dbContext = _contextFactory.GetNewDbContext();
            return await dbContext.Prices.Include("RoomType").Where(p => p.Room.Equals(roomType)).FirstAsync();
        }
        
        public async Task AddPrices(List<Price> prices)
        {
            var dbContext = _contextFactory.GetNewDbContext();
            await dbContext.Prices.AddRangeAsync(prices);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Hotel>> GetAllHotels()
        {
            return await _contextFactory.GetNewDbContext().Hotels.ToListAsync();
        }
    }
}