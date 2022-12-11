using System.Collections.Generic;
using System.Linq;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    public class MoveCross : IMoveRule
    {
        private ChessBoard _board;
        private int _stride;
        private Vector2Int[] _dirs = {
            new (1, 1),
            new (-1, 1),
            new (-1, -1),
            new (1, -1)
        };

        public MoveCross(ChessBoard board, int stride)
        {
            _stride = stride;
            _board = board;
            
            _dirs = _dirs
                .Select(d => d * stride)
                .ToArray();
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var calculator = 
                new SimpleMove(piece.GridPos, _dirs, _board, piece.Owner, 1);

            return calculator.GetMoves();
        }
        
    }
}
