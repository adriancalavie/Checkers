using CheckersV4.Models;
using CheckersV4.Services;
using CheckersV4.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CheckersV4.Commands
{
    public class SelectPieceCommand : BaseCommand
    {
        private readonly PieceVM pieceVM;
        public SelectPieceCommand(PieceVM piece)
        {
            this.pieceVM = piece;
        }

        public ObservableCollection<PieceVM> Pieces { get; set; }
        public ObservableCollection<CellVM> Tiles { get; set; }

        

        public override void Execute(object parameter)
        {
            //ConsoleManager.Show();
            //Console.WriteLine("Hello!");

            if (Logic.selectedPiece != null)
            {
                Logic.selectedPiece.IsSelected = System.Windows.Media.Colors.Black;
            }

            Logic.selectedPiece = pieceVM;

            pieceVM.IsSelected = System.Windows.Media.Colors.HotPink;

            Pieces = (parameter as BoardVM).Pieces;
            Tiles = (parameter as BoardVM).Tiles;

            //Pieces.Remove(pieceVM);

            var moves = Logic.GetMoves(Logic.selectedPiece.Piece.PieceLocation);
            //TODO: arata ca dreq structura proiectului, sa mai revizuim putin, ca am facut ciorba in Utils
            Services.Services.RedrawTiles();
            Logic.selectedTiles = new System.Collections.Generic.List<CellVM>();
            foreach (var move in moves)
            {
                var finalTile = Logic.GetCellByLocation(move.FinalLocation);
                Logic.selectedTiles.Add(finalTile);
                finalTile.Background = Brushes.SeaGreen;
            }
            Logic.movesForSelectedPiece = moves;
        }
    }
}
