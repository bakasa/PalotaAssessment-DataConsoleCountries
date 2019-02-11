using System;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PalotaInterviewCS
{
    public partial class Country
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("topLevelDomain")]
        public string[] TopLevelDomain { get; set; }

        [JsonProperty("alpha2Code")]
        public string Alpha2Code { get; set; }

        [JsonProperty("alpha3Code")]
        public string Alpha3Code { get; set; }

        [JsonProperty("callingCodes")]
        public string[] CallingCodes { get; set; }

        [JsonProperty("capital")]
        public string Capital { get; set; }

        [JsonProperty("altSpellings")]
        public string[] AltSpellings { get; set; }

        [JsonProperty("region")]
        public Region Region { get; set; }

        [JsonProperty("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }

        [JsonProperty("latlng")]
        public double[] Latlng { get; set; }

        [JsonProperty("demonym")]
        public string Demonym { get; set; }

        [JsonProperty("area")]
        public double? Area { get; set; }

        [JsonProperty("gini")]
        public double? Gini { get; set; }

        [JsonProperty("timezones")]
        public string[] Timezones { get; set; }

        [JsonProperty("borders")]
        public string[] Borders { get; set; }

        [JsonProperty("nativeName")]
        public string NativeName { get; set; }

        [JsonProperty("numericCode")]
        public string NumericCode { get; set; }

        [JsonProperty("currencies")]
        public Currency[] Currencies { get; set; }

        [JsonProperty("languages")]
        public Language[] Languages { get; set; }

        [JsonProperty("translations")]
        public Translations Translations { get; set; }

        [JsonProperty("flag")]
        public Uri Flag { get; set; }

        [JsonProperty("regionalBlocs")]
        public RegionalBloc[] RegionalBlocs { get; set; }

        [JsonProperty("cioc")]
        public string Cioc { get; set; }
    }

    public class Currency
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }

    public class Language
    {
        [JsonProperty("iso639_1")]
        public string Iso6391 { get; set; }

        [JsonProperty("iso639_2")]
        public string Iso6392 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nativeName")]
        public string NativeName { get; set; }
    }

    public class RegionalBloc
    {
        [JsonProperty("acronym")]
        public string Acronym { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("otherAcronyms")]
        public string[] OtherAcronyms { get; set; }

        [JsonProperty("otherNames")]
        public string[] OtherNames { get; set; }
    }

    public class Translations
    {
        [JsonProperty("de")]
        public string De { get; set; }

        [JsonProperty("es")]
        public string Es { get; set; }

        [JsonProperty("fr")]
        public string Fr { get; set; }

        [JsonProperty("ja")]
        public string Ja { get; set; }

        [JsonProperty("it")]
        public string It { get; set; }

        [JsonProperty("br")]
        public string Br { get; set; }

        [JsonProperty("pt")]
        public string Pt { get; set; }

        [JsonProperty("nl")]
        public string Nl { get; set; }

        [JsonProperty("hr")]
        public string Hr { get; set; }

        [JsonProperty("fa")]
        public string Fa { get; set; }
    }

    public enum Region { Africa, Americas, Asia, Empty, Europe, Oceania, Polar };

    public partial class Country
    {
        public static Country[] FromJson(string json) => JsonConvert.DeserializeObject<Country[]>(json, PalotaInterviewCS.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Country[] self) => JsonConvert.SerializeObject(self, PalotaInterviewCS.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                RegionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class RegionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Region) || t == typeof(Region?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Region.Empty;
                case "Africa":
                    return Region.Africa;
                case "Americas":
                    return Region.Americas;
                case "Asia":
                    return Region.Asia;
                case "Europe":
                    return Region.Europe;
                case "Oceania":
                    return Region.Oceania;
                case "Polar":
                    return Region.Polar;
            }
            throw new Exception("Cannot unmarshal type Region");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Region)untypedValue;
            switch (value)
            {
                case Region.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Region.Africa:
                    serializer.Serialize(writer, "Africa");
                    return;
                case Region.Americas:
                    serializer.Serialize(writer, "Americas");
                    return;
                case Region.Asia:
                    serializer.Serialize(writer, "Asia");
                    return;
                case Region.Europe:
                    serializer.Serialize(writer, "Europe");
                    return;
                case Region.Oceania:
                    serializer.Serialize(writer, "Oceania");
                    return;
                case Region.Polar:
                    serializer.Serialize(writer, "Polar");
                    return;
            }
            throw new Exception("Cannot marshal type Region");
        }

        public static readonly RegionConverter Singleton = new RegionConverter();
    }

}