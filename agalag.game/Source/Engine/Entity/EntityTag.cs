using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.engine
{
    public enum EntityTag
    {
        None,
        Player,
        Enemy,
        PlayerBullet,
        PickUp,
        Wall
    }

    public static class TagUtils
    {
        public static readonly Dictionary<EntityTag, string> Tags = new()
        {
            { EntityTag.None, "unknown" },
            { EntityTag.Player, "Player" },
            { EntityTag.Enemy, "Enemy"},
            { EntityTag.PlayerBullet, "PlayerBullet" },
            { EntityTag.PickUp, "PickUp" },
            { EntityTag.Wall, "Wall" },
        };
        
        private static bool[][] _tags;

        public static bool[] GetMask(EntityTag tag){
            if(_tags == null)
                SetupTags();

            return _tags[(int) tag];
        }

        public static bool GetInteraction(EntityTag tag_a, EntityTag tag_b){
            if(_tags == null)
                SetupTags();

            // System.Diagnostics.Debug.WriteLine(tag_a + ", " + tag_b + ": " + _tags[(int) tag_a][(int) tag_b] + " & " + _tags[(int) tag_b][(int) tag_a]);
            return _tags[(int) tag_a][(int) tag_b];
        }

        public static void SetMask(EntityTag tag_a, EntityTag tag_b, bool value)
        {
            if(_tags == null)
                SetupTags();

            _tags[(int) tag_a][(int) tag_b] = value;
            _tags[(int) tag_b][(int) tag_a] = _tags[(int) tag_a][(int) tag_b];

        }

        private static void SetupTags()
        {
            int length = Enum.GetValues(typeof(EntityTag)).Length;

            _tags = new bool[length][];

            for (int i = 0; i < length; i++)
            {
                _tags[i] = new bool[length];
                for (int j = 0; j < length; j++)
                {
                    _tags[i][j] = true;
                }
            }
        }
    }
}
