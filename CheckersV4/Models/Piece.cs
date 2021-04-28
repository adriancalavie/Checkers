using CheckersV4.Services;
using CheckersV4.ViewModels;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CheckersV4.Models
{
    [XmlRoot]
    public class Piece : BaseVM
    {
        public enum Color
        {
            [Description("red")]
            RED,

            [Description("white")]
            WHITE,

            [Description("free")]
            FREE
        }

        public enum Type
        {
            [Description("common")]
            COMMON,

            [Description("king")]
            KING,
        }

        private Color color;
        private Type type;

        [XmlElement(ElementName = "color")]
        public Color PieceColor
        {
            get
            {
                return color;
            }
            set => SetProperty(ref color, value);
        }
        [XmlElement(ElementName = "type")]
        public Type PieceType
        {
            get
            {
                return type;
            }
            set => SetProperty(ref type, value);
        }

        private Location location;
        public Location PieceLocation
        {
            get
            {
                return location;
            }
            set => SetProperty(ref location, value);
        }

        public Piece()
        {
            PieceColor = Color.FREE;
            PieceType = Type.COMMON;
        }

        public Piece(Color color, Type type)
        {
            PieceColor = color;
            PieceType = type;
        }

        public Piece(Color color, Location location)
        {
            PieceColor = color;
            PieceLocation = location;
            PieceType = Type.COMMON;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Piece piece = obj as Piece;
            if ((System.Object)piece == null)
            {
                return false;
            }

            return PieceLocation == piece.PieceLocation;
        }

        public override string ToString()
        {
            return PieceColor.GetDescription() + " " + PieceType.GetDescription();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
