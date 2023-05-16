using Intro_Task.Entities;

namespace Intro_Task.Model
{
    public class DisplayDetailsViewModel
    {
        public Meal Meal { get; set; }

        public DisplayDetailsViewModel()
        {
            Meal = new Meal();
        }
    }
}
