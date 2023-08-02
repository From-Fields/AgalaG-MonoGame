using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace agalag.game.scenes
{
    public static class GameWaves
    {
        public static iLevel GetLevel(Rectangle levelBounds, Action<int> onDeath)
        {
            return new EndlessLevel(new Queue<WaveController>(GetWaves(levelBounds, onDeath)));
        }

        private static List<WaveController> GetWaves(Rectangle levelBounds, Action<int> onDeath)
        {
            List<WaveController> waves = new List<WaveController>();

            float width = levelBounds.Width, height = levelBounds.Height;
            
            Wave doubleKami = new Wave
            (
                6, 
                new List<iWaveUnit>() 
                {
                    new WaveUnit<EnemyKamikaze>
                    (
                        new Vector2(width * 0.25f, -height * 0.2f),
                        new MoveTowards(new Vector2(width * 0.2f, height * 0.25f)),
                        new MoveTowards(new Vector2(width * 0.45f, height * 1.1f), speedModifier: 4f),
                        new Queue<iEnemyAction>(new[] { new WaitSeconds(2) }),
                        onDeath
                    ),
                    new WaveUnit<EnemyKamikaze>
                    (
                        new Vector2(width * 0.75f, -height * 0.2f),
                        new MoveTowards(new Vector2(width * 0.8f, height * 0.25f)),
                        new MoveTowards(new Vector2(width * 0.55f, height * 1.1f), speedModifier: 4f),
                        new Queue<iEnemyAction>(new[] { new WaitSeconds(2), }),
                        onDeath
                    ),
                    new WaveHazard(new Vector2(-width * 0.2f, height / 2), new Vector2(2, -0.75f), maxBounces: 5),
                    new WaveHazard(new Vector2(width * 1.2f, height / 2), new Vector2(-2, -1), maxBounces: 5)
                }
            );

            Wave bumbleTrouble = new Wave
            (
                8, 
                new List<iWaveUnit>() 
                {
                    new WaveUnit<EnemyBumblebee>
                    (
                        new Vector2(width * 0.5f, -height * 0.2f),
                        new WaitSeconds(1),
                        new MoveTowards(new Vector2(-width*0.2f, height * 0.6f), speedModifier: 4f),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new MoveTowards(new Vector2(width * 0.5f, height * 0.3f)), 
                            new Shoot(3),
                            new MoveAndShoot(new Vector2(width * 0.5f, height * 0.6f)),
                        }),
                        onDeath
                    ),
                    new WaveUnit<EnemyBumblebee>
                    (
                        new Vector2(width * 0.2f, -height * 0.2f),
                        new MoveTowards(new Vector2(width * 0.2f, height * 0.2f)),
                        new MoveTowards(new Vector2(width * 0.5f, height * 1.2f), speedModifier: 4f, stopOnEnd: false, trackingSpeed: 5),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new MoveAndShoot(new Vector2(width * 0.4f, height * 0.1f)),
                            new MoveAndShoot(new Vector2(width * 0.6f, height * 0.1f)),
                            new MoveAndShoot(new Vector2(width * 0.8f, height * 0.2f)),
                            new MoveAndShoot(new Vector2(width * 0.5f, height * 0.4f)),
                            new MoveAndShoot(new Vector2(width * 0.15f, height * 0.2f)),
                        }),
                        onDeath
                    ),
                }
            );

            Wave geminiSentry = new Wave
            (
                9, 
                new List<iWaveUnit>() 
                {
                    new WaveUnit<EnemyGemini>
                    (
                        new Vector2(width * 0.5f, -height * 0.2f),
                        new MoveTowards(new Vector2(width * 0.5f, height * 0.2f)),
                        new MoveTowards(new Vector2(width * 0.5f, -height * 0.2f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new Shoot(8),
                        }),
                        onDeath
                    ),
                    new WaveUnit<EnemyBumblebee>
                    (
                        new Vector2(-width * 0.1f, -height * 0.2f),
                        new WaitSeconds(1),
                        new MoveAndShoot(new Vector2(width * 1.2f, height * 1f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new MoveTowards(new Vector2(width * 0.25f, height * 0.3f)), 
                            new Shoot(2)
                        }),
                        onDeath
                    ),
                    new WaveUnit<EnemyBumblebee>
                    (
                        new Vector2(width * 1.1f, -height * 0.2f),
                        new WaitSeconds(1),
                        new MoveAndShoot(new Vector2(-width * 0.2f, height * 1f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new MoveTowards(new Vector2(width * 0.75f, height * 0.2f)), 
                            new Shoot(2)
                        }),
                        onDeath
                    ),
                    new WaveHazard(new Vector2(width * 0.3f, -height * 0.1f), new Vector2(0.75f, 1.5f), maxBounces: 5),
                    new WaveHazard(new Vector2(width * 0.7f, -height * 0.1f), new Vector2(-0.75f, 3), maxBounces: 5)
                }
            );

            Wave asteroidClock = new Wave
            (
                5,
                new List<iWaveUnit>()
                {
                    new WaveHazard(new Vector2(width * 1.1f, height * 0.9f), new Vector2(-0.75f, -0.75f), maxBounces: 9),
                    new WaveHazard(new Vector2(width * 0.7f, -height * 0.6f), new Vector2(-0.25f, 1.25f), maxBounces: 7),
                    new WaveHazard(new Vector2(-width * 0.4f, height * 0.1f), new Vector2(1f, 0.25f), maxBounces: 5),
                    new WaveHazard(new Vector2(-width * 0.2f, height * 1.9f), new Vector2(0.75f, -1.5f), maxBounces: 10),
                }
            );

            Wave flyByNight = new Wave
            (
                15, 
                new List<iWaveUnit>() 
                {
                    new WaveUnit<EnemyGemini>
                    (
                        new Vector2(width * 0.2f, -height * 0.2f),
                        new WaitSeconds(1),
                        new MoveAndShoot(new Vector2(-width * 0.2f, height * 0.8f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new MoveTowards(new Vector2(width * 0.8f, height * 0.3f)),
                            new Shoot(4),
                        }),
                        onDeath
                    ),
                    new WaveUnit<EnemyGemini>
                    (
                        new Vector2(width * 1.2f, -height * 0.2f),
                        new WaitSeconds(2),
                        new MoveAndShoot(new Vector2(width * 1.2f, height * 0.8f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] { 
                            new MoveTowards(new Vector2(width * 0.2f, height * 0.3f)),
                            new Shoot(3),
                        }),
                        onDeath
                    ),
                    new WaveUnit<EnemyBumblebee>
                    (
                        new Vector2(-width * 0.1f, height * 0.25f),
                        new MoveTowards(new Vector2(width * 0.15f, height * 0.25f)), 
                        new MoveAndShoot(new Vector2(width * 1.2f, height * 0.25f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] {
                            new Shoot(1)
                        }),
                        onDeath
                    ),
                    new WaveUnit<EnemyBumblebee>
                    (
                        new Vector2(width * 1.1f, height * 0.4f),
                        new MoveTowards(new Vector2(width * 0.85f, height * 0.4f)), 
                        new MoveAndShoot(new Vector2(-width * 0.2f, height * 0.4f)),
                        new Queue<iEnemyAction>(new iEnemyAction[] {
                            new Shoot(1)
                        }),
                        onDeath
                    )
                }
            );

            waves.AddRange(
                new List<WaveController>()
                    {
                        // new WaveController(doubleKami.timeout, levelBounds, doubleKami.units),
                        // new WaveController(bumbleTrouble.timeout, levelBounds, bumbleTrouble.units),
                        // new WaveController(geminiSentry.timeout, levelBounds, geminiSentry.units),
                        // new WaveController(asteroidClock.timeout, levelBounds, asteroidClock.units),
                        new WaveController(flyByNight.timeout, levelBounds, flyByNight.units),
                        // new WaveController(simmetry.timeout, levelBounds, simmetry.units),
                        // new WaveController(tsuKami.timeout, levelBounds, tsuKami.units),
                        // new WaveController(pincerBlow.timeout, levelBounds, pincerBlow.units),
                        // new WaveController(divideConquer.timeout, levelBounds, divideConquer.units),
                    }
            );

            return waves;
        }
        private struct Wave
        {
            public float timeout;
            public List<iWaveUnit> units;

            public Wave(float timeout, List<iWaveUnit> units)
            {
                this.timeout = timeout;
                this.units = units;
            }
        }
    }

}