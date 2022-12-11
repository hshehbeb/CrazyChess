using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    public class MoveHorse : IMoveRule
    {
        private ChessBoard _board;
        private bool _blockable;
        
        private Vector2Int[] _dirs = {
            new (1, 2),
            new (2, 1),
            new (-1, 2),
            new (-2, 1),
            new (-1, -2),
            new (-2, -1),
            new (1, -2),
            new (2, -1)
        };

        public MoveHorse(ChessBoard board, bool blockable)
        {
            _board = board;
            _blockable = blockable;
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var calculator = new SimpleMove(
                piece.GridPos, _dirs,
                _board, piece.Owner, 1
            );
            
            return calculator.GetMoves();
        }

    }
}
