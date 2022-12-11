using CrazyChess.Scripts.Models;
using CrazyChess.Scripts.Views;

namespace CrazyChess.Scripts
{
    public class HumanPlayer : Player
    {
        private ChessBoard_View Board_View 
            => CrazyChessGame.Singleton.board;
        private ChessBoard Board_Model
            => CrazyChessGame.Singleton.boardModel;

        public override void StartItsTurn()
        {
            Board_View.GetPiecesOwnedBy(this)
                .ForEach( RegisterOnClickHandle );
        }

        private void RegisterOnClickHandle(ChessPiece_View piece)
        {
            piece.onClick
                .AddListener( RenderItsAvailableMoves );
        }

        private void RenderItsAvailableMoves(ChessPiece_View piece)
        {
            var itsMoves = Board_Model
                .GetPieceById(piece.id)
                .GetAvailableMoves();
            
            Board_View.RenderItsAvailableMoves(itsMoves);
        }

        public override void FinishItsTurn()
        {
            Board_View.GetPiecesOwnedBy(this)
                .ForEach( UnRegisterOnClickHandle );
        }
        
        private void UnRegisterOnClickHandle(ChessPiece_View piece)
        {
            piece.onClick
                .RemoveListener( RenderItsAvailableMoves );
        }
        
    }
}
