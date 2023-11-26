using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.Abilities
{
    public abstract class Ability : IAbility
    {
        public bool IsRunning { get; private set; }
        public bool IsOnCooldown => _stopwatch.ElapsedMilliseconds < Cooldown;
        public int Cooldown { get; }

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        protected Ability(int cooldown)
        {
            Cooldown = cooldown;
        }

        public async UniTask Use(CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.Assert(IsRunning, $"Ability '{GetType().Name}' is still running.");
            Debug.Assert(IsOnCooldown, $"Ability '{GetType().Name}' is on cooldown.");

            _stopwatch.Restart();

            IsRunning = true;

            await OnUse(cancellationToken);

            IsRunning = false;
        }

        protected abstract UniTask OnUse(CancellationToken cancellationToken = default(CancellationToken));
    }
}
