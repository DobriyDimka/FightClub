using System;

namespace FightClub
{
    public class Person
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Durability { get; set; }
        public Person(string name, int health, int durability)
        {
            Name = name;
            Health = health;
            Durability = durability;
        }
    }
    class Program
    {
        static void Main()
        {

            Console.WriteLine("Введите своё имя.");
            Person player = new Person(Console.ReadLine(), 100, 100);
            if (player.Name == "")
            {
                do
                {
                    Console.WriteLine("Имя должно содержать хотя бы один символ или одну букву.");
                    player.Name = Console.ReadLine();
                } while (player.Name == "");
            }
            Console.WriteLine("Привет, " + player.Name);
            Console.WriteLine("Вы готовы к битве? Нажмите Y или N.");

            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Y && cki.Key != ConsoleKey.N);
            Console.Clear();
            if (cki.Key == ConsoleKey.N)
            {
                Console.WriteLine("Очень печально:( Заходите к нам ещё. Пока!");
                Console.ReadKey();
                return;
            }


            Console.WriteLine();
        }
    }

}
