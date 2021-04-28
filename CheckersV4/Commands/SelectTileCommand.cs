using CheckersV4.Services;
using CheckersV4.ViewModels;
using System.Collections.ObjectModel;

namespace CheckersV4.Commands
{
    public class SelectTileCommand : BaseCommand
    {
        private readonly CellVM cellVM;
        public SelectTileCommand(CellVM cell)
        {
            this.cellVM = cell;
        }

        public ObservableCollection<PieceVM> Pieces { get; set; }
        public ObservableCollection<CellVM> Tiles { get; set; }

        public bool TurnOf { get; set; }

        public override void Execute(object parameter)
        {
            if (CanSelect())
            {

                Pieces = (parameter as BoardVM).Pieces;
                Tiles = (parameter as BoardVM).Tiles;


                Services.Services.RedrawTiles();

                foreach (var tile in Logic.selectedTiles)
                {
                    if (tile == cellVM)
                    {
                        Logic.movesForSelectedPiece.Sort( 
                            (move1, move2) => -move1.TakenPieces.Count.CompareTo(move2.TakenPieces.Count));

                        foreach (var move in Logic.movesForSelectedPiece)
                        {
                            if (move.FinalLocation.Equals(tile.Location))
                            {
                                foreach (var piece in move.TakenPieces)
                                {
                                    Pieces.Remove(piece);
                                }
                                break;
                            }
                        }

                        Logic.selectedPiece.Piece.PieceLocation = tile.Location;
                        break;

                    }
                }
                Logic.IsKing(Logic.selectedPiece);
                if (Logic.IsFinal(Pieces))
                {
                    BoardVM.CurentWindows.StartNewGame();
                }
                Logic.ChangePlayer();
                (parameter as BoardVM).IsPlayer1 = true;

                Logic.selectedPiece.IsSelected = System.Windows.Media.Colors.Black;
            }
        }

        private bool CanSelect()
        {
            if (Logic.selectedTiles == null)
                return false;
            foreach (var tile in Logic.selectedTiles)
            {
                if (tile.Equals(cellVM))
                    return true;
            }

            return false;
        }
    }
}
