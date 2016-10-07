using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NAT.Menu;
using NAT.Services;
using NAT.Views;

namespace NAT.Controllers {

    public class ControllerSenpai {

        private static ControllerSenpai _self { get; set; }
        public static ControllerSenpai Instance { get {
                return _self;
            } }

        public string username { get; set; }

        public ICoreView CoreView { get; set; }

        public static bool AnySelector(KeyValuePair<string, Tuple<IGameController, bool>> x) {
            return true;
        }

        public static bool ActiveSelector(KeyValuePair<string, Tuple<IGameController, bool>> x) {
            return x.Value.Item2;
        }

        public Dictionary<string, Tuple<IGameController, bool>> _controllers 
            = new Dictionary<string, Tuple<IGameController, bool>>();

        private bool IsPCON { get; set; } = false;

        public ControllerSenpai(List<Tuple<string,IGameController>> controllers) {
            this._controllers = controllers.ToDictionary(x => x.Item1 ,x => new Tuple<IGameController,bool>(x.Item2,false));
            ControllerSenpai._self = this;
        }

        public void SetControllerActive(string name, bool active) {
            if (!_controllers.ContainsKey(name)) return;
            else _controllers[name] = new Tuple<IGameController, bool>(_controllers[name].Item1, active);
        }

        public void SetAllControllersActive(bool active) {
            _controllers = _controllers.ToDictionary(x => x.Key, x => new Tuple<IGameController, bool>(x.Value.Item1,active));    
        }

        public void Update(GameTime _time, Func<KeyValuePair<string, Tuple<IGameController, bool>>,bool> selector) {
            foreach (var controller in GetBySelector(selector))
                controller.Update(_time);
            CoreView.Update(_time);
        }
        public void Render(Func<KeyValuePair<string, Tuple<IGameController, bool>>, bool> selector) {
            CoreView.GlobalDisplay();
            foreach (var controller in GetBySelector(selector)) controller.Render();
        }
        public void Start(Func<KeyValuePair<string, Tuple<IGameController, bool>>, bool> selector) {
            var bootingAnim = new BootingAnimationService();
            var shutdownAnim = new BootingAnimationService("shutdown.xml");
            CoreView.EnableInput = true;
            foreach (var controller in GetBySelector(selector)) controller.Start();
            CoreView.TurnOnButtonPressed += () => {
                CoreView.EnableInput = false;
                if (!IsPCON) {
                    CoreView.StartBootingAnimation(bootingAnim);
                    IsPCON = true;
                } else {
                    CoreView.StartBootingAnimation(shutdownAnim);
                    SetProcessTurn(false);
                }
            };
            CoreView.OnBootingAnimationEnd += (service) => {
                if (service == bootingAnim) {
                    CoreView.DisplayUserNameInput(new NamerKeysInputAdapter(new Namer()));
                } else {
                    //todo shuting down
                    Environment.Exit(0);
                }
            };
            CoreView.SetUserName += (name) => {
                username = name;
                CoreView.UserName = username;
                SetControllerActive("race",true);
                _controllers["race"].Item1.Start();
                CoreView.EnableInput = true;
            };
            CoreView.GameSelected += (game) => {
                SetAllControllersActive(false);
                foreach (var c in _controllers)ResetController(c.Key);
                SetControllerActive(game, true);
                _controllers[game].Item1.Start();
            };
        }

        private IEnumerable<IGameController> GetBySelector(Func<KeyValuePair<string, Tuple<IGameController, bool>>, bool> selector) {
            return _controllers.Where(x => selector(x) ).Select(x => x.Value.Item1);
        }

        private void ResetController(string name) {
            var controller = _controllers.Where(x => name == x.Key).Last();
            controller.Value.Item1.ResetModel();
        }
        private void SetProcessTurn(bool ProcessTurn) {
            foreach(var controller in _controllers.Values) {
                controller.Item1.ProcessTurns = ProcessTurn;
            }
        }
    }
}
