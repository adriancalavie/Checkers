using CheckersV4.Models;
using CheckersV4.Services;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CheckersV4.ViewModels
{
    [XmlRoot]
    public class PieceVM : BaseVM
    {
        private Piece piece;
        public Piece Piece
        {
            get
            {
                return piece;
            }
            set => SetProperty(ref piece, value);
        }

        private string path;
        public string Path
        {
            get
            {
                if (path == null)
                {
                    if (Piece.PieceColor == Piece.Color.WHITE)
                        path = Services.Services.whitePiece;
                    if (Piece.PieceColor == Piece.Color.RED)
                        path = Services.Services.redPiece;
                    //path = Utils.redPiece;
                }
                return path;
            }

            set
            {
                SetProperty(ref path, value);
            }

        }

        public PieceVM()
        {
            Piece = new Piece();
            isSelected = System.Windows.Media.Colors.Black;
        }

        public PieceVM(Piece piece)
        {
            this.piece = piece;
            isSelected = System.Windows.Media.Colors.Black;
        }

        public PieceVM(int x, int y)
        {
            Piece temp = new Piece(Piece.Color.FREE, new Location(x, y));
            isSelected = System.Windows.Media.Colors.Black;
        }

        private System.Windows.Media.Color isSelected;
        [XmlIgnore]
        public System.Windows.Media.Color IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                SetProperty(ref isSelected, value);
            }
        }

        [XmlIgnore]
        public System.Windows.Media.Color ColorPieceView
        {
            get
            {
                if (Piece.PieceColor == Piece.Color.WHITE)
                    return System.Windows.Media.Colors.White;
                if (Piece.PieceColor == Piece.Color.RED)
                    return System.Windows.Media.Colors.Red;
                return System.Windows.Media.Colors.Black;
            }
        }

        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            PieceVM piece = obj as PieceVM;
            if (piece == null)
            {
                return false;
            }

            return Piece == piece.Piece;
        }

       

        private ICommand selectPieceCommand;
        [XmlIgnore]
        public ICommand SelectPieceCommand
        {
            get
            {
                if (selectPieceCommand == null)
                {
                    selectPieceCommand = new Commands.SelectPieceCommand(this);
                }
                return selectPieceCommand;
            }
        }
        [XmlIgnore]
        public bool IsPhantom { get; set; }

        public PieceVM GetPhantomAt(Location location)
        {
            var phantom = new PieceVM();
            phantom.Piece.PieceColor = piece.PieceColor;
            phantom.piece.PieceLocation = location;
            phantom.Piece.PieceType = piece.PieceType;

            phantom.IsPhantom = true;
            Services.Services.boardVM.Pieces.Add(phantom);
            return phantom;
        }


    }
}
