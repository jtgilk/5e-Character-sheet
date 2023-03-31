using System;
using CharSheet;

namespace PlayerCharacter
{

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
        public SkillScores SkillScores { get; set; }


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
    public class AbilityScores
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int Acrobatics { get; set; }

    }
    public class CharacterSkills
    {
        public Dictionary<string, Skill> Skills { get; set; }

        public CharacterSkills()
        {
            Skills = new Dictionary<string, Skill>
        {
            { "Acrobatics", new Skill("Acrobatics", "Dexterity", false, 0) },
            { "Animal Handling", new Skill("Animal Handling", "Wisdom", false, 0) },
            { "Arcana", new Skill("Arcana", "Intelligence", false, 0) },
            { "Athletics", new Skill("Athletics", "Strength", false, 0) },
            { "Deception", new Skill("Deception", "Charisma", false, 0) },
            { "History", new Skill("History", "Intelligence", false, 0) },
            { "Insight", new Skill("Insight", "Wisdom", false, 0) },
            { "Intimidation", new Skill("Intimidation", "Charisma", false, 0) },
            { "Investigation", new Skill("Investigation", "Intelligence", false, 0) },
            { "Medicine", new Skill("Medicine", "Wisdom", false, 0) },
            { "Nature", new Skill("Nature", "Intelligence", false, 0) },
            { "Perception", new Skill("Perception", "Wisdom", false, 0) },
            { "Performance", new Skill("Performance", "Charisma", false, 0) },
            { "Persuasion", new Skill("Persuasion", "Charisma", false, 0) },
            { "Religion", new Skill("Religion", "Intelligence", false, 0) },
            { "Sleight of Hand", new Skill("Sleight of Hand", "Dexterity", false, 0) },
            { "Stealth", new Skill("Stealth", "Dexterity", false, 0) },
            { "Survival", new Skill("Survival", "Wisdom", false, 0) }
        };
        }

        public void SetProficiency(string skillName, bool proficiency)
        {
            if (Skills.ContainsKey(skillName))
            {
                Skills[skillName].Proficiency = proficiency;
            }
        }

        public void SetBonus(string skillName, int bonus)
        {
            if (Skills.ContainsKey(skillName))
            {
                Skills[skillName].Bonus = bonus;
            }
        }

        public int GetTotalBonus(string skillName, int abilityScore)
        {
            if (Skills.ContainsKey(skillName))
            {
                return Skills[skillName].GetTotalBonus(abilityScore);
            }
            return 0;
        }
    }

    public class Skill
    {
        public string Name { get; set; }
        public string AbilityScore { get; set; }
        public bool Proficiency { get; set; }
        public int Bonus { get; set; }

        public Skill(string name, string abilityScore, bool proficiency, int bonus)
        {
            Name = name;
            AbilityScore = abilityScore;
            Proficiency = proficiency;
            Bonus = bonus;
        }

        public int GetModifier(int abilityScore)
        {
            return (int)Math.Floor((abilityScore - 10) / 2.0);
        }

        public int GetTotalBonus(int abilityScore)
        {
            int modifier = GetModifier(abilityScore);
            int proficiencyBonus = Proficiency ? 2 : 0;
            return modifier + proficiencyBonus + Bonus;
        }
    }

}
