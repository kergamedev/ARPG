using Photon.Deterministic;
using Quantum;

namespace ARPG.Simulation
{
    public unsafe static class SimulationUtilities
    {
        public static bool CheckTeams(Character self, Character other, TeamCheck check)
        {
            return CheckTeams(self.Side, other.Side, check);
        }
        public static bool CheckTeams(TeamSide self, TeamSide other, TeamCheck check)
        {
            switch (check)
            {
                case TeamCheck.AnyTeam:
                    return true;

                case TeamCheck.OpposingTeams:
                    return self != other;

                case TeamCheck.SameTeam:
                    return self == other;

                default: throw new System.Exception($"Unexpected 'TeamCheck={check}'");
            }
        }

        public static bool TryGetClosestCharacter(Frame f, EntityRef self, FP maxDistance, TeamCheck teamCheck, out EntityRef closestEntity, out Character* closestCharacter)
        {
            closestEntity = default;
            closestCharacter = default;

            if (!f.Unsafe.TryGetPointer(self, out Transform3D* selfTransform) ||
                !f.Unsafe.TryGetPointer(self, out Character* selfCharacter))
                return false;

            var maxDistanceSquared = maxDistance * maxDistance;
            var closestDistanceSquared = default(FP?);

            var filter = f.Filter<Transform3D, Character>();
            while (filter.NextUnsafe(out var entity, out var transform, out var character))
            {
                var distanceSquared = FPVector3.DistanceSquared(selfTransform->Position, transform->Position);
                if (distanceSquared >= maxDistanceSquared || closestDistanceSquared != null && distanceSquared >= closestDistanceSquared.Value)
                    continue;

                if (!CheckTeams(*selfCharacter, *character, teamCheck))
                    continue;

                closestDistanceSquared = distanceSquared;
                closestEntity = entity;
                closestCharacter = character;
            }

            return closestDistanceSquared != null;
        }

        public static FPVector3 Navmesh2DToWorld(Frame f, FPVector2 position2D)
        {
            var position3D = position2D.XOY;

            var endHit = f.Physics3D.Raycast(position3D + FPVector3.Up * 50, FPVector3.Down, 100, f.RuntimeConfig.GetGameConfig(f).FloorMask, QueryOptions.HitStatics);
            if (endHit != null)
                position3D.Y = endHit.Value.Point.Y;

            return position3D;
        }
    }
}
