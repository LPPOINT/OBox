using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Rotorz.Tile;
using UnityEngine;

namespace Assets.Scripts.Map
{
    [RequireComponent(typeof(GameMap))]
    [ExecuteInEditMode]
    public class MapItemSizeHelper : MonoBehaviour
    {
        public enum ExecuteMode
        {
            Edit,
            Player,
            Both,
            NoExecute
        }

        public ExecuteMode Mode;
        public bool CanExecute
        {
            get
            {
                if (Mode == ExecuteMode.Both) return true;
                if (Mode == ExecuteMode.Edit && Application.isEditor) return true;
                if (Mode == ExecuteMode.Player && Application.isPlaying) return true;

                return false;
            }
        }

        private void Start()
        {
            oldItems = new List<MapItem>();
        }

        private TileSystem ts;
        private TileSystem TileSystem
        {
            get { return ts ?? (ts = GetComponent<TileSystem>()); }
        }

        [SerializeField]
        [HideInInspector]
        private List<MapItem> oldItems;

        private void AlignMapItem(MapItem mapItem)
        {
            mapItem.AlignMapItemToCellSize();
        }

        private void Update()
        {
            if (!CanExecute) return;
            var allItems = gameObject.GetComponentsInChildren<MapItem>();
            var newItems =
                new List<MapItem>(allItems.Where(item => !oldItems.Contains(item)));


            for (var i = 0; i < oldItems.Count; i++)
            {
                if (!allItems.Contains(oldItems[i]))
                {
                    Debug.Log("Remove deleted map item");
                    oldItems.RemoveAt(i);
                }
            }

            foreach (var newItem in newItems)
            {
                AlignMapItem(newItem);
            }


            oldItems.AddRange(newItems);


        }

    }
}
