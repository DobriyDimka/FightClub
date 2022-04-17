using System;

namespace FightClub
{

    class Program
    {
        //параметры боя
        public static int crossHead = 15, //1
                            crossBody = 13, //2
                            kickHead = 25, //3
                            kickBody = 23, //4
                            undercut = 20, //5
                            blockHead = 4, //6
                            blockBody = 4, //7
                            blockLegs = 4, //8
                            sitdown = 6, //9
                            jump = 10; //10
        //восстановление выносливости
        public static int durabilityRepair = (crossHead + crossBody + kickHead + kickBody + undercut + blockHead + blockBody + blockLegs + sitdown + jump)/10;


        static void Main()
        {

            Console.WriteLine("Введите своё имя.");

            //определяем игрока

            Person player = new Person(Console.ReadLine(), 100, 100);
            if (player.Name == "")
            {
                do
                {
                    Console.WriteLine("Имя должно содержать хотя бы один символ или одну букву.");
                    player.Name = Console.ReadLine();
                } while (player.Name == "");
            }

            //определяем врага
            Person enemy = new Person("Злодей", 100, 100);

            //цикл пока не будет обнулено здоровье у врага или у игрока
            ConsoleKeyInfo cki;
            Boolean showRules = false;
            enemy.ResetActions();
            player.ResetActions();
            int action = -1;
            RefreshInterface(player, enemy, showRules);
            do
            {
                cki = Console.ReadKey();
                //показ/скрытие правил игры
                if (cki.Key == ConsoleKey.H)
                {
                    showRules = !showRules;
                    RefreshInterface(player, enemy, showRules);
                }
                else //действие игрока
                {
                    action = getActionByKey(cki);
                    if (action >= 0)
                    {
                        if (enemy.firstAttack < 0)
                        {
                            enemy.firstAttack = action;
                            player.firstAction= action;
                            Console.WriteLine("Ваше первое действие: {0}", getActionString(action));
                        }
                        else
                        {
                            if (enemy.secondAttack < 0)
                            {
                                enemy.secondAttack = action;
                                player.secondAction = action;
                            }
                            Console.WriteLine("Ваше второе действие: {0}", getActionString(action));
                            var rand = new Random();
                            player.firstAttack = 1+rand.Next(5);
                            enemy.firstAction = player.firstAttack;
                            player.secondAttack = rand.Next(10);
                            enemy.secondAction = player.secondAttack;
                            Console.WriteLine("Первое действие врага: {0}", player.firstAttack);
                            Console.WriteLine("Второе действие врага: {0}", player.secondAttack);
                            //Console.WriteLine("Нанесён урон врагу: {0}", enemy.DamageOne(100));
                            //Console.WriteLine("Нанесён урон игроку: {0}", player.DamageOne(150));
                            //Console.WriteLine("Нанесён урон врагу: {0}", enemy.DamageTwo(100));
                            //Console.WriteLine("Нанесён урон игроку: {0}", player.DamageTwo(150));

                            //первое действие
                            if (player.Durability < Person.getDurabilityByAction(player.firstAction)) 
                            {
                                player.firstAction = 10;
                                Console.WriteLine("{0} выдохся и не может сделать первое действие", player.Name);
                            }
                            else
                            {
                                enemy.DamageOne(100);
                            }
                            if (enemy.Durability < Person.getDurabilityByAction(enemy.firstAction))
                            {
                                enemy.firstAction = 10;
                                Console.WriteLine("{0} выдохся и не может сделать первое действие", enemy.Name);
                            }
                            else
                            {
                                player.DamageOne(120);
                            }


                            //второе действие
                            if (enemy.Durability < Person.getDurabilityByAction(enemy.secondAction))
                            {
                                enemy.firstAction = 10;
                                Console.WriteLine("{0} выдохся и не может сделать первое действие", player.Name);
                            }
                            else
                            {
                                player.DamageTwo(120);
                            }
                            if (player.Durability < Person.getDurabilityByAction(player.secondAction))
                            {
                                player.firstAction = 10;
                                Console.WriteLine("{0} выдохся и не может сделать первое действие", player.Name);
                            }
                            else
                            {
                                enemy.DamageTwo(100);
                            }

                            player.Durability = player.Durability + durabilityRepair;
                            enemy.Durability = enemy.Durability + durabilityRepair;

                            if (player.Durability > 100) player.Durability = 100;
                            if (enemy.Durability > 100) player.Durability = 100;

                            Console.ReadKey();
                            enemy.ResetActions();
                            player.ResetActions();
                            action = -1;
                            RefreshInterface(player, enemy, showRules);
                        };
                    };
                }

            } while (enemy.Health>0 && player.Health>0);

            //конец игры
            if (player.Health > 0)
            {
                Console.WriteLine("Вы победили, поздравляем!!!");
            }
            else
            {
                Console.WriteLine("Вы Проиграли, не расстраивайтесь, вам просто не повезло.");
            }
            Console.ReadKey();
        }

