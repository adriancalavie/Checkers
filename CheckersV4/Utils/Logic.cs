using CheckersV4.Models;
using CheckersV4.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace CheckersV4.Services
{
    //mutat tot din utils
    public static class Logic
    {
        public static PieceVM selectedPiece = null;

        private static int NORTH = -1;
        private static int SOUTH = 1;
        private static int EAST = 1;
        private static int WEST = -1;

        public static List<CellVM> selectedTiles = null;
        public static List<Move> movesForSelectedPiece = null;


        public static void IsKing(PieceVM piece)
        {
            if (piece != null)
            {
                if (piece.Piece.PieceColor == Piece.Color.RED && piece.Piece.PieceLocation.Row == 0)
                {
                    piece.Piece.PieceType = Piece.Type.KING;
                    piece.Path = Services.redKing;
                    return;
                }
                if (piece.Piece.PieceColor == Piece.Color.WHITE && piece.Piece.PieceLocation.Row == 7)
                {
                    piece.Piece.PieceType = Piece.Type.KING;
                    piece.Path = Services.whiteKing;
                    return;
                }
            }
        }

        public static bool IsFinal(ObservableCollection<PieceVM> pieces)
        {
            bool white = false;
            bool red = false;

            if(IsDraw(pieces))
            {
                MessageBoxResult result = MessageBox.Show("Draw!");
                BoardVM.Player1.Draws += 1;
                BoardVM.Player2.Draws += 1;
                return true;
            }


            if (pieces == null)
                return false;
            foreach (var piece in pieces)
            {
                if (piece.Piece.PieceColor == Piece.Color.WHITE)
                {
                    white = true;
                }
                else
                {
                    red = true;
                }
            }
            if (white == false)
            {
                MessageBoxResult result = MessageBox.Show(BoardVM.Player1.Name + " wins!");
                BoardVM.Player1.Wins += 1;
                BoardVM.Player2.Losses += 1;
                return true;
            }
            if (red == false)
            {
                MessageBoxResult result = MessageBox.Show(BoardVM.Player2.Name + " wins!");
                BoardVM.Player1.Losses += 1;
                BoardVM.Player2.Wins += 1;
                return true;
            }
            return false;
        }

        public static bool IsDraw(ObservableCollection<PieceVM> pieces)
        {
            foreach(var piece in pieces)
            {
                var moves = GetMoves(piece.Piece.PieceLocation);
                if(moves.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckPiece(PieceVM piece)
        {
            if (piece == null)
                return false;
            if (piece.Piece.PieceColor == BoardVM.Player1.Color && BoardVM.Player1.HasTurn)
            {
                return true;
            }
            if (piece.Piece.PieceColor == BoardVM.Player2.Color && BoardVM.Player2.HasTurn)
            {
                return true;
            }
            return false;
        }

        public static void ChangePlayer()
        {
            BoardVM.Player1.HasTurn = !BoardVM.Player1.HasTurn;
            BoardVM.Player2.HasTurn = !BoardVM.Player2.HasTurn;
            
        }

        private static bool IsOnTable(Location location)
        {
            int x = location.Row;
            int y = location.Column;

            return (x >= 0 && x < 8) && (y >= 0 && y < 8);
        }

        private static PieceVM GetPieceByLocation(Location location)
        {
            // THIS CAN BE GREATLY IMPROVED BY USING A DICTIONARY FOR FAST SEARCH, DUNNO HOW YET
            foreach (var item in Services.boardVM.Pieces)
            {
                if (item.Piece.PieceLocation.Equals(location))
                    return item;
            }

            return null;
        }

        public static CellVM GetCellByLocation(Location location)
        {
            // THIS CAN BE GREATLY IMPROVED BY USING A DICTIONARY FOR FAST SEARCH, DUNNO HOW YET
            foreach (var item in Services.boardVM.Tiles)
            {
                if (item.Location.Equals(location))
                {
                    return item;
                }
            }

            return null;
        }


        private static List<Move> GetMovesUp(PieceVM piece, bool checksForFreeTiles = true, List<PieceVM> pastPiecesTaken = null)
        {
            var moves = new List<Move>();
            var cameFrom = piece.Piece.PieceLocation;

            int x = piece.Piece.PieceLocation.Row;
            int y = piece.Piece.PieceLocation.Column;


            Location NorthWest = new Location(x + NORTH, y + WEST);
            Location NorthEast = new Location(x + NORTH, y + EAST);

            if (IsOnTable(NorthWest))
            {
                var NorthWestPiece = GetPieceByLocation(NorthWest);
                if (NorthWestPiece == null)
                {
                    if (checksForFreeTiles)
                    {
                        var plainMove = new Move(NorthWest);
                        moves.Add(plainMove);
                    }
                }
                else if (NorthWestPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {
                    Location NextNW = new Location(NorthWest.Row + NORTH, NorthWest.Column + WEST);
                    if (IsOnTable(NextNW))
                    {
                        var PseudoNWPiece = GetPieceByLocation(NextNW);
                        if (PseudoNWPiece == null)
                        {
                            var simpleJumpMove = new Move(NextNW);

                            if (!checksForFreeTiles)
                            {
                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }
                            simpleJumpMove.TakenPieces.Add(NorthWestPiece);
                            moves.Add(simpleJumpMove);
                            moves.AddRange(GetMovesUp(piece.GetPhantomAt(NextNW), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }

            if (IsOnTable(NorthEast))
            {
                var NorthEastPiece = GetPieceByLocation(NorthEast);
                if (NorthEastPiece == null)
                {
                    if (checksForFreeTiles)
                    {

                        var plainMove = new Move(NorthEast);
                        moves.Add(plainMove);
                    }
                }
                else if (NorthEastPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {

                    Location NextNE = new Location(NorthEast.Row + NORTH, NorthEast.Column + EAST);
                    if (IsOnTable(NextNE))
                    {
                        var PseudoNEPiece = GetPieceByLocation(NextNE);
                        if (PseudoNEPiece == null)
                        {
                            var simpleJumpMove = new Move(NextNE);

                            if (!checksForFreeTiles)
                            {

                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(NorthEastPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesUp(piece.GetPhantomAt(NextNE), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }
            piece.Piece.PieceLocation = cameFrom;

            if (piece.IsPhantom)
                Services.boardVM.Pieces.Remove(piece);

            return moves;
        }

        private static List<Move> GetMovesDown(PieceVM piece, bool checksForFreeTiles = true, List<PieceVM> pastPiecesTaken = null)
        {
            var moves = new List<Move>();
            var cameFrom = piece.Piece.PieceLocation;

            int x = piece.Piece.PieceLocation.Row;
            int y = piece.Piece.PieceLocation.Column;


            Location SouthWest = new Location(x + SOUTH, y + WEST);
            Location SouthEast = new Location(x + SOUTH, y + EAST);

            if (IsOnTable(SouthWest))
            {
                var SouthWestPiece = GetPieceByLocation(SouthWest);
                if (SouthWestPiece == null)
                {
                    if (checksForFreeTiles)
                    {

                        var plainMove = new Move(SouthWest);
                        moves.Add(plainMove);
                    }
                }
                else if (SouthWestPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {

                    Location NextSW = new Location(SouthWest.Row + SOUTH, SouthWest.Column + WEST);
                    if (IsOnTable(NextSW))
                    {
                        var PseudoNWPiece = GetPieceByLocation(NextSW);
                        if (PseudoNWPiece == null)
                        {

                            var simpleJumpMove = new Move(NextSW);

                            if (!checksForFreeTiles)
                            {
                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(SouthWestPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesDown(piece.GetPhantomAt(NextSW), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }

            if (IsOnTable(SouthEast))
            {
                var SouthEastPiece = GetPieceByLocation(SouthEast);
                if (SouthEastPiece == null)
                {
                    if (checksForFreeTiles)
                    {
                        var plainMove = new Move(SouthEast);
                        moves.Add(plainMove);
                    }
                }
                else if (SouthEastPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {
                    Location NextSE = new Location(SouthEast.Row + SOUTH, SouthEast.Column + EAST);
                    if (IsOnTable(NextSE))
                    {
                        var PseudoNEPiece = GetPieceByLocation(NextSE);
                        if (PseudoNEPiece == null)
                        {

                            var simpleJumpMove = new Move(NextSE);

                            if (!checksForFreeTiles)
                            {

                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(SouthEastPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesDown(piece.GetPhantomAt(NextSE), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }
            piece.Piece.PieceLocation = cameFrom;

            if (piece.IsPhantom)
                Services.boardVM.Pieces.Remove(piece);

            return moves;
        }

        public static List<Move> GetMoves(Location location)
        {
            var result = new List<Move>();

            var piece = GetPieceByLocation(location);

            if (piece == null)
            {
                ConsoleManager.Show();
                System.Console.WriteLine("ERROR: at postion: " + location.ToString() + " there is no piece");
            }
            else if (Logic.CheckPiece(piece))
            {
                if (piece.Piece.PieceType == Piece.Type.KING)
                {

                    result.AddRange(GetMovesKing(piece));
                }
                else if (piece.Piece.PieceColor == Piece.Color.WHITE)
                {

                    result.AddRange(GetMovesDown(piece));
                }
                else
                {

                    result.AddRange(GetMovesUp(piece));
                }
            }

            return result;
        }

        private static List<Move> GetMovesKing(PieceVM piece, bool checksForFreeTiles = true, List<PieceVM> pastPiecesTaken = null)
        {
            var moves = new List<Move>();
            var cameFrom = piece.Piece.PieceLocation;

            int x = piece.Piece.PieceLocation.Row;
            int y = piece.Piece.PieceLocation.Column;

            Location NorthWest = new Location(x + NORTH, y + WEST);
            Location NorthEast = new Location(x + NORTH, y + EAST);

            if (IsOnTable(NorthWest))
            {
                var NorthWestPiece = GetPieceByLocation(NorthWest);
                if (NorthWestPiece == null)
                {
                    if (checksForFreeTiles)
                    {

                        var plainMove = new Move(NorthWest);
                        moves.Add(plainMove);
                    }
                }
                else if (NorthWestPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {

                    Location NextNW = new Location(NorthWest.Row + NORTH, NorthWest.Column + WEST);
                    if (IsOnTable(NextNW))
                    {
                        var PseudoNWPiece = GetPieceByLocation(NextNW);
                        if (PseudoNWPiece == null)
                        {

                            var simpleJumpMove = new Move(NextNW);

                            if (!checksForFreeTiles)
                            {

                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(NorthWestPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesKing(piece.GetPhantomAt(NextNW), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }

            if (IsOnTable(NorthEast))
            {
                var NorthEastPiece = GetPieceByLocation(NorthEast);
                if (NorthEastPiece == null)
                {
                    if (checksForFreeTiles)
                    {

                        var plainMove = new Move(NorthEast);
                        moves.Add(plainMove);
                    }
                }
                else if (NorthEastPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {

                    Location NextNE = new Location(NorthEast.Row + NORTH, NorthEast.Column + EAST);
                    if (IsOnTable(NextNE))
                    {
                        var PseudoNEPiece = GetPieceByLocation(NextNE);
                        if (PseudoNEPiece == null)
                        {

                            var simpleJumpMove = new Move(NextNE);

                            if (!checksForFreeTiles)
                            {

                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(NorthEastPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesKing(piece.GetPhantomAt(NextNE), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }

            Location SouthWest = new Location(x + SOUTH, y + WEST);
            Location SouthEast = new Location(x + SOUTH, y + EAST);

            if (IsOnTable(SouthWest))
            {
                var SouthWestPiece = GetPieceByLocation(SouthWest);
                if (SouthWestPiece == null)
                {
                    if (checksForFreeTiles)
                    {

                        var plainMove = new Move(SouthWest);
                        moves.Add(plainMove);
                    }
                }
                else if (SouthWestPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {

                    Location NextSW = new Location(SouthWest.Row + SOUTH, SouthWest.Column + WEST);
                    if (IsOnTable(NextSW))
                    {
                        var PseudoNWPiece = GetPieceByLocation(NextSW);
                        if (PseudoNWPiece == null)
                        {

                            var simpleJumpMove = new Move(NextSW);

                            if (!checksForFreeTiles)
                            {

                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(SouthWestPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesKing(piece.GetPhantomAt(NextSW), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }

            if (IsOnTable(SouthEast))
            {
                var SouthEastPiece = GetPieceByLocation(SouthEast);
                if (SouthEastPiece == null)
                {
                    if (checksForFreeTiles)
                    {
                        var plainMove = new Move(SouthEast);
                        moves.Add(plainMove);
                    }
                }
                else if (SouthEastPiece.Piece.PieceColor != piece.Piece.PieceColor)
                {
                    Location NextSE = new Location(SouthEast.Row + SOUTH, SouthEast.Column + EAST);
                    if (IsOnTable(NextSE))
                    {
                        var PseudoNEPiece = GetPieceByLocation(NextSE);
                        if (PseudoNEPiece == null)
                        {
                            var simpleJumpMove = new Move(NextSE);

                            if (!checksForFreeTiles)
                            {
                                foreach (var pastPiece in pastPiecesTaken)
                                {
                                    simpleJumpMove.TakenPieces.Add(pastPiece);
                                }
                            }

                            simpleJumpMove.TakenPieces.Add(SouthEastPiece);
                            moves.Add(simpleJumpMove);

                            moves.AddRange(GetMovesKing(piece.GetPhantomAt(NextSE), false, simpleJumpMove.TakenPieces));
                        }
                    }
                }
            }

            piece.Piece.PieceLocation = cameFrom;

            if (piece.IsPhantom)
                Services.boardVM.Pieces.Remove(piece);

            return moves;
        }
    }

}
