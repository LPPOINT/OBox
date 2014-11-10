using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using WellFired;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class GUILSPage : GUIPage
    {


        public List<LSChunk> Chunks; 

        public LSChunk CurrentChunk { get; private set; }
        public LSChunk NextChunk { get; private set; }
        public LSChunk PrevChunk { get; private set; }

        public void DeployChunk(LSChunk chunkPrefab)
        {
            
        }

        public void DestroyChunk(LSChunk chunkInstance)
        {
            USSequencer s;
        }

        public List<LSIconModel> IconModels { get; private set; }

        public LSIconModel GetIconModel(int levelNumber)
        {
            var index = GameModel.GetLevelIndexByGlobalNumber(levelNumber);
            return
                IconModels.FirstOrDefault(model => model.Number == index.LevelNumber && model.World == index.WorldNumber);
        }

        public override void OnShow()
        {
            base.OnShow();
            InitializeIconModels();
        }

        public void InitializeIconModels()
        {
            IconModels = new List<LSIconModel>();
            foreach (var w in GameModel.EnumerateWorlds())
            {
                foreach (var l in GameModel.EnumerateLevels())
                {
                    IconModels.Add(LSIconModel.CreateFromGameModel(l, w, GameModel.Instance));
                }
            }
        }
    }
}
