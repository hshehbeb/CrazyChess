using System;
using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    public class MoveStraight : IMoveRule
    {
        private ChessBoard _board;
        private int _gridsCanMove;
        private List<Vector2Int> _directions;

        public MoveStraight(ChessBoard board, int gridsCanMove, Direction dir)
        {
            _board = board;
            _gridsCanMove = gridsCanMove;
            
            ParseDirectionFlag(dir);
        }

        private void ParseDirectionFlag(Direction dir)
        {
            _directions = new List<Vector2Int>(4);

            if (dir.HasFlag(Direction.East))
                _directions.Add( new Vector2Int(1, 0) );
            if (dir.HasFlag(Direction.West))
                _directions.Add( new Vector2Int(-1, 0) );
            if (dir.HasFlag(Direction.North))
                _directions.Add( new Vector2Int(0, 1) );
            if (dir.HasFlag(Direction.South))
                _directions.Add( new Vector2Int(0, -1) );
        }

        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var calculator = new SimpleMove(
                piece.GridPos, _directions.ToArray(),
                _board, piece.Owner, _gridsCanMove
            );
            
            return calculator.GetMoves();
        }
        
        [Flags]
        public enum Direction
        {
            East = 1 << 0,
            West = 1 << 1,
            South = 1 << 2,
            North = 1 << 3,
        }
        
    }
}
