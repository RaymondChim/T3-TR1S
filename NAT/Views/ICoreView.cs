using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAT.Menu;
using NAT.Services;

namespace NAT.Views {
    public interface ICoreView : IView{
        Action TurnOnButtonPressed { get; set; }

        void StartBootingAnimation(IBootingAnimationService service);
        Action<IBootingAnimationService> OnBootingAnimationEnd { get; set; }

        void GlobalDisplay();

        void DisplayUserNameInput(NamerKeysInputAdapter adapter);
        Action<string> SetUserName { get; set; }
        Action<string> GameSelected { get; set; }

        bool EnableInput { get; set; }
    }
}
