using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SeaBattle
{
    [Serializable]
    class Game : GameInterface
    {
        PlayerInterface player1, player2;
        BoardInterface board1, board2;

        Dictionary<string, ShipInterface> Ships { get; set; }

        bool OnPause { get; set; } = false;

        bool Player1Ready { get; set; } = false;
        bool Player2Ready { get; set; } = false;

        public Game(PlayerInterface player1, PlayerInterface player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            board1 = new Board();
            board2 = new Board();
            OnPause = false;

            Ships = new Dictionary<string, ShipInterface>();

            Ships.Add("Aircraft carrier", new Ship(5));
            Ships.Add("Battleship", new Ship(4));
            Ships.Add("Destroyer", new Ship(3));
            Ships.Add("Submarine", new Ship(3));
            Ships.Add("Patrol boat", new Ship(2));
        }

        public void loadGame(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Game newGame = formatter.Deserialize(fs) as Game;
                board1 = newGame.board1;
                board2 = newGame.board2;
                player1 = newGame.player1;
                player2 = newGame.player2;
                Player1Ready = newGame.Player1Ready;
                Player2Ready = newGame.Player2Ready;
            }
        }

        public PlayerInterface play()
        {
            Console.WriteLine("Вы можете в любую минуту поставить игру на паузу.\nДля этого введите \"пауза\" в командную строку\n");
            if (!OnPause)
            {
                Console.Clear();
                if (!Player1Ready)
                {
                    Console.WriteLine($"{player1.ToString()}:");
                    foreach(KeyValuePair<string, ShipInterface> kvp in Ships)
                    {
                        Console.WriteLine($"Выберите расположение для \"{kvp.Key}\"");

                        Ship ship = new Ship(kvp.Value.getSize());
                        try
                        {
                            Placement pl = player1.choosePlacement(ship, board1.Clone() as BoardInterface);
                            board1.placeShip(ship, pl.getPosition(), pl.isVerticalValue());
                        }
                        catch(PauseException e)
                        {
                            Console.Clear();
                            Console.WriteLine("Игра остановлена на этапе растановки кораблей, расстановка текущего игрока не будет запомнена");
                            return null;
                        }              
                    }
                    Player1Ready = true;
                }

                Console.Clear();
                if (!Player2Ready && !(player2 is ComputerPlayer))
                {
                    Console.WriteLine($"{player2.ToString()}:");
                    foreach (KeyValuePair<string, ShipInterface> kvp in Ships)
                    {
                        Console.WriteLine($"Выберите расположение для \"{kvp.Key}\"");

                        Ship ship = new Ship(kvp.Value.getSize());
                        try
                        {
                            Placement pl = player2.choosePlacement(ship, board2.Clone() as BoardInterface);
                            board2.placeShip(ship, pl.getPosition(), pl.isVerticalValue());
                        }
                        catch (PauseException e)
                        {
                            Console.Clear();
                            Console.WriteLine("Игра остановлена на этапе растановки кораблей, расстановка текущего игрока не будет запомнена");
                            return null;
                        }
                    }
                    Player2Ready = true;
                }
                else if (player2 is ComputerPlayer)
                {
                    foreach (KeyValuePair<string, ShipInterface> kvp in Ships)
                    {
                        Ship ship = new Ship(kvp.Value.getSize());
                        Placement placement = player2.choosePlacement(ship, board2.Clone() as BoardInterface);
                        board2.placeShip(ship, placement.getPosition(), placement.isVerticalValue());
                    }
                }
            }

            Console.Clear();
            while (!(board1.allSunk() || board2.allSunk()))
            {
                try
                {
                    if (!board2.allSunk())
                    {
                        ConsoleGraphics.showField(board1, 60);
                        Position position1 = player1.chooseShot();
                        try
                        {
                            board2.shoot(position1);
                        }
                        catch(InvalidPositionException e) { }
                    
                        checkStatus(position1, player1, board2);
                        Console.Clear();
                        player2.opponentShot(position1);
                    }

                    if (!board1.allSunk() && !(player2 is ComputerPlayer))
                    {
                        ConsoleGraphics.showField(board2, 60);
                        Position position2 = player2.chooseShot();
                        try
                        {
                            board1.shoot(position2);
                        }
                        catch(InvalidPositionException e) { }
                    
                        checkStatus(position2, player2, board1);
                        Console.Clear();
                        player1.opponentShot(position2);
                    }

                    if (!board1.allSunk() && player2 is ComputerPlayer)
                    {
                        Position position3 = player2.chooseShot();
                        try
                        {
                            board1.shoot(position3);
                        }
                        catch(InvalidPositionException e) { }

                        player1.opponentShot(position3);
                    }
                }
                catch(PauseException e)
                {
                    Console.WriteLine("Игра остановлена");
                    OnPause = true;
                    return null;
                }
            }

            if (board1.allSunk())
                return player2;

            if (board2.allSunk())
                return player1;

            return null;
        }

        private void checkStatus(Position position, PlayerInterface currPlayer, BoardInterface enemyBoard)
        {
            try
            {
                switch (enemyBoard.getStatus(position))
                {
                    case ShipStatus.HIT:
                        currPlayer.shotResult(position, ShotStatus.HIT);
                        break;
                    case ShipStatus.SUNK:
                        currPlayer.shotResult(position, ShotStatus.SUNK);
                        break;
                }
            }
            catch
            {
                currPlayer.shotResult(position, ShotStatus.MISS);
            }
        }

        public void saveGame(string filename)
        {
            OnPause = true;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }
    }
}
