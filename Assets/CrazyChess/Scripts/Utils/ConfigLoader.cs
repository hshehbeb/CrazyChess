using System;
using System.Collections.Generic;
using System.Linq;
using CrazyChess.Scripts.DataStructures;
using CrazyChess.Scripts.Models;
using CrazyChess.Scripts.Views;
using LitJson;
using UnityEngine;

namespace CrazyChess.Scripts.Utils
{
    public class ConfigLoader : MonoBehaviour
    {
        public TextAsset configJson;
        public CrazyChessGame game;
        public ChessBoard_View board;

        private void Start()
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            var text = configJson.text;
            var conf = JsonMapper.ToObject(text);

            SpawnPlayers(conf);
            ConfigurateChessBoard(conf);
            SpawnChessPieces(conf);
            
            game.Start();
        }

        private void ConfigurateChessBoard(JsonData conf)
        {
            var totalCols = (int)conf["map"]["total_cols"];
            var totalRows = (int)conf["map"]["total_rows"];
            
            board.TotalCols = totalCols;
            board.TotalRows = totalRows;

            game.boardModel = new ChessBoard(totalCols, totalRows);

            game.board.gizmosEnabled = (bool)conf["map"]["draw_gizmos"];
        }
        
        private void SpawnChessPieces(JsonData conf)
        {
            foreach (JsonData pieceConf in conf["placement"])
            {
                var gridPos = new Vector2Int(
                    (int) pieceConf["pos"]["x"],
                    (int) pieceConf["pos"]["y"]
                );

                var type = (string)pieceConf["type"];
                var rule = GetCorrespondingRule(conf["pieces"][type]["rule"]);
                var isKing = conf["pieces"][type].Keys.Contains("is_king") 
                             && (bool)conf["pieces"][type]["is_king"];
                var owner = (string)pieceConf["owner"];
                var model = new ChessPiece(rule, owner) { IsKing = isKing };
                game.boardModel.RegisterPiece(model, gridPos);

                var view = game.board.SpawnChessPiece(gridPos, model.Id);
                view.SetIcon((string)conf["pieces"][type]["sprite"]);
            }
        }

        private IMoveRule GetCorrespondingRule(JsonData ruleConf)
        {
            switch ((string)ruleConf["type"])
            {
                case "straight_move":
                    return new MoveStraight(
                        game.boardModel, (int)ruleConf["args"]["grids_can_move"],
                        GetDirection(ruleConf["args"]["dir"])
                    );
                case "horse_move":
                    return new MoveHorse(
                        game.boardModel, (bool)ruleConf["args"]["blockable"]);
                case "cross_move":
                    return new MoveCross(
                        game.boardModel, (int)ruleConf["args"]["stride"]);
                case "advisor_move":
                    return new MoveAdvisor(
                        game.boardModel, GetPalace(ruleConf["args"]["palace"]));
                case "general_move":
                    return new MoveGeneral(
                        game.boardModel, GetPalace(ruleConf["args"]["palace"]),
                        GetDirection(ruleConf["args"]["dir"])
                    );
                case "pawn_move":
                    return new MovePawn(
                        game.boardModel, GetDirection(ruleConf["args"]["dir"]));
                case "free_move":
                    return new MoveFree(
                        game.boardModel, (int)ruleConf["args"]["grids_can_move"]);
                
                default:
                    throw new NotSupportedException((string)ruleConf["type"]);
            }
        }

        private MoveStraight.Direction GetDirection(JsonData dirConf)
        {
            MoveStraight.Direction dirFlag = 0;
            foreach (JsonData d in dirConf)
            {
                if ((string)d is "north") 
                    dirFlag |= MoveStraight.Direction.North;
                else if ((string)d is "south")
                    dirFlag |= MoveStraight.Direction.South;
                else if ((string)d is "east")
                    dirFlag |= MoveStraight.Direction.East;
                else if ((string)d is "west")
                    dirFlag |= MoveStraight.Direction.West;
            }

            return dirFlag;
        }
        
        private Palace GetPalace(JsonData palaceConf)
        {
            var minX = (int)palaceConf["min"]["x"]; 
            var minY = (int)palaceConf["min"]["y"];
            var maxX = (int)palaceConf["max"]["x"];
            var maxY = (int)palaceConf["max"]["y"];
            
            return new Palace(new RectInt(
                (int)palaceConf["min"]["x"],
                (int)palaceConf["min"]["y"],
                Mathf.Abs(maxX - minX),
                Mathf.Abs(maxY - minY)
            ));
        }
        
        private void SpawnPlayers(JsonData conf)
        {
            foreach (JsonData playerConf in conf["players"])
            {
                var player = new GameObject();
                player.transform.SetParent(game.transform);
                
                Player comp = (string)playerConf["type"] switch {
                    "human" => player.AddComponent<HumanPlayer>(),
                    "ai"    => player.AddComponent<AiPlayer>(),
                    _ => throw new NotSupportedException()
                };
                
                game.Players.Add(comp);
                
                var itsId = (string)playerConf["id"];
                comp.id = itsId;
                player.name = itsId;
            }
        }
        
    }
}
