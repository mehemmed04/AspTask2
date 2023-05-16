using System.Collections.Generic;

namespace Intro_Task.Entities
{
    public class Meal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumb { get; set; }

        public string Category { get; set; }

        public string Region { get; set; }

        public string Instructions { get; set; }

        public string VideoId { get; set; }

        public string Tag { get; set; }
        public List<string> Ingredients { get; set; }

        public List<string> Measures { get; set; }


        public Meal()
        {
            Ingredients = new List<string>();
            Measures = new List<string>();
        }

    }
}
