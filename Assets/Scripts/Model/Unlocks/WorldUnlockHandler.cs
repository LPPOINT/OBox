using Assets.Scripts.Model.Numeration;
using UnityEngine;

namespace Assets.Scripts.Model.Unlocks
{
    public class WorldUnlockHandler : IWorldUnlockHandler
    {

        public static WorldUnlockHandler UnlockAllWorlds()
        {
            var h = new WorldUnlockHandler();
            h.OnUnlockPerformed(new AllWorldsUnlockContext());
            return h;
        }

        public void OnUnlockPerformed(IWorldUnlockContext context)
        {
            Debug.Log("OnUnlockPerformed");
            foreach (var w in context.GetWorldsToUnlock())
            {
                GameModel.Instance.RegisterWorldUnlock(w);
            }
        }

        public void OnUnlockCanceled()
        {
            Debug.Log("OnUnlockCanceled");
        }
    }
}
