using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;

namespace CrazyChess.Scripts.Models
{
    public class MoveGeneral : IMoveRule
    {
        private ChessBoard _board;
        private Palace _palace;
        private MoveStraight.Direction _dir;

        public MoveGeneral(
            ChessBoard board, Palace palace, 
            MoveStraight.Direction faceDir)
        {
            _board = board;
            _palace = palace;
            _dir = MoveStraight.Direction.East | MoveStraight.Direction.West | faceDir;
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var beforeCulling
                = new MoveStraight(_board, 1, _dir).GetAvailableMoves(piece);

            return _palace.KeepInsidersOnly(beforeCulling);
        }

    }
}
