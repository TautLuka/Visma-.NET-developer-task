using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask
{
    public enum CategoriesEnum
    {
        CodeMonkey,
        Hub,
        Short,
        TeamBuilding
    }

    public enum TypesEnum
    {
        Live,
        InPerson
    }

    public class Meeting
    {
        public Meeting(string name, string responsiblePerson, string description, int category, int type, DateTime startDate, DateTime endDate)
        {
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = (CategoriesEnum)category;
            Type = (TypesEnum)type;
            StartDate = startDate;
            EndDate = endDate;
            People = new List<Person>();
        }

        public string Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public CategoriesEnum Category { get; set; }
        public TypesEnum Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Person> People { get; set; }

    }
}
