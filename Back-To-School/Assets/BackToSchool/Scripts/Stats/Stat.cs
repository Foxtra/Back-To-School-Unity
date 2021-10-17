using System.Collections.Generic;


namespace Assets.BackToSchool.Scripts.Stats
{
    public class Stat
    {
        private readonly List<int> _modifiers = new List<int>();
        private readonly int _baseValue;

        public Stat(int baseValue) => _baseValue = baseValue;

        public int GetValue()
        {
            var finalValue = _baseValue;
            _modifiers.ForEach(x => finalValue += x);
            return finalValue;
        }

        public void AddModifier(int modifier)
        {
            if (modifier != 0)
                _modifiers.Add(modifier);
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0)
                _modifiers.Remove(modifier);
        }
    }
}