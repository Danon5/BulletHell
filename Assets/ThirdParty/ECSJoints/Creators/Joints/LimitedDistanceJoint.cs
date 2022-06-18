using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using static Unity.Physics.Math;

namespace BulletHell.ThirdParty.ECSJoints.Creators.Joints
{
    public class LimitedDistanceJoint : BallAndSocketJoint
    {
        public float MinDistance;
        public float MaxDistance;

        public override void Create(EntityManager entityManager, GameObjectConversionSystem conversionSystem)
        {
            UpdateAuto();
            conversionSystem.World.GetOrCreateSystem<EndJointConversionSystem>().CreateJointEntity(
                this,
                GetConstrainedBodyPair(conversionSystem),
                PhysicsJoint.CreateLimitedDistance(
                    PositionLocal,
                    PositionInConnectedEntity,
                    new FloatRange(MinDistance, MaxDistance)
                )
            );
        }
    }
}
