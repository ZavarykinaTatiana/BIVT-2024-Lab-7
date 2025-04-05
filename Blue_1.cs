using System;
using System.Linq;

namespace Lab_7
{
    public class Blue_1
    {
        public class Response {

            // приватные поля
            private string _name;
            protected int _votes;

            // публичные свойства

            public string Name => _name;
            public int Votes => _votes;

            // конструктор
            public Response(string name) {
                _name = name;
                _votes = 0;
            }

            // методы
            // метод для подсчета голосов
            public virtual int CountVotes(Response[] responses) {
                if (responses == null || _name == null || responses.Length == 0) return 0;
            
                _votes = responses.Count(response => response.Name == _name);
                return _votes;
            }
            // метод для вывода информации о человеке
            public virtual void Print() {
                Console.WriteLine($"{_name}: {_votes}");
            }
        }

        public class HumanResponse : Response {
            // приватные поля
            private string _surname;
            

            // публичные свойства
            public string Surname => _surname;
            
            // конструктор
            public HumanResponse(string name, string surname) : base(name)
            {
                _surname = surname;
                _votes = 0;
            }
            // методы
            // метод подсчета голосов
            public override int CountVotes(Response[] responses)
            {
                if (responses == null || _surname == null || responses.Length == 0) return 0;
                
                foreach (var response in responses) {
                    if (response is HumanResponse humanResponse && 
                        humanResponse != null &&
                        humanResponse.Name == this.Name && 
                        humanResponse.Surname == this.Surname)
                    {
                        _votes++;
                    }
                }
                
                return _votes;
            }
            
            public override void Print()
            {
                Console.WriteLine($"{Name} {_surname}: {_votes}");
            }
        }
    }
}