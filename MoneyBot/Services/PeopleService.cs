using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyBot.DB.Model;

namespace MoneyBot.Services
{
    public class PeopleService
    {
        private readonly PeopleRepository _repository;
        private readonly Account _account;

        public PeopleService(PeopleRepository repository, Account account)
        {
            _repository = repository;
            _account = account;
        }

        public async Task<bool> AliasAvailable(string alias) => !await _repository.AliasExists(alias);

        public async Task<string> GetAlias(string name)
        {
            var alias = name.Length > 3
                ? name[0..3].ToUpper()
                : name.ToUpper();

            if (await AliasAvailable(alias))
            {
                return alias;
            }

            return null;
        }

        public async Task AddPerson(string name, string alias)
        {
            await _repository.AddPerson(new Person
            {
                Alias = alias,
                Name = name
            });
        }

        // public async Task AddPeople(string text)
        // {
        //     var people = ParsePeople(text);
        //
        //     await _repository.AddPeople(people);
        // }
        //
        //
        // public async Task OverridePeople(string text)
        // {
        //     var people = ParsePeople(text);
        // }

        private IEnumerable<Person> ParsePeople(string text)
        {
            var values = text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
            var people = values.Select(v => new Person()
            {
                Account = _account,
                Alias = v[0],
                Name = v[1]
            });
            return people;
        }
    }
}