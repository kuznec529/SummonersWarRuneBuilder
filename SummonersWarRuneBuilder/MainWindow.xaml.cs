using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SummonersWarRuneBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rune[] _runes;
        private int[] _star;
        private int[] _level;
        private Rune.Type[] _type;
        private Rune.Grade[] _grade;
        private RuneStat[] _primary;
        private RuneStat[] _inherent;
        private RuneStat[,] _secondary;
        private int[] _overallStats;
        private TextBlock[,] _runeStats;
        private string _filePath = "monsters.csv";
        private string[] _monsterdata;
        private List<Monster> _monsters;
        private Monster _activeMonster;
        private Monster _runedActiveMonster;

        public MainWindow()
        {
            InitializeComponent();
            LoadFile();
            InitStats();
            BindForm();
        }

        private void LoadFile()
        {
            _monsterdata = File.ReadAllLines(_filePath);
            _monsters = new List<Monster>();
            _activeMonster = new Monster();
            foreach (string line in _monsterdata)
            {
                if (line.Length != 0)
                {
                    _monsters.Add(new Monster(line));
                }
            }
            MonsterSelectionCombo.ItemsSource = Monster.getNameList(_monsters);
        }

        private void BindForm()
        {
            _runeStats = new TextBlock[6, 6] { {RuneStats1Primary, RuneStats1Inherent, RuneStats1Secondary1, RuneStats1Secondary2, RuneStats1Secondary3, RuneStats1Secondary4},
                                               {RuneStats2Primary, RuneStats2Inherent, RuneStats2Secondary1, RuneStats2Secondary2, RuneStats2Secondary3, RuneStats2Secondary4},
                                               {RuneStats3Primary, RuneStats3Inherent, RuneStats3Secondary1, RuneStats3Secondary2, RuneStats3Secondary3, RuneStats3Secondary4},
                                               {RuneStats4Primary, RuneStats4Inherent, RuneStats4Secondary1, RuneStats4Secondary2, RuneStats4Secondary3, RuneStats4Secondary4},
                                               {RuneStats5Primary, RuneStats5Inherent, RuneStats5Secondary1, RuneStats5Secondary2, RuneStats5Secondary3, RuneStats5Secondary4},
                                               {RuneStats6Primary, RuneStats6Inherent, RuneStats6Secondary1, RuneStats6Secondary2, RuneStats6Secondary3, RuneStats6Secondary4}};
        }

        private void InitStats()
        {
            _runes = new Rune[6];
            _star = new int[6];
            _level = new int[6];
            _type = new Rune.Type[6];
            _grade = new Rune.Grade[6];
            _primary = new RuneStat[6];
            _inherent = new RuneStat[6];
            _secondary = new RuneStat[6, 4];
            for (int i = 0; i < 6; i++)
            {
                _runes[i] = new Rune(i+1);
                _primary[i] = new RuneStat();
                _inherent[i] = new RuneStat();
                for (int j = 0; j < 4; j++)
                {
                    _secondary[i, j] = new RuneStat();
                }
            }
            _overallStats = new int[Enum.GetNames(typeof(RuneStat.Property)).Length];

        }

        private void Rune1_Click(object sender, RoutedEventArgs e)
        {
            RuneClick(sender, e, 1);
        }

        private void Rune2_Click(object sender, RoutedEventArgs e)
        {
            RuneClick(sender, e, 2);
        }

        private void Rune3_Click(object sender, RoutedEventArgs e)
        {
            RuneClick(sender, e, 3);
        }

        private void Rune4_Click(object sender, RoutedEventArgs e)
        {
            RuneClick(sender, e, 4);
        }

        private void Rune5_Click(object sender, RoutedEventArgs e)
        {
            RuneClick(sender, e, 5);
        }

        private void Rune6_Click(object sender, RoutedEventArgs e)
        {
            RuneClick(sender, e, 6);
        }

        private void RuneClick(object sender, RoutedEventArgs e, int rune)
        {
            if ((sender as System.Windows.Controls.Primitives.ToggleButton).IsChecked ?? false)
            {
                RuneWindow runeWin = new RuneWindow(_runes[rune-1]);
                runeWin.DialogFinished += new EventHandler<WindowEventArgs>(runeWin_DialogFinished);
                runeWin.Show();
            }
        }

        void runeWin_DialogFinished(object sender, WindowEventArgs e)
        {
            
            String statString = e.Message;
            String[] stats = statString.Split(',');
            int slot = Int32.Parse(stats[0])-1;
            _level[slot] = Int32.Parse(stats[1]);
            _type[slot] = (Rune.Type)Enum.Parse(typeof(Rune.Type), stats[2], true);
            _star[slot] = Int32.Parse(stats[3]);
            _primary[slot] = RuneStat.Parse(stats[4]);
            _inherent[slot] = RuneStat.Parse(stats[5]);

            for (int i = 0; i < 4; i++)
            {
                _secondary[slot, i] = new RuneStat();
            }
            for (int i = 6; i < Math.Min(stats.Length - 1,10); i++)
            {
                _secondary[slot, i - 6] = RuneStat.Parse(stats[i]);
            }

            calculateOverallStats();
            calculateMonsterOverall();
            displayStats(slot);
            displayOverallStats();
            displayMonsterOverall();
        }

        private void displayStats(int i)
        {
            String primaryHeader = "Pri: ";
            String secondaryHeader = "Sec: ";
            String inherentHeader = "Inh: ";
            for (int j = 0; j < 6; j++)
            {
                _runeStats[i, j].Text = "";
            }

            if (_primary[i].property != RuneStat.Property.None)
            {
                _runeStats[i, 0].Text = primaryHeader + _primary[i].ToDisplayString();
            }
            if (_inherent[i].property != RuneStat.Property.None)
            {
                _runeStats[i, 1].Text = inherentHeader + _inherent[i].ToDisplayString();
            }
            if (_secondary[i, 0].property != RuneStat.Property.None)
            {
                _runeStats[i, 2].Text = secondaryHeader + _secondary[i, 0].ToDisplayString();
            }
            if (_secondary[i, 1].property != RuneStat.Property.None)
            {
                _runeStats[i, 3].Text = secondaryHeader + _secondary[i, 1].ToDisplayString();
            }
            if (_secondary[i, 2].property != RuneStat.Property.None)
            {
                _runeStats[i, 4].Text = secondaryHeader + _secondary[i, 2].ToDisplayString();
            }
            if (_secondary[i, 3].property != RuneStat.Property.None)
            {
                _runeStats[i, 5].Text = secondaryHeader + _secondary[i, 3].ToDisplayString();
            }
        }

        private void calculateOverallStats()
        {
            for (int i = 0; i < _overallStats.Length; i++)
            {
                _overallStats[i] = 0;
            }

            for (int i = 0; i < 6; i++)
            {
                _overallStats[(int)_primary[i].property] += _primary[i].amount;
                _overallStats[(int)_inherent[i].property] += _inherent[i].amount;
                for (int j = 0; j < 4; j++)
                {
                    _overallStats[(int)_secondary[i, j].property] += _secondary[i, j].amount;
                }
            }
        }

        private void calculateMonsterOverall()
        {
            _runedActiveMonster = new Monster();
            _runedActiveMonster.setName(_activeMonster.Name);
            _runedActiveMonster.setHP(calculateStat(_activeMonster.HP, _overallStats[(int)RuneStat.Property.HPPercent], _overallStats[(int)RuneStat.Property.HP]));
            _runedActiveMonster.setAtk(calculateStat(_activeMonster.Atk, _overallStats[(int)RuneStat.Property.AtkPercent], _overallStats[(int)RuneStat.Property.Atk]));
            _runedActiveMonster.setDef(calculateStat(_activeMonster.Def, _overallStats[(int)RuneStat.Property.DefPercent], _overallStats[(int)RuneStat.Property.Def]));
            _runedActiveMonster.setSPD(_activeMonster.SPD + _overallStats[(int)RuneStat.Property.SPD]);
            _runedActiveMonster.setCritRate(_activeMonster.CritRate + _overallStats[(int)RuneStat.Property.CritRate]);
            _runedActiveMonster.setCritDmg(_activeMonster.CritDmg + _overallStats[(int)RuneStat.Property.CritDmg]);
            _runedActiveMonster.setRes(_activeMonster.Res + _overallStats[(int)RuneStat.Property.Res]);
            _runedActiveMonster.setAcc(_activeMonster.Acc + _overallStats[(int)RuneStat.Property.Acc]);
                        
        }

        private void displayOverallStats()
        {
            string result = "";
            for (int i = 1; i < _overallStats.Length; i++)
            {
                if (_overallStats[i] != 0)
                {
                    result += (new RuneStat((RuneStat.Property)i, _overallStats[i])).ToDisplayString() + "\n";
                }
            }
            OverallStats.Text = result;
        }

        private void displayMonsterOverall()
        {
            NewStats.Text = _runedActiveMonster.ToDisplayString();
        }

        private void MonsterSelectionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _activeMonster = _monsters[MonsterSelectionCombo.SelectedIndex];
            OriginalStats.Text = _activeMonster.ToDisplayString();
            calculateMonsterOverall();
            displayMonsterOverall();
        }

        private int calculateStat(int baseAmount, int percentageIncrease, int staticIncrease)
        {
            double amount = (double)baseAmount;
            return (int)(amount * (100 + percentageIncrease) / 100.0 + staticIncrease);
        }

    }


}
