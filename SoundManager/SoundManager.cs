using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using ICT309Game.Cars;
using BEPUphysics.Vehicle;



namespace ICT309Game.SoundMngr
{
    public class Cue3D
    {
        public Cue cue;
        public AudioEmitter emitter = new AudioEmitter();
        public AudioListener listener = new AudioListener();
        public Vector3 entityPosition;
    }

    public class SceneSound
    {
        public Cue3D crowdSound = new Cue3D();

        public SceneSound()
        {
            crowdSound.cue = SoundManager.GetEffect(SoundManager.SoundEffects.CROWDSOUND);
        }
    }

    public static class SoundManager
    {
        #region soundEffects
        public enum SoundEffects
        {
            SHIFT_1 = 1,
            SHIFT_2,
            SHIFT_3,
            SHIFT_4,
            CHECKPOINT_1 = 5,
            CROWDSOUND = 6,
            SONG = 7,
            CRASH_1 = 8,
            CRASH_2,
            CRASH_3,
            ROAD_RIDE,
            OUTOFROAD,
            OUTOFTIME

        }
        #endregion

       



        private static List<SoundEffectInstance> soundEffects;
        private static ContentManager content;
        private static Dictionary<SoundEffects, string> effectsLib;
        private static AudioEngine audioEngine = null;
        private static WaveBank waveBank = null;
        private static WaveBank musicWaveB = null;
        private static SoundBank soundBank = null;
        private static Cue currentEffect;
        private static Cue3D crowd = new Cue3D(); 
        private static string waveBankName = "Content\\Assets\\Sound\\Wave Bank.xwb";
        private static string musicWaveBank = "Content\\Assets\\Sound\\Music.xwb";
        private static string soundbankName = "Content\\Assets\\Sound\\Sound Bank.xsb";
        private static string settingsFile = "Content\\Assets\\Sound\\GearShifts.xgs";

 
        public static void Initialize(Game game)
        {
            
            SoundManager.content = game.Content;
            soundEffects = new List<SoundEffectInstance>();
            effectsLib = new Dictionary<SoundEffects, string>();
            audioEngine = new AudioEngine(settingsFile);
            musicWaveB = new WaveBank(audioEngine, musicWaveBank);
            waveBank = new WaveBank(audioEngine, waveBankName);
            soundBank = new SoundBank(audioEngine, soundbankName);

            //effectsLib.Add(SoundEffects.SHIFT_1, "ferrarif355 1");
            //effectsLib.Add(SoundEffects.SHIFT_2, "ferrarif355 2");
            //effectsLib.Add(SoundEffects.SHIFT_3, "ferrarif355 3");
            //effectsLib.Add(SoundEffects.SHIFT_4, "ferrarif355 4");
            effectsLib.Add(SoundEffects.SHIFT_1, "evo7 1");
            effectsLib.Add(SoundEffects.SHIFT_2, "evo7 2");
            effectsLib.Add(SoundEffects.SHIFT_3, "evo7 3");
            effectsLib.Add(SoundEffects.SHIFT_4, "evo7 4");
            effectsLib.Add(SoundEffects.CHECKPOINT_1, "checkpoint");
            effectsLib.Add(SoundEffects.CROWDSOUND, "CrowdCheering");
            effectsLib.Add(SoundEffects.SONG, "LevelSong");
            effectsLib.Add(SoundEffects.CRASH_1, "crash1");
            effectsLib.Add(SoundEffects.CRASH_2, "crash2");
            effectsLib.Add(SoundEffects.CRASH_3, "crash3");
            effectsLib.Add(SoundEffects.ROAD_RIDE, "road-ride");
            effectsLib.Add(SoundEffects.OUTOFROAD, "outOfRoad");
            effectsLib.Add(SoundEffects.OUTOFTIME, "TimeUp");

            audioEngine.Update();
        }
        public static void Update()
        {
            audioEngine.Update();
        }
        public static Cue GetEffect(SoundEffects soundEffect)
        {
            return soundBank.GetCue(effectsLib[soundEffect]);
        }

        public static void PlayEffect(SoundEffects soundEffect)
        {
            currentEffect = soundBank.GetCue(effectsLib[soundEffect]);
            currentEffect.Play();
            
        }
        public static void Play3D(SceneSound sound, Vehicle vehicle, Vector3 emitterPos)
        {

            if (Vector3.Distance(vehicle.Body.Position, emitterPos) > 600)
            {
                return;
            }

            sound.crowdSound.emitter.Position = emitterPos;
            sound.crowdSound.listener.Position = vehicle.Body.Position;
            sound.crowdSound.listener.Velocity = vehicle.Body.LinearVelocity;

            sound.crowdSound.cue.Apply3D(sound.crowdSound.listener, sound.crowdSound.emitter);

            if(!sound.crowdSound.cue.IsPlaying)
            {
                sound.crowdSound.cue = GetEffect(SoundEffects.CROWDSOUND);
                sound.crowdSound.cue.Apply3D(sound.crowdSound.listener, sound.crowdSound.emitter);
                sound.crowdSound.cue.Play();
            }
        }
        public static SoundEffectInstance PlayEffect(SoundEffects soundEffect, bool loop, float volume)
        {
            SoundEffect effect = content.Load<SoundEffect>("Assets\\Sound\\" + effectsLib[soundEffect]);
            
            SoundEffectInstance instance = effect.CreateInstance();
            instance.IsLooped = loop;
            instance.Play();
            instance.Volume = 0.5f;
            soundEffects.Add(instance);

            return instance;
        }
        public static void StopEffect(SoundEffectInstance soundEffectInstance)
        {
            if (soundEffectInstance != null)
            {
                soundEffectInstance.Stop();
                soundEffects.Remove(soundEffectInstance);
            }
        }

        public static void PauseAllEffects()
        {
            foreach (SoundEffectInstance soundEffectInstance in soundEffects)
            {
                soundEffectInstance.Pause();
            }
        }

        public static void ResumeAllEffects()
        {
            foreach (SoundEffectInstance soundEffectInstance in soundEffects)
            {
                soundEffectInstance.Resume();
            }
        }

        public static void StopAllSoundEffects()
        {
            //foreach (SoundEffectInstance soundEffectInstance in soundEffects)
            //{
            //    soundEffectInstance.Stop();
            //}
            //soundEffects.Clear();
        }

    }

}
