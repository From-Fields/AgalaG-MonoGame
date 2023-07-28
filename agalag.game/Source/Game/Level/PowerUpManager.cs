using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public static class PowerUpManager
    {
        private static int _nullDropChance = 8;
        private static Dictionary<Type, int> _dropPorportions = new Dictionary<Type, int>() 
        {
          {typeof(ShieldPowerUp), 4},
          {typeof(RepairPowerUp), 4},
          {typeof(MissilePowerUp), 2},
          {typeof(TripleMachineGunPowerUp), 2},
        };
        private static Dictionary<Type, Point> _dropRates = null;

        public static PowerUp GetRandomPowerup()
        {
            if(_dropRates == null)
                CalculateDropRates();

            int totalChance = _dropPorportions.Values.Sum() + _nullDropChance;

            int chance = Random.Shared.Next() % totalChance;
            System.Diagnostics.Debug.WriteLine(chance);

            if(chance < _nullDropChance)
                return null;

            foreach (var item in _dropRates)
            {
                Type type = item.Key;

                if(chance < item.Value.Y && chance >= item.Value.X)
                    return (PowerUp) Activator.CreateInstance(type);
                    // return (PowerUp) item.Key.GetConstructors()[0].Invoke(null);
            }

            return null;
        }
        private static void CalculateDropRates()
        {
            int accumulator = _nullDropChance;
            _dropRates = new Dictionary<Type, Point>();

            foreach (var item in _dropPorportions)
            {
                Point values = new Point(accumulator, accumulator + item.Value);

                _dropRates.Add(item.Key, values);
                accumulator += item.Value;

                System.Diagnostics.Debug.WriteLine(item.Key + ": " + values.X + "-" + (values.Y - 1));
            }
        }
    }
}