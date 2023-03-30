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

        private static readonly HashSet<string> DnD5eRaces = new HashSet<string>
        {
            "Dragonborn","Dwarf","Elf","Gnome","Half-Elf","Half-Orc","Halfling","Human","Tiefling"
        };

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the 5e character creator.\r\nPlease enter the name of the character you wish to create to get started.");
            string CharacterName = Console.ReadLine();
            Console.Clear();
            using var client = new HttpClient();

            // Set the base URL of the API endpoint you want to call
            client.BaseAddress = new Uri("https://www.dnd5eapi.co/api/");
        RaceChoice:
            Console.WriteLine("What race is " + CharacterName + "?");
            // Print the numbered list of races to the console
            Console.WriteLine("Choose a race from the following options:");
            int index = 1;
            foreach (string race in DnD5eRaces)
            {
                Console.WriteLine(index + ". " + race);
                index++;
            }

            // Prompt the user to choose a race
            Console.Write("Enter the number of the race you want to choose: ");
            string userChoiceString = Console.ReadLine();
            // Validate the user's choice
            string chosenRace ="";
            if (int.TryParse(userChoiceString, out int userChoice) && userChoice > 0 && userChoice <= DnD5eRaces.Count)
            {
                chosenRace = DnD5eRaces.ElementAt(userChoice - 1);
                Console.WriteLine("You have chosen the " + chosenRace + " race.\r\nPress enter to continue");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please choose a race number from the list. Try again");
                goto RaceChoice;
            }



        ClassChoice:
            Console.WriteLine(CharacterName + " the " + chosenRace + " is a... what was it again?");
            // Print the numbered list of classes to the console
            Console.WriteLine("Choose a class from the following options:");
            index = 1;
            foreach (string classes in DnD5eClasses)
            {
                Console.WriteLine(index + ". " + classes);
                index++;
            }

            // Prompt the user to choose a class
            Console.Write("Enter the number of the class you want to choose: ");
            userChoiceString = Console.ReadLine();

            // Validate the user's choice
            string chosenClass = "";
            if (int.TryParse(userChoiceString, out userChoice) && userChoice > 0 && userChoice <= DnD5eClasses.Count)
            {
                chosenClass = DnD5eClasses.ElementAt(userChoice - 1);
                Console.WriteLine("You have chosen the " + chosenClass + " class.\r\nPress enter to continue");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please choose a class number from the list. Try again");
                goto ClassChoice;
            }

            //Console.WriteLine("What class is " + CharacterName + "?");
            //string PlayerClassChoice = Console.ReadLine();
            //if (IsValidDnD5eClass(PlayerClassChoice))
            //{
            HttpResponseMessage response = await client.GetAsync("/api/classes/" + chosenClass.ToLower());;

            // Read the response content as a string

            string responseBody = await response.Content.ReadAsStringAsync();
            //CharEntry myDeserializedClass = JsonConvert.DeserializeObject(responseBody);

            // Do something with the response data
            ClassEntry? ClassReturn = JsonSerializer.Deserialize<ClassEntry>(responseBody);
            Console.Clear();

            //Choose the proficiencies
            Console.WriteLine(CharacterName + " the " + chosenRace + " " + chosenClass);
            Console.WriteLine("So what are they proficient at?");
            int[] profchoices = ChooseProficiencies(ClassReturn);
            //return the choices
            Console.Clear();
            Console.WriteLine("You have chosen:");
            for (int i = 0; i < profchoices.Length; i++)
            {
                string source = ClassReturn.proficiency_choices[0].from.options[profchoices[i]-1].item.name;
                string toRemove = "Skill: ";
                string result = string.Empty;
                int j = source.IndexOf(toRemove);
                if (j >= 0)
                {
                    result = source.Remove(j, toRemove.Length);
                }
                if (i + 1 < profchoices.Length)
                {
                    Console.WriteLine(result);
                    Console.WriteLine("&");
                }
                else
                {
                    Console.WriteLine(result);
                }
            }
            //Roll the stats
            Console.ReadLine();
        //}

        //else
        //{
        //    Console.WriteLine($"{PlayerClassChoice} is not a valid DnD 5e class.");
        //    Console.WriteLine("Would you like to try again? Y/N?");
        //    string RedoChoice = Console.ReadLine();
        //    if (RedoChoice == "Y" || RedoChoice == "y" || RedoChoice == "yes" || RedoChoice == "Yes")
        //    {
        //        goto ClassChoice;
        //    }
        //    else
        //    {
        //        goto End;
        //    }
        //}

        // Call the API endpoint and get the response


        //Console.WriteLine(ClassReturn.proficiencies.Count);
        //foreach (var i in ClassReturn.proficiencies.Count)
        //{
        //    Console.WriteLine(ClassReturn.proficiencies[i].name);
        //}
        //  Console.WriteLine(ClassEntry1?.hit_die);
        End:;

        }
        static int[] ChooseProficiencies(ClassEntry ClassReturn)
        {
            int ChoiceNum = ClassReturn.proficiency_choices[0].choose;
            Console.WriteLine("Choose " + ChoiceNum + " from the following options");

            //List all choices and remove the Skill: part from the string
            int CRProfsChoice = ClassReturn.proficiency_choices[0].from.options.Count;
            var CRProfsChoiceNormalize = CRProfsChoice - 1;
            for (int i = 1; i <= CRProfsChoice; i++)
            {
                string source = ClassReturn.proficiency_choices[0].from.options[i-1].item.name;
                string toRemove = "Skill: ";
                string result = string.Empty;
                int j = source.IndexOf(toRemove);
                if (j >= 0)
                {
                    result = source.Remove(j, toRemove.Length);
                }
                Console.WriteLine("[" + i + "]" + result);
            }
            int[] returnchoices = new int[ChoiceNum];
            //Choose the options
            for (int i = 1; i <= ChoiceNum; i++)
            {
            ProfsChoice:
                string playerchoicestring = Console.ReadLine();
                int playerchoice;
                if (string.IsNullOrWhiteSpace(playerchoicestring) || !int.TryParse(playerchoicestring, out playerchoice) || playerchoice < 1 || playerchoice > CRProfsChoice)
                {
                    Console.WriteLine("Invalid input. Please enter a non-blank integer between 1 & " + CRProfsChoice + ".");
                    goto ProfsChoice;
                }
                returnchoices[i - 1] = playerchoice;
            }
            //Choose and check that the choices are valid integers in the list
            return returnchoices;
        }
        static bool IsValidDnD5eClass(string input)
        {
            return DnD5eClasses.Contains(input.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        static bool IsValidDnD5eRace(string input)
        {
            return DnD5eRaces.Contains(input.Trim(), StringComparer.OrdinalIgnoreCase);
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

    }
}
