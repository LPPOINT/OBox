using System;
using Assets.Scripts.Model;

namespace Assets.Scripts.GameGUI
{

    [Serializable]
    public class GUILevelIconModel : ICloneable
    {
        public GUILevelIconModel(IconType type, int number, StarsCount stars)
        {
            Stars = stars;
            Number = number;
            Type = type;
        }
        public GUILevelIconModel()
        {
            
        }

        public enum IconType
        {
            LockedLevel,
            CompletedLevel,
            CurrentLevel
        }


        public IconType Type;
        public int Number;
        public StarsCount Stars;

        protected bool Equals(GUILevelIconModel other)
        {
            return Type == other.Type && Number == other.Number && Stars == other.Stars;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GUILevelIconModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode*397) ^ Number;
                hashCode = (hashCode*397) ^ (int) Stars;
                return hashCode;
            }
        }

        public object Clone()
        {
            return new GUILevelIconModel(Type, Number, Stars);
        }

        public static GUILevelIconModel LockedLevelModel()
        {
            return new GUILevelIconModel(IconType.LockedLevel, 0, StarsCount.None);
        }

        public static GUILevelIconModel CurrentLevelModel(int levelNumber)
        {
            return new GUILevelIconModel(IconType.CurrentLevel, levelNumber, StarsCount.None);
        }

        public static GUILevelIconModel CompletedLevelModel(int levelNumber, StarsCount stars)
        {
            return new GUILevelIconModel(IconType.CompletedLevel, levelNumber, stars);
        }

        public static GUILevelIconModel CreateFromGameModel(int levelNumber, WorldNumber worldNumber,
            GameModel gameModel)
        {
            var status = gameModel.GetLevelStatus((LevelNumber)levelNumber, worldNumber);

            if (status == LevelStatus.NotCompleted)
            {
                return gameModel.IsCurrentLevel((LevelNumber) levelNumber, worldNumber) ? CurrentLevelModel(levelNumber) : LockedLevelModel();
            }
            else
            {
                return CompletedLevelModel(levelNumber,  GameModel.GetStarsCountByLevelStatus(status));
            }
            return null;
        }

    }
}
