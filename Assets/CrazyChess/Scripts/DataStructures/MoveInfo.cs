using System.Linq;
using UnityEngine;

namespace CrazyChess.Scripts.DataStructures
{
    public class MoveInfo
    {
        public Vector2Int ArriveGrid;
        public Vector2Int StartGrid;
        public int PieceId_Eaten;
        public int PieceId_Moved;
        private Models.ChessBoard _board;
        private Models.ChessPiece _pieceMoved;
        private string _playerId;

        public MoveInfo(
            Models.ChessBoard board, 
            Vector2Int startGrid, 
            Vector2Int arriveGrid)
        {
            StartGrid = startGrid;
            ArriveGrid = arriveGrid;
            _board = board;
            _pieceMoved = _board.GetPiece(StartGrid);
            _playerId = _pieceMoved.Owner;
            
            PieceId_Moved = _pieceMoved.Id;
            PieceId_Eaten = _board.GetPiece(ArriveGrid)?.Id ?? -1;
        }

        public void ConfirmMove()
        {
            if (AnyPieceEaten())
                _board.RemovePiece(ArriveGrid);
            
            _board.MovePiece(StartGrid, ArriveGrid);
        }

        // according to chess rule,
        // when king is attacked by a piece or pawn,
        // he is said to be in “check”
        public bool CauseCheck()
        {
            var reachableGrids = _board
                .GetAllPiecesOwnedBy(_playerId)
                .SelectMany(piece => piece.GetAvailableMoves())
                .Select(move => move.ArriveGrid);

            return reachableGrids
                .Select(atPos => _board.GetPiece(atPos))
                .Any(piece => piece != null && NotOwnerByUs(piece) && piece.IsKing);
        }

        private bool NotOwnerByUs(Models.ChessPiece piece)
        {
            return piece.Owner != _playerId;
        }

        public bool AnyPieceEaten()
            => PieceId_Eaten != -1;
    }
}
