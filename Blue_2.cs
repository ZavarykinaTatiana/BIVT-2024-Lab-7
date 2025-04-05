using System;
using System.Linq;

namespace Lab_7
{ 
    public class Blue_2
    {
        public struct Participant {

            // приватные поля
            private string _name;
            private string _surname;
            private int[,] _marks;
            private int recordedJums;

            // публичные свойства
            public string Name => _name;
            public string Surname => _surname;
            public int[,] Marks {
                get {
                    if (_marks == null) return null;
                    
                    int[,] copy = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }
            public int TotalScore
            {
                get
                {
                    if (_marks == null) return 0;
                    int sum = 0;
                    for (int i = 0; i < recordedJums; i++)
                    {
                        for (int j = 0; j < _marks.GetLength(1); j++) sum += _marks[i, j];
                    }
                    return sum;
                }
            }

            // конструктор
            public Participant(string name, string surname) {
                _name = name;
                _surname = surname;
                _marks = new int[2, 5];
                recordedJums = 0;
            }

            // методы
            // метод для добавления результатов прыжков
            public void Jump(int[] result)
            {
                if (result == null || _marks == null) return;
                if (recordedJums < 2) {
                    for (int j=0; j<5; j++) {
                        _marks[recordedJums, j] = result[j];
                    }
                    recordedJums++;
                }
            }
            
            public static void Sort(Participant[] array) {
                if (array == null) return;
                for (int i=0; i < array.Length-1; i++) {
                    for (int j=0; j < array.Length-i-1; j++) {
                        if (array[j].TotalScore < array[j+1].TotalScore) {
                            Participant temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }
            public void Print() {

                Console.WriteLine($"{_name} {_surname}: {TotalScore}");
                Console.WriteLine("Оценки судей:");
                for (int i = 0; i < Marks.GetLength(0); i++)
                {
                    for (int j = 0; j < Marks.GetLength(1); j++)
                    {
                        Console.Write($"{Marks[i, j]} ");
                    }
                    Console.WriteLine();
                }
            }
        }

        public abstract class WaterJump
        {
            // приватные поля
            private string _name;
            private int _bank;
            private Participant[] _participants;

            // публичные свойства
            public string Name => _name;
            public int Bank => _bank;
            public Participant[] Participants => _participants;

            // абстрактное свойство для призовых мест
            public abstract double[] Prize { get; }

            // конструктор
            public WaterJump(string name, int bank)
            {
                _name = name;
                _bank = bank;
                _participants = new Participant[0];
            }

            // методы для добавления участников
            public void Add(Participant participant)
            {
                if (_participants == null) return;

                Participant[] newParticipants = new Participant[_participants.Length+1];
                for (int i=0; i<_participants.Length; i++) newParticipants[i] = _participants[i];

                newParticipants[_participants.Length] = participant;
                _participants = newParticipants;
            }

            public void Add(Participant[] participants)
            {
                if (participants == null || participants.Length == 0 || _participants == null) return;

                foreach (Participant participant in participants) Add(participant);
            }
        }

        public class WaterJump3m : WaterJump
        {
            public WaterJump3m(string name, int bank) : base(name, bank) { }

            public override double[] Prize
            {
                get
                {
                    if (Participants == null || Participants.Length < 3) return null;
                    
                    return new double[]
                    {
                        Bank * 0.5,  // первое место 50проц
                        Bank * 0.3,  // второе 30проц
                        Bank * 0.2   // третье двадцать проце
                    };
                }
            }
        }

        public class WaterJump5m : WaterJump
        {
            public WaterJump5m(string name, int bank) : base(name, bank) { }

            public override double[] Prize
            {
                get
                {
                    if (Participants == null || Participants.Length < 3) return null;
                    
                    double[] basePrizes = { Bank * 0.4, Bank * 0.25, Bank * 0.15 };
                    int count = Participants.Length / 2;
                    count = Math.Clamp(count, 3, 10);
                    
                    // распределение остатка
                    double remaining = Bank * 0.20;
                    double perPerson = remaining / count;
                    
                    double[] finalPrizes = new double[count];
                    for (int i = 0; i < basePrizes.Length; i++) finalPrizes[i] = basePrizes[i];
                    for (int i = 0; i < count; i++) finalPrizes[i] += perPerson;
                    return finalPrizes;
                }
            }
        }
    }
}
