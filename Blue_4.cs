using System;
using System.Linq;

namespace Lab_7
{
    public class Blue_4
    {
        public abstract class Team
        {
            // поля
            private string _name;
            private int[] _scores;

            // свойства
            public string Name => _name;
            public int[] Scores
            {
                get
                {
                    if (_scores == null) return null;
                    return _scores.ToArray();
                }
            }
            public int TotalScore
            {
                get
                {
                    if (_scores == null) return 0;
                    return _scores.Sum();
                }
            }

            // конструктор
            public Team(string name)
            {
                _name = name;
                _scores = new int[0];
            }

            // методы
            public void PlayMatch(int result)
            {
                if (_scores == null) return;
                Array.Resize(ref _scores, _scores.Length + 1);
                _scores[_scores.Length - 1] = result;
            }

            public virtual void Print()
            {
                Console.WriteLine($"Команда: {_name}, Сумма очков: {TotalScore}");
            }
        }

        public class ManTeam : Team
        {
            public ManTeam(string name) : base(name) { }
        }

        public class WomanTeam : Team
        {
            public WomanTeam(string name) : base(name) { }
        }

        public class Group
        {
            // поля
            private string _name;
            private ManTeam[] _manTeams;
            private WomanTeam[] _womanTeams;
            private int _manCount;
            private int _womanCount;

            // свойства
            public string Name => _name;
            public ManTeam[] ManTeams => _manTeams;
            public WomanTeam[] WomanTeams => _womanTeams;

            // конструктор
            public Group(string name)
            {
                _name = name;
                _manTeams = new ManTeam[12];
                _womanTeams = new WomanTeam[12];
                _manCount = 0;
                _womanCount = 0;
            }

            // методы
            public void Add(Team team)
            {
                if (team == null || _manTeams == null || _womanTeams == null) return;

                if (team is ManTeam manTeam && _manTeams.Length > _manCount) _manTeams[_manCount++]  = manTeam;
                else if (team is WomanTeam womanTeam && _womanCount <  _womanTeams.Length) _womanTeams[_womanCount++]  = womanTeam;
            }

            public void Add(Team[] teams)
            {
                if (teams == null || _manTeams == null || _womanTeams == null) return;
                foreach (var team in teams) Add(team);
            }

            private void SortTeams(Team[] teams, int count)
            {
                if (teams == null || count == 0) return;
                for (int i = 0; i < count - 1; i++)
                {
                    for (int j = 0; j < count - i - 1; j++)
                    {
                        if (teams[j].TotalScore < teams[j + 1].TotalScore)
                        {
                            var temp = teams[j];
                            teams[j] = teams[j + 1];
                            teams[j + 1] = temp;
                        }
                    }
                }
            }

            public void Sort()
            {
                SortTeams(_manTeams, _manCount);
                SortTeams(_womanTeams, _womanCount);
            }

            private static Group MergeTeams(Team[] team1, Team[] team2, int size)
            {
                if (size <= 0) return null;
    
                Group result = new Group("временная группа");
                int i = 0, j = 0;
                int halfSize = size / 2;

                while(i < halfSize && j < halfSize) {
                    if(i >= team1.Length || j >= team2.Length) break;
                    if (team1[i] == null || team2[j] == null) continue;
                    
                    if(team1[i].TotalScore >= team2[j].TotalScore) result.Add(team1[i++]);
                    else result.Add(team2[j++]);
                }

                while(i < halfSize && i < team1.Length) result.Add(team1[i++]);
                while(j < halfSize && j < team2.Length) result.Add(team2[j++]);

                return result;
            }

            public static Group Merge(Group group1, Group group2, int size)
            {
                if (size <= 0) return null;
                group1.Sort();
                group2.Sort();
                
                Group result = new Group("Финалисты");
                
                // мужские команды
                Group mergedMen = MergeTeams(group1.ManTeams, group2.ManTeams, size);
                result.Add(mergedMen.ManTeams);
                
                // женские команды
                Group mergedWomen = MergeTeams(group1.WomanTeams, group2.WomanTeams, size);
                result.Add(mergedWomen.WomanTeams);
                
                return result;
            }

            public void Print()
            {
                Console.WriteLine($"Группа: {_name}");
                
                Console.WriteLine("Мужские команды:");
                foreach (var team in ManTeams)
                    team.Print();
                
                Console.WriteLine("Женские команды:");
                foreach (var team in WomanTeams)
                    team.Print();
            }
        }
    }
}