using System;
using Sample.AI.Composites;
using Sample.AI.Conditions;
using Sample.AI.Tasks;
using UnityEngine;

namespace Sample.AI
{
    public static class BehaviourTreePresets
    {
        public static BehaviourTree Create(BehaviourTreePresetType type, GameObject owner, BehaviourTreeProperties properties)
        {
            switch (type)
            {
                case BehaviourTreePresetType.Ground:
                    return new BehaviourTreeBuilder()
                        .BeginComposite(new Sequence())
                        .Node(new SetTargetUnitToClosestEnemyIfNotSet(properties))
                        .BeginComposite(new Sequence())
                        .Node(new IsTargetUnitInRange(properties))
                        .Node(new AttackTarget(properties))
                        .EndComposite()
                        .Node(new FollowTargetGround(properties))
                        .EndComposite()
                        .Build(owner);

                case BehaviourTreePresetType.Flying:
                    return new BehaviourTreeBuilder()
                        .BeginComposite(new Sequence())
                        .Node(new SetTargetUnitToClosestEnemyIfNotSet(properties))
                        .BeginComposite(new Sequence())
                        .Node(new IsTargetUnitInRange(properties))
                        .Node(new AttackTarget(properties))
                        .EndComposite()
                        .Node(new FollowTargetKamikaze(properties))
                        .EndComposite()
                        .Build(owner);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
