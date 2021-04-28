using System.Xml.Serialization;
using CheckersV4.ViewModels;

namespace CheckersV4.Services
{
    [XmlRoot(ElementName = "location")]
    public class Location : BaseVM
    {
        private int column;
        [XmlElement(ElementName ="column")]
        public int Column 
        {
            get => column;
            set => SetProperty(ref column, value);
        }

        private int row;
        [XmlElement(ElementName ="row")]
        public int Row
        {
            get => row;
            set => SetProperty(ref row, value);
        }

        public Location(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Location()
        {
            Row = 0;
            Column = 0;
        }

        public override string ToString()
        {
            return Row + " " + Column;
        }

        public override bool Equals(object obj)
        {
            return obj is Location location &&
                   Column == location.Column &&
                   Row == location.Row;
        }

        public override int GetHashCode()
        {
            int hashCode = 656739706;
            hashCode = hashCode * -1521134295 + Column.GetHashCode();
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            return hashCode;
        }
    }
}
