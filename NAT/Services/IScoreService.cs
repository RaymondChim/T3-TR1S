using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace NAT.Services {

    [Serializable]
    public class ScoreHeaver {
        [XmlAttribute("Name")]
        public string Name;
        [XmlAttribute("Count")]
        public int Score;
        [XmlAttribute("Game")]
        public string Game;

        public ScoreHeaver(string _Name,int _Score, string _Game) {
            Score = _Score;
            Name = _Name;
            Game = _Game;
        }

        public ScoreHeaver() {}
    }

    [Serializable]
    [XmlRoot("ScoreTable")]
    public class Scores {
        [XmlArray("Scores")]
        [XmlArrayItem("Score")]
        public ScoreHeaver[] Heaver;

        public Scores() { }
        public Scores(ScoreHeaver[] _Heaver) {
            Heaver = _Heaver;
        }

    }

    public interface IScoreService {
        Scores GetScores(string Game);
        void AddScore(Tuple<string, int, string> score);
    }
}
