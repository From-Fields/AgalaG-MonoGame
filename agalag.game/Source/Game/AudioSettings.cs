using System;
using System.Collections.Generic;

namespace agalag.game
{
    public static class AudioSettings
    {
        private static Dictionary<AudioGroup, float> _volumes = new Dictionary<AudioGroup, float>();
        private static Dictionary<AudioGroup, float> _baseVolumeMultipliers = new Dictionary<AudioGroup, float>()
        {
            {AudioGroup.Master, 0.5f},
            {AudioGroup.Ambient, 0.02f},
            {AudioGroup.SFX, 0.8f},
            {AudioGroup.Music, 0.7f},
            {AudioGroup.UI, 0.8f},
            {AudioGroup.Explosion, 0.2f}
        };
        private static Dictionary<EntitySoundType, AudioGroup> _groupByType = new Dictionary<EntitySoundType, AudioGroup>()
        {
            {EntitySoundType.Damage, AudioGroup.SFX},
            {EntitySoundType.Death, AudioGroup.Explosion},
            {EntitySoundType.Movement, AudioGroup.Ambient},
            {EntitySoundType.Shot, AudioGroup.SFX},
            {EntitySoundType.Bounce, AudioGroup.SFX},
            {EntitySoundType.PowerUp, AudioGroup.SFX}
        };

        public static float GetVolume(AudioGroup group)
        {
            if(!_volumes.ContainsKey(group))
                _volumes.Add(group, 1.0f);

            float volume = _volumes[group] * _baseVolumeMultipliers[group];

            if(group != AudioGroup.Master)
                volume *= GetVolume(AudioGroup.Master);

            return volume;
        }
        public static void SetVolume(AudioGroup group, float value)
        {
            float adjusted = Math.Clamp(value, 0, 1);

            if(!_volumes.ContainsKey(group))
                _volumes.Add(group, adjusted);
            else
                _volumes[group] = adjusted;
        }

        public static float GetVolumeBySoundType(EntitySoundType soundType) => GetVolume(_groupByType[soundType]);
    }

    public enum AudioGroup
    {
        Master,
        Ambient,
        SFX,
        Explosion,
        Music,
        UI
    }
}