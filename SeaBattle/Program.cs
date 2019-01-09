using System;

namespace SeaBattle
{ 
    class Program
    {
        static void Main(string[] args) 
        {
            Game game = null;
            while (true)
            {
                Console.WriteLine("Основное меню:\n1. Новая игра \n2. Продолжить игру \n3. Загрузить игру \n4. Сохранить и выйти");
                string answ = Console.ReadLine();
                if (answ == "1")
                {
                    Console.WriteLine("Выберите режим игры:\n1. Игрок против компьютера \n2. Игрок против игрока");
                    answ = Console.ReadLine();
                    if (answ == "1")
                    {
                        Console.WriteLine("Игрок, введите своё имя");
                        game = new Game(new HumanConsolePlayer(Console.ReadLine()), new ComputerPlayer());
                        PlayerInterface winner = game.play();
                        if (winner != null)
                            Console.WriteLine($"Побеждает {winner.ToString()}");
                    }
                    else if (answ == "2")
                    {
                        Console.WriteLine("Игрок 1, введите своё имя");
                        string name1 = Console.ReadLine();
                        Console.WriteLine("Игрок 2, введите своё имя");
                        string name2 = Console.ReadLine();
                        game = new Game(new HumanConsolePlayer(name1), new HumanConsolePlayer(name2));
                        PlayerInterface winner = game.play();
                        if (winner != null)
                            Console.WriteLine($"Побеждает {winner.ToString()}");
                    }
                    else
                        Console.WriteLine("Такого пункта нет в меню");
                }   
                else if (answ == "2")
                {
                    if (game != null)
                    {
                        PlayerInterface winner = game.play();
                        if (winner != null)
                            Console.WriteLine($"Побеждает {winner.ToString()}");
                    }
                    else
                        Console.WriteLine("Начатой игры нет");
                }
                else if (answ == "3")
                {
                    Console.WriteLine("Введите название сохранения");
                    string name = Console.ReadLine();
                    game = new Game(null, null);
                    try
                    {
                        game.loadGame($"{name}.dat");
                    }
                    catch
                    {
                        Console.WriteLine("Такого сохранения нет");
                    }
                }
                else if (answ == "4")
                {
                    if (game != null)
                    {
                        Console.WriteLine("Сохранить текущую игру?(да/нет)");
                        answ = Console.ReadLine();
                        if (answ == "да")
                        {
                            Console.WriteLine("Введите название сохранения");
                            string name = Console.ReadLine();
                            game.saveGame($"{name}.dat");
                        }
                        else if (answ == "нет") { }
                        else
                            Console.WriteLine("Введено неизвестное значение");
                    }
                    break;
                }
                else
                    Console.WriteLine("Такого пункта нет в меню");
            }
        }
    }
}
