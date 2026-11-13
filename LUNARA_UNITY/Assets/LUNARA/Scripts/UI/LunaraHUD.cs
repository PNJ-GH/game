using UnityEngine;

namespace Lunara.Gameplay
{
    public sealed class LunaraHUD : MonoBehaviour
    {
        [SerializeField] private LunaraPlayerController player;
        private GUIStyle healthStyle;
        private GUIStyle instructionStyle;

        private void Awake()
        {
            healthStyle = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
            healthStyle.normal.textColor = new Color(0.78f, 0.92f, 1f);
            instructionStyle = new GUIStyle(GUI.skin.label) { fontSize = 14 };
            instructionStyle.normal.textColor = new Color(0.78f, 0.86f, 0.96f);
        }

        private void OnGUI()
        {
            if (player == null) return;
            GUI.Label(new Rect(24, 20, 360, 34), $"VITALITY  {player.Health} / 100", healthStyle);
            GUI.Label(new Rect(24, 58, 760, 26),
                "WASD Move | Mouse Look | Space Jump | Shift Sprint | Left Click Moonblade | Esc Cursor",
                instructionStyle);
            GUI.Label(new Rect(Screen.width * 0.5f - 8, Screen.height * 0.5f - 18, 24, 36), "+", healthStyle);
        }
    }
}
