using System;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{

    [Serializable]
    public class LSIconModel : ICloneable
    {
        public LSIconModel(IconType type, LevelNumber number, WorldNumber world, StarsCount stars)
        {
            Stars = stars;
            Number = number;
            World = world;
            Type = type;
        }
        public LSIconModel()
        {
            
        }

        public enum IconType
        {
            LockedLevel,
            CompletedLevel,
            CurrentLevel
        }


        public IconType Type;
        public LevelNumber Number;
        public WorldNumber World;
        public StarsCount Stars;

        protected bool Equals(LSIconModel other)
        {
            return Type == other.Type && Number == other.Number && Stars == other.Stars;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LSIconModel) obj);
        }



        public object Clone()
        {
            return new LSIconModel(Type, Number, World, Stars);
        }

        public static LSIconModel LockedLevelModel(LevelNumber level, WorldNumber world)
        {
            return new LSIconModel(IconType.LockedLevel, level, world, StarsCount.None);
        }

        public static LSIconModel CurrentLevelModel(LevelNumber levelNumber, WorldNumber w)
        {
            return new LSIconModel(IconType.CurrentLevel, levelNumber, w, StarsCount.None);
        }

        public static LSIconModel CompletedLevelModel(LevelNumber levelNumber,WorldNumber w, StarsCount stars)
        {
            return new LSIconModel(IconType.CompletedLevel, levelNumber, w, stars);
        }

        public static LSIconModel CreateFromGameModel(LevelNumber levelNumber, WorldNumber worldNumber,
            GameModel gameModel)
        {
            var status = gameModel.GetLevelStatus((LevelNumber)levelNumber, worldNumber);

            if (status == LevelStatus.NotCompleted)
            {
                return gameModel.IsCurrentLevel((LevelNumber) levelNumber, worldNumber) ? CurrentLevelModel(levelNumber, worldNumber) : LockedLevelModel(levelNumber, worldNumber);
            }
            else
            {
                return CompletedLevelModel(levelNumber,   worldNumber, GameModel.GetStarsCountByLevelStatus(status));
            }
            return null;
        }

    }
}
