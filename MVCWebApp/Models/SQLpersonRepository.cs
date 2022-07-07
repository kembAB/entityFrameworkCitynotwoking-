using MVCWebApp.Models.Person;
using MVCWebApp.Models.Person.ViewModels;
using MVCWebApp.Data;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.SharePoint.Client.UserProfiles;

namespace MVCWebApp.Models
{
    public class SQLpersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext context;
        //ctor injects DBContext
        public SQLpersonRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Person.Person> GetAllPersons()
        {
            context.Countries.ToList();
            return context.People.ToList();
        }

        public Person.Person GetPerson(int id)
        {
            return context.People.Find(id);
        }

        public List<Person.Person> Search(string searchTerm, bool caseSensitive)
        {
            List<Person.Person> searchList = new List<Person.Person>();

            if (searchTerm != null)
            {
                if (caseSensitive)
                {
                    IEnumerable<Person.Person> searchList2 = (from Person in context.People
                                                                 where Person.Name.Contains(searchTerm) || Person.City.CityName.Contains(searchTerm)
                                                                 select Person)
                                                      .ToList();


                    foreach (Person.Person item in searchList2)
                    {
                        if (item.Name.Contains(searchTerm) || item.City.CityName.Contains(searchTerm))
                        {
                            searchList.Add(item);
                        }
                    }
                }
                else
                {

                    searchList = context.People.Where(p => p.City.CityName.Contains(searchTerm) ||
                                                    p.Name.Contains(searchTerm)).ToList();
                }
            }

            return searchList;
        }

        public List<Person.Person> Sort(SortOptionsViewModel sortOptions, string sortType)
        {

            List<Person.Person> sortedList = context.People.ToList();

            if (sortType == "city")
            {
                sortedList = context.People.OrderBy(p => p.City).ToList();
            }
            else if (sortType == "name")
            {
                sortedList = context.People.OrderBy(p => p.Name).ToList();
            }

            if (sortOptions.ReverseAplhabeticalOrder == true)
            {
                sortedList.Reverse();
            }

            return sortedList;
        }

        public Person.Person Add(CreatePersonViewModel createPersonViewModel)
        {
            Person.Person person = new Person.Person();
            person.Name = createPersonViewModel.Name;
            person.PhoneNumber = createPersonViewModel.PhoneNumber;
            City.City city = context.Cities.Find(createPersonViewModel.City);
            person.City = city;

            city.People.Add(person);

            context.Update(city);
            context.People.Add(person);
            context.SaveChanges();

            return person;
        }

        public bool Delete(int id)
        {
            if (id > 0)
            {
                var personToDelete = context.People.Find(id);

                if (personToDelete != null)
                {
                    context.People.Remove(personToDelete);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}

    

