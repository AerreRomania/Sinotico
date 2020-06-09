using System.Drawing;

namespace Sinotico
{
    class Globals
    {
        public Color[] GetStopColors() => new Color[] { Color.DeepSkyBlue, Color.DarkKhaki, Color.Red, Color.Purple, Color.Plum, Color.Tan, Color.Coral, Color.RosyBrown };
        public string[] StopReasonsEn { get => new string[] { "Knitt", "Comb", "Manual", "Yarn", "Needle", "Shock", "Roller", "Other" }; }
        public string[] StopReasonsIt { get => new string[] { "Shima", "Pettine", "Manuale", "Filato", "Aghi", "Urto", "Rulli", "Altro" }; }
        public Color[] GetStopColorsOrigin() => new Color[] { };
        public string[] StopReasonsOrigin { get => new string[] { "" }; }
    }
}