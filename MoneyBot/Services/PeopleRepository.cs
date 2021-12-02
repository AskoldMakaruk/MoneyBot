using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyBot.DB;
using MoneyBot.DB.Model;

namespace MoneyBot.Services
{
    public class PeopleRepository
    {
        private readonly TelegramContext _context;
        private readonly User _user;

        public PeopleRepository(TelegramContext context, User user)
        {
            _context = context;
            _user = user;
        }

        public async Task AddPerson(Person person)
        {
            _user.Frens.Add(person);
            await _context.SaveChangesAsync();
        }

        // public async Task AddPeople(IEnumerable<Person> people)
        // {
        //     await _context.People.AddRangeAsync(people);
        //     await _context.SaveChangesAsync();
        // }

        public async Task<bool> AliasExists(string alias)
        {
            return await _context.People.AnyAsync(a => a.Alias == alias && a.User == _user); // todo to id
        }
    }
}