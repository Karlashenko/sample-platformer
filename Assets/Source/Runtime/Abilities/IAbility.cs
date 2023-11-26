using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sample.Abilities
{
    public interface IAbility
    {
        bool IsRunning { get; }

        bool IsOnCooldown { get; }

        int Cooldown { get; }

        UniTask Use(CancellationToken cancellationToken = default(CancellationToken));
    }
}
