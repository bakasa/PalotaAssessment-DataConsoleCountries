using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace PalotaInterviewCS
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string countriesEndpoint = "https://restcountries.eu/rest/v2/all";

        static void Main(string[] args)
        {
            Country[] countries = GetCountries(countriesEndpoint).GetAwaiter().GetResult();

            //TODO use data operations and data structures to optimally find the correct value (N.B. be aware of null values)

            Random rnd = new Random();
            int southAfricanGiniPlace = rnd.Next(1, 10); // Use correct value

            string lowestGiniCountry = "ExampleCountry"; // Use correct value

            string countryWithMostRegions = "ExampleRegion"; // Use correct value
            int amountOfTimezonesInRegion = rnd.Next(1, 10); // Use correct value

            string mostPopularCurrency = "ExampleCurrency"; // Use correct value
            int numCountriesUsedByCurrency = rnd.Next(1, 10); // Use correct value

            string countryWithBorderingCountries = "ExampleCountry"; // Use correct value
            int numberOfBorderingCountries = rnd.Next(1, 10); // Use correct value
            int combinnedPopulation = rnd.Next(1000000, 10000000); // Use correct value

            string mostPopularLanguage1 = "ExampleLanguage"; // Use correct value
            string mostPopularLanguage2 = "ExampleLanguage"; // Use correct value
            string mostPopularLanguage3 = "ExampleLanguage"; // Use correct value

            string lowPopDensityName = "ExampleCountry"; // Use correct value
            double lowPopDensity = rnd.NextDouble() * 100; // Use correct value
            string highPopDensityName = "ExampleCountry"; // Use correct value
            double highPopDensity = rnd.NextDouble() * 100; // Use correct value

            string largestAreaSubregion = "ExampleSubRegion"; // Use correct value

            string mostEqualRegionalBlock = "ExampleRegionalBlock"; // Use correct value
            double lowestRegionalBlockGini = rnd.NextDouble() * 10; // Use correct value

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Palota Interview: Country Facts");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("1. South Africa's Gini coefficient is the {0} highest", GetOrdinal(southAfricanGiniPlace));
            Console.WriteLine("2. {0} has the lowest Gini Coefficient", lowestGiniCountry);
            Console.WriteLine("3. {0} is the region that spans most timezones at {1} timezones", countryWithMostRegions, amountOfTimezonesInRegion);
            Console.WriteLine("4. {0} is the most popular currency and is used in {1} countries", mostPopularCurrency, numCountriesUsedByCurrency);
            Console.WriteLine("5. The top three popular languages are {0}, {1} and {2}", mostPopularLanguage1, mostPopularLanguage2, mostPopularLanguage3);
            Console.WriteLine("6. {0} and it's {1} bordering countries has the highest combined population of {2}", countryWithBorderingCountries, numberOfBorderingCountries, combinnedPopulation);
            Console.WriteLine("7. {0} has the lowest population density of {1}", lowPopDensityName, lowPopDensity);
            Console.WriteLine("8. {0} has the highest population density of {1}", highPopDensityName, highPopDensity);
            Console.WriteLine("9. {0} is the subregion that covers the most area", largestAreaSubregion);
            Console.WriteLine("10. {0} is the regional block with the lowest average Gini coefficient of {1}", mostEqualRegionalBlock,lowestRegionalBlockGini);
        }

        static async Task<Country[]> GetCountries(string path)
        {
            Country[] countries = null;
            //TODO get data from endpoint and convert it to a typed array using Country.FromJson
            HttpResponseMessage response = await client.GetAsync(path);
            return countries;
        }

        /// <summary>
        /// Gets the ordinal value of a number (e.g. 1 to 1st)
        /// </summary>
        /// <returns>The ordinal.</returns>
        /// <param name="num">Number.</param>
        public static string GetOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
    }
}
