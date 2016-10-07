﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NAT.Models;
using NAT.Views;

namespace NAT.Controllers {
    public class RaceGameController : ControllerBase<IRaceGameModel, IView> {

        public override int[] GameTurnDelta { get; protected set; } = new int[] { 300, 300 };
        public override int GameInputDelta { get; protected set; } = 50;

        protected override int minTurnDelta { get; set; } = 100;
        protected override int startTurnDelta { get; set; } = 200;

        protected override int GameTurnDecreaseIndex { get; set; } = 10000;
        protected override int GameInputDecreaseIndex { get; set; } = 5000;

        protected override string GameName { get; } = "race";

        public override void Render() {
            _view.Display(_model);
        }

        public override void _start() {
            _model.GameEnd += () => {
                ProcessTurns = false;
                _view.DisplayGameOver();
            };
        }

        public override void _update(GameTime _time) {
            if (!ProcessTurns || !IsGameStarted) return;

            if (_GameTurnTimer[0] >= GameTurnDelta[0]) {
                _model.ProcessTurn(0);
                _model.ProcessTurn(1);

                _GameTurnTimer[0] = 0;
            }

        }

        protected override void ProcessInput(Keys key) {
            if (key == Keys.Left) _model.MoveCar(-1);
            if (key == Keys.Right) _model.MoveCar(1);
            if (key == Keys.Space)
                _model.Ferrari.mapId = _model.Ferrari.mapId == 0 ? 1 : 0;
        }
    }
}
