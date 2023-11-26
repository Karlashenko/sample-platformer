using System;
using System.Collections.Generic;
using Sample.Abilities;

namespace Sample.Components
{
    public class AbilitiesComponent : Component
    {
        private readonly Dictionary<Type, IAbility> _abilities = new();

        public TAbility Get<TAbility>()
        {
            return (TAbility) _abilities[typeof(TAbility)];
        }

        public void Add(IAbility ability)
        {
            _abilities.Add(ability.GetType(), ability);
        }
    }
}
