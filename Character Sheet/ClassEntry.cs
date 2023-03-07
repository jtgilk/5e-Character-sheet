using Newtonsoft.Json;

namespace CharSheet
{
    public class ClassEntry
    {
        [JsonProperty(PropertyName = "name")]
        public string ClassName { get; set; }

        [JsonProperty(PropertyName = "hit_dice")]
        public int HitDice { get; set; }

        [JsonProperty(PropertyName = "proficiency_choices")]
        public ProficiencyChoices[] ProficiencyChoices { get; set; }

        [JsonProperty(PropertyName = "proficiencies")]
        public Proficiencies[] Proficiencies { get; set; }

        [JsonProperty(PropertyName = "saving_throws")]
        public SavingThrows[] SavingThrows { get; set; }

        [JsonProperty(PropertyName = "starting_equipment")]
        public StartingEquip[] StartingEquip { get; set; }

        [JsonProperty(PropertyName = "starting_equipment_options")]
        public StartingEquipOptions[] StartingEquipOptions { get; set; }

        [JsonProperty(PropertyName = "class_levels")]
        public string ClassLevelsUrl { get; set; }

        [JsonProperty(PropertyName = "subclasses")]
        public Subclasses[] Subclasses { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    public class Proficiencies
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }
    }
    public class ProficiencyChoices
    {
        [JsonProperty(PropertyName = "desc")]
        public string ProfChoiceDesc { get; set; }

        [JsonProperty(PropertyName = "choose")]
        public int PropChoiceNum { get; set; }

        [JsonProperty(PropertyName = "from")]
        public ProfChoiceFrom PropChoiceFrom { get; set; }
    }

    public class ProfChoiceFrom
    {
        [JsonProperty(PropertyName = "options")]
        public ProfChoiceOptions ProfChoiceOptions { get; set; }
    }
    public class ProfChoiceOptions
    {
        [JsonProperty(PropertyName = "item")]
        public ProfChoiceOptionsItems ProfChoiceOptionsItems { get; set; }
    }
    public class ProfChoiceOptionsItems
    {
        [JsonProperty(PropertyName = "name")]
        public string ProfChoiceOptionsItemsName { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
    public class SavingThrows
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }
    }
    public class StartingEquip
    {
        [JsonProperty(PropertyName = "equipment")]
        public StartingEquipEquipment StartingEquipEquipment { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public double quantity { get; set; }
    }
    public class StartingEquipEquipment
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }
    }
    public class StartingEquipOptions
    {
        [JsonProperty(PropertyName = "desc")]
        public string StartingEquipOptionsDesc { get; set; }
    }
    public class Subclasses
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }
    }

}