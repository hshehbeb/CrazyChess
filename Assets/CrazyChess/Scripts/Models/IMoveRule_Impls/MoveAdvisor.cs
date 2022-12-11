using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;

namespace CrazyChess.Scripts.Models
{
    public class MoveAdvisor : IMoveRule
    {
        private ChessBoard _board;
        private Palace _palace;

        public MoveAdvisor(ChessBoard board, Palace palace)
        {
            _board = board;
            _palace = palace;
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var beforeCulling = new MoveCross(_board, 1)
                .GetAvailableMoves(piece);

            return _palace.KeepInsidersOnly(beforeCulling);
        }
        
    }
}
