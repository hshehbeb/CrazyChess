using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    public class MoveFree : IMoveRule
    {
        private int _gridsCanMove;
        private ChessBoard _board;
        
        private readonly Vector2Int[] _velocities = {
            new(1, 0), new (-1, 0),
            new(0, 1), new (0, -1),
            new(1, 1), new (-1, -1),
            new(-1, 1), new (1, -1)
        };

        public MoveFree(ChessBoard board, int gridsCanMove)
        {
            _gridsCanMove = gridsCanMove;
            _board = board;
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var calculator = new SimpleMove(piece.GridPos, 
                _velocities, _board, piece.Owner, _gridsCanMove);

            return calculator.GetMoves();
        }
        
    }
}
