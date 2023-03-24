using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text.Json;
using System.Threading.Tasks;


namespace CharSheet
{
    internal class Program
    {
        //Create valid list of class for logic check where needed; Hashes were reccomended for speed
        private static readonly HashSet<string> DnD5eClasses = new HashSet<string>
        {
            "Barbarian", "Bard", "Cleric", "Druid", "Fighter", "Monk",
            "Paladin", "Ranger", "Rogue", "Sorcerer", "Warlock", "Wizard"
        };

        public static async Task Main(string[] args)
        {

            using var client = new HttpClient();

            // Set the base URL of the API endpoint you want to call
            client.BaseAddress = new Uri("https://www.dnd5eapi.co/api/");
        ClassChoice:
            Console.WriteLine("What class are you creating?");
            string PlayerClassChoice = Console.ReadLine();
            if (IsValidDnD5eClass(PlayerClassChoice))
            {
                HttpResponseMessage response = await client.GetAsync("/api/classes/" + PlayerClassChoice);

                // Read the response content as a string

                string responseBody = await response.Content.ReadAsStringAsync();
                //CharEntry myDeserializedClass = JsonConvert.DeserializeObject(responseBody);

                // Do something with the response data
                ClassEntry? ClassReturn = JsonSerializer.Deserialize<ClassEntry>(responseBody);
                int CRProfs = ClassReturn.proficiencies.Count;
                for (int i = 0; i < CRProfs; i++)
                {
                    Console.WriteLine(ClassReturn.proficiencies[i].name);
                }
            }
            else
            {
                Console.WriteLine($"{PlayerClassChoice} is not a valid DnD 5e class.");
                Console.WriteLine("Would you like to try again? Y/N?");
                string RedoChoice = Console.ReadLine();
                if (RedoChoice == "Y" || RedoChoice == "y" || RedoChoice == "yes" || RedoChoice == "Yes")
                {
                    goto ClassChoice;
                }
                else
                {
                    goto End;
                }
            }

            // Call the API endpoint and get the response


            //Console.WriteLine(ClassReturn.proficiencies.Count);
            //foreach (var i in ClassReturn.proficiencies.Count)
            //{
            //    Console.WriteLine(ClassReturn.proficiencies[i].name);
            //}
            //  Console.WriteLine(ClassEntry1?.hit_die);
            Console.ReadLine();
        End:;

        }
        static void CreateAndReturnStats()
        {
            CharacterStats stats = DnD5eCharacterStatsRoller.RollCharacterStats();

            Console.WriteLine("Your character's stats are:");
            Console.WriteLine($"Strength: {stats.Strength}");
            Console.WriteLine($"Dexterity: {stats.Dexterity}");
            Console.WriteLine($"Constitution: {stats.Constitution}");
            Console.WriteLine($"Intelligence: {stats.Intelligence}");
            Console.WriteLine($"Wisdom: {stats.Wisdom}");
            Console.WriteLine($"Charisma: {stats.Charisma}");

            Console.ReadLine();
        }
        static bool IsValidDnD5eClass(string input)
        {
            return DnD5eClasses.Contains(input.Trim(), StringComparer.OrdinalIgnoreCase);
        }

    }
}