using CheckersV4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.ObjectModel;


namespace CheckersV4.ViewModels
{
    public class OldGame
    {
        private Player player1Serialize;
        [XmlElement(ElementName = "player1")]
        public Player Player1Serialize
        {
            get => player1Serialize;
            set
            {
                player1Serialize = value;
            }
        }

        private Player player2Serialize;
        [XmlElement(ElementName = "player2")]
        public Player Player2Serialize
        {
            get => player2Serialize;
            set
            {
                player2Serialize = value;
            }
        }

        [XmlArray(ElementName = "pieces")]
        public ObservableCollection<PieceVM> Pieces { get; set; }

        public OldGame(Player player1Serialize, Player player2Serialize, ObservableCollection<PieceVM> pieces)
        {
            this.player1Serialize = player1Serialize;
            this.player2Serialize = player2Serialize;
            Pieces = pieces;
        }

        public OldGame()
        {
            player1Serialize = new Player();
            player2Serialize = new Player();
            Pieces = new ObservableCollection<PieceVM>();
        }
    }
}
