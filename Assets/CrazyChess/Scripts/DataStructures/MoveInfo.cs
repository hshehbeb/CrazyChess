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

        public MoveInfo(
            Models.ChessBoard board, 
            Vector2Int startGrid, 
            Vector2Int arriveGrid)
        {
            _board = board;
            StartGrid = startGrid;
            ArriveGrid = arriveGrid;

            PieceId_Moved = _board.GetPiece(StartGrid).Id;
            PieceId_Eaten = _board.GetPiece(ArriveGrid)?.Id ?? -1;
        }

        public void ConfirmMove()
        {
            if (AnyPieceEaten())
                _board.RemovePiece(ArriveGrid);
            
            _board.MovePiece(StartGrid, ArriveGrid);
        }

        public bool AnyPieceEaten()
            => PieceId_Eaten != -1;
    }
}