        //вывод интерфейса и информации о параметрах игроков на экран
        private static void RefreshInterface(Person player, Person enemy, Boolean showRules)
        {
            int origWidth, halfWidth;
            origWidth = Console.WindowWidth;
            halfWidth = origWidth / 2 - 10;
            Console.Clear();
            Console.WriteLine("{0,-25}     |     {1,-25}", "Игрок 1: " + player.Name, "Игрок 2: " + enemy.Name);
            Console.WriteLine("{0,-25}     |     {1,-25}", "Здоровье: " + player.Health, "Здоровье: " + enemy.Health);
            Console.WriteLine("{0,-25}     |     {1,-25}", "Выносливость: " + player.Durability, "Выносливость: " + enemy.Durability);
            Console.WriteLine("{0,-25}     |     {1,-25}", "Очки защиты: " + player.Points, "Очки защиты: " + enemy.Points);
            Console.WriteLine("____________________________________________________________");
            if (showRules)
            {
                Console.WriteLine("Правила игры:");
                Console.WriteLine("За ход игрок выбирает 2 действия, последовательно нажимая на цифорвые клавиши согласно списка ниже.");
                Console.WriteLine("Справа от действия указано количество очков выносливости, которое будет за него списано.");
                Console.WriteLine("В игре присутствует элемент случайности нанесения урона. Т.е. одним и тем же действием может наноситься разный уровень урона.");
                Console.WriteLine("Кроме того, урон зависит от уровня выносливости игрока. Чем выше уровень, тем больше урон.");
                Console.WriteLine("Каждый ход выносливость восстанавливается на среднее арифметическое число от всех комбинаций действий умноженное на 2 и равно {0}\n", durabilityRepair);
            }

            Console.WriteLine("1: удар рукой в голову -{0}", crossHead);
            Console.WriteLine("2: удар рукой в туловище -{0}", crossBody);
            Console.WriteLine("3: удар ногой в голову -{0}", kickHead);
            Console.WriteLine("4: удар ногой в туловище -{0}", kickBody);
            Console.WriteLine("5: подсечка -{0}", undercut);
            Console.WriteLine("6: блок головы -{0}", blockHead);
            Console.WriteLine("7: блок туловища -{0}", blockBody);
            Console.WriteLine("8: блок ног -{0}", blockLegs);
            Console.WriteLine("9: присесть -{0}", sitdown);
            Console.WriteLine("0: подпрыгнуть -{0}", jump);
            Console.WriteLine("h: показать/скрыть правила игры");
            Console.WriteLine();
        }


        //получение действия
        private static string getActionString(int action)
        {
            switch (action)
            {
                case 1:
                    return "удар рукой в голову";
                case 2:
                    return "удар рукой в туловище";
                case 3:
                    return "удар ногой в голову";
                case 4:
                    return "удар ногой в туловище";
                case 5:
                    return "подсечка";
                case 6:
                    return "блок головы";
                case 7:
                    return "блок туловища";
                case 8:
                    return "блок ног";
                case 9:
                    return "присесть";
                case 0:
                    return "подпрыгнуть";
            };
            return "";
        }
        private static int getActionByKey(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    return 1;
                case ConsoleKey.D2:
                    return  2;
                case ConsoleKey.D3:
                    return  3;
                case ConsoleKey.D4:
                    return  4;
                case ConsoleKey.D5:
                    return  5;
                case ConsoleKey.D6:
                    return  6;
                case ConsoleKey.D7:
                    return  7;
                case ConsoleKey.D8:
                    return  8;
                case ConsoleKey.D9:
                    return 9;
                case ConsoleKey.D0:
                    return 0;
            };
            return -1;
        }



