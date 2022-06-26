using BulletHell.ECS.Components;
using BulletHell.ECS.SystemGroups;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    [UpdateInGroup(typeof(PostPhysicsSystemGroup))]
    public partial class CameraFollowSystem : SystemBase
    {
        private Transform _cameraTransform;

        protected override void OnUpdate()
        {
            if (_cameraTransform == null)
                _cameraTransform = Camera.main?.transform;

            if (_cameraTransform == null) return;

            Entities.ForEach((
                in CameraFollowTargetComponent cameraFollowTarget,
                in Translation translation) =>
            {
                Vector3 targetPos = translation.Value;
                targetPos.z = -10f;
                _cameraTransform.position = targetPos;
            }).WithoutBurst().Run();
        }
    }
}