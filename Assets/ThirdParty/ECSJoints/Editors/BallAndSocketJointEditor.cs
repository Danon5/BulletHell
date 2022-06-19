#if UNITY_EDITOR

using BulletHell.ThirdParty.ECSJoints.Creators.Joints;
using UnityEditor;

namespace BulletHell.ThirdParty.ECSJoints.Editors
{
    [CustomEditor(typeof(BallAndSocketJoint))]
    public class BallAndSocketEditor : UnityEditor.Editor
    {
        protected virtual void OnSceneGUI()
        {
            BallAndSocketJoint ballAndSocket = (BallAndSocketJoint)target;

            EditorGUI.BeginChangeCheck();

            EditorUtilities.EditPivot(ballAndSocket.worldFromA, ballAndSocket.worldFromB, ballAndSocket.AutoSetConnected,
                ref ballAndSocket.PositionLocal, ref ballAndSocket.PositionInConnectedEntity, ballAndSocket);
        }
    }
}

#endif
