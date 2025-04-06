using System;
using System.Linq;

namespace Lab_7 {
    public class Blue_3
    {
        public class Participant {
            // приватные поля
            private string _name;
            private string _surname;
            protected int[] _penaltyTimes; // массив штрафных штук за каждый матч


            // публичные свойства
            public string Name => _name;
            public string Surname => _surname;
            public int[] Penalties {
                get {
                    if (_penaltyTimes == null) return null;
                    int[] copyArr = new int[_penaltyTimes.Length];
                    Array.Copy(_penaltyTimes, copyArr, _penaltyTimes.Length);
                    return copyArr;
                }
            }

            public int Total {
                get {
                    if (_penaltyTimes == null) return 0;
                    return _penaltyTimes.Sum();
                }
            }

            public virtual bool IsExpelled => _penaltyTimes.Any(t => t == 10);

            // конструктор
            public Participant(string name, string surname) {
                _name = name;
                _surname = surname;
                _penaltyTimes = new int[0];
            }

            // методы
            public virtual void PlayMatch(int time) {
                if (_penaltyTimes == null) return;
                Array.Resize(ref _penaltyTimes, _penaltyTimes.Length + 1);
                _penaltyTimes[_penaltyTimes.Length - 1] = time;
                            
            }

            public static void Sort(Participant[] array) {
                if (array == null || array.Length == 0) return;
                for (int i = 0; i < array.Length; i++) {
                    for (int j = 0; j < array.Length-i-1; j++) {
                        if (array[j].Total > array[j + 1].Total) {
                            Participant tmp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = tmp;
                        }
                    }
                }
            }
            public void Print() {
                Console.WriteLine($"{_name} {_surname}, Штрафное время: {Total} мин");
            }
        }

        public class BasketballPlayer : Participant
        {
            public BasketballPlayer(string name, string surname) : base(name, surname) { }

            public override bool IsExpelled {
                get {
                    if (_penaltyTimes == null) return false;
                    int criticalMatches = _penaltyTimes.Count(p => p == 5);
                    bool condition1 = criticalMatches > _penaltyTimes.Length * 0.1;
                    bool condition2 = Total > _penaltyTimes.Length * 2;
                    
                    return (condition1 || condition2) && _penaltyTimes.Length > 0;
                }
            }

            public override void PlayMatch(int time)
            {
                if (time < 0 || time > 5) return;

                base.PlayMatch(time);
            }
        }

        public class HockeyPlayer : Participant
        {
            private static int _totalPenaltyTime = 0;
            private static int _playersCount = 0;

            public HockeyPlayer(string name, string surname) : base(name, surname)
            {
                _playersCount++;
            }

            public override bool IsExpelled
            {
                get
                {
                    if (_penaltyTimes == null) return false;
                    if ( _penaltyTimes.Any(p => p == 10)) return true;
                    double average = _totalPenaltyTime / _playersCount;
                    bool condition2 = Total > average * 0.1;
                    
                    return condition2;
                }
            }

            public override void PlayMatch(int time)
            {
                if (_penaltyTimes == null) return;
                base.PlayMatch(time);
                _totalPenaltyTime += time;
            }
        }
    }
};
