using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model.Numeration;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorldDataManager : MonoBehaviour
    {
        private static GUIWorldDataManager Main { get;  set; }
        private static List<GUIWorldData> MainData { get;  set; }

        private static void EstablishData(GUIWorldData data)
        {
            foreach (var f in data.Features)
            {
                if (string.IsNullOrEmpty(f.Description))
                {
                    f.Description = "Worlds.Feature.World" + (int)data.Number;
                }
            }
        }

        public static GUIWorldData GetWorldDataByNumber(WorldNumber number)
        {
            var instanceData = FindObjectOfType<GUIWorldDataManager>().Data;
            var result = instanceData.FirstOrDefault(data => data.Number == number);
            EstablishData(result);
            return result;
        }

        private void Awake()
        {
            Main = this;
            MainData = Data;
        }

        [SerializeField]
        private List<GUIWorldData> Data;
    }
}
