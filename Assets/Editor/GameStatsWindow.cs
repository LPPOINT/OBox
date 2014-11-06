using System.Globalization;
using Assets.Scripts.Model;
using UnityEditor;

namespace Assets.Editor
{
    public class GameStatsWindow : EditorWindow
    {
        [MenuItem("Window/Game Stats")]
        private static void Init()
        {
            var w = GetWindow<GameStatsWindow>();
            w.title = "Game Stats";
            w.Show();
        }

        private void OnGUI()
        {
            var model = GameModel.Instance;

            EditorGUILayout.LabelField("Currency", model.GetCurrency().ToString(CultureInfo.InvariantCulture));

        }
    }
}
