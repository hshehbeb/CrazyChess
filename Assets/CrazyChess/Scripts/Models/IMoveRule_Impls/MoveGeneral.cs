using System.Collections.Generic;
using System.Linq;
using CrazyChess.Scripts.DataStructures;

namespace CrazyChess.Scripts.Models
{
    public class MoveGeneral : IMoveRule
    {
        private ChessBoard _board;
        private Palace _palace;
        private MoveStraight.Direction _dir;

        public MoveGeneral(
            ChessBoard board, Palace palace, 
            MoveStraight.Direction faceDir)
        {
            _board = board;
            _palace = palace;
            _dir = MoveStraight.Direction.East | MoveStraight.Direction.West | faceDir;
        }
        
        public IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece)
        {
            var beforeCulling
                = new MoveStraight(_board, 1, _dir).GetAvailableMoves(piece);
            
            var cullOutsidePalace 
                = _palace.KeepInsidersOnly(beforeCulling);
            
            var cullEaten 
                = AvoidBeingEaten(cullOutsidePalace, piece.Owner);

            return cullEaten;
        }

        private IEnumerable<MoveInfo> AvoidBeingEaten(
            IEnumerable<MoveInfo> moves, string owner)
        {
            var reachableGridsByOthers = _board
                .GetAllPiecesOwnedByOthers(owner)
                .SelectMany(piece => piece.GetAvailableMoves())
                .Select(move => move.ArriveGrid);

            return moves.Where(
                move => !reachableGridsByOthers.Contains(move.ArriveGrid)
            );
        }

    }
}
