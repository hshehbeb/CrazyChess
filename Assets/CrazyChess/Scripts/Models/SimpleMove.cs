using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    public class SimpleMove
    {
        private Vector2Int _startPos;
        private readonly Vector2Int[] _velocities;
        private int _stepsCanTake;
        private ChessBoard _board;
        private string _turnOwner;

        public SimpleMove(
            Vector2Int startPos, Vector2Int[] velocities,
            ChessBoard board, string turnOwner, int stepsCanTake)
        {
            _startPos = startPos;
            _velocities = velocities;
            _board = board;
            _turnOwner = turnOwner;
            _stepsCanTake = stepsCanTake;
        }

        public IEnumerable<MoveInfo> GetMoves()
        {
            var moves = new List<MoveInfo>();
            
            foreach (var vel in _velocities)
            {
                CollectMovesRecursively(moves, vel, _startPos, 0);
            }

            return moves;
        }

        private void CollectMovesRecursively(
            List<MoveInfo> moves, Vector2Int vel, Vector2Int atPos, int stepsTaken)
        {
            if (stepsTaken >= _stepsCanTake)
                return;
                
            var dst = atPos + vel;

            if (!_board.InsideBound(dst) || _board.IsOccupiedByUs(dst, _turnOwner))
                return;

            var newMove = new MoveInfo(_board, _startPos, dst);
            moves.Add(newMove);

            if (_board.IsOccupiedByOthers(dst, _turnOwner))
                return;
            
            CollectMovesRecursively(moves, vel, dst, stepsTaken + 1);
        }

    }
}
