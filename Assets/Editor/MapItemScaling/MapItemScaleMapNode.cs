using System;

namespace Assets.Editor.MapItemScaling
{

    [Serializable]
    public class MapItemScaleMapNode
    {
        public MapItemScaleMapNode(MapResolution resolution, float scale)
        {
            Resolution = resolution;
            Scale = scale;
        }

        public MapItemScaleMapNode()
        {
            
        }

        protected bool Equals(MapItemScaleMapNode other)
        {
            return Equals(Resolution, other.Resolution) && Scale.Equals(other.Scale);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapItemScaleMapNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Resolution != null ? Resolution.GetHashCode() : 0)*397) ^ Scale.GetHashCode();
            }
        }

        public MapResolution Resolution;
        public float Scale;
    }
}
