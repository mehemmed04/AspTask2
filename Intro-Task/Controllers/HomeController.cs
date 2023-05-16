using Intro_Task.Entities;
using Intro_Task.Helpers.FileHelper;
using Intro_Task.Model;
using Intro_Task.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_Task.Controllers
{
    public class HomeController : Controller
    {

        public DisplayMealViewModel VM { get; set; }

        public List<Meal> AddedMeals { get; set; }



        public HomeController()
        {
            AddedMeals = new List<Meal>();

            if (System.IO.File.Exists("categories"))
            {

                if (JSONHelper.ReadListFromFile<string>("categories") != null)
                    VM = new DisplayMealViewModel
                    {
                        Categories = JSONHelper.ReadListFromFile<string>("categories")
                    };
            }
            else VM = new DisplayMealViewModel();
        }
        public IActionResult Index()
        {

            return View(VM);
        }


        public async Task<ViewResult> GetMeal(string str)
        {
            var meals = new List<Meal>();


            if (str == "Newly Added")
            {
                if (System.IO.File.Exists("meals"))
                    meals = JSONHelper.ReadListFromFile<Meal>("meals");
            }
            else
                meals = await FoodApiService.GetMealsByCategory(str);


            VM.Meals = meals;
            return View(VM);
        }



        public async Task<ViewResult> GetDetailedFood(string str)
        {

            var meal = await FoodApiService.GetFoodDetails(str);
            var vm = new DisplayDetailsViewModel();
            if (meal != null)
                 vm.Meal = meal;

            return View(vm);

        }


        public IActionResult AddMeal()
        {
            var vm = new AddMealViewModel
            {
                AddedMeal = new Meal()
            };


            return View(vm);
        }


        [HttpPost]
        public RedirectResult AddMeal(AddMealViewModel viewModel)
        {
            AddedMeals = new List<Meal>();
            var vm = new AddMealViewModel
            {
                AddedMeal = new Meal()
            };


            if (VM.Categories.Count == 14)
            {
                VM.Categories.Add("Newly Added");
                JSONHelper.WriteListToFile<string>(VM.Categories, "categories");
            }


            if (System.IO.File.Exists("meals"))
                if (JSONHelper.ReadListFromFile<Meal>("meals") != null)
                    AddedMeals = JSONHelper.ReadListFromFile<Meal>("meals");

            AddedMeals.Add(viewModel.AddedMeal);
            JSONHelper.WriteListToFile<Meal>(AddedMeals, "meals");
            return Redirect("/home/index");

        }






    }
}
