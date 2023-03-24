using System;
using System.Linq;

namespace CharSheet
{
    public class CharacterStats
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class DnD5eCharacterStatsRoller
    {
        private static Random _rand = new Random();

        public static CharacterStats RollCharacterStats()
        {
            CharacterStats stats = new CharacterStats
            {
                Strength = RollStat(),
                Dexterity = RollStat(),
                Constitution = RollStat(),
                Intelligence = RollStat(),
                Wisdom = RollStat(),
                Charisma = RollStat()
            };

            return stats;
        }

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
    }


}
