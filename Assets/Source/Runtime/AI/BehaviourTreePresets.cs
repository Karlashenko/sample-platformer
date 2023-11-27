using System;
using Sample.AI.Composites;
using Sample.AI.Conditions;
using Sample.AI.Decorators;
using Sample.AI.Tasks;
using UnityEngine;

namespace Sample.AI
{
    public static class BehaviourTreePresets
    {
        private const float MoveToSpawnRange = 30;
        private const float TargetLossRange = 20;
        private const float TargetDetectionRange = 15;

        public static BehaviourTree Create(BehaviourTreePresetType type, GameObject owner)
        {
            // @formatter:off
            switch (type)
            {
                case BehaviourTreePresetType.Ground:
                    return new BehaviourTreeBuilder()
                        .Begin(new Selector())
                            .Begin(new Sequence())
                                .Node(new Inverter(new IsSpawnPointInRange(MoveToSpawnRange)))
                                .Node(new SetTargetPointToSpawnPoint())
                                .Node(new MoveToTargetPoint(1))
                            .End()

                            .Begin(new Sequence())
                                .Node(new SetTargetPointToNextPatrolPoint())
                                .Node(new IsTargetPointReachableByFoot())
                                .Node(new MoveToTargetPoint(1).While(
                                        new IsSpawnPointInRange(MoveToSpawnRange), new Inverter(new Sequence(new FindTargetUnit(TargetDetectionRange), new IsTargetUnitReachableByFoot()))))
                                .Node(new Wait(1, 5))
                            .End()

                            .Begin(new Sequence())
                                .Node(new IsTargetUnitSet())
                                .Begin(new Selector())
                                    .Node(new MoveToTargetUnit(2).While(
                                            new IsTargetUnitInRange(TargetLossRange), new IsTargetUnitReachableByFoot(), new IsSpawnPointInRange(MoveToSpawnRange)))
                                    .Begin(new Sequence())
                                        .Node(new ClearTargetUnit())
                                        .Node(new Break())
                                    .End()
                                .End()
                                .Node(new AttackTarget())
                            .End()
                        .End()
                        .Build(owner);

                case BehaviourTreePresetType.Flying:
                    return new BehaviourTreeBuilder()
                        .Begin(new Selector())
                            // Move to spawn
                            .Begin(new Sequence())
                                .Node(new Inverter(new IsSpawnPointInRange(MoveToSpawnRange)))
                                .Node(new SetTargetPointToSpawnPoint())
                                .Node(new FlyToTargetPoint(1))
                            .End()

                            // Patrol
                            .Begin(new Sequence())
                                .Node(new SetTargetPointToNextPatrolPoint())
                                .Node(new FlyToTargetPoint(1).While(new IsSpawnPointInRange(MoveToSpawnRange), new Inverter(new FindTargetUnit(TargetDetectionRange))))
                            .End()

                            // Attack
                            .Begin(new Sequence())
                                .Node(new IsTargetUnitSet())
                                .Begin(new Selector())
                                    .Node(new FlyToTargetUnit(1).While(new IsSpawnPointInRange(MoveToSpawnRange), new IsTargetUnitInRange(TargetLossRange)))
                                    .Begin(new Sequence())
                                        .Node(new ClearTargetUnit())
                                        .Node(new Break())
                                    .End()
                                .End()
                                .Node(new AttackTarget())
                            .End()
                        .End()
                        .Build(owner);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
