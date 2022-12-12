using UnityEngine;

namespace CrazyChess.Scripts
{
    public abstract class Player : MonoBehaviour
    {
        public string id;
        
        public abstract void StartItsTurn();
        public abstract void FinishItsTurn();
    }
}
