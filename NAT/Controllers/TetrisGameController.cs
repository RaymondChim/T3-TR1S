﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NAT.Models;
using NAT.Services;
using NAT.Views;

namespace NAT.Controllers {
    public class TetrisGameController : ControllerBase<ITetrisGameModel,IView> {

        public override int[] GameTurnDelta { get; protected set; } = new int[] { 500, 500 };
        public override int GameInputDelta { get; protected set; } = 75;

        //protected override int minTurnDelta { get; set; } = 100;
        protected override int startTurnDelta { get; set; } = 500;

        protected override int GameTurnDecreaseIndex { get; set; } = 15000;
        protected override int GameInputDecreaseIndex { get; set; } = 8000;

        protected override string GameName { get; } = "tetris";

        public override void Render() {
            _view.Display(_model);
        }

        public override void _start() {
            _model.GameEnd += () =>{
                _view.DisplayGameOver();
                ProcessTurns = false;
            };

            _model.MapLocked += (mapId) => {
                GameTurnDelta[mapId] = 70;
            };
            _model.MapUnlocked += (mapId) =>{
                GameTurnDelta[mapId] = 500;
            };


        }

        public override void _update(GameTime _time) {

            if (!ProcessTurns || !IsGameStarted) return;

            if (_GameTurnTimer[0] >= GameTurnDelta[0]) {
                _model.ProccessTurn(0);
                _GameTurnTimer[0] = 0;
            }

            if (_GameTurnTimer[1] >= GameTurnDelta[1]) {
                _model.ProccessTurn(1);
                _GameTurnTimer[1] = 0;
            }

        }

        protected override void ProcessInput(Keys key) {

            if (key == Keys.Left) _model.MoveCurrentBlock(-1, _model.CurrentMapId);
            if (key == Keys.Right) _model.MoveCurrentBlock(1, _model.CurrentMapId);
            if (key == Keys.Up) _model.FlipCurrentBlock(_model.CurrentMapId);
            if (key == Keys.Down) _model.ProccessTurn(_model.CurrentMapId);

            if (key == Keys.Space) _model.CurrentMapId = _model.CurrentMapId == 0 ? 1 : 0;


        }
    }
}
