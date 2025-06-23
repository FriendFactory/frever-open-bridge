using System.Collections.Generic;
using Bridge.ExternalPackages.Protobuf;
using Bridge.Models.ClientServer.Assets;

namespace Bridge.Models.ClientServer.Level.Full
{
    public class CameraControllerFullInfo
    {
        public long Id { get; set; }
        public long CameraAnimationTemplateId { get; set; }
        public int StartFocusDistance { get; set; }
        public int EndFocusDistance { get; set; }
        public int CameraNoiseSettingsIndex { get; set; }
        public int TemplateSpeed { get; set; }
        public bool FollowAll { get; set; }
        public int? LookAtIndex { get; set; }
        public bool AnimationRegenerationRequired { get; set; }

        public CameraAnimationFullInfo CameraAnimation { get; set; }
        [ProtoNewField(1)] public CameraAnimationFrameInfo RegenerationStartFrame { get; set; }
    }
    
    public sealed class CameraAnimationFrameInfo
    {
        public FrameValues FrameValues { get; set; }
        public Dictionary<string, CinemachineComposerStateInfo> CinemachineComposersState { get; set; }
    }
    
    public struct FrameValues
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }

        public float FoV { get; set; }
        public float DoF { get; set; }
        public float FocusDistance { get; set; }
        
        public float AxisX { get; set; }
        public float AxisY { get; set; }
        public float OrbitRadius { get; set; }
        public float HeightRadius { get; set; }
        public float Dutch { get; set; }
    }
    
    public class CinemachineComposerStateInfo
    {
        public Vector3Dto CameraPosPrevFrame { get; set; }
        public Vector3Dto LookAtPrevFrame { get; set; }
        public Vector2Dto ScreenOffsetPrevFrame { get; set; }
        public QuaternionDto CameraOrientationPrevFrame { get; set; }
        public Vector3Dto CameraPosition { get; set; }
        public QuaternionDto CameraRotation { get; set; }
        [ProtoNewField(1)] public OrbitState OrbitState {get; set; }
    }
    
    public class OrbitState
    {
        public Vector3Dto LastTargetPosition { get; set; }
        public QuaternionDto HeadingPrevFrame { get; set; }
        public Vector3Dto OffsetPrevFrame { get; set; }
        public float LastHeading { get; set; }
    }

    public struct Vector3Dto
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    
    public struct Vector2Dto
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
    
    public struct QuaternionDto
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
    }
}