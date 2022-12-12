using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    public class ChessPiece
    {
        public int Id { get; }
        private IMoveRule MoveRule { get; }
        public Vector2Int GridPos { get; set; }
        public string Owner { get; }
        public bool HasBeenMoved { get; set; }
        public bool IsKing { get; set; } // eat it will cause game ends
        private static int _serialNumber;

        public ChessPiece(IMoveRule rule, string owner)
        {
            Id = (++_serialNumber);
            MoveRule = rule;
            Owner = owner;
        }

        public IEnumerable<MoveInfo> GetAvailableMoves()
            => MoveRule.GetAvailableMoves(this);

    }
}
