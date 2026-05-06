//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AppDAL.Context;
//using AppDAL.Entities;
//using AppDAL.IRepo;
//using AppDAL.Repo;

//namespace AppBL.Helpers
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly HotelDbContext _context;

//        public IGenericRepository<Chalet> Chalets { get; }
//        public IGenericRepository<Booking> Bookings { get; }

//        public UnitOfWork(HotelDbContext context)
//        {
//            _context = context;

//            Chalets = new GenericRepository<Chalet>(_context);
//            Bookings = new GenericRepository<Booking>(_context);
//        }

//        public async Task<int> CompleteAsync()
//            => await _context.SaveChangesAsync();
//    }
//}
