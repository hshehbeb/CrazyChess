using System.Collections;
using System.Collections.Generic;
using CrazyChess.Scripts.DataStructures;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace CrazyChess.Scripts.Views
{
    public class ChessBoard_View : MonoBehaviour
    {
        // PUBLIC
        public int TotalCols
        {
            get => _totalCols;
            set
            {
                _totalCols = value;
                
                var rt = GetComponent<RectTransform>();
                _cellSizeX = rt.rect.width / _totalCols;
            }
        }
        public int TotalRows
        {
            get => _totalRows;
            set
            {
                _totalRows = value;
                
                var rt = GetComponent<RectTransform>();
                _cellSizeY = rt.rect.height / _totalRows;
            } 
        }
        public List<ChessPiece_View> piecesAlive = new (20);
        public List<ChessPiece_View> piecesEaten = new (20);
        public bool gizmosEnabled;
        // PRIVATE
        private int _totalCols;
        private int _totalRows;
        private float _cellSizeX;
        private float _cellSizeY;
        private Dictionary<int, ChessPiece_View> _idToPieceMap = new ();
        [SerializeField] private ChessPiece_View piecePrefab;
        [SerializeField] private Transform piecesHolder;
        [SerializeField] private Transform moveTargetHolder;
        [SerializeField] private GameObject moveTargetPrefab;

        public ChessPiece_View SpawnChessPiece(Vector2Int gridPos, int id)
        {
            var worldPos = GridToWorldSpace(gridPos);
            var go = Instantiate(
                piecePrefab, worldPos,
                piecePrefab.transform.rotation, piecesHolder
            );
            
            go.id = id;
            
            piecesAlive.Add(go);
            _idToPieceMap.Add(id, go);

            ResizeRectTransformToFitCellSize(go.gameObject);
            return go;
        }
        
        public Vector3 GridToWorldSpace(Vector2Int gridCoord)
        {
            var local = new Vector3(gridCoord.x * _cellSizeX, gridCoord.y * _cellSizeY);
            var world = transform.TransformPoint(local);
            
            return world;
        }

        public void RenderItsAvailableMoves(IEnumerable<MoveInfo> moves)
        {
            DestroyStaleMoveTargets();
            
            foreach (var move in moves)
            {
                var pos = GridToWorldSpace(move.ArriveGrid);

                SpawnMoveTarget(pos)
                    .onClick
                    .AddListener(() => StartCoroutine(ConfirmMove(move)));
            }
        }

        private void DestroyStaleMoveTargets()
        {
            for (var i = 0; i < moveTargetHolder.childCount; i++) 
                Destroy(moveTargetHolder.GetChild(i).gameObject);
        }

        private Button SpawnMoveTarget(Vector3 atPos)
        {
            var go = Instantiate(
                moveTargetPrefab, atPos, Quaternion.identity, moveTargetHolder);
            
            ResizeRectTransformToFitCellSize(go);

            return go.GetComponentInChildren<Button>();
        }

        private void ResizeRectTransformToFitCellSize(GameObject go)
        {
            go.GetComponent<RectTransform>().sizeDelta
                = new Vector2(_cellSizeX, _cellSizeY);
        }

        private IEnumerator ConfirmMove(MoveInfo move)
        {
            move.ConfirmMove();
            
            DestroyStaleMoveTargets();
                
            yield return RenderMove(move);
            
            if (move.AnyPieceEaten())
                yield return RenderEat(move);
        } 

        public IEnumerator RenderMove(MoveInfo move)
        {
            const float duration = 1f;

            var src = GridToWorldSpace(move.StartGrid);
            var dst = GridToWorldSpace(move.ArriveGrid); 
            var velocity = (dst - src) / duration;

            var pieceMoving = _idToPieceMap[move.PieceId_Moved].transform;
            const float approximate = .3f;
            while (Vector3.Distance(pieceMoving.position, dst) > approximate)
            {
                pieceMoving.transform.Translate(velocity * Time.deltaTime);
                yield return null;
            }
            
            CrazyChessGame.Singleton.FinishCurrentTurn();
        }
        
        public IEnumerator RenderEat(MoveInfo move)
        {
            var eaten = _idToPieceMap[move.PieceId_Eaten];
            eaten.gameObject.SetActive(false);

            piecesAlive.Remove(eaten);
            piecesEaten.Add(eaten);

            yield break;
        }

        public void OnDrawGizmos()
        {
            if (!gizmosEnabled) 
                return;

            var rect = GetComponent<RectTransform>().rect;
            var leftBottomCorner = rect.min;
            var rightTopCorner = rect.max;
            var leftTopCorner = new Vector2(leftBottomCorner.x, rightTopCorner.y);

            for (var i = 0; i <= TotalCols; i++)
            {
                var offset = new Vector2(i * _cellSizeX, 0);
                
                DrawGizmosLine(
                    leftBottomCorner + offset,
                    leftTopCorner + offset
                );
            }
            
            for (var i = 0; i <= TotalRows; i++)
            {
                var offset = new Vector2(0, -i * _cellSizeY);
                
                DrawGizmosLine(
                    leftTopCorner + offset,
                    rightTopCorner + offset
                );
            }
        }

        private void DrawGizmosLine(Vector3 p1, Vector3 p2)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                transform.TransformPoint(p1), transform.TransformPoint(p2));
            
            Gizmos.color = oldColor;
        }

        public List<ChessPiece_View> GetPiecesOwnedBy(HumanPlayer owner)
        {
            return piecesAlive.FindAll(piece => piece.owner == owner);
        }
        
    }
}
