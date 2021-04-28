using CheckersV4.ViewModels;
using System;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CheckersV4.Services
{
    public static class Services
    {

        public static BoardVM boardVM;


        public static string redPiece = @"Resources/red_piece.png";
        public static string whitePiece = @"Resources/white_piece.png";
        public static string redKing = @"Resources/red_king.png";
        public static string whiteKing = @"Resources/white_king.png";

        public static void SerializeObjectToXML<T>(T item, string FilePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(FilePath))
            {
                xs.Serialize(wr, item);
            }
        }

        public static T DeserializeFromXML<T>(string FilePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamReader rd = new StreamReader(FilePath))
            {
                return (T)xs.Deserialize(rd);
            }
        }

        public static void RedrawTiles()
        {
            Func<int, bool> IsOdd = x => x % 2 != 0;

            foreach (var tile in boardVM.Tiles)
            {
                int i = tile.Location.Row;
                int j = tile.Location.Column;
                if (IsOdd(i))
                {
                    if (IsOdd(j))
                    {
                        tile.Background = Brushes.Transparent;
                    }
                    else
                    {
                        tile.Background = Brushes.Black;
                    }
                }
                else
                {
                    if (IsOdd(j))
                    {
                        tile.Background = Brushes.Black;
                    }
                    else
                    {
                        tile.Background = Brushes.Transparent;
                    }
                }
            }
        }
    }
}
