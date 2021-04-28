using CheckersV4.ViewModels;
using System.Collections.Generic;

namespace CheckersV4.Services
{
    public class Move
    {
        public Location FinalLocation
        {
            get;
            set;
        }
        
        public List<PieceVM> TakenPieces
        {
            get;
            set;
        }

        public Move(Location final)
        {
            TakenPieces = new List<PieceVM>();
            FinalLocation = final;
        }
    }
}
