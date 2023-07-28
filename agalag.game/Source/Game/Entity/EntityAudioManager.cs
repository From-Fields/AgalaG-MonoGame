using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace agalag.game
{
    public class EntityAudioManager
    {
        private AudioEmitter _emitter;
        private Dictionary<EntitySoundType, SoundEffect> _sounds;   
        private Dictionary<EntitySoundType, SoundEffectInstance> _soundInstances;   

        public EntityAudioManager
        (
            SoundEffect dmgSound = null, SoundEffect deathSound = null, SoundEffect moveSound = null,
            SoundEffect shotSound = null, SoundEffect bounceSound = null, SoundEffect powerUpSound = null
        ) 
        {
            _sounds = new Dictionary<EntitySoundType, SoundEffect>() 
            {
                {EntitySoundType.Damage, dmgSound},
                {EntitySoundType.Death, deathSound},
                {EntitySoundType.Movement, moveSound},
                {EntitySoundType.Shot, shotSound},
                {EntitySoundType.Bounce, bounceSound},
                {EntitySoundType.PowerUp, powerUpSound}
            };
            _soundInstances = new Dictionary<EntitySoundType, SoundEffectInstance>();

            _emitter = new AudioEmitter();
        }

        public void PlaySoundOneShot(SoundEffectInstance instance, AudioGroup audioGroup, Vector2? position = null, AudioListener listener = null) 
        {
            float volume = AudioSettings.GetVolume(audioGroup);

            PlaySound(instance, volume, position, listener);
        }
        public void PlaySound(EntitySoundType soundType, Vector2? position = null, AudioListener listener = null, bool looping = false)
        {
            SoundEffectInstance instance = GetSoundInstance(soundType);

            if(instance == null)
                return;

            instance.IsLooped = looping;
            float volume = AudioSettings.GetVolumeBySoundType(soundType);

            PlaySound(instance, volume, position, listener);
        }
        private SoundEffectInstance GetSoundInstance(EntitySoundType soundType)
        {
            if(!_sounds.ContainsKey(soundType) || _sounds[soundType] == null)
                return null;

            SoundEffectInstance instance;

            if(_soundInstances.ContainsKey(soundType))
                instance = _soundInstances[soundType];
            else
                instance = _sounds[soundType].CreateInstance();

            return instance;
        }
        private void PlaySound(SoundEffectInstance instance, float volume, Vector2? position = null, AudioListener listener = null)
        {
            instance.Volume = volume;

            if(position.HasValue && listener != null) {
                _emitter.Position = new Vector3(position.Value.X, position.Value.Y, 0);
                instance.Apply3D(listener, _emitter);
            }

            instance.Play();
        }

        public void StopSound(EntitySoundType soundType)
        {
            if(!_soundInstances.ContainsKey(soundType))
                return;

            _soundInstances[soundType].Stop();
        }

        public void Clear() 
        {
            foreach (var instance in _soundInstances.Values)
            {
                instance.Stop();
            }    
        }
    }

    public enum EntitySoundType
    {
        Damage,
        Death,
        Movement,
        Shot,
        Bounce,
        PowerUp
    }
}