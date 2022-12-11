using System;
using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;

namespace CrazyChess.Scripts.Models
{
    // simple container class of a group of ChessPiece
    public class ChessBoard
    {
        public int TotalCols { get; }
        public int TotalRows { get; }
        public readonly ChessPiece[,] Map;
        private Dictionary<int, ChessPiece> _idToPieceMap; 

        public ChessBoard(int totalCols, int totalRows)
        {
            TotalCols = totalCols;
            TotalRows = totalRows;
            Map = new ChessPiece[totalRows, totalCols];
            _idToPieceMap = new Dictionary<int, ChessPiece>();
        }

        public void RegisterPiece(ChessPiece piece, Vector2Int atPos)
        {
            if (GetPiece(atPos) != null)
                throw new InvalidOperationException();

            Map[atPos.x, atPos.y] = piece;
            _idToPieceMap[piece.Id] = piece;
            piece.GridPos = atPos;
        }

        public ChessPiece GetPiece(int row, int col)
            => GetPiece(new Vector2Int(row, col));

        public ChessPiece GetPiece(Vector2Int atPos)
        {
            if (!InsideBound(atPos))
                throw new ArgumentException();

            return Map[atPos.x, atPos.y];
        }
        
        public ChessPiece GetPieceById(int id) 
            => _idToPieceMap[id];

        public void RemovePiece(Vector2Int atPos)
        {
            var toRemove = GetPiece(atPos);
            _idToPieceMap.Remove(toRemove.Id);
            
            Map[atPos.x, atPos.y] = null;
        }

        public void MovePiece(Vector2Int srcPos, Vector2Int dstPos)
        {
            var pieceToMove = GetPiece(srcPos);
            if (pieceToMove is null) 
                return;

            pieceToMove.HasBeenMoved = true;
                
            RemovePiece(srcPos);
            RegisterPiece(pieceToMove, dstPos); 
        }

        public bool InsideBound(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < TotalCols && pos.y >= 0 && pos.y < TotalRows;
        }

        public bool IsOccupiedByOthers(Vector2Int atPos, string owner)
        {
            var piece = GetPiece(atPos);
            return piece != null && piece.Owner != owner;
        }

        public bool IsOccupiedByUs(Vector2Int atPos, string owner)
        {
            var piece = GetPiece(atPos);
            return piece != null && piece.Owner == owner;
        }
        
    }
}
