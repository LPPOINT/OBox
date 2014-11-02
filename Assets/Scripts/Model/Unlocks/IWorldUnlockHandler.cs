using Assets.Scripts.Model.Numeration;

namespace Assets.Scripts.Model.Unlocks
{
    public interface IWorldUnlockHandler
    {

        void OnUnlockPerformed(IWorldUnlockContext context);
        void OnUnlockCanceled();

    }
}
