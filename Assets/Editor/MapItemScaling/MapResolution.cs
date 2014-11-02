using System;
using System.Collections.Generic;
using System.Linq;
using Rotorz.Tile;

namespace Assets.Editor.MapItemScaling
{

    [Serializable]
    public class MapResolution
    {

        private static readonly string[] presets = new[]
                                   {
                                       "22x11",
                                       "11x12",
                                       "14x19",
                                       "7x9"
                                   };



        public static IEnumerable<string> GetPresetsStrings()
        {
            return presets;
        }


        public static MapResolution FromTileSystem(TileSystem ts)
        {
            return new MapResolution(ts.ColumnCount, ts.RowCount);
        }

        public static MapResolution CreateResolutionFromPresetString(int strIndex)
        {

            if (strIndex < 0 || strIndex > presets.Length - 1)
            {
                return null;
            }

            return CreateResolutionFromPresetString(presets[strIndex]);
        }
        public static MapResolution CreateResolutionFromPresetString(string str)
        {
            try
            {
                var pair = str.Split('x');

                var x = Convert.ToInt32(pair[0]);
                var y = Convert.ToInt32(pair[1]);

                return new MapResolution(x, y);
            }
            catch 
            {
                return null;
            }
        }

        public static string GetPresetStringFromResolution(MapResolution r)
        {
            return r.Columns + "x" + r.Rows;
        }

        public static int GetPresetStringIndexFromResolution(MapResolution r)
        {
            return presets.ToList().IndexOf(GetPresetStringFromResolution(r));
        }


        public static MapResolution Resolution22x11 = new MapResolution(22, 11);
        public static MapResolution Resolution14x19 = new MapResolution(14, 19);
        public static MapResolution Resolution7x19 = new MapResolution(14, 9);

        public MapResolution()
        {
            
        }

        public MapResolution(int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
        }



        public override string ToString()
        {
            return string.Format("[Rows: {0}, Columns: {1}]", Rows, Columns);
        }

        public int Rows;
        public int Columns;

        protected bool Equals(MapResolution other)
        {
            return Columns == other.Columns && Rows == other.Rows;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapResolution) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Columns*397) ^ Rows;
            }
        }
    }
}
