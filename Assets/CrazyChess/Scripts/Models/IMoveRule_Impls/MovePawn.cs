using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;

namespace CrazyChess.Scripts.Models
{
    public class MovePawn : IMoveRule
    {
        private ChessBoard _board;
        private MoveStraight.Direction _dir;

        public MovePawn(ChessBoard board, MoveStraight.Direction faceDir)
        {
            _board = board;
            _dir = faceDir;
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var gridsCanMove = 
                (piece.HasBeenMoved ? 1 : 2);
            
            return new MoveStraight(_board, gridsCanMove, _dir)
                .GetAvailableMoves(piece);
        }
        
    }
}
