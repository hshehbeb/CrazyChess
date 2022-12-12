using System.Collections.Generic;
using System.Linq;
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
        private IEnumerable<ChessPiece_View> _ownedPieces;

        public override void StartItsTurn()
        {
            _ownedPieces = Board_Model
                .GetAllPiecesOwnedBy(id)
                .Select(model => Board_View.GetPieceById(model.Id));
            
            RegisterOnClickHandle();
        }

        private void RegisterOnClickHandle()
        {
            foreach (var piece in _ownedPieces)
                piece.onClick.AddListener( RenderItsAvailableMoves );
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
            UnRegisterOnClickHandle();
        }
        
        private void UnRegisterOnClickHandle()
        {
            foreach (var piece in _ownedPieces)
                piece.onClick.RemoveListener( RenderItsAvailableMoves );
        }
        
    }
}