        public class Person
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Durability { get; set; }
            public int firstAttack { get; set; }
            public int secondAttack { get; set; }
            public int firstAction { get; set; }
            public int secondAction { get; set; }
            public int Points { get; set; }
            public Person(string name, int health, int durability)
            {
                Name = name;
                Health = health;
                Durability = durability;
                Points = 0;
            }
            public bool ResetActions()
            {
                firstAttack = -1;
                secondAttack = -1;
                firstAction = -1;
                secondAction = -1;
                return true;
            }
            public int DamageOne(int koef)
            {
                int damage = 0;
                int resist = 0;
                var rand = new Random();
                //первая атака
                if (firstAttack == 1 || firstAttack == 3) //нанесён удар в голову
                {
                
                    if (firstAction == 6 || secondAction == 6) //удар в голову блокирован
                    {
                        Console.WriteLine("{0} получает удар в голову, но успешно блокирует его, чем снижает урон.", Name);
                        resist++; //сопротивление +1
                        Points = Points + 10; //очки +10
                    }
                    else if (firstAction == 9 || secondAction == 9) //удар в голову не достиг цели, противник присел
                    {
                        Console.WriteLine("{0} вовремя присел и чудом уворачивается от удара в голову мастерски прочитав противника.", Name);

                        resist = resist + 2; //сопротивление +2
                        Points = Points + 20; //очки +20
                    }
                    else if (firstAction == 0 || secondAction == 0) //противник подпрыгнул и получает удар по корпусу
                    {
                        Console.WriteLine("{0} прыгнул, но, к сожаленю не вовремя. Коварный удар пришёлся прямо в пищеварительную систему.", Name);
                        resist -= 1; //сопротивление -1
                    }
                    else if (firstAttack == 1)
                    {
                        Console.WriteLine("{0} получает прямой наводкой удар кулаком по котелку.", Name);
                    }
                    else if (firstAttack == 3)
                    {
                        Console.WriteLine("{0} не ожидал что нога противника приземлится ему прямо в ухо.", Name);
                    }
                    damage = damage + getDamageByAction(firstAttack);
                }

                if (firstAttack == 2 || firstAttack == 4) //нанесён удар в туловище
                {
                    if (firstAction == 7 || secondAction == 7) //удар в туловище блокирован
                    {
                        Console.WriteLine("{0} блокирует удар по корпусу.", Name);
                        resist++; //сопротивление +1
                        Points = Points + 10; ; //очки +10
                    }
                    else if (firstAction == 9 || secondAction == 9) //противник присел и вместо удара в туловище получил удар в голову
                    {
                        Console.WriteLine("{0} неудачно присев, получает сокрушительной силы плюху прямо в лоб.", Name);
                        resist--; //сопротивление +2
                        Points = Points - 10; //очки -10
                    }
                    else if (firstAction == 0 || secondAction == 0) //противник подпрыгнул и получает удар по ногам
                    {
                        Console.WriteLine("{0} прыгнул, но в полёте его настиг внезапный удар по ногам.", Name);
                        resist--; //сопротивление -1
                    }
                    else if (firstAttack == 2)
                    {
                        Console.WriteLine("{0} снова пропускает удар по корпусу.", Name);
                    }
                    else if (firstAttack == 4)
                    {
                        Console.WriteLine("{0} еле стоит на ногах после мощнейшего пинка в живот.", Name);
                    }
                    damage = damage + getDamageByAction(firstAttack);
                }

                if (firstAttack == 5) //подсечка
                {
                    if (firstAction == 8 || secondAction == 8) //подсечка блокирована
                    {
                        Console.WriteLine("{0} крепко стоит на ногах и противник не смог уложить его на канвас при помощи подсечки.", Name);
                        resist++; //сопротивление +1
                        Points = Points + 10; //очки +10
                    }
                    else if (firstAction == 9 || secondAction == 9) //противник присел и вместо удара в туловище получил удар в голову
                    {
                        Console.WriteLine("{0} получает по булкам за то что приседает когда в него летит удар по ногам.", Name);
                    }
                    else if (firstAction == 0 || secondAction == 0) //противник подпрыгнул
                    {
                        Console.WriteLine("{0} совершает умопомрачительный прыжок и уходит от коварной подсечки противника.", Name);
                        resist = resist + 2; //сопротивление +2
                        Points = Points + 30; ; //очки 
                    }
                    else
                    {
                        Console.WriteLine("{0} после великолепной подсечки распластаный лежит на полу.", Name);
                    }
                    damage = damage + getDamageByAction(firstAttack);
                }
                damage = koef/100 * (damage + damage * rand.Next(10) / 100 - resist);
                Health = Health - damage;
                Durability = Durability - getDurabilityByAction(firstAction);
                return damage;
            }
            public int DamageTwo(int koef)
            {
                int damage = 0;
                int resist = 0;
                var rand = new Random();
                //первая атака
                if (secondAttack == 1 || secondAttack == 3) //нанесён удар в голову
                {

                    if (firstAction == 6 || secondAction == 6) //удар в голову блокирован
                    {
                        Console.WriteLine("{0} получает удар в голову, но успешно блокирует его, чем снижает урон.", Name);
                        resist++; //сопротивление +1
                        Points = Points + 10; //очки +10
                    }
                    else if (firstAction == 9 || secondAction == 9) //удар в голову не достиг цели, противник присел
                    {
                        Console.WriteLine("{0} вовремя присел и чудом уворачивается от удара в голову мастерски прочитав противника.", Name);

                        resist = resist + 2; //сопротивление +2
                        Points = Points + 20; //очки +20
                    }
                    else if (firstAction == 0 || secondAction == 0) //противник подпрыгнул и получает удар по корпусу
                    {
                        Console.WriteLine("{0} прыгнул, но, к сожаленю не вовремя. Коварный удар пришёлся прямо в пищеварительную систему.", Name);
                        resist--; //сопротивление -1
                    }
                    else if (secondAttack == 1)
                    {
                        Console.WriteLine("{0} получает прямой наводкой удар кулаком по котелку.", Name);
                    }
                    else if (secondAttack == 3)
                    {
                        Console.WriteLine("{0} не ожидал что нога противника приземлится ему прямо в ухо.", Name);
                    }
                    damage = damage + getDamageByAction(secondAttack);
                }

                if (secondAttack == 2 || secondAttack == 4) //нанесён удар в туловище
                {
                    if (firstAction == 7 || secondAction == 7) //удар в туловище блокирован
                    {
                        Console.WriteLine("{0} блокирует удар по корпусу.", Name);
                        resist++; //сопротивление +1
                        Points = Points + 10; //очки +10
                    }
                    else if (firstAction == 9 || secondAction == 9) //противник присел и вместо удара в туловище получил удар в голову
                    {
                        Console.WriteLine("{0} неудачно присев, получает сокрушительной силы плюху прямо в лоб.", Name);
                        resist--; //сопротивление +2
                        Points = Points - 10; //очки -10
                    }
                    else if (firstAction == 0 || secondAction == 0) //противник подпрыгнул и получает удар по ногам
                    {
                        Console.WriteLine("{0} прыгнул, но в полёте его настиг внезапный удар по ногам.", Name);
                        resist--; //сопротивление +1
                    }
                    else if (secondAttack == 2)
                    {
                        Console.WriteLine("{0} снова пропускает удар по корпусу.", Name);
                    }
                    else if (secondAttack == 4)
                    {
                        Console.WriteLine("{0} еле стоит на ногах после мощнейшего пинка в живот.", Name);
                    }
                    damage = damage + getDamageByAction(secondAttack);
                }

                if (secondAttack == 5) //подсечка
                {
                    if (firstAction == 8 || secondAction == 8) //подсечка блокирована
                    {
                        Console.WriteLine("{0} крепко стоит на ногах и противник не смог уложить его на канвас при помощи подсечки.", Name);
                        resist++; //сопротивление +1
                        Points = Points + 10; //очки +10
                    }
                    else if (firstAction == 9 || secondAction == 9) //противник присел и вместо удара в туловище получил удар в голову
                    {
                        Console.WriteLine("{0} получает по булкам за то что приседает когда в него летит удар по ногам.", Name);
                    }
                    else if (firstAction == 0 || secondAction == 0) //противник подпрыгнул
                    {
                        Console.WriteLine("{0} совершает умопомрачительный прыжок и уходит от коварной подсечки противника.", Name);

                        resist = resist + 2; //сопротивление +2
                        Points = Points + 30; //очки 
                    }
                    else
                    {
                        Console.WriteLine("{0} после великолепной подсечки распластаный лежит на полу.", Name);
                    }
                    damage = damage + getDamageByAction(secondAttack);
                }
                damage = koef / 100 * (damage + damage * rand.Next(10) / 100 - resist);
                Health = Health - damage;
                Durability = Durability - getDurabilityByAction(secondAction);
                return damage;
            }
            private static int getDamageByAction(int action)
            {
                switch (action)
                {
                    case 1:
                        return 5;
                    case 2:
                        return 4;
                    case 3:
                        return 7;
                    case 4:
                        return 6;
                    case 5:
                        return 3;
                };
                return 0;
            }
            public static int getDurabilityByAction(int action)
            {
                switch (action)
                {
                    case 1:
                        return crossHead;
                    case 2:
                        return crossBody;
                    case 3:
                        return kickHead;
                    case 4:
                        return kickBody;
                    case 5:
                        return undercut;
                    case 6:
                        return blockHead;
                    case 7:
                        return blockBody;
                    case 8:
                        return blockLegs;
                    case 9:
                        return sitdown;
                    case 0:
                        return jump;
                };
                return 0;
            }
        }

    }

}
