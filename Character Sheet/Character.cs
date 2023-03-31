using System;
using CharSheet;
using static CharSheet.ClassEntry;

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
        public CharacterSkills CharacterSkills { get; set; }


        //public DnD5ePlayerCharacter(string name, string race, string characterClass, int level, AbilityScores abilityScores, int hitPoints, string background, string alignment)
        public DnD5ePlayerCharacter(string name, string race, string characterClass, int level, AbilityScores abilityScores, int hitPoints, string[] proficiencies)
        {
            Name = name;
            Race = race;
            Class = characterClass;
            Level = level;
            AbilityScores = abilityScores;
            HitPoints = hitPoints;
            CharacterSkills = new CharacterSkills(abilityScores.Strength, abilityScores.Dexterity, abilityScores.Constitution, abilityScores.Intelligence, abilityScores.Charisma, abilityScores.Wisdom, proficiencies);
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
    }
    public class CharacterSkills
    {
        public Dictionary<string, Skill> Skills { get; set; }

        public CharacterSkills(int strength, int dexterity, int constitution, int intelligence, int charisma, int wisdom, string[] proficiencies)
        {
            Skills = new Dictionary<string, Skill>
        {
            { "Acrobatics", new Skill("Acrobatics", "Dexterity", false, 0, 0) },
            { "Animal Handling", new Skill("Animal Handling", "Wisdom", false, 0, 0) },
            { "Arcana", new Skill("Arcana", "Intelligence", false, 0, 0) },
            { "Athletics", new Skill("Athletics", "Strength", false, 0, 0) },
            { "Deception", new Skill("Deception", "Charisma", false, 0, 0) },
            { "History", new Skill("History", "Intelligence", false, 0, 0) },
            { "Insight", new Skill("Insight", "Wisdom", false, 0, 0) },
            { "Intimidation", new Skill("Intimidation", "Charisma", false, 0, 0) },
            { "Investigation", new Skill("Investigation", "Intelligence", false, 0, 0) },
            { "Medicine", new Skill("Medicine", "Wisdom", false, 0, 0) },
            { "Nature", new Skill("Nature", "Intelligence", false, 0, 0) },
            { "Perception", new Skill("Perception", "Wisdom", false, 0, 0) },
            { "Performance", new Skill("Performance", "Charisma", false, 0, 0) },
            { "Persuasion", new Skill("Persuasion", "Charisma", false, 0, 0) },
            { "Religion", new Skill("Religion", "Intelligence", false, 0, 0) },
            { "Sleight of Hand", new Skill("Sleight of Hand", "Dexterity", false, 0, 0) },
            { "Stealth", new Skill("Stealth", "Dexterity", false, 0, 0) },
            { "Survival", new Skill("Survival", "Wisdom", false, 0, 0) }
        };
            ProfSet(proficiencies);
            ScoreSet(strength, dexterity, constitution, intelligence, charisma, wisdom);
            SetBonus();

        }

        public void ProfSet(string[] profImport)
        {
            foreach (string prof in profImport)
            {
                if (Skills.ContainsKey(prof))
                {
                    Skills[prof].Proficiency = true;
                }
            }
        }
        public void ScoreSet(int strength, int dexterity, int constitution, int intelligence, int charisma, int wisdom)
        {
            foreach (Skill skill in Skills.Values)
            {
                if (skill.AbilityScoreName == "Strength")
                {
                    skill.AbilityBonus = skill.GetModifier(strength);
                }
                if (skill.AbilityScoreName == "Dexterity")
                {
                    skill.AbilityBonus = skill.GetModifier(dexterity);
                }
                if (skill.AbilityScoreName == "Constitution")
                {
                    skill.AbilityBonus = skill.GetModifier(constitution);
                }
                if (skill.AbilityScoreName == "Intelligence")
                {
                    skill.AbilityBonus = skill.GetModifier(intelligence);
                }
                if (skill.AbilityScoreName == "Wisdom")
                {
                    skill.AbilityBonus = skill.GetModifier(wisdom);
                }
                if (skill.AbilityScoreName == "Charisma")
                {
                    skill.AbilityBonus = skill.GetModifier(charisma);
                }
            }
        }
        public void SetBonus()
        {
            foreach (Skill skill in Skills.Values)
            {
                int proficiencyBonus = skill.Proficiency ? 2 : 0;
                skill.Bonus = skill.AbilityBonus + proficiencyBonus;
            }
        }
    }

    public class Skill
    {
        public string Name { get; set; }
        public string AbilityScoreName { get; set; }
        public bool Proficiency { get; set; }
        public int AbilityBonus { get; set; }
        public int Bonus { get; set; }

        public Skill(string name, string abilityScoreName, bool proficiency, int abilityScore, int bonus)
        {
            Name = name;
            AbilityScoreName = abilityScoreName;
            Proficiency = proficiency;
            AbilityBonus = abilityScore;
            Bonus = bonus;
        }

        public int GetModifier(int abilityScore)
        {
            return (int)Math.Floor((abilityScore - 10) / 2.0);
        }
    }

}
