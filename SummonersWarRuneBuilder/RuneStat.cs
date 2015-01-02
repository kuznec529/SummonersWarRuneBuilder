using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersWarRuneBuilder
{
    public class RuneStat
    {
        
        public enum Property
        {
            None = 0,
            HP = 1,
            HPPercent = 2,
            Atk = 3,
            AtkPercent = 4,
            Def = 5,
            DefPercent = 6 ,
            SPD = 7,
            CritRate = 8,
            CritDmg = 9,
            Res = 10,
            Acc = 11
        }

        public Property property { get; private set; }
        public int amount { get; private set; }

        public RuneStat()
        {
            property = Property.None;
            amount = 0;
        }

        public RuneStat(Property property, int amount)
        {
            this.property = property;
            this.amount = amount;
        }

        public void setProperty(Property property)
        {
            this.property = property;
        }

        public void setAmount(int amount)
        {
            this.amount = amount;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            RuneStat p = obj as RuneStat;
            if ((Object)p == null)
            {
                return false;
            }
            return this.property == p.property;
        }

        public bool Equals(RuneStat p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return this.property == p.property;
        }

        public static bool operator ==(RuneStat a, RuneStat b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.property == b.property;
        }

        public static bool operator !=(RuneStat a, RuneStat b)
        {
            return !(a == b);
        }

        public override String ToString()
        {
            return property.ToString() + " " + amount.ToString();
        }

        public String ToDisplayString()
        {
            switch (property)
            {
                default:
                case Property.None: 
                    return "";
                case Property.HP:
                    return "HP +" + amount.ToString();
                case Property.HPPercent:
                    return "HP +" + amount.ToString() + "%";
                case Property.Atk:
                    return "Atk +" + amount.ToString();
                case Property.AtkPercent:
                    return "Atk +" + amount.ToString() + "%";
                case Property.Def:
                    return "Def +" + amount.ToString();
                case Property.DefPercent:
                    return "Def +" + amount.ToString() + "%";
                case Property.SPD:
                    return "SPD +" + amount.ToString();
                case Property.CritRate:
                    return "Crit Rate +" + amount.ToString() + "%";
                case Property.CritDmg:
                    return "Crit Dmg +" + amount.ToString() + "%";
                case Property.Res:
                    return "Res +" + amount.ToString() + "%";
                case Property.Acc:
                    return "Acc +" + amount.ToString() + "%";

            }
            
        }

        public static RuneStat.Property parseProperty(string property)
        {
            return (RuneStat.Property)Enum.Parse(typeof(RuneStat.Property), property, true);
        }

        public static RuneStat Parse(string stat)
        {
            string[] info = stat.Split(' ');
            if (info.Length < 2)
            {
                return new RuneStat();
            }
            else
            {
                return new RuneStat((RuneStat.Property)Enum.Parse(typeof(RuneStat.Property), info[0], true), Int32.Parse(info[1]));
            }
        }

        public static RuneStat.Property toProperty(String property)
        {
            if (property == null || property == "")
            {
                return RuneStat.Property.None;
            }
            return (RuneStat.Property)Enum.Parse(typeof(RuneStat.Property), property, true);
        }


    }

}
