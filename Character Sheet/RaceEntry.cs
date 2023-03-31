using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace CharSheet
{
    public class RaceEntry
    {
        public string index { get; set; }
        public string name { get; set; }
        public int speed { get; set; }
        public List<AbilityBonuse> ability_bonuses { get; set; }
        public string alignment { get; set; }
        public string age { get; set; }
        public string size { get; set; }
        public string size_description { get; set; }
        public List<object> starting_proficiencies { get; set; }
        public List<Language> languages { get; set; }
        public string language_desc { get; set; }
        public List<Trait> traits { get; set; }
        public List<object> subraces { get; set; }
        public string url { get; set; }


        public class AbilityBonuse
        {
            public AbilityScore ability_score { get; set; }
            public int bonus { get; set; }
        }

        public class AbilityScore
        {
            public string index { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Language
        {
            public string index { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Trait
        {
            public string index { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }
    }
}