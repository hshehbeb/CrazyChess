using System;
using System.Linq;
using CrazyChess.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CrazyChess.Scripts.Views
{
    public class ChessPiece_View : MonoBehaviour
    {
        // PUBLIC
        public int id;
        public UnityEvent<ChessPiece_View> onClick;
        public Player owner; 
        public SpriteDatabaseEntry[] iconsDatabase;
        // PRIVATE
        [SerializeField] private Image imgIcon;
        private Button _btn;

        private void Start()
        {
            _btn = GetComponentInChildren<Button>();
            _btn.onClick.AddListener( ConvertSignature(onClick) );
        }

        private UnityAction ConvertSignature(UnityEvent<ChessPiece_View> convertee)
        {
            return () => convertee.Invoke(this);
        }

        public void SetIcon(string spriteId)
        {
            imgIcon.sprite = FIndSprite(spriteId);
        }
        
        private Sprite FIndSprite(string spriteId)
        {
            return iconsDatabase
                .FirstOrDefault(entry => entry.spriteId == spriteId)
                .sprite;
        }
        
    }
}
