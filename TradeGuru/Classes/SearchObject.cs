using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TradeGuru
{
    [Serializable()]
    public class SearchObject
    {
        public enum SortType { Price, Last_Seen }

        public string url { get; set; }
        public string pattern { get; set; }
        public int category1Id { get; set; } = -1;
        public int category2Id { get; set; } = -1;
        public int category3Id { get; set; } = -1;
        public int traitId { get; set; }
        public int qualityId { get; set; }
        public bool isChampionPoint { get; set; }
        public int level_min { get; set; }
        public int level_max { get; set; }
        public int voucher_min { get; set; }
        public int voucher_max { get; set; }
        public int amount_min { get; set; }
        public int amount_max { get; set; }
        public double price_min { get; set; }
        public double price_max { get; set; }
        public SortType sortType { get; set; } = SortType.Price;
        public int last_seen_max_minutes { get; set; }
    }

    public static class SearchAttributeTranslator
    {
        public enum Traits { Any_Trait = -1, Arcane = 17, Bloodthirsty = 21, Charged = 1, Decisive = 7, Defending = 4,
            Divines = 13, Harmony = 22, Healthy = 18, Impenetrable = 9, Infused = 3, Intricate = 15,
            Invigorating = 12, Nirnhorned = 14, Ornate = 16, Powered = 0, Precise = 2, Protective = 23, 
            Reinforced = 10, Robust = 19, Sharpened = 6, Special = 20, Sturdy = 8, Swift = 24, Training = 5,
            Triune = 25, Well_Fitted = 11 }
        public enum Qualities { Any_Quality = -1, Normal = 0, Fine = 1, Superior = 2, Epic = 3, Legendary = 4 }

        static class ItemCategory
        {
            public static class All_Items
            {
                public const int id = -1;
                public const bool hasTrait = true;
            }
            public static class Apparel
            {
                public const int id = 1;
                public const bool hasTrait = true;

                public static class All_Items
                {
                    public const int id = -1;
                    public const bool hasTrait = true;
                }
                public static class Accessory
                {
                    public const int id = 6;
                    public const bool hasTrait = true;
                    public enum Type { All_Items = -1, Appearance = 33, Neck = 34, Ring = 35 }
                }
                public static class Heavy_Armor
                {
                    public const int id = 4;
                    public const bool hasTrait = true;
                    public enum Type { All_Items = -1, Chest = 26, Feet = 27, Hand = 28, Head = 29, Legs = 30, Shoulders = 31, Waist = 32 }
                }
                public static class Light_Armor
                {
                    public const int id = 2;
                    public const bool hasTrait = true;
                    public enum Type { All_Items = -1, Chest = 12, Feet = 13, Hand = 14, Head = 15, Legs = 16, Shoulders = 17, Waist = 18 }
                }
                public static class Medium_Armor
                {
                    public const int id = 3;
                    public const bool hasTrait = true;
                    public enum Type { All_Items = -1, Chest = 19, Feet = 20, Hand = 21, Head = 22, Legs = 23, Shoulders = 24, Waist = 25 }
                }
                public static class Shield
                {
                    public const int id = 5;
                    public const bool hasTrait = true;
                }
            }

            public static class Crafting
            {
                public const int id = 3;
                public const bool hasTrait = true;

                public static class Alchemy
                {
                    public const int id = 11;
                    public const bool hasTrait = false;
                    public enum Type { Poison_Base = 52, Potion_Base = 36, Reagent = 37}
                }
                public static class Armor_Trait
                {
                    public const int id = 19;
                    public const bool hasTrait = true;
                }
                public static class Blacksmithing
                {
                    public const int id = 12;
                    public const bool hasTrait = false;
                    public enum Type { Material = 39, Raw_Material = 38, Temper = 40 }
                }
                public static class Clothing
                {
                    public const int id = 13;
                    public const bool hasTrait = false;
                    public enum Type { Material = 42, Raw_Material = 41, Temper = 43 }
                }
                public static class Enchanting
                {
                    public const int id = 14;
                    public const bool hasTrait = false;
                    public enum Type { Aspect_Runestone = 44, Essence_Runestone = 45, Potency_Runestone = 46 }
                }
                public static class Jewelry_Crafting
                {
                    public const int id = 41;
                    public const bool hasTrait = false;
                    public enum Type { Material = 54, Plating = 55, Raw_Material = 53 }
                }
                public static class Jewelry_Trait
                {
                    public const int id = 42;
                    public const bool hasTrait = true;
                }
                public static class Master_Writ
                {
                    public const int id = 39;
                    public const bool hasTrait = false;
                }
                public static class Motif
                {
                    public const int id = 17;
                    public const bool hasTrait = false;
                }
                public static class Provisioning
                {
                    public const int id = 15;
                    public const bool hasTrait = false;
                    public enum Type { Ingredient = 47, Recipe = 48 }
                }
                public static class Raw_Trait
                {
                    public const int id = 43;
                    public const bool hasTrait = true;
                }
                public static class Style_Material
                {
                    public const int id = 18;
                    public const bool hasTrait = false;
                }
                public static class Style_Raw_Material
                {
                    public const int id = 30;
                    public const bool hasTrait = false;
                }
                public static class Weapon_Trait
                {
                    public const int id = 20;
                    public const bool hasTrait = true;
                }
                public static class Woodworking
                {
                    public const int id = 16;
                    public const bool hasTrait = false;
                    public enum Type { Material = 50, Raw_Material = 49, Rosin = 51 }
                }
            }

            public static class Food_and_Potions
            {
                public const int id = 4;
                public const bool hasTrait = false;
                public enum Type { All_Items = -1, Drink = 22, Food = 21, Poison = 32, Potion = 23 }
            }

            public static class Furnishing
            {
                public const int id = 6;
                public const bool hasTrait = false;
                public enum Type { Crafting_Station = 33, Light = 34, Material = 40, Ornamental = 35, Recipe = 38, Seating = 36, Target_Dummy = 37 }
            }

            public static class Other
            {
                public const int id = 5;
                public const bool hasTrait = false;
                public enum Type { Bait = 24, Container = 28, Fish = 29, Misc = 31, Seige = 26, Tool = 25, Trophy = 27 }
            }

            public static class Soul_gems_and_Glyphs
            {
                public const int id = 2;
                public const bool hasTrait = false;
                public enum Type { Armor_Glyph = 8, Jewelry_Glyph = 10, Soul_Gem = 7, Weapon_Glyph = 9 }
            }

            public static class Weapon
            {
                public const int id = 0;
                public const bool hasTrait = true;

                public static class One_Hand
                {
                    public const int id = 0;
                    public const bool hasTrait = true;
                    public enum Type { All_Items = -1, Axe = 0, Dagger = 3, Mace = 1, Sword = 2 }
                }
                public static class Two_Hand
                {
                    public const int id = 1;
                    public const bool hasTrait = true;
                    public enum Type { All_Items = -1, Axe = 4, Bow = 7, Flame_Staff = 9, Frost_Staff = 10, Healing_Staff = 8, Lightning_Staff = 11, Mace = 6, Sword = 5 }
                }
            }
        }

        public static Dictionary<int, string> GetDictionaryOfCategory1()
        {
            return Helpers.GetNestedTypes(typeof(ItemCategory));
        }

        public static Dictionary<int, string> GetDictionaryOfCategory2(int category1)
        {
            if (category1 == 0)
            {
                return Helpers.GetNestedTypes(typeof(ItemCategory.Weapon));
            }
            else if (category1 == 1)
            {
                return Helpers.GetNestedTypes(typeof(ItemCategory.Apparel));
            }
            else if (category1 == 2)
            {
                return Helpers.EnumDictionary<ItemCategory.Soul_gems_and_Glyphs.Type>();
            }
            else if (category1 == 3)
            {
                return Helpers.GetNestedTypes(typeof(ItemCategory.Crafting));
            }
            else if (category1 == 4)
            {
                return Helpers.EnumDictionary<ItemCategory.Food_and_Potions.Type>();
            }
            else if (category1 == 5)
            {
                return Helpers.EnumDictionary<ItemCategory.Other.Type>();
            }
            else if (category1 == 6)
            {
                return Helpers.EnumDictionary<ItemCategory.Furnishing.Type>();
            }
            else
            {
                return null;
            }
        }

        public static Dictionary<int, string> GetDictionaryOfCategory3(int category1, int category2)
        {
            if (category1 == 0)
            {
                if (category2 == 0)
                {
                    return Helpers.EnumDictionary<ItemCategory.Weapon.One_Hand.Type>();
                }
                else if (category2 == 1)
                {
                    return Helpers.EnumDictionary<ItemCategory.Weapon.Two_Hand.Type>();
                }
                else
                {
                    return null;
                }
            }
            else if (category1 == 1)
            {
                if (category2 == 2)
                {
                    return Helpers.EnumDictionary<ItemCategory.Apparel.Light_Armor.Type>();
                }
                else if (category2 == 3)
                {
                    return Helpers.EnumDictionary<ItemCategory.Apparel.Medium_Armor.Type>();
                }
                else if (category2 == 4)
                {
                    return Helpers.EnumDictionary<ItemCategory.Apparel.Heavy_Armor.Type>();
                }
                else if (category2 == 6)
                {
                    return Helpers.EnumDictionary<ItemCategory.Apparel.Accessory.Type>();
                }
                else
                {
                    return null;
                }
            }
            else if (category1 == 3)
            {
                if (category2 == 11)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Alchemy.Type>();
                }
                else if (category2 == 12)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Blacksmithing.Type>();
                }
                else if (category2 == 13)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Clothing.Type>();
                }
                else if (category2 == 14)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Enchanting.Type>();
                }
                else if (category2 == 15)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Provisioning.Type>();
                }
                else if (category2 == 16)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Woodworking.Type>();
                }
                else if (category2 == 41)
                {
                    return Helpers.EnumDictionary<ItemCategory.Crafting.Jewelry_Crafting.Type>();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static Dictionary<int, string> GetDictionaryOfTraits()
        {
            return Helpers.EnumDictionary<Traits>();
        }

        public static Dictionary<int, string> GetDictionaryOfQualities()
        {
            return Helpers.EnumDictionary<Qualities>();
        }

        public static Traits GetItemTraitFromId(int traitId)
        {
            return (Traits)traitId;
        }

        public static Qualities GetItemQualityFromId(int qualityId)
        {
            return (Qualities)qualityId;
        }

        public static bool HasTrait(string category1Name, string category2Name, string category3Name)
        {
            var typeName1 = category1Name.UncleanString();
            var typeName2 = category2Name.UncleanString();
            var typeName3 = category3Name.UncleanString();

            var fullTypeName = BuildAssemblyQualifiedName(typeName1, typeName2, typeName3);

            var type = Helpers.GetType(fullTypeName);
            bool value = true;

            if (type != null)
            {
                var property = type.GetField("hasTrait");
                value = (bool)property.GetRawConstantValue();
            }

            return value;
        }


        private static string BuildAssemblyQualifiedName(string category1Name, string category2Name, string category3Name)
        {
            if (category1Name == String.Empty) return String.Empty;

            var type = typeof(SearchAttributeTranslator);
            var ns = type.Namespace;
            var className = type.Name;

            var fullyQualifiedName = ns + "." + className;
            fullyQualifiedName += "+ItemCategory+" + category1Name;
            if (category3Name != String.Empty)
                fullyQualifiedName += category2Name == String.Empty ? "" : "+" + category2Name;
            
            fullyQualifiedName += ", " + type.Assembly.FullName;
            
            return fullyQualifiedName;
        }

        public static Brush GetBrushForItemQuality(SearchAttributeTranslator.Qualities quality)
        {
            if (quality == SearchAttributeTranslator.Qualities.Normal ||
                quality == SearchAttributeTranslator.Qualities.Any_Quality)
                return Brushes.Gray;
            else if (quality == SearchAttributeTranslator.Qualities.Fine)
                return Brushes.Green;
            else if (quality == SearchAttributeTranslator.Qualities.Superior)
                return Brushes.Blue;
            else if (quality == SearchAttributeTranslator.Qualities.Epic)
                return Brushes.Purple;
            else if (quality == SearchAttributeTranslator.Qualities.Legendary)
                return Brushes.Gold;
            else
                return Brushes.Gray;
        }

        public static string GetSearchCategoryText(int category1, int category2, int category3)
        {
            return Helpers.CleanString(TranslateCategory(category1, category2, category3));
        }
        private static string TranslateCategory(int category1, int category2, int category3)
        {
            if (category1 == -1)
            {
                return "   All Items";
            }
            else if (category1 == 0)
            {
                var cat1 = typeof(ItemCategory.Weapon).Name;
                if (category2 == 0)
                {
                    var cat2 = typeof(ItemCategory.Weapon.One_Hand).Name;
                    var cat3 = ((ItemCategory.Weapon.One_Hand.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else
                {
                    var cat2 = typeof(ItemCategory.Weapon.Two_Hand).Name;
                    var cat3 = ((ItemCategory.Weapon.Two_Hand.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
            }
            else if (category1 == 1)
            {
                var cat1 = typeof(ItemCategory.Apparel).Name;
                if (category2 == 2)
                {
                    var cat2 = typeof(ItemCategory.Apparel.Light_Armor).Name;
                    var cat3 = ((ItemCategory.Apparel.Light_Armor.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 3)
                {
                    var cat2 = typeof(ItemCategory.Apparel.Medium_Armor).Name;
                    var cat3 = ((ItemCategory.Apparel.Medium_Armor.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 4)
                {
                    var cat2 = typeof(ItemCategory.Apparel.Heavy_Armor).Name;
                    var cat3 = ((ItemCategory.Apparel.Heavy_Armor.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 5)
                {
                    var cat2 = typeof(ItemCategory.Apparel.Shield).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 6)
                {
                    var cat2 = typeof(ItemCategory.Apparel.Accessory).Name;
                    var cat3 = ((ItemCategory.Apparel.Accessory.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else
                {
                    return String.Format("   {0}", cat1);
                }
            }
            else if (category1 == 2)
            {
                var cat1 = typeof(ItemCategory.Soul_gems_and_Glyphs).Name;
                var cat2 = ((ItemCategory.Soul_gems_and_Glyphs.Type)category2).ToString();
                return String.Format("   {0}\n   {1}", cat1, cat2);
            }
            else if (category1 == 3)
            {
                var cat1 = typeof(ItemCategory.Crafting).Name;
                if (category2 == 11)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Alchemy).Name;
                    var cat3 = ((ItemCategory.Crafting.Alchemy.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 12)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Blacksmithing).Name;
                    var cat3 = ((ItemCategory.Crafting.Blacksmithing.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 13)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Clothing).Name;
                    var cat3 = ((ItemCategory.Crafting.Clothing.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 14)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Enchanting).Name;
                    var cat3 = ((ItemCategory.Crafting.Enchanting.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 15)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Provisioning).Name;
                    var cat3 = ((ItemCategory.Crafting.Provisioning.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 16)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Woodworking).Name;
                    var cat3 = ((ItemCategory.Crafting.Woodworking.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 17)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Motif).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 18)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Style_Material).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 19)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Armor_Trait).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 20)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Weapon_Trait).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 30)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Style_Raw_Material).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 39)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Master_Writ).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 41)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Jewelry_Crafting).Name;
                    var cat3 = ((ItemCategory.Crafting.Jewelry_Crafting.Type)category3).ToString();
                    return String.Format("   {0}\n   {1}\n   {2}", cat1, cat2, cat3);
                }
                else if (category2 == 42)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Jewelry_Trait).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else if (category2 == 43)
                {
                    var cat2 = typeof(ItemCategory.Crafting.Raw_Trait).Name;
                    return String.Format("   {0}\n   {1}", cat1, cat2);
                }
                else
                {
                    return String.Format("   {0}", cat1);
                }
            }
            else if (category1 == 4)
            {
                var cat1 = typeof(ItemCategory.Food_and_Potions).Name;
                var cat2 = ((ItemCategory.Food_and_Potions.Type)category2).ToString();
                return String.Format("   {0}\n   {1}", cat1, cat2);
            }
            else if (category1 == 5)
            {
                var cat1 = typeof(ItemCategory.Other).Name;
                var cat2 = ((ItemCategory.Other.Type)category2).ToString();
                return String.Format("   {0}\n   {1}", cat1, cat2);
            }
            else if (category1 == 6)
            {
                var cat1 = typeof(ItemCategory.Furnishing).Name;
                var cat2 = ((ItemCategory.Furnishing.Type)category2).ToString();
                return String.Format("   {0}\n   {1}", cat1, cat2);
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
