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
        private static string _FilePath = "monsters.csv";

        private Rune[] _runes;
        private Boolean[] _completedRunes;
        private List<Rune.Type> _runeBonuses;
        private List<RuneStat> _overallStats;
        private double[] _additionalStats;
        private TextBlock[,] _runeStats;
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
            _monsterdata = File.ReadAllLines(_FilePath);
            _monsters = new List<Monster>();
            _activeMonster = new Monster();
            _runeBonuses = new List<Rune.Type>();
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
            _completedRunes = new Boolean[6];
            _additionalStats = new double[Enum.GetNames(typeof(Monster.Stat)).Length];

            for (int i = 0; i < 6; i++)
            {
                _runes[i] = new Rune(i+1);
                _completedRunes[i] = false;
            }

            for (int i = 0; i < _additionalStats.Length; i++)
            {
                _additionalStats[i] = 0;
            }

            _overallStats = new List<RuneStat>();

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
            _completedRunes[slot] = true;
            displayStats(slot);
            updateRuneBonuses();
            updateOverallStats();
            updateMonsterOverall();
        }

        private void displayStats(int i)
        {
            String primaryHeader = "";
            String secondaryHeader = "";
            String inherentHeader = "";
            for (int j = 0; j < 6; j++)
            {
                _runeStats[i, j].Text = "";
            }

            if (_runes[i].getPrimary().property != RuneStat.Property.None)
            {
                _runeStats[i, 0].Text = primaryHeader + _runes[i].getPrimary().ToDisplayString();
            }
            if (_runes[i].getInherent().property != RuneStat.Property.None)
            {
                _runeStats[i, 1].Text = inherentHeader + _runes[i].getInherent().ToDisplayString();
            }
            List<RuneStat> stats = _runes[i].getOverallSecondaries();
            for (int j = 0; j < 4; j++)
            {
                if (j < stats.Count && stats[j].property != RuneStat.Property.None)
                {
                    _runeStats[i, j + 2].Text = secondaryHeader + stats[j].ToDisplayString();
                }
                else
                {
                    _runeStats[i, j + 2].Text = "";
                }
            }
        }

        private void updateMonsterOverall()
        {
            _runedActiveMonster = new Monster(_activeMonster);
            int[] overallStats = new int[Enum.GetNames(typeof(RuneStat.Property)).Length];
            
            for (int i = 0; i < _additionalStats.Length; i++)
            {
                _additionalStats[i] = 0;
            }

            for (int i = 0; i < overallStats.Length; i++)
            {
                overallStats[i] = 0;
            }

            foreach (RuneStat stat in _overallStats)
            {
                overallStats[(int)stat.property] += stat.amount;
            }

            foreach (Rune.Type type in _runeBonuses)
            {
                switch (type)
                {
                    default:
                        break;
                    case Rune.Type.Energy:
                        _additionalStats[(int)Monster.Stat.HP] += _activeMonster.HP * 0.15;
                        break;
                    case Rune.Type.Fatal:
                        _additionalStats[(int)Monster.Stat.Atk] += _activeMonster.Atk * 0.3;
                        break;
                    case Rune.Type.Blade:
                        _additionalStats[(int)Monster.Stat.CritRate] += 12;
                        break;
                    case Rune.Type.Rage:
                        _additionalStats[(int)Monster.Stat.CritDmg] += 40;
                        break;
                    case Rune.Type.Swift:
                        _additionalStats[(int)Monster.Stat.SPD] += _activeMonster.SPD * 0.25;
                        break;
                    case Rune.Type.Focus:
                        _additionalStats[(int)Monster.Stat.Acc] += 20;
                        break;
                    case Rune.Type.Guard:
                        _additionalStats[(int)Monster.Stat.Def] += _activeMonster.Def * 0.15;
                        break;
                    case Rune.Type.Endure:
                        _additionalStats[(int)Monster.Stat.Res] += 20;
                        break;
                    case Rune.Type.Violent:
                    case Rune.Type.Will:
                    case Rune.Type.Nemesis:
                    case Rune.Type.Shield:
                    case Rune.Type.Revenge:
                    case Rune.Type.Despair:
                    case Rune.Type.Vampire:
                        //No implementation so far
                        break;


                }
            }
            
            _additionalStats[(int)Monster.Stat.HP] += calculateStatIncrease(_activeMonster.HP, overallStats[(int)RuneStat.Property.HPPercent], overallStats[(int)RuneStat.Property.HP]);
            _additionalStats[(int)Monster.Stat.Atk] += calculateStatIncrease(_activeMonster.Atk, overallStats[(int)RuneStat.Property.AtkPercent], overallStats[(int)RuneStat.Property.Atk]);
            _additionalStats[(int)Monster.Stat.Def] += calculateStatIncrease(_activeMonster.Def, overallStats[(int)RuneStat.Property.DefPercent], overallStats[(int)RuneStat.Property.Def]);
            _additionalStats[(int)Monster.Stat.SPD] += overallStats[(int)RuneStat.Property.SPD];
            _additionalStats[(int)Monster.Stat.CritRate] += overallStats[(int)RuneStat.Property.CritRate];
            _additionalStats[(int)Monster.Stat.CritDmg] += overallStats[(int)RuneStat.Property.CritDmg];
            _additionalStats[(int)Monster.Stat.Res] += overallStats[(int)RuneStat.Property.Res];
            _additionalStats[(int)Monster.Stat.Acc] += overallStats[(int)RuneStat.Property.Acc];

            for (int i = 0; i < _additionalStats.Length; i++)
            {
                _runedActiveMonster.setStat((Monster.Stat)i, _activeMonster.getStat((Monster.Stat)i) + (int)_additionalStats[i]);
            }

                
            displayMonsterOverall();
        }

        private void updateRuneBonuses()
        {
            _runeBonuses = Rune.getSetBonuses(getCompletedRunes());
            string result = "";
            foreach (Rune.Type type in _runeBonuses)
            {
                result += type.ToString() + "\n";
            }
            RuneBonus.Text = result;
        }

        private void updateOverallStats()
        {
            string result = "";
            
            _overallStats = Rune.getCombinedStats(getCompletedRunes());
            foreach (RuneStat stat in _overallStats)
            {

                result += stat.ToDisplayString() + "\n";
            }
            OverallStats.Text = result;
        }

        private List<Rune> getCompletedRunes()
        {
            List<Rune> result = new List<Rune>();
            for (int i = 0; i < 6; i++)
            {
                if (_completedRunes[i])
                {
                    result.Add(_runes[i]);
                }
            }
            return result;
        }

        private void displayMonsterOverall()
        {
            NewStats.Text = _runedActiveMonster.ToDisplayString();
        }

        private void MonsterSelectionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _activeMonster = _monsters[MonsterSelectionCombo.SelectedIndex];
            OriginalStats.Text = _activeMonster.ToDisplayString();
            updateMonsterOverall();
        }

        private int calculateStat(int baseAmount, int percentageIncrease, int staticIncrease)
        {
            double amount = (double)baseAmount;
            return (int)(amount * (100 + percentageIncrease) / 100.0 + staticIncrease);
        }

        private int calculateStatIncrease(int baseAmount, int percentageIncrease, int staticIncrease)
        {
            double amount = (double)baseAmount;
            return (int)(amount * percentageIncrease / 100.0 + staticIncrease);
        }


    }


}
