using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersWarRuneBuilder
{
    class Monster
    {
        private int[] _stats;

        public string Name { get; private set; }
        public int HP 
        { 
            get 
            { 
                return _stats[(int)Stat.HP]; 
            } 
            private set 
            {
                _stats[(int)Stat.HP] = value; 
            } 
        }
        public int Atk
        {
            get
            {
                return _stats[(int)Stat.Atk];
            }
            private set
            {
                _stats[(int)Stat.Atk] = value;
            }
        }
        public int Def
        {
            get
            {
                return _stats[(int)Stat.Def];
            }
            private set
            {
                _stats[(int)Stat.Def] = value;
            }
        }
        public int SPD
        {
            get
            {
                return _stats[(int)Stat.SPD];
            }
            private set
            {
                _stats[(int)Stat.SPD] = value;
            }
        }
        public int CritRate
        {
            get
            {
                return _stats[(int)Stat.CritRate];
            }
            private set
            {
                _stats[(int)Stat.CritRate] = value;
            }
        }
        public int CritDmg
        {
            get
            {
                return _stats[(int)Stat.CritDmg];
            }
            private set
            {
                _stats[(int)Stat.CritDmg] = value;
            }
        }
        public int Res
        {
            get
            {
                return _stats[(int)Stat.Res];
            }
            private set
            {
                _stats[(int)Stat.Res] = value;
            }
        }
        public int Acc
        {
            get
            {
                return _stats[(int)Stat.Acc];
            }
            private set
            {
                _stats[(int)Stat.Acc] = value;
            }
        }


        public enum Stat
        {
            HP = 0,
            Atk = 1,
            Def = 2,
            SPD = 3,
            CritRate = 4,
            CritDmg = 5,
            Res = 6,
            Acc = 7
        }

        private void initStats()
        {
            _stats = new int[Enum.GetNames(typeof(Stat)).Length];
            for (int i = 0; i < _stats.Length; i++)
            {
                _stats[i] = 0;
            }
        }

        public Monster()
        {
            Name = "";
            initStats();
            
        }

        public Monster(Monster other)
        {
            initStats();
            this.Name = other.Name;
            this.HP = other.HP;
            this.Atk = other.Atk;
            this.Def = other.Def;
            this.SPD = other.SPD;
            this.CritRate = other.CritRate;
            this.CritDmg = other.CritDmg;
            this.Res = other.Res;
            this.Acc = other.Acc;
        }

        public Monster(string data)
        {
            initStats();
            unserialize(data);
        }

        public void setStat(Stat stat, int amount)
        {
            _stats[(int)stat] = amount;
        }

        public int getStat(Stat stat)
        {
            return _stats[(int)stat];
        }

        public void setName(string name)
        {
            Name = name;
        }
        public void setHP(int hp) 
        {
            HP = hp;
        }

        public void setAtk(int atk)
        {
            Atk = atk;
        }
        
        public void setDef(int def)
        {
            Def = def;
        }

        public void setSPD(int spd)
        {
            SPD = spd;
        }

        public void setCritRate(int crit)
        {
            CritRate = crit;
        }

        public void setCritDmg(int crit)
        {
            CritDmg = crit;
        }

        public void setRes(int res)
        {
            Res = res;
        }

        public void setAcc(int acc)
        {
            Acc = acc;
        }

        public string serialize()
        {
            return Name + "," + HP + "," + Atk + "," + Def + "," + SPD + "," + CritRate + "," + CritDmg + "," + Res + "," + Acc;
        }

        public void unserialize(string data)
        {
            string[] stats = data.Split(',');
            Name = stats[0];
            for (int i = 0; i < _stats.Length; i++)
            {
                _stats[i] = Int32.Parse(stats[i + 1]);
            }
        }

        public string ToDisplayString()
        {
            string result = "";
            result += "HP: " + HP + "\n";
            result += "Atk: " + Atk + "\n";
            result += "Def: " + Def + "\n";
            result += "SPD: " + SPD + "\n";
            result += "Crit Rate: " + CritRate + "%\n";
            result += "Crit Dmg: " + CritDmg + "%\n";
            result += "Resistance: " + Res + "%\n";
            result += "Accuracy: " + Acc + "%";
            return result;
        }

        public static List<String> getNameList(List<Monster> monsters)
        {
            List<String> result = new List<String>();
            foreach (Monster monster in monsters)
            {
                result.Add(monster.Name);
            }
            return result;
        }

        public Monster copy()
        {
            return new Monster(this);
        }

    }

}
