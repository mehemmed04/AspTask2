using Intro_Task.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Intro_Task.Services
{
    public class FoodApiService
    {
        public static dynamic Data { get; set; }
        public static dynamic SingleData { get; set; }
        public static async Task<List<Meal>> GetMealsByCategory(string ctg)
        {

            var client = new HttpClient();
            var url = $@"https://www.themealdb.com/api/json/v1/1/filter.php?c={ctg}";

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new System.Uri(url)
            };

            using (var response = await client.SendAsync(requestMessage))
            {

                try
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject(responseBody);

                    List<Meal> meals = new List<Meal>();

                    for (int i = 0; i < 10; i++)
                    {

                        if (i < Data.meals.Count)
                        {
                            var meal = new Meal
                            {
                                Id = Data.meals[i].idMeal,
                                Name = Data.meals[i].strMeal,
                                Thumb = Data.meals[i].strMealThumb
                            };

                            meals.Add(meal);
                        }
                        else break;
                    }
                    return meals;
                }


                catch (System.Exception ex)
                {

                    throw;
                }
            }

        }



        public async static Task<List<Meal>> GetMealsByRegion(string nationality)
        {
            var client = new HttpClient();
            var url = $@"https://www.themealdb.com/api/json/v1/1/filter.php?a={nationality}";

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new System.Uri(url)
            };

            using (var response = await client.SendAsync(requestMessage))
            {

                try
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject(responseBody);

                    List<Meal> meals = new List<Meal>();

                    for (int i = 0; i < 10; i++)
                    {

                        var meal = new Meal
                        {
                            Id = Data.meals[i].idMeal,
                            Name = Data.meals[i].strMeal,
                            Thumb = Data.meals[i].strMealThumb
                        };

                        meals.Add(meal);
                    }
                    return meals;
                }


                catch (System.Exception ex)
                {

                    throw;
                }
            }
        }


        public async static Task<Meal> GetFoodDetails(string foodname)
        {

            var client = new HttpClient();
            string encodedFoodName = Uri.EscapeUriString(foodname);
            string url = $@"https://www.themealdb.com/api/json/v1/1/search.php?s={encodedFoodName}";

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            using (var response = await client.SendAsync(requestMessage))
            {
                try
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject(responseBody);
                    

                    var meal = new Meal();
                    if (Data.meals.Count > 0)
                    {
                        var info = Data.meals[0];

                        meal.Id = info.idMeal;
                        meal.Category = info.strCategory;
                        meal.Name = info.strMeal;
                        meal.Region = info.strArea;
                        meal.Thumb = info.strMealThumb;
                        meal.Instructions = info.strInstructions;
                        meal.Tag = info.strTags;

                        string str = info.strYoutube;
                        var index = str.IndexOf('=');
                        var substring=str.Substring(index+1,str.Length-index-1);
                        meal.VideoId=substring;


                        JObject json = JObject.Parse(responseBody);
                        JArray meals = (JArray)json["meals"];

                        foreach (JObject item in meals)
                        {

                            var a = item.Properties();

                            foreach (var property in item.Properties())
                            {
                                if (property.Name.StartsWith("strIngredient") && property.Value.ToString() != "" && property.Value != null)
                                {
                                    var value = property.Value;
                                    meal.Ingredients.Add(value.ToString());
                                }

                                if (property.Name.StartsWith("strMeasure"))
                                {
                                    meal.Measures.Add(property.Value.ToString());
                                }

                            }
                        }


                    }
                    return meal;
                }
                catch (Exception)
                {

                    return null;
                }

            }
        }

    }
}
