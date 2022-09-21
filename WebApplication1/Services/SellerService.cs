using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Exceptions;

namespace WebApplication1.Services {
    public class SellerService {
        private readonly WebApplication1Context _context;

        public SellerService(WebApplication1Context context) {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync() {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller) {
            _context.Add(seller);
            _context.SaveChangesAsync();
        }
        public async Task<Seller> FindByIdAsync(int id) {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }
        public async Task RemoveAsync(int id) {
            try {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                throw new IntegrityException(ex.Message);
            }
        }
        public async Task UpdateAsync(Seller seller) {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
            if (!hasAny) {
                throw new NotFoundException("Id not found");
            }
            try {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException ex) {
                throw new DbConcurrencyException(ex.Message);
            }
        }
    }
}
