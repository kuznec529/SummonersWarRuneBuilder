﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersWarRuneBuilder
{
    public class Rune
    {

        public enum Type
        {
            Energy,
            Fatal,
            Blade,
            Rage,
            Swift,
            Focus,
            Guard,
            Endure,
            Violent,
            Will,
            Nemesis,
            Shield,
            Revenge,
            Despair,
            Vampire
        }

        public enum Grade
        {
            Common = 0,
            Magic = 1,
            Rare = 2,
            Hero = 3,
            Legendary = 4
        }


        public int level { get; private set; }
        public int star { get; private set; }
        public Type type { get; private set; }
        public int slot { get; private set; }
        private RuneStat.Property _primary;
        private RuneStat _inherent;
        private RuneStat[] _secondary;
        private RuneStat[] _upgrades;
        private RuneStat[] _overallSecondary;
        
        private static readonly int[,] _initialStatTable = new int[,] {{0,0,0,0,0,0},  //None
                                                                {40,70,100,160,270,360}, //HP
                                                                {1,2,4,5,8,11}, //HPPercent
                                                                {3,5,7,10,15,22}, //Atk
                                                                {1,2,4,5,8,11}, //AtkPercent
                                                                {3,5,7,10,15,22}, //Def
                                                                {1,2,4,5,8,11}, //DefPercent
                                                                {1,2,3,4,5,7}, //Spd
                                                                {1,2,3,4,6,7}, //CritRate
                                                                {2,3,4,6,8,11}, //CritDmg
                                                                {1,2,4,6,9,12}, //Acc
                                                                {1,2,4,6,9,12}}; //Res

        private static readonly int[,] _statIncrementTable = new int[12, 6] {{0,0,0,0,0,0},  //None
                                                                  {45,60,75,90,105,120}, //HP
                                                                  {1,1,2,2,2,3}, //HPPercent
                                                                  {3,4,5,6,7,8}, //Atk
                                                                  {1,1,2,2,2,3}, //AtkPercent
                                                                  {3,4,5,6,7,8}, //Def
                                                                  {1,1,2,2,2,3}, //DefPercent
                                                                  {1,1,1,1,2,2}, //Spd
                                                                  {1,1,2,2,2,3}, //CritRate
                                                                  {1,2,2,3,3,4}, //CritDmg
                                                                  {1,1,2,2,2,3}, //Acc
                                                                  {1,1,2,2,2,3}}; //Res


        private static readonly int[, ,] _secondaryStatRange = new int[12, 6, 2] { { {0,0},{0,0},{0,0},{0,0},{0,0},{0,0} }, //None
                                                                                 { {11,60},{27,117},{49,168},{80,222},{119,279},{166,339} }, //HP
                                                                                 { {1,2},{1,4},{2,5},{3,7},{4,8},{5,10} }, //HPPercent
                                                                                 { {3,5},{4,7},{5,10},{7,12},{8,15},{10,23} }, //Atk
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //AtkPercent
                                                                                 { {3,5},{4,7},{5,10},{7,12},{8,15},{10,23} }, //Def
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //DefPercent
                                                                                 { {1,2},{1,3},{2,3},{3,4},{3,5},{4,6} }, //SPD
                                                                                 { {1,2},{1,3},{2,3},{3,4},{3,5},{4,6} }, //CritRate
                                                                                 { {1,4},{2,5},{2,7},{2,8},{3,10},{4,12} }, //CritDmg
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //Res
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} } }; //Acc

        private static readonly int[, ,] _upgradeAmountRange = new int[12, 6, 2] { { {0,0},{0,0},{0,0},{0,0},{0,0},{0,0} }, //None
                                                                                 { {14,54},{33,80},{55,115},{81,166},{113,240},{149,344} }, //HP
                                                                                 { {1,2},{1,4},{2,5},{2,6},{3,7},{4,8} }, //HPPercent
                                                                                 { {3,5},{4,7},{5,10},{7,12},{9,15},{11,18} }, //Atk
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //AtkPercent
                                                                                 { {3,5},{4,7},{5,10},{7,12},{9,15},{11,18} }, //Def
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //DefPercent
                                                                                 { {1,2},{1,3},{2,3},{3,4},{3,5},{4,6} }, //SPD
                                                                                 { {1,2},{1,3},{2,3},{3,4},{3,5},{4,6} }, //CritRate
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //CritDmg
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} }, //Res
                                                                                 { {1,3},{1,4},{2,5},{2,6},{3,7},{4,8} } }; //Acc

        private static readonly int[] _runeSetCount = new int[15] { 2, 4, 2, 4, 4, 2, 2, 2, 4, 2, 2, 2, 2, 4, 4 };

        public static readonly int[] Stars = new int[6] { 1, 2, 3, 4, 5, 6 };
        public static readonly int[] Levels = new int[16] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public Rune(int slot)
        {
            level = 0;
            star = 1;
            this.slot = slot;
            _inherent = new RuneStat();
            _upgrades = new RuneStat[4];
            _secondary = new RuneStat[4];
            _overallSecondary = new RuneStat[4];
            for (int i = 0; i < 4; i++)
            {
                _upgrades[i] = new RuneStat();
                _secondary[i] = new RuneStat();
                _overallSecondary[i] = new RuneStat();
            }
        }


        public void setStar(int star)
        {
            this.star = star;
        }

        public void setType(Type type)
        {
            this.type = type;
        }

        public void setProperties(int star, Type type)
        {
            this.star = star;
            this.type = type;
        }

        public List<RuneStat.Property> getPrimaryPropertySelection()
        {
            List<RuneStat.Property> selection = new List<RuneStat.Property>();
            if (slot == 1)
            {
                selection.Add(RuneStat.Property.Atk);
                return selection;
            }
            else if (slot == 3)
            {
                selection.Add(RuneStat.Property.Def);
                return selection;
            }
            else if (slot == 5)
            {
                selection.Add(RuneStat.Property.HP);
                return selection;
            }
            else
            {
                selection.Add(RuneStat.Property.Atk);
                selection.Add(RuneStat.Property.Def);
                selection.Add(RuneStat.Property.HP);
                selection.Add(RuneStat.Property.AtkPercent);
                selection.Add(RuneStat.Property.DefPercent);
                selection.Add(RuneStat.Property.HPPercent);
                if (slot == 2)
                {
                    selection.Add(RuneStat.Property.SPD);
                    return selection;
                }
                else if (slot == 3)
                {
                    selection.Add(RuneStat.Property.CritRate);
                    selection.Add(RuneStat.Property.CritDmg);
                    return selection;
                }
                else
                {
                    selection.Add(RuneStat.Property.Acc);
                    selection.Add(RuneStat.Property.Res);
                    return selection;
                }
            }

        }

        public Boolean setPrimaryProperty(RuneStat.Property primaryStat)
        {
            if (validPrimaryStat(primaryStat))
            {
                _primary = primaryStat;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean setInherent(RuneStat stat)
        {
            _inherent = stat;
            return true;
        }

        public Boolean setSecondaries(List<RuneStat> stats)
        {
            for (int i = 0; i < 4; i++)
            {
                _secondary[i] = new RuneStat();
            }

            for (int i = 0; i < stats.Count; i++)
            {
                _secondary[i] = stats[i];
            }
            calculateSecondaries();
            return true;
        }

        public Boolean setSecondary(int i, RuneStat stat)
        {
            _secondary[i] = stat;
            calculateSecondaries();
            return true;
        }

        public Boolean setUpgrades(List<RuneStat> stats)
        {
            for (int i = 0; i < 4; i++)
            {
                _upgrades[i] = new RuneStat();
            }

            for (int i = 0; i < stats.Count; i++)
            {
                _upgrades[i] = stats[i];
            }
            calculateSecondaries();
            return true;
        }

        public Boolean setUpgrade(int i, RuneStat stats)
        {
            _upgrades[i] = stats;
            calculateSecondaries();
            return true;
        }

        public Boolean setLevel(int level)
        {
            if (level <= 15)
            {
                this.level = level;
                return true;
            }
            else
            {
                return false;
            }
        }

        public RuneStat getPrimary()
        {
            return new RuneStat(_primary, getPrimaryStatAmount(_primary, level, star));
        }

        public RuneStat getInherent()
        {
            return _inherent;
        }

        public RuneStat getSecondary(int i)
        {
            return _secondary[i];
        }

        private RuneStat getUpgrade(int i)
        {
            return _upgrades[i];
        }

        private void calculateSecondaries()
        {
            List<RuneStat> stats = new List<RuneStat>();
            foreach (RuneStat stat in _secondary)
            {
                if (stat.property != RuneStat.Property.None && stat.amount > 0)
                {
                    stats.Add(stat.copy());
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (i < stats.Count)
                {
                    _overallSecondary[i] = stats[i];
                }
                else
                {
                    _overallSecondary[i] = new RuneStat();
                }
            }

            foreach (RuneStat upgrade in _upgrades)
            {
                for (int i = 0; i < _overallSecondary.Length; i++)
                {
                    if (_overallSecondary[i].property == upgrade.property)
                    {
                        _overallSecondary[i].setAmount(_overallSecondary[i].amount + upgrade.amount);
                        break;
                    }
                    else if (_overallSecondary[i].property == RuneStat.Property.None)
                    {
                        _overallSecondary[i] = upgrade.copy();
                        break;
                    }

                }
            }
        }

        public List<RuneStat> getOverallSecondaries()
        {
            List<RuneStat> result = new List<RuneStat>();
            foreach (RuneStat stat in _overallSecondary)
            {
                if (stat.property != RuneStat.Property.None)
                {
                    result.Add(stat);
                }
            }
            return result;
        }

        public static int[] getSecondaryBaseRange(RuneStat.Property property, int star)
        {
            return new int[2] { _secondaryStatRange[(int)property, star - 1, 0], _secondaryStatRange[(int)property, star - 1, 1] };
        }

        public static int[] getSecondaryIncrementRange(RuneStat.Property property, int star)
        {
            return new int[2] { _upgradeAmountRange[(int)property, star-1, 0], _upgradeAmountRange[(int)property, star-1, 1] };
        }

        private Boolean validPrimaryStat(RuneStat.Property primaryStat) 
        {
            return true;
        }

        private int getPrimaryStatAmount(RuneStat.Property property, int level, int star)
        {
            int result = 0;
            if (level > 14)
            {
                result = (int)((_initialStatTable[(int)property, star - 1] + _statIncrementTable[(int)property, star - 1] * 14) * 1.2);
            }
            else
            {
                result = _initialStatTable[(int)property, star - 1] + _statIncrementTable[(int)property, star - 1] * level;
            }

            return result;
        }

        public static List<Type> getSetBonuses(List<Rune> runes)
        {
            int[] runeTypes = new int[15];
            for (int i = 0; i < runeTypes.Length; i++)
            {
                runeTypes[i] = 0;
            }

            foreach (Rune rune in runes)
            {
                runeTypes[(int)rune.type] += 1;
            }

            List<Type> result = new List<Type>();
            for (int i = 0; i < runeTypes.Length; i++)
            {
                for (int j = 0; j < runeTypes[i] / _runeSetCount[i]; j++)
                {
                    result.Add((Type)i);
                }
            }
            return result;
        }

        public override String ToString()
        {
            String result = slot.ToString() + ",";
            result += level.ToString() + ",";
            result += type.ToString() + ",";
            result += star.ToString() + ",";
            result += getPrimary().ToString() + ",";
            result += _inherent.ToString() + ",";
            List<RuneStat> secondaries = getOverallSecondaries();

            foreach (RuneStat item in secondaries)
            {
                result += item.ToString();
                result += ",";
            } 
            
            return result;
        }

        public static List<RuneStat> getCombinedStats(List<Rune> runes)
        {
            int[] overallStats = new int[12];
            List<RuneStat> result = new List<RuneStat>();
            foreach (Rune rune in runes)
            {
                RuneStat primary = rune.getPrimary();
                RuneStat inherent = rune.getInherent();
                List<RuneStat> secondaries = rune.getOverallSecondaries();
                overallStats[(int)primary.property] += primary.amount;
                overallStats[(int)inherent.property] += inherent.amount;
                foreach (RuneStat stat in secondaries)
                {
                    overallStats[(int)stat.property] += stat.amount;
                }
            }

            for (int i = 1; i < overallStats.Length; i++)
            {
                if (overallStats[i] != 0)
                {
                    result.Add(new RuneStat((RuneStat.Property)i, overallStats[i]));
                }
            }

            return result;

        }

    }
}
