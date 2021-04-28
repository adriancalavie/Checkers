using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CheckersV4.Models
{
    [XmlRoot(ElementName = "player")]
    public class Player
    {

        public Player()
        {
            name = "";
            wins = 0;
            losses = 0;
            draws = 0;
            color = Piece.Color.FREE;
            hasTurn = false;
        }

        public Player(String name, Piece.Color color = Piece.Color.FREE, int wins = 0, int losses = 0, int draws = 0, bool hasTurn = false)
        {
            this.name = name;
            this.wins = wins;
            this.losses = losses;
            this.draws = draws;
            this.color = color;
            this.hasTurn = hasTurn;
        }

        private String name;
        [XmlElement(ElementName = "name")]
        public String Name
        {
            get => name;
            set
            {
                name = value;
            }
        }

        private Piece.Color color;
        [XmlIgnore]
        public Piece.Color Color
        {
            get => color;
            set
            {
                color = value;
            }
        }

        private bool hasTurn;
        [XmlElement(ElementName = "hasTurn")]
        public bool HasTurn
        {
            get => hasTurn;
            set
            {
                hasTurn = value;
            }
        }

        private int wins;
        [XmlElement(ElementName = "wins")]
        public int Wins
        {
            get => wins;
            set
            {
                wins = value;
            }
        }

        private int losses;
        [XmlElement(ElementName = "losses")]
        public int Losses
        {
            get => losses;
            set
            {
                losses = value;
            }
        }

        private int draws;
        [XmlElement(ElementName = "draws")]
        public int Draws
        {
            get => draws;
            set
            {
                draws = value;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Player player && Name.Equals(player.Name);
        }

        public override int GetHashCode()
        {
            int hashCode = 656739706;
            hashCode = hashCode * -1521134295 + name.GetHashCode();
            return hashCode;
        }
    }
}