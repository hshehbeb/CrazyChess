using System.Collections.Generic;
using System.Linq;
using CrazyChess.Scripts.Views;
using UnityEngine;

namespace CrazyChess.Scripts
{
    public class CrazyChessGame : MonoBehaviour
    {
        // PUBLIC
        public static CrazyChessGame Singleton
        {
            get
            {
                if (!_singleton)
                {
                    _singleton = FindObjectOfType<CrazyChessGame>() 
                                 ?? throw new MissingReferenceException();
                }
                
                return _singleton;
            }
        }
        public ChessBoard_View board;
        public Models.ChessBoard boardModel;
        public List<Player> Players { get; private set; } = new ();

        // PRIVATE
        private int _current;
        private static CrazyChessGame _singleton;

        public void Start()
        {
            if (Players.Count is 0)
            {
                Players = FindObjectsOfType<Player>().ToList();

                var stillEmpty = (Players.Count is 0);
                if (stillEmpty) return;
            }
            
            Players[_current].StartItsTurn();
        }

        public void FinishCurrentTurn()
        {
            Players[_current].FinishItsTurn();
            
            _current = (_current + 1) % Players.Count;
            Players[_current].StartItsTurn();
        }
        
    }
}
