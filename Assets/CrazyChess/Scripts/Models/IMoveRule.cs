using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;

namespace CrazyChess.Scripts.Models
{
    public interface IMoveRule
    {
        IEnumerable<MoveInfo> GetAvailableMoves(ChessPiece piece);
    }
}
