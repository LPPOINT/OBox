using System.Collections.Generic;
using Assets.Scripts.Model.Numeration;

namespace Assets.Scripts.Model.Unlocks
{
    public interface IWorldUnlockContext
    {
        IEnumerable<WorldNumber> GetWorldsToUnlock();

    }

    public class SingleWorldUnlockContext : IWorldUnlockContext
    {
        public SingleWorldUnlockContext(WorldNumber world)
        {
            World = world;
        }

        public WorldNumber World { get; private set; }

        public IEnumerable<WorldNumber> GetWorldsToUnlock()
        {
            yield return World;
        }
    }

    public class AllWorldsUnlockContext : IWorldUnlockContext
    {
        public IEnumerable<WorldNumber> GetWorldsToUnlock()
        {
            yield return WorldNumber.World1;
            yield return WorldNumber.World2;
            yield return WorldNumber.World3;
            yield return WorldNumber.World4;
            yield return WorldNumber.World5;
        }
    }
}
