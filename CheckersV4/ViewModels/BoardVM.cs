using CheckersV4.Models;
using CheckersV4.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CheckersV4.ViewModels
{
    public class BoardVM : BaseVM
    {

        public static Player Player1 { get; set; }
        public static Player Player2 { get; set; }

        [XmlElement(ElementName = "player1")]
        public Player Player1Serialize { get { return Player1; } set { Player1 = value; } }

        [XmlElement(ElementName = "player2")]
        public Player Player2Serialize { get { return Player2; } set { Player2 = value; } }


        public static MainWindow CurentWindows { get; set; }

        [XmlIgnore]
        public HashSet<Player> Players { get; set; }

        [XmlArray(ElementName = "pieces")]
        public ObservableCollection<PieceVM> Pieces { get; set; }

        [XmlIgnore]
        public ObservableCollection<CellVM> Tiles { get; set; }

        private static int[,] initialTop = {
            { 0, 1 }, { 0, 3 }, { 0, 5 }, { 0, 7 },
            { 1, 0 }, { 1, 2 }, { 1, 4 }, { 1, 6 },
            { 2, 1 }, { 2, 3 }, { 2, 5 }, { 2, 7 }
        };

        private static int[,] initialBot = {
            { 5, 0 }, { 5, 2 }, { 5, 4 }, { 5, 6 },
            { 6, 1 }, { 6, 3 }, { 6, 5 }, { 6, 7 },
            { 7, 0 }, { 7, 2 }, { 7, 4 }, { 7, 6 },
        };

        public void LoadInitialLineup()
        {

            Pieces = new ObservableCollection<PieceVM>();

            for (int i = 0; i < initialTop.GetLength(0); i++)
            {
                int x = initialTop[i, 0];
                int y = initialTop[i, 1];

                Pieces.Add(new PieceVM(new Models.Piece(Models.Piece.Color.WHITE, new Location(x, y))));
            }

            for (int i = 0; i < initialBot.GetLength(0); i++)
            {
                int x = initialBot[i, 0];
                int y = initialBot[i, 1];

                Pieces.Add(new PieceVM(new Models.Piece(Models.Piece.Color.RED, new Location(x, y))));
            }
        }

        private void FillEmpty()
        {
            Func<int, bool> IsOdd = x => x % 2 != 0;

            Tiles = new ObservableCollection<CellVM>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    CellVM c = new CellVM(i, j);

                    if (IsOdd(i))
                    {
                        if (IsOdd(j))
                        {
                            c.Background = Brushes.Transparent;
                        }
                        else
                        {
                            c.Background = Brushes.Black;
                        }
                    }
                    else
                    {
                        if (IsOdd(j))
                        {
                            c.Background = Brushes.Black;
                        }
                        else
                        {
                            c.Background = Brushes.Transparent;
                        }
                    }
                    Tiles.Add(c);
                }
            }
        }

        private void LoadPlayersFromMemory()
        {
            Players = new HashSet<Player>();
            Players = Services.Services.DeserializeFromXML<HashSet<Player>>(@"..\\..\\Resources\\players.xml");
        }

        private void NewGame()
        {
            if (Player1 == null)
            {
                Player1 = new Player("Player1", Piece.Color.RED);
            }
            if (Player2 == null)
            {
                Player2 = new Player("Player2", Piece.Color.WHITE);
            }
            Players.Add(Player1);
            Players.Add(Player2);

            Player oldPlayer;
            if (Players.TryGetValue(Player1, out oldPlayer))
            {
                Player1.Wins = oldPlayer.Wins;
                Player1.Losses = oldPlayer.Losses;
                Player1.Draws = oldPlayer.Draws;
            }
            if (Players.TryGetValue(Player2, out oldPlayer))
            {
                Player2.Wins = oldPlayer.Wins;
                Player2.Losses = oldPlayer.Losses;
                Player2.Draws = oldPlayer.Draws;
            }
            if (Player1.HasTurn == Player2.HasTurn)
            {
                Player1.HasTurn = true;
                Player2.HasTurn = false;
            }
            if (Player1.Color == Player2.Color)
            {
                Player1.Color = Piece.Color.RED;
                Player2.Color = Piece.Color.WHITE;
            }
        }

        private void GameRun()
        {
            LoadInitialLineup();
            FillEmpty();
            LoadPlayersFromMemory();
            NewGame();

            Services.Services.boardVM = this;
        }



        public BoardVM()
        {
            GameRun();
        }

        public BoardVM(OldGame oldGame)
        {
            this.Pieces = oldGame.Pieces;
            this.Player1Serialize = oldGame.Player1Serialize;
            this.Player2Serialize = oldGame.Player2Serialize;

            FillEmpty();
            LoadPlayersFromMemory();
            NewGame();
            Services.Services.boardVM = this;
        }

        [XmlIgnore]
        public bool IsPlayer1
        {
            get
            {
                return Player1.HasTurn;
            }
            set
            {
                OnPropertyChanged();
            }
        }
    }
}
