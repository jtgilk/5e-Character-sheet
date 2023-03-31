using PlayerCharacter;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
            string characterName = Console.ReadLine();
            Console.Clear();
            using var client = new HttpClient();

            // Set the base URL of the API endpoint you want to call
            client.BaseAddress = new Uri("https://www.dnd5eapi.co/api/");
        RaceChoice:
            Console.WriteLine("What race is " + characterName + "?");
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
            string chosenRace = "";
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
            Console.WriteLine(characterName + " the " + chosenRace + " is a... what was it again?");
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
            Console.Write("What level is " + characterName + "?\r\n");
            int charLevel;
            bool isValid = int.TryParse(Console.ReadLine(), out charLevel);

            while (!isValid || charLevel < 1 || charLevel >= 20)
            {
                Console.WriteLine("Invalid input. Please enter an integer between 1 and 20: ");
                isValid = int.TryParse(Console.ReadLine(), out charLevel);
            }


            //Call the API with the class choice
            HttpResponseMessage response = await client.GetAsync("/api/classes/" + chosenClass.ToLower());

            // Read the response content as a string

            string responseBody = await response.Content.ReadAsStringAsync();

            // Do something with the response data
            ClassEntry? classReturn = JsonSerializer.Deserialize<ClassEntry>(responseBody);

            //Call the API with the race choice
            response = await client.GetAsync("/api/races/" + chosenRace.ToLower());

            // Read the response content as a string

            responseBody = await response.Content.ReadAsStringAsync();

            // Do something with the response data
            RaceEntry? raceReturn = JsonSerializer.Deserialize<RaceEntry>(responseBody);

            Console.Clear();

            //Choose the proficiencies
            Console.WriteLine(characterName + " the " + chosenRace + " " + chosenClass);
            Console.WriteLine("So what are they proficient at?");
            int[] profchoices = ChooseProficiencies(classReturn);
            //return the choices
            Console.Clear();
            Console.WriteLine("You have chosen:");
            string[] profResults = new string[profchoices.Length];
            for (int i = 0; i < profchoices.Length; i++)
            {

                string source = classReturn.proficiency_choices[0].from.options[profchoices[i] - 1].item.name;
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
                    profResults[i] = result;
                }
                else
                {
                    Console.WriteLine(result);
                    profResults[i] = result;
                }
            }
            Console.WriteLine(chosenClass + " also has the following proficiencies by default:");
            for (int i = 0; i < classReturn.proficiencies.Count; i++)
            {
                Console.Write(classReturn.proficiencies[i].name);
                if (i == classReturn.proficiencies.Count - 2)
                {
                    Console.Write(", & ");
                }
                else if (i != classReturn.proficiencies.Count - 1)
                {
                    Console.Write(", ");
                }
                //System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine("\r\nPress enter to continue");
            Console.ReadLine();
            Console.Clear();
            //Roll the stats
            Console.WriteLine("Alright! Lets roll some stats! Everyone loves math right?!");
            //I know this is probably redundent, it's something to optimize later.
            int[] racialBonuses = RacialBonuses(raceReturn);
            int hitDiceNumber = classReturn.hit_die;
            Console.WriteLine("Did you know " + chosenClass + " has a d" + hitDiceNumber + " for a hitdice?");
            if (hitDiceNumber < 10)
            {
                Console.WriteLine("So, yeah, " + characterName + " probably doesn't wanna be your main tank...\r\nOh well, mage armor is always a thing I guess, or maybe they can dodge well!");
            }
            if (hitDiceNumber == 10)
            {
                Console.WriteLine(characterName + " is a bit of a beefy lad/lass/eldritch horror/*insert your own witty bit here*, don't let them get at your wizards!");
            }
            if (hitDiceNumber > 10)
            {
                Console.WriteLine("Oh my, " + characterName + " has got some bulk to them.\r\nLet's hope that keeps them on thier feet/wings/fins/or whatever.");
            }
            Console.WriteLine();

            //Call the actual method to roll the stats
            int[] stats = CreateAndReturnStats(hitDiceNumber, charLevel, racialBonuses);
            //End stat roll

            AbilityScores abilityScores = new AbilityScores();
            abilityScores.Strength = stats[0];
            abilityScores.Dexterity = stats[1];
            abilityScores.Constitution = stats[2];
            abilityScores.Intelligence = stats[3];
            abilityScores.Wisdom = stats[4];
            abilityScores.Charisma = stats[5];
            //CharacterSkills characterSkills = new CharacterSkills();
            //foreach (string prof in profResults)
            //{
            //    if (characterSkills.Skills.ContainsKey(prof))
            //    {
            //        Skill skill = characterSkills.Skills[prof];
            //        skill.Proficiency = true;
            //        skill.Bonus += 2;
            //    }
            //}
            DnD5ePlayerCharacter playerCharacter = new DnD5ePlayerCharacter(characterName, chosenRace, chosenClass, charLevel, abilityScores, stats[6], profResults);

            //update and add bonuses to chosen proficiencies
            Console.Clear();
            Console.WriteLine(playerCharacter.Name + "the " + playerCharacter.Race + " is a mighty " + playerCharacter.Class + ".");
            switch (playerCharacter.Level)
            {
                case 1: Console.WriteLine("They are of the " + playerCharacter.Level + "st level."); break;
                case 2: Console.WriteLine("They are of the " + playerCharacter.Level + "nd level."); break;
                case 3: Console.WriteLine("They are of the " + playerCharacter.Level + "rd level."); break;
                default: Console.WriteLine("They are of the " + playerCharacter.Level + "th level."); break;
            }
            Console.WriteLine("If one were to attempt to numerate thier might they might say they are about a " + playerCharacter.AbilityScores.Strength + " or so.");
            Console.WriteLine("If one were to attempt to put a rating to thier incredible agility, I'd say it's about a " + playerCharacter.AbilityScores.Dexterity + ".");
            Console.WriteLine("And with such fortititude! My, it's like it's a  " + playerCharacter.AbilityScores.Constitution + "!");
            Console.WriteLine("But what of thier mind?! Such vast knowledge, as intelligent as say... a solid " + playerCharacter.AbilityScores.Intelligence + ".");
            Console.WriteLine("And thier incredible wisdom! They got street smarts of like " + playerCharacter.AbilityScores.Wisdom + ", no one is fooling them!");
            Console.WriteLine("And so charming! Watch out you foolish undead, even you can't stand up to thier " + playerCharacter.AbilityScores.Charisma + "!");
            Console.WriteLine();
            Console.WriteLine("Did you want more infromation? Y/N");
            string continueOn = Console.ReadLine();
            if (continueOn == "Y" || continueOn == "y" || continueOn == "yes" || continueOn == "Yes")
            {
                foreach (KeyValuePair<string, Skill> kvp in playerCharacter.CharacterSkills.Skills)
                {
                    Console.WriteLine("{0}: {1} ({2})", kvp.Key, kvp.Value.Proficiency ? "proficient" : "not proficient", kvp.Value.Bonus);
                }
            }



            Console.ReadLine();

        End:;

        }
        static int[] ChooseProficiencies(ClassEntry classReturn)
        {
            int choiceNum = classReturn.proficiency_choices[0].choose;
            Console.WriteLine("Choose " + choiceNum + " from the following options");

            //List all choices and remove the Skill: part from the string
            int charProficienciesChoice = classReturn.proficiency_choices[0].from.options.Count;
            for (int i = 1; i <= charProficienciesChoice; i++)
            {
                string source = classReturn.proficiency_choices[0].from.options[i - 1].item.name;
                string toRemove = "Skill: ";
                string result = string.Empty;
                int j = source.IndexOf(toRemove);
                if (j >= 0)
                {
                    result = source.Remove(j, toRemove.Length);
                }
                Console.WriteLine("[" + i + "]" + result);
            }
            int[] returnChoices = new int[choiceNum];
            //Choose the options
            for (int i = 1; i <= choiceNum; i++)
            {
            ProfsChoice:
                string playerChoiceString = Console.ReadLine();
                int playerChoice;
                if (string.IsNullOrWhiteSpace(playerChoiceString) || !int.TryParse(playerChoiceString, out playerChoice) || playerChoice < 1 || playerChoice > charProficienciesChoice)
                {
                    Console.WriteLine("Invalid input. Please enter a non-blank integer between 1 & " + charProficienciesChoice + ". ");
                    goto ProfsChoice;
                }
                if (returnChoices.Contains(playerChoice))
                {
                    Console.WriteLine("You seem to have chosen the same skill more than once, try again.");
                    goto ProfsChoice;
                }
                returnChoices[i - 1] = playerChoice;
            }
            return returnChoices;
        }

        static int[] CreateAndReturnStats(int hitDiceNumber, int charLevel, int[] racialBonuses)
        {
            CharacterStats stats = DnD5eCharacterStatsRoller.RollCharacterStats(hitDiceNumber, charLevel, racialBonuses);

            Console.WriteLine("Your character's stats are:");
            Console.WriteLine($"Hitpoints: {stats.HitPoints}     Hit dice: {hitDiceNumber}");
            Console.WriteLine($"Strength: {stats.Strength}    STR({stats.strMod})");
            Console.WriteLine($"Dexterity: {stats.Dexterity}    DEX({stats.dexMod})");
            Console.WriteLine($"Constitution: {stats.Constitution}    CON({stats.conMod})");
            Console.WriteLine($"Intelligence: {stats.Intelligence}    INT({stats.intMod})");
            Console.WriteLine($"Wisdom: {stats.Wisdom}    WIS({stats.wisMod})");
            Console.WriteLine($"Charisma: {stats.Charisma}    CHA({stats.chaMod})");
            Console.WriteLine("You have a racial bonus of " + racialBonuses[0] + " to STR," + racialBonuses[1] + " to DEX, "
                + racialBonuses[2] + " to CON, " + racialBonuses[3] + " to INT, " + racialBonuses[4] +
                " to WIS, and " + racialBonuses[5] + " to CHA,");
            int[] statSave = new int[7];
            statSave[0] = stats.Strength; statSave[1] = stats.Dexterity; statSave[2] = stats.Constitution;
            statSave[3] = stats.Intelligence; statSave[4] = stats.Wisdom; statSave[5] = stats.Charisma;
            statSave[6] = stats.HitPoints;
            return statSave;
        }

        static int[] RacialBonuses(RaceEntry raceReturn)
        {
            //self reference => str dex con int wis cha
            int[] racialBonuses = { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < raceReturn.ability_bonuses.Count; i++)
            {
                string indexFloat = raceReturn.ability_bonuses[i].ability_score.index;
                switch (indexFloat)
                {
                    case "str":
                        racialBonuses[0] = raceReturn.ability_bonuses[i].bonus; break;
                    case "dex":
                        racialBonuses[1] = raceReturn.ability_bonuses[i].bonus; break;
                    case "con":
                        racialBonuses[2] = raceReturn.ability_bonuses[i].bonus; break;
                    case "int":
                        racialBonuses[3] = raceReturn.ability_bonuses[i].bonus; break;
                    case "wis":
                        racialBonuses[4] = raceReturn.ability_bonuses[i].bonus; break;
                    case "cha":
                        racialBonuses[5] = raceReturn.ability_bonuses[i].bonus; break;
                    default: break;

                }
            }
            return racialBonuses;
        }
    }
}
