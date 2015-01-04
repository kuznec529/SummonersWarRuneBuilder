using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersWarRuneBuilder
{
    class Monster
    {
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int SPD { get; private set; }
        public int CritRate { get; private set; }
        public int CritDmg { get; private set; }
        public int Res { get; private set; }
        public int Acc { get; private set; }

        public Monster()
        {
            Name = "";
            HP = 0;
            Atk = 0;
            Def = 0;
            SPD = 0;
            CritRate = 0;
            CritDmg = 0;
            Res = 0;
            Acc = 0;
        }

        public Monster(Monster other)
        {
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
            unserialize(data);
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
            HP = Int32.Parse(stats[1]);
            Atk = Int32.Parse(stats[2]);
            Def = Int32.Parse(stats[3]);
            SPD = Int32.Parse(stats[4]);
            CritRate = Int32.Parse(stats[5]);
            CritDmg = Int32.Parse(stats[6]);
            Res = Int32.Parse(stats[7]);
            Acc = Int32.Parse(stats[8]);
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
