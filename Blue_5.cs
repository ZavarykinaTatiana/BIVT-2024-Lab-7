using System;

namespace Lab_7
{
    public class Blue_5
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private int _place;
            private bool _flag;

            public string Name => _name;
            public string Surname => _surname;
            public int Place => _place;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _place = 0;
                _flag = true;
            }

            public void SetPlace(int place)
            {
                if (_flag)
                {
                    _place = place;
                    _flag = false;
                }
            }

            public void Print()
            {
                Console.WriteLine($"Спортсмен: {_name}, Место: {_place}");
            }
        }

        public abstract class Team
        {
            private string _name;
            protected Sportsman[] _sportsmen;
            private int _count;

            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;
            
            public int SummaryScore
            {
                get
                {
                    int result = 0;
                    if (_sportsmen == null) return 0;
                    foreach (var sportsman in _sportsmen)
                    {
                        if (sportsman == null) continue;
                        result += sportsman.Place switch
                        {
                            1 => 5,
                            2 => 4,
                            3 => 3,
                            4 => 2,
                            5 => 1,
                            _ => 0
                        };
                    }
                    return result;
                }
            }

            public int TopPlace
            {
                get
                {
                    if (_sportsmen == null) return 0;
                    int result = 18;
                    for (int i = 0; i < _count; i++)
                    {
                        if (_sportsmen[i] != null && _sportsmen[i].Place > 0 && _sportsmen[i].Place < result) result = _sportsmen[i].Place;
                    }
                    return result;
                }
            }

            public Team(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[6];
                _count = 0;
            }

            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null || sportsman == null || _sportsmen.Length == 0 || _count == 6) return;
                _sportsmen[_count++] = sportsman;
            }

            public void Add(Sportsman[] sportsmen)
            {
                if (_sportsmen == null) return;
                foreach (var sportsman in sportsmen) Add(sportsman);
            }

            public static void Sort(Team[] teams)
            {
                if (teams == null || teams.Length == 0) return;
                int n = teams.Length;
                for (int i=0; i < n-1; i++) {
                    for (int j=0; j < n-i-1; j++) {
                        if (teams[j].SummaryScore < teams[j + 1].SummaryScore ||
                            (teams[j].SummaryScore == teams[j + 1].SummaryScore && teams[j].TopPlace > teams[j + 1].TopPlace))
                        {
                            Team temp = teams[j];
                            teams[j] = teams[j + 1];
                            teams[j + 1] = temp;
                        }
                    }
                }
            }

            protected abstract double GetTeamStrength();

            public static Team GetChampion(Team[] teams)
            {
                if (teams == null) return null;

                Team answer = teams[0];
                double maxStrength = answer.GetTeamStrength();

                for(int i=0; i < teams.Length; i++)
                {
                    double teamStrength = teams[i].GetTeamStrength();
                    if (teamStrength > maxStrength)
                    {
                        maxStrength = teamStrength;
                        answer = teams[i];
                    }
                }
                return answer;
            }

            public void Print()
            {
                Console.WriteLine($"Команда: {Name}, Очки: {SummaryScore}, Лучшее место: {TopPlace}");
            }
        }

        public class ManTeam : Team
        {
            public ManTeam(string name) : base(name) { }

            protected override double GetTeamStrength()
            {   
                int sum = 0;
                int count = 0;
                foreach (var sportsman in Sportsmen)
                {
                    if (sportsman != null)
                    {
                        sum += sportsman.Place;
                        count++;
                    }
                }
                return count > 0 ? 100.0 / (sum / (double)count) : 0;
            }
        }

        public class WomanTeam : Team
        {
            public WomanTeam(string name) : base(name) { }

            protected override double GetTeamStrength()
            {
                int sumPlaces = 0;
                int productPlaces = 1;
                int count = 0;
                
                foreach (var sportsman in Sportsmen)
                {
                    if (sportsman != null)
                    {
                        sumPlaces += sportsman.Place;
                        productPlaces *= sportsman.Place;
                        count++;
                    }
                }
                
                return productPlaces == 0 ? 0 : 100*sumPlaces*count/productPlaces;
            }
        }
    }
}
