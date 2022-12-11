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
        public List<Player> Players
        {
            get
            {
                if (_players.Count is 0)
                    _players = FindObjectsOfType<Player>().ToList();
                
                return _players;
            }
            set => _players = value;
        }
        // PRIVATE
        private int _current;
        private static CrazyChessGame _singleton;
        private List<Player> _players = new ();

        public void Start()
        {
            if (Players.Count is 0)
                return;
            
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
