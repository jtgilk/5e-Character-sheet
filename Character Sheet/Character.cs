using System;
using CharSheet;

namespace PlayerCharacter
{
    public class AbilityScores
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class DnD5ePlayerCharacter
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public AbilityScores AbilityScores { get; set; }
        public int HitPoints { get; set; }
        public string Background { get; set; }
        public string Alignment { get; set; }

        public DnD5ePlayerCharacter(string name, string race, string characterClass, int level, AbilityScores abilityScores, int hitPoints, string background, string alignment)
        {
            Name = name;
            Race = race;
            Class = characterClass;
            Level = level;
            AbilityScores = abilityScores;
            HitPoints = hitPoints;
            Background = background;
            Alignment = alignment;
        }
    }
}
