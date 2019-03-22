using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

namespace PalotaInterviewCS
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string CountriesEndpoint = "https://restcountries.eu/rest/v2/all";
        private const string ZAFAlpha3Code = "ZAF";

        static void Main(string[] args)
        {
            Country[] countries = GetCountries(CountriesEndpoint).GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Palota Interview: Country Facts");
            Console.WriteLine();
            Console.ResetColor();

            Random rnd = new Random(); // random to populate fake answer - you can remove this once you use real values
            
            var sortedCountries = ((countries.ToList())
                                    .OrderByDescending(p => p.Gini)
                                    .ThenBy(p => p.Gini.HasValue)).ToList();

            int southAfricanGiniPlace = sortedCountries.FindIndex(c => c.Alpha3Code.Equals(ZAFAlpha3Code, StringComparison.CurrentCultureIgnoreCase)) + 1; 
            Console.WriteLine($"1. South Africa's Gini coefficient is the {GetOrdinal(southAfricanGiniPlace)} highest");
            
            var sortedAscGiniCountries = sortedCountries
                    .OrderBy(p => p.Gini)
                    .Where(p => p.Gini.HasValue)
                    .ToList();

            string lowestGiniCountry = sortedAscGiniCountries[0].Name;
            Console.WriteLine($"2. {lowestGiniCountry} has the lowest Gini Coefficient");

            var regionsTimeZoned = new List<RegionTimeZoned>();
            var regions = sortedCountries.GroupBy(c => c.Region).ToList();
            
            //foreach (var region in regions)
            regions.ForEach(r => 
            {
                var uniqueTimeZonesByRegion = new Dictionary<string, string>();
                var timeZones = r.Select(a => a.Timezones);
                foreach (var timeZone in timeZones)
                {
                    foreach (var s in timeZone)
                    {
                        if (!s.Equals("UTC"))
                            uniqueTimeZonesByRegion.TryAdd($"{r.Key.ToString()},{s}", s);
                    }
                }

                regionsTimeZoned.Add( new RegionTimeZoned{ Region = r.Key, Timezones = uniqueTimeZonesByRegion});
            });

            var mostTimeZonesRegion = regionsTimeZoned.OrderByDescending(p => p.Timezones.Count).First();

            string regionWithMostTimezones = mostTimeZonesRegion.Region.ToString();

            int amountOfTimezonesInRegion = mostTimeZonesRegion.Timezones.Count();
            Console.WriteLine($"3. {regionWithMostTimezones} is the region that spans most timezones at {amountOfTimezonesInRegion} timezones");
          
            var countryCurrencies = new List<string>();
            sortedCountries.ForEach(sc => sc.Currencies.ToList().ForEach(c => countryCurrencies.Add(c.Name)));
            var mostPopularCurrencyObj = countryCurrencies.GroupBy(str => str)
                                                            .Where(gd => !string.IsNullOrEmpty(gd.Key))
                                                            .ToDictionary(group => group.Key, group => group.Count())
                                                            .OrderByDescending(c => c.Value)
                                                            .First();
         
            string mostPopularCurrency = mostPopularCurrencyObj.Key;

            int numCountriesUsedByCurrency = mostPopularCurrencyObj.Value;
            Console.WriteLine($"4. {mostPopularCurrency} is the most popular currency and is used in {numCountriesUsedByCurrency} countries");
          
            var languages = new List<string>();
            sortedCountries.ForEach(sc => sc.Languages.ToList().ForEach(c => languages.Add(c.Name)));
            var mostPopularLanguagesList = languages.GroupBy(str => str)
                                    .Where(gd => !string.IsNullOrEmpty(gd.Key))
                                    .ToDictionary(group => group.Key, group => group.Count())
                                    .OrderByDescending(c => c.Value)
                                    .Take(3).ToList();

            string[] mostPopularLanguages = mostPopularLanguagesList.Select(x => x.Key).ToArray();
            Console.WriteLine($"5. The top three popular languages are {mostPopularLanguages[0]}, {mostPopularLanguages[1]} and {mostPopularLanguages[2]}");
           
            var countryWithBorderingCountriesDic = new Dictionary<string, long>();
            sortedCountries.ForEach(country =>
            {
                if (country.Borders.Length == 0)
                    countryWithBorderingCountriesDic.TryAdd($"{country.Name}|0|{country.Population}|{country.Area}", country.Population);
                else
                    country.Borders.ToList()
                        .ForEach(c =>
                            countryWithBorderingCountriesDic.TryAdd(
                                $"{country.Name}|{sortedCountries.First(cc => cc.Name == country.Name).Borders.Count()}|{country.Population}|{country.Area}",
                                country.Population +
                                country.Borders.Sum(b => sortedCountries.Find(borderingCountry =>
                                        borderingCountry.Alpha3Code.Equals(b,
                                            StringComparison.InvariantCultureIgnoreCase))
                                    .Population)));
            });
            
            var countryWithBorderingCountriesObj = countryWithBorderingCountriesDic.OrderByDescending(y => y.Value).First();
            var countryWithBorderParts = countryWithBorderingCountriesObj.Key.Split(new[] { '|' });

            string countryWithBorderingCountries = countryWithBorderParts[0];
            int.TryParse(countryWithBorderParts[1], out var numberOfBorderingCountries);
            long combinedPopulation = countryWithBorderingCountriesObj.Value;

            Console.WriteLine($"6. {countryWithBorderingCountries} and it's {numberOfBorderingCountries} bordering countries has the highest combined population of {combinedPopulation}");
            
            var popDensityDic = new Dictionary<string, decimal>();
            countryWithBorderingCountriesDic.ToList().ForEach(c =>
            {
                var keyParts = c.Key.Split(new[] { '|' });

                decimal.TryParse(keyParts[2], out var population);
                decimal.TryParse(keyParts[3], out var area);

                if (area != 0)
                {
                    var popDensity = population / area;
                    popDensityDic.TryAdd(keyParts[0], popDensity);
                }
            });

            var popDensityObj = popDensityDic.OrderBy(c => c.Value);

            string lowPopDensityName = popDensityObj.First().Key;
            decimal lowPopDensity = popDensityObj.First().Value;
            Console.WriteLine($"7. {lowPopDensityName} has the lowest population density of {lowPopDensity}");

            popDensityObj = popDensityDic.OrderByDescending(c => c.Value);

            string highPopDensityName = popDensityObj.First().Key;
            decimal highPopDensity = popDensityObj.First().Value;
            Console.WriteLine($"8. {highPopDensityName} has the highest population density of {highPopDensity}");

            var largestAreaSubregionList = sortedCountries.GroupBy(country => country)
                .GroupBy(g => g.Key.Subregion)
                .Where(gd => !string.IsNullOrEmpty(gd.Key))
                .ToDictionary(g => g.Key, g => sortedCountries.Where(c => c.Subregion == g.Key).Sum(c => c.Area));
                

            string largestAreaSubregion = largestAreaSubregionList.First().Key;
            Console.WriteLine($"9. {largestAreaSubregion} is the subregion that covers the most area");

            /*
             * HINT: Group by regional blocks (`Country.RegionalBlocks`). For each regional block, average out the gini coefficient (`Country.Gini`) of all member countries
             * Sort the regional blocks by the average country gini coefficient to find the lowest (or find the lowest without sorting)
             * Return the name of the regional block (`RegionalBloc.Name`) along with the calculated average gini coefficient
             */
         
            var regionBlockDicTemp = new Dictionary<string, double>();
            var regionBlockDic = new Dictionary<string, double>();
            var regionalBlockList = sortedCountries.Select(c => c.RegionalBlocs);

            foreach (var block in regionalBlockList)
            {
                foreach (var regionalBloc in block)
                    regionBlockDicTemp.TryAdd(regionalBloc.Name, 0);
            }

            foreach (var block in regionBlockDicTemp)
            {
                var counter = 0;
                var total = 0D;
                var average = 0D;
                
                sortedCountries.ForEach(c =>
                {
                    c.RegionalBlocs.ToList().ForEach(r =>
                    {
                        if (r.Name == block.Key && c.Gini != null)
                        {
                            total += c.Gini.Value;
                            counter++;
                        }
                    });
                });

                average = total / counter;
                regionBlockDic.TryAdd(block.Key, average);
            }

            var mostEqualRegionalBlockObj = regionBlockDic.ToList().OrderBy(c => c.Value).FirstOrDefault();


            string mostEqualRegionalBlock = mostEqualRegionalBlockObj.Key;
            double lowestRegionalBlockGini = mostEqualRegionalBlockObj.Value;
            Console.WriteLine($"10. {mostEqualRegionalBlock} is the regional block with the lowest average Gini coefficient of {lowestRegionalBlockGini}");

            Console.ReadLine();
        }
      

        /// <summary>
        /// Gets the countries from a specified endpiny
        /// </summary>
        /// <returns>The countries.</returns>
        /// <param name="path">Path endpoint for the API.</param>
        static async Task<Country[]> GetCountries(string path)
        {
            Country[] countries = null;

            var response = await client.GetStringAsync(path);

            //countries = Country.FromJson(HttpRestApiHelper.GetAsync(CountriesEndpoint));
            countries = Country.FromJson(response);

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

    public class RegionTimeZoned
    {
        public Region Region { get; set; }
        public Dictionary<string, string> Timezones;
    }

    public class CountryCurrency
    {
        public Country Region { get; set; }
        public Dictionary<string, string> Currency;
    }



}
