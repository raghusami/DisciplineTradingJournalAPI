using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DisciplineTradingJournalAPI.Helper;
using Microsoft.Extensions.Options;
using System.Text;
using System;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class TradingUsersRepository : ITradingUsersRepository
    {
        private readonly TradingJournalDbContext _context;
        private readonly IOptionsSnapshot<AppConfiguration> appConfiguration;
        private readonly string _aes_EncryptionKey;
        public TradingUsersRepository(IOptionsSnapshot<AppConfiguration> _AppConfiguration, TradingJournalDbContext context)
        {
            _context = context;
            _aes_EncryptionKey = _AppConfiguration.Value.ApplicationConfiguration?.AESEncryptionDecryptionKey ?? string.Empty;

        }
        public async Task<TradingUsers> AddAsync(TradingUsers tradingUser,string rawPassword)
        {
            
                byte[] keyIvValue = Encoding.UTF8.GetBytes(this._aes_EncryptionKey);
                tradingUser.PasswordHash = EncryptionDecryptionHelper.AESEncryption(rawPassword, keyIvValue, keyIvValue);
                await _context.TradingUsers.AddAsync(tradingUser);
                await _context.SaveChangesAsync();
                return tradingUser;
            
            
        }
        public async Task<TradingUsers> SignInAsync(string userName, string passWord)
        {
            byte[] keyIvValue = Encoding.UTF8.GetBytes(this._aes_EncryptionKey);
            string username = EncryptionDecryptionHelper.AESDecryption(Convert.FromBase64String(userName), keyIvValue, keyIvValue);
            string password = EncryptionDecryptionHelper.AESDecryption(Convert.FromBase64String(passWord), keyIvValue, keyIvValue);
            var user = await _context.TradingUsers.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (user.PasswordHash != passWord)
            {
                throw new Exception("Invalid password.");
            }
            return user;
        }
        public async Task<TradingUsers> GetByIdAsync(int userId)
        {
            return await _context.TradingUsers.FindAsync(userId);
        }

        public async Task<IEnumerable<TradingUsers>> GetAllAsync()
        {
            return await _context.TradingUsers.ToListAsync();
        }

        public async Task<TradingUsers> UpdateAsync(TradingUsers tradingUser)
        {
            _context.TradingUsers.Update(tradingUser);
            await _context.SaveChangesAsync();
            return tradingUser;
        }
        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _context.TradingUsers.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            _context.TradingUsers.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
