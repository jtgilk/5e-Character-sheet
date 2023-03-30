using System;
using System.Linq;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CharSheet
{
    public class CharacterStats
    {
        public int hitDiceNumber;
        public int charLevel;
        public int Strength { get; set; }
        public int strMod { get; set; }
        public int Dexterity { get; set; }
        public int dexMod { get; set; }
        public int Constitution { get; set; }
        public int conMod { get; set; }
        public int Intelligence { get; set; }
        public int intMod { get; set; }
        public int Wisdom { get; set; }
        public int wisMod { get; set; }
        public int Charisma { get; set; }
        public int chaMod { get; set; }
        public int HitPoints { get; set; }
        private static int GetModifier(int abilityScore)
        {
            int modifier = (abilityScore - 10) / 2;
            return modifier;
        }

        public void CalculateModifiers()
        {
            strMod = GetModifier(Strength);
            dexMod = GetModifier(Dexterity);
            conMod = GetModifier(Constitution);
            intMod = GetModifier(Intelligence);
            wisMod = GetModifier(Wisdom);
            chaMod = GetModifier(Charisma);
        }
    }

    public class DnD5eCharacterStatsRoller
    {
        private static Random _rand = new Random();

        public static CharacterStats RollCharacterStats(int hitDiceNumber, int charLevel)
        {
            CharacterStats stats = new CharacterStats
            {
                Strength = RollStat(),
                Dexterity = RollStat(),
                Constitution = RollStat(),
                Intelligence = RollStat(),
                Wisdom = RollStat(),
                Charisma = RollStat(),
                HitPoints = RollHP(hitDiceNumber, charLevel),
            };
            stats.CalculateModifiers();
            return stats;
        }


        //public static CharacterStats CharacterMods()
        //{
        //    CharacterStats stats = new CharacterStats
        //    {
        //        strMod = GetModifier(Strength),
        //        Dexterity = RollStat(),
        //        Constitution = RollStat(),
        //        Intelligence = RollStat(),
        //        Wisdom = RollStat(),
        //        Charisma = RollStat(),
        //        HitPoints = RollHP(hitDiceNumber, charLevel),


        //    };

        //    return stats;
        //}

        private static int RollStat()
        {
            int[] rolls = new int[4];

            for (int i = 0; i < 4; i++)
            {
                rolls[i] = _rand.Next(1, 7);
            }

            Array.Sort(rolls);
            Array.Reverse(rolls);

            return rolls[0] + rolls[1] + rolls[2];
        }

        private static int RollHP(int hitDiceNumber, int charLevel)
        {
            HitPointsRoll:
            if (hitDiceNumber >= 0)
            {
                int HitPoints = 0;
                int[] rolls = new int[charLevel];

                for (int i = 0; i < charLevel; i++)
                {
                    rolls[i] = _rand.Next(1, hitDiceNumber);
                }
                foreach (int i in rolls)
                {
                    HitPoints += i;
                }
                return HitPoints;
            }
            else
            {
                Console.WriteLine("There seems to be an error with the hitdice value.\r\nPlease provide a value for the hitdice of the class greater than 0:");
                Console.ReadLine();
                bool isValid = int.TryParse(Console.ReadLine(), out hitDiceNumber);

                while (!isValid || hitDiceNumber < 0)
                {
                    Console.WriteLine("Invalid input. Please enter an integer greater than 0:");
                    isValid = int.TryParse(Console.ReadLine(), out hitDiceNumber);
                }
                goto HitPointsRoll;
            }
        }

    }


}
