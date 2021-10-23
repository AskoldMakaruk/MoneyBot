using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB;
using MoneyBot.DB.Model;

namespace MoneyBot.Services
{
    public class PeopleRepository
    {
        private readonly TelegramContext _context;
        private readonly Account _account;

        public PeopleRepository(TelegramContext context, Account account)
        {
            _context = context;
            _account = account;
        }

        public async Task AddPerson(Person person)
        {
            _account.People.Add(person);
            await _context.SaveChangesAsync();
        }

        // public async Task AddPeople(IEnumerable<Person> people)
        // {
        //     await _context.People.AddRangeAsync(people);
        //     await _context.SaveChangesAsync();
        // }

        public async Task<bool> AliasExists(string alias)
        {
            return await _context.People.AnyAsync(a => a.Alias == alias && a.Account == _account); // todo to id
        }
    }
}