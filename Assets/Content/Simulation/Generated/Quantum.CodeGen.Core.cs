// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum {
  using Photon.Deterministic;
  using Quantum;
  using Quantum.Core;
  using Quantum.Collections;
  using Quantum.Inspector;
  using Quantum.Physics2D;
  using Quantum.Physics3D;
  using Byte = System.Byte;
  using SByte = System.SByte;
  using Int16 = System.Int16;
  using UInt16 = System.UInt16;
  using Int32 = System.Int32;
  using UInt32 = System.UInt32;
  using Int64 = System.Int64;
  using UInt64 = System.UInt64;
  using Boolean = System.Boolean;
  using String = System.String;
  using Object = System.Object;
  using FlagsAttribute = System.FlagsAttribute;
  using SerializableAttribute = System.SerializableAttribute;
  using MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
  using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;
  using FieldOffsetAttribute = System.Runtime.InteropServices.FieldOffsetAttribute;
  using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
  using LayoutKind = System.Runtime.InteropServices.LayoutKind;
  #if QUANTUM_UNITY //;
  using TooltipAttribute = UnityEngine.TooltipAttribute;
  using HeaderAttribute = UnityEngine.HeaderAttribute;
  using SpaceAttribute = UnityEngine.SpaceAttribute;
  using RangeAttribute = UnityEngine.RangeAttribute;
  using HideInInspectorAttribute = UnityEngine.HideInInspector;
  using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
  using FormerlySerializedAsAttribute = UnityEngine.Serialization.FormerlySerializedAsAttribute;
  using MovedFromAttribute = UnityEngine.Scripting.APIUpdating.MovedFromAttribute;
  using CreateAssetMenu = UnityEngine.CreateAssetMenuAttribute;
  using RuntimeInitializeOnLoadMethodAttribute = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;
  #endif //;
  
  public enum State : int {
    Locomotion,
    Dashing,
  }
  [System.FlagsAttribute()]
  public enum InputButtons : int {
    Dash = 1 << 0,
  }
  public static unsafe partial class FlagsExtensions {
    public static Boolean IsFlagSet(this InputButtons self, InputButtons flag) {
      return (self & flag) == flag;
    }
    public static InputButtons SetFlag(this InputButtons self, InputButtons flag) {
      return self | flag;
    }
    public static InputButtons ClearFlag(this InputButtons self, InputButtons flag) {
      return self & ~flag;
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct BitSet128 {
    public const Int32 SIZE = 16;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public fixed UInt64 Bits[2];
    public const Int32 BitsSize = 128;
    public Int32 Length {
      get {
        return 128;
      }
    }
    public static void Print(void* ptr, FramePrinter printer) {
      var p = (BitSet128*)ptr;
      printer.ScopeBegin();
      UnmanagedUtils.PrintBytesBits((byte*)&p->Bits, 128, 64, printer);
      printer.ScopeEnd();
    }
    public static BitSet128 FromArray(UInt64[] values) {
      Assert.Always(2 == values.Length, "Invalid array size", values.Length);
      BitSet128 result = default;
      for (int i = 0; i < 2; ++i) {
        result.Bits[i] = values[i];
      }
      return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 128);
      fixed (UInt64* p = Bits) (p[bit/64]) |= (1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 128);
      fixed (UInt64* p = Bits) (p[bit/64]) &= ~(1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearAll() {
      fixed (UInt64* p = Bits) Native.Utils.Clear(p, 16);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean IsSet(Int32 bit) {
      fixed (UInt64* p = Bits) return ((p[bit/64])&(1UL<<(bit%64))) != 0UL;
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 4463;
        fixed (UInt64* p = Bits) hash = hash * 31 + HashCodeUtils.GetArrayHashCode(p, 2);
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (BitSet128*)ptr;
        serializer.Stream.SerializeBuffer(&p->Bits[0], 2);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct BitSet2048 {
    public const Int32 SIZE = 256;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public fixed UInt64 Bits[32];
    public const Int32 BitsSize = 2048;
    public Int32 Length {
      get {
        return 2048;
      }
    }
    public static void Print(void* ptr, FramePrinter printer) {
      var p = (BitSet2048*)ptr;
      printer.ScopeBegin();
      UnmanagedUtils.PrintBytesBits((byte*)&p->Bits, 2048, 64, printer);
      printer.ScopeEnd();
    }
    public static BitSet2048 FromArray(UInt64[] values) {
      Assert.Always(32 == values.Length, "Invalid array size", values.Length);
      BitSet2048 result = default;
      for (int i = 0; i < 32; ++i) {
        result.Bits[i] = values[i];
      }
      return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 2048);
      fixed (UInt64* p = Bits) (p[bit/64]) |= (1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 2048);
      fixed (UInt64* p = Bits) (p[bit/64]) &= ~(1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearAll() {
      fixed (UInt64* p = Bits) Native.Utils.Clear(p, 256);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean IsSet(Int32 bit) {
      fixed (UInt64* p = Bits) return ((p[bit/64])&(1UL<<(bit%64))) != 0UL;
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 3319;
        fixed (UInt64* p = Bits) hash = hash * 31 + HashCodeUtils.GetArrayHashCode(p, 32);
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (BitSet2048*)ptr;
        serializer.Stream.SerializeBuffer(&p->Bits[0], 32);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct BitSet256 {
    public const Int32 SIZE = 32;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public fixed UInt64 Bits[4];
    public const Int32 BitsSize = 256;
    public Int32 Length {
      get {
        return 256;
      }
    }
    public static void Print(void* ptr, FramePrinter printer) {
      var p = (BitSet256*)ptr;
      printer.ScopeBegin();
      UnmanagedUtils.PrintBytesBits((byte*)&p->Bits, 256, 64, printer);
      printer.ScopeEnd();
    }
    public static BitSet256 FromArray(UInt64[] values) {
      Assert.Always(4 == values.Length, "Invalid array size", values.Length);
      BitSet256 result = default;
      for (int i = 0; i < 4; ++i) {
        result.Bits[i] = values[i];
      }
      return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 256);
      fixed (UInt64* p = Bits) (p[bit/64]) |= (1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 256);
      fixed (UInt64* p = Bits) (p[bit/64]) &= ~(1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearAll() {
      fixed (UInt64* p = Bits) Native.Utils.Clear(p, 32);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean IsSet(Int32 bit) {
      fixed (UInt64* p = Bits) return ((p[bit/64])&(1UL<<(bit%64))) != 0UL;
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 14057;
        fixed (UInt64* p = Bits) hash = hash * 31 + HashCodeUtils.GetArrayHashCode(p, 4);
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (BitSet256*)ptr;
        serializer.Stream.SerializeBuffer(&p->Bits[0], 4);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct BitSet4096 {
    public const Int32 SIZE = 512;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public fixed UInt64 Bits[64];
    public const Int32 BitsSize = 4096;
    public Int32 Length {
      get {
        return 4096;
      }
    }
    public static void Print(void* ptr, FramePrinter printer) {
      var p = (BitSet4096*)ptr;
      printer.ScopeBegin();
      UnmanagedUtils.PrintBytesBits((byte*)&p->Bits, 4096, 64, printer);
      printer.ScopeEnd();
    }
    public static BitSet4096 FromArray(UInt64[] values) {
      Assert.Always(64 == values.Length, "Invalid array size", values.Length);
      BitSet4096 result = default;
      for (int i = 0; i < 64; ++i) {
        result.Bits[i] = values[i];
      }
      return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 4096);
      fixed (UInt64* p = Bits) (p[bit/64]) |= (1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 4096);
      fixed (UInt64* p = Bits) (p[bit/64]) &= ~(1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearAll() {
      fixed (UInt64* p = Bits) Native.Utils.Clear(p, 512);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean IsSet(Int32 bit) {
      fixed (UInt64* p = Bits) return ((p[bit/64])&(1UL<<(bit%64))) != 0UL;
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 1433;
        fixed (UInt64* p = Bits) hash = hash * 31 + HashCodeUtils.GetArrayHashCode(p, 64);
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (BitSet4096*)ptr;
        serializer.Stream.SerializeBuffer(&p->Bits[0], 64);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct BitSet512 {
    public const Int32 SIZE = 64;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public fixed UInt64 Bits[8];
    public const Int32 BitsSize = 512;
    public Int32 Length {
      get {
        return 512;
      }
    }
    public static void Print(void* ptr, FramePrinter printer) {
      var p = (BitSet512*)ptr;
      printer.ScopeBegin();
      UnmanagedUtils.PrintBytesBits((byte*)&p->Bits, 512, 64, printer);
      printer.ScopeEnd();
    }
    public static BitSet512 FromArray(UInt64[] values) {
      Assert.Always(8 == values.Length, "Invalid array size", values.Length);
      BitSet512 result = default;
      for (int i = 0; i < 8; ++i) {
        result.Bits[i] = values[i];
      }
      return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 512);
      fixed (UInt64* p = Bits) (p[bit/64]) |= (1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 512);
      fixed (UInt64* p = Bits) (p[bit/64]) &= ~(1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearAll() {
      fixed (UInt64* p = Bits) Native.Utils.Clear(p, 64);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean IsSet(Int32 bit) {
      fixed (UInt64* p = Bits) return ((p[bit/64])&(1UL<<(bit%64))) != 0UL;
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 17491;
        fixed (UInt64* p = Bits) hash = hash * 31 + HashCodeUtils.GetArrayHashCode(p, 8);
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (BitSet512*)ptr;
        serializer.Stream.SerializeBuffer(&p->Bits[0], 8);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct BitSet6 {
    public const Int32 SIZE = 8;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public fixed UInt64 Bits[1];
    public const Int32 BitsSize = 6;
    public Int32 Length {
      get {
        return 6;
      }
    }
    public static void Print(void* ptr, FramePrinter printer) {
      var p = (BitSet6*)ptr;
      printer.ScopeBegin();
      UnmanagedUtils.PrintBytesBits((byte*)&p->Bits, 6, 64, printer);
      printer.ScopeEnd();
    }
    public static BitSet6 FromArray(UInt64[] values) {
      Assert.Always(1 == values.Length, "Invalid array size", values.Length);
      BitSet6 result = default;
      for (int i = 0; i < 1; ++i) {
        result.Bits[i] = values[i];
      }
      return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 6);
      fixed (UInt64* p = Bits) (p[bit/64]) |= (1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(Int32 bit) {
      Assert.Check(bit >= 0 && bit < 6);
      fixed (UInt64* p = Bits) (p[bit/64]) &= ~(1UL<<(bit%64));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearAll() {
      fixed (UInt64* p = Bits) Native.Utils.Clear(p, 8);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean IsSet(Int32 bit) {
      fixed (UInt64* p = Bits) return ((p[bit/64])&(1UL<<(bit%64))) != 0UL;
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 13669;
        fixed (UInt64* p = Bits) hash = hash * 31 + HashCodeUtils.GetArrayHashCode(p, 1);
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (BitSet6*)ptr;
        serializer.Stream.SerializeBuffer(&p->Bits[0], 1);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct Input {
    public const Int32 SIZE = 32;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(16)]
    public FPVector2 Move;
    [FieldOffset(0)]
    public Button Dash;
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 19249;
        hash = hash * 31 + Move.GetHashCode();
        hash = hash * 31 + Dash.GetHashCode();
        return hash;
      }
    }
    static partial void GetMaxCountCodeGen(ref int maxCount) {
      maxCount = 6;
    }
    public Boolean IsDown(InputButtons button) {
      switch (button) {
        case InputButtons.Dash: return Dash.IsDown;
        default: return false;
      }
    }
    public Boolean WasPressed(InputButtons button) {
      switch (button) {
        case InputButtons.Dash: return Dash.WasPressed;
        default: return false;
      }
    }
    static partial void SerializeCodeGen(void* ptr, FrameSerializer serializer) {
        var p = (Input*)ptr;
        Button.Serialize(&p->Dash, serializer);
        FPVector2.Serialize(&p->Move, serializer);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct _globals_ {
    public const Int32 SIZE = 760;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(0)]
    public AssetRef<Map> Map;
    [FieldOffset(8)]
    public FP DeltaTime;
    [FieldOffset(16)]
    public NavMeshRegionMask NavMeshRegions;
    [FieldOffset(32)]
    public PhysicsEngineState PhysicsState2D;
    [FieldOffset(48)]
    public PhysicsEngineState PhysicsState3D;
    [FieldOffset(64)]
    public RNGSession RngSession;
    [FieldOffset(80)]
    public FrameMetaData FrameMetaData;
    [FieldOffset(128)]
    public BitSet1024 Systems;
    [FieldOffset(256)]
    public PhysicsSceneSettings PhysicsSettings;
    [FieldOffset(552)]
    public Int32 PlayerConnectedCount;
    [FieldOffset(560)]
    [FramePrinter.FixedArrayAttribute(typeof(Input), 6)]
    private fixed Byte _input_[192];
    [FieldOffset(752)]
    public BitSet6 PlayerLastConnectionState;
    public FixedArray<Input> input {
      get {
        fixed (byte* p = _input_) { return new FixedArray<Input>(p, 32, 6); }
      }
    }
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 12473;
        hash = hash * 31 + Map.GetHashCode();
        hash = hash * 31 + DeltaTime.GetHashCode();
        hash = hash * 31 + NavMeshRegions.GetHashCode();
        hash = hash * 31 + PhysicsState2D.GetHashCode();
        hash = hash * 31 + PhysicsState3D.GetHashCode();
        hash = hash * 31 + RngSession.GetHashCode();
        hash = hash * 31 + FrameMetaData.GetHashCode();
        hash = hash * 31 + Systems.GetHashCode();
        hash = hash * 31 + PhysicsSettings.GetHashCode();
        hash = hash * 31 + PlayerConnectedCount.GetHashCode();
        hash = hash * 31 + HashCodeUtils.GetArrayHashCode(input);
        hash = hash * 31 + PlayerLastConnectionState.GetHashCode();
        return hash;
      }
    }
    static partial void SerializeCodeGen(void* ptr, FrameSerializer serializer) {
        var p = (_globals_*)ptr;
        AssetRef.Serialize(&p->Map, serializer);
        FP.Serialize(&p->DeltaTime, serializer);
        NavMeshRegionMask.Serialize(&p->NavMeshRegions, serializer);
        PhysicsEngineState.Serialize(&p->PhysicsState2D, serializer);
        PhysicsEngineState.Serialize(&p->PhysicsState3D, serializer);
        RNGSession.Serialize(&p->RngSession, serializer);
        FrameMetaData.Serialize(&p->FrameMetaData, serializer);
        Quantum.BitSet1024.Serialize(&p->Systems, serializer);
        PhysicsSceneSettings.Serialize(&p->PhysicsSettings, serializer);
        serializer.Stream.Serialize(&p->PlayerConnectedCount);
        FixedArray.Serialize(p->input, serializer, Statics.SerializeInput);
        Quantum.BitSet6.Serialize(&p->PlayerLastConnectionState, serializer);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct Dash : Quantum.IComponent {
    public const Int32 SIZE = 32;
    public const Int32 ALIGNMENT = 8;
    [FieldOffset(8)]
    public FPVector3 Destination;
    [FieldOffset(0)]
    public FP Speed;
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 2543;
        hash = hash * 31 + Destination.GetHashCode();
        hash = hash * 31 + Speed.GetHashCode();
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (Dash*)ptr;
        FP.Serialize(&p->Speed, serializer);
        FPVector3.Serialize(&p->Destination, serializer);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct PlayerInfo : Quantum.IComponent {
    public const Int32 SIZE = 8;
    public const Int32 ALIGNMENT = 4;
    [FieldOffset(4)]
    public PlayerRef Owner;
    [FieldOffset(0)]
    public Int32 LastDashTick;
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 13049;
        hash = hash * 31 + Owner.GetHashCode();
        hash = hash * 31 + LastDashTick.GetHashCode();
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (PlayerInfo*)ptr;
        serializer.Stream.Serialize(&p->LastDashTick);
        PlayerRef.Serialize(&p->Owner, serializer);
    }
  }
  [StructLayout(LayoutKind.Explicit)]
  public unsafe partial struct StateMachine : Quantum.IComponent {
    public const Int32 SIZE = 4;
    public const Int32 ALIGNMENT = 4;
    [FieldOffset(0)]
    public State State;
    public override Int32 GetHashCode() {
      unchecked { 
        var hash = 15913;
        hash = hash * 31 + (Int32)State;
        return hash;
      }
    }
    public static void Serialize(void* ptr, FrameSerializer serializer) {
        var p = (StateMachine*)ptr;
        serializer.Stream.Serialize((Int32*)&p->State);
    }
  }
  public static unsafe partial class Constants {
  }
  public unsafe partial class Frame {
    partial void AllocGen() {
      _globals = (_globals_*)Context.Allocator.AllocAndClear(sizeof(_globals_));
    }
    partial void FreeGen() {
      Context.Allocator.Free(_globals);
    }
    partial void CopyFromGen(Frame frame) {
      Native.Utils.Copy(_globals, frame._globals, sizeof(_globals_));
    }
    partial void InitGen() {
      Initialize(this, this.SimulationConfig.Entities, 256);
      _ComponentSignalsOnAdded = new ComponentReactiveCallbackInvoker[ComponentTypeId.Type.Length];
      _ComponentSignalsOnRemoved = new ComponentReactiveCallbackInvoker[ComponentTypeId.Type.Length];
      BuildSignalsArrayOnComponentAdded<CharacterController2D>();
      BuildSignalsArrayOnComponentRemoved<CharacterController2D>();
      BuildSignalsArrayOnComponentAdded<CharacterController3D>();
      BuildSignalsArrayOnComponentRemoved<CharacterController3D>();
      BuildSignalsArrayOnComponentAdded<Quantum.Dash>();
      BuildSignalsArrayOnComponentRemoved<Quantum.Dash>();
      BuildSignalsArrayOnComponentAdded<MapEntityLink>();
      BuildSignalsArrayOnComponentRemoved<MapEntityLink>();
      BuildSignalsArrayOnComponentAdded<NavMeshAvoidanceAgent>();
      BuildSignalsArrayOnComponentRemoved<NavMeshAvoidanceAgent>();
      BuildSignalsArrayOnComponentAdded<NavMeshAvoidanceObstacle>();
      BuildSignalsArrayOnComponentRemoved<NavMeshAvoidanceObstacle>();
      BuildSignalsArrayOnComponentAdded<NavMeshPathfinder>();
      BuildSignalsArrayOnComponentRemoved<NavMeshPathfinder>();
      BuildSignalsArrayOnComponentAdded<NavMeshSteeringAgent>();
      BuildSignalsArrayOnComponentRemoved<NavMeshSteeringAgent>();
      BuildSignalsArrayOnComponentAdded<PhysicsBody2D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsBody2D>();
      BuildSignalsArrayOnComponentAdded<PhysicsBody3D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsBody3D>();
      BuildSignalsArrayOnComponentAdded<PhysicsCallbacks2D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsCallbacks2D>();
      BuildSignalsArrayOnComponentAdded<PhysicsCallbacks3D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsCallbacks3D>();
      BuildSignalsArrayOnComponentAdded<PhysicsCollider2D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsCollider2D>();
      BuildSignalsArrayOnComponentAdded<PhysicsCollider3D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsCollider3D>();
      BuildSignalsArrayOnComponentAdded<PhysicsJoints2D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsJoints2D>();
      BuildSignalsArrayOnComponentAdded<PhysicsJoints3D>();
      BuildSignalsArrayOnComponentRemoved<PhysicsJoints3D>();
      BuildSignalsArrayOnComponentAdded<Quantum.PlayerInfo>();
      BuildSignalsArrayOnComponentRemoved<Quantum.PlayerInfo>();
      BuildSignalsArrayOnComponentAdded<Quantum.StateMachine>();
      BuildSignalsArrayOnComponentRemoved<Quantum.StateMachine>();
      BuildSignalsArrayOnComponentAdded<Transform2D>();
      BuildSignalsArrayOnComponentRemoved<Transform2D>();
      BuildSignalsArrayOnComponentAdded<Transform2DVertical>();
      BuildSignalsArrayOnComponentRemoved<Transform2DVertical>();
      BuildSignalsArrayOnComponentAdded<Transform3D>();
      BuildSignalsArrayOnComponentRemoved<Transform3D>();
      BuildSignalsArrayOnComponentAdded<View>();
      BuildSignalsArrayOnComponentRemoved<View>();
    }
    partial void SetPlayerInputCodeGen(PlayerRef player, Input input) {
      if ((int)player >= (int)_globals->input.Length) { throw new System.ArgumentOutOfRangeException("player"); }
      var i = _globals->input.GetPointer(player);
      i->Move = input.Move;
      i->Dash = i->Dash.Update(this.Number, input.Dash);
    }
    public Input* GetPlayerInput(PlayerRef player) {
      if ((int)player >= (int)_globals->input.Length) { throw new System.ArgumentOutOfRangeException("player"); }
      return _globals->input.GetPointer(player);
    }
    partial void GetPlayerLastConnectionStateCodeGen(ref BitSetRef bitSet) {
      bitSet = new(_globals->PlayerLastConnectionState.Bits, _globals->PlayerLastConnectionState.Length);
    }
    partial void ResetPhysicsCodeGen() {
      if (Context.Physics2D != null && Physics2D.Map != null && Physics2D.Map.Guid.IsDynamic) Physics2D.ResetMap();
      Physics2D.Init(_globals->PhysicsState2D.MapStaticCollidersState.TrackedMap);
      if (Context.Physics3D != null && Physics3D.Map != null && Physics3D.Map.Guid.IsDynamic) Physics3D.ResetMap();
      Physics3D.Init(_globals->PhysicsState3D.MapStaticCollidersState.TrackedMap);
    }
    public unsafe partial struct FrameSignals {
    }
  }
  public unsafe partial class Statics {
    public static FrameSerializer.Delegate SerializeInput;
    static partial void InitStaticDelegatesGen() {
      SerializeInput = Quantum.Input.Serialize;
    }
    static partial void RegisterSimulationTypesGen(TypeRegistry typeRegistry) {
      typeRegistry.Register(typeof(AssetGuid), AssetGuid.SIZE);
      typeRegistry.Register(typeof(AssetRef), AssetRef.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet1024), Quantum.BitSet1024.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet128), Quantum.BitSet128.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet2048), Quantum.BitSet2048.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet256), Quantum.BitSet256.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet4096), Quantum.BitSet4096.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet512), Quantum.BitSet512.SIZE);
      typeRegistry.Register(typeof(Quantum.BitSet6), Quantum.BitSet6.SIZE);
      typeRegistry.Register(typeof(Button), Button.SIZE);
      typeRegistry.Register(typeof(CallbackFlags), 4);
      typeRegistry.Register(typeof(CharacterController2D), CharacterController2D.SIZE);
      typeRegistry.Register(typeof(CharacterController3D), CharacterController3D.SIZE);
      typeRegistry.Register(typeof(ColorRGBA), ColorRGBA.SIZE);
      typeRegistry.Register(typeof(ComponentPrototypeRef), ComponentPrototypeRef.SIZE);
      typeRegistry.Register(typeof(ComponentTypeRef), ComponentTypeRef.SIZE);
      typeRegistry.Register(typeof(Quantum.Dash), Quantum.Dash.SIZE);
      typeRegistry.Register(typeof(DistanceJoint), DistanceJoint.SIZE);
      typeRegistry.Register(typeof(DistanceJoint3D), DistanceJoint3D.SIZE);
      typeRegistry.Register(typeof(EntityPrototypeRef), EntityPrototypeRef.SIZE);
      typeRegistry.Register(typeof(EntityRef), EntityRef.SIZE);
      typeRegistry.Register(typeof(FP), FP.SIZE);
      typeRegistry.Register(typeof(FPBounds2), FPBounds2.SIZE);
      typeRegistry.Register(typeof(FPBounds3), FPBounds3.SIZE);
      typeRegistry.Register(typeof(FPMatrix2x2), FPMatrix2x2.SIZE);
      typeRegistry.Register(typeof(FPMatrix3x3), FPMatrix3x3.SIZE);
      typeRegistry.Register(typeof(FPMatrix4x4), FPMatrix4x4.SIZE);
      typeRegistry.Register(typeof(FPQuaternion), FPQuaternion.SIZE);
      typeRegistry.Register(typeof(FPVector2), FPVector2.SIZE);
      typeRegistry.Register(typeof(FPVector3), FPVector3.SIZE);
      typeRegistry.Register(typeof(FrameMetaData), FrameMetaData.SIZE);
      typeRegistry.Register(typeof(FrameTimer), FrameTimer.SIZE);
      typeRegistry.Register(typeof(HingeJoint), HingeJoint.SIZE);
      typeRegistry.Register(typeof(HingeJoint3D), HingeJoint3D.SIZE);
      typeRegistry.Register(typeof(Hit), Hit.SIZE);
      typeRegistry.Register(typeof(Hit3D), Hit3D.SIZE);
      typeRegistry.Register(typeof(Quantum.Input), Quantum.Input.SIZE);
      typeRegistry.Register(typeof(Quantum.InputButtons), 4);
      typeRegistry.Register(typeof(IntVector2), IntVector2.SIZE);
      typeRegistry.Register(typeof(IntVector3), IntVector3.SIZE);
      typeRegistry.Register(typeof(Joint), Joint.SIZE);
      typeRegistry.Register(typeof(Joint3D), Joint3D.SIZE);
      typeRegistry.Register(typeof(LayerMask), LayerMask.SIZE);
      typeRegistry.Register(typeof(MapEntityId), MapEntityId.SIZE);
      typeRegistry.Register(typeof(MapEntityLink), MapEntityLink.SIZE);
      typeRegistry.Register(typeof(NavMeshAvoidanceAgent), NavMeshAvoidanceAgent.SIZE);
      typeRegistry.Register(typeof(NavMeshAvoidanceObstacle), NavMeshAvoidanceObstacle.SIZE);
      typeRegistry.Register(typeof(NavMeshPathfinder), NavMeshPathfinder.SIZE);
      typeRegistry.Register(typeof(NavMeshRegionMask), NavMeshRegionMask.SIZE);
      typeRegistry.Register(typeof(NavMeshSteeringAgent), NavMeshSteeringAgent.SIZE);
      typeRegistry.Register(typeof(NullableFP), NullableFP.SIZE);
      typeRegistry.Register(typeof(NullableFPVector2), NullableFPVector2.SIZE);
      typeRegistry.Register(typeof(NullableFPVector3), NullableFPVector3.SIZE);
      typeRegistry.Register(typeof(NullableNonNegativeFP), NullableNonNegativeFP.SIZE);
      typeRegistry.Register(typeof(PhysicsBody2D), PhysicsBody2D.SIZE);
      typeRegistry.Register(typeof(PhysicsBody3D), PhysicsBody3D.SIZE);
      typeRegistry.Register(typeof(PhysicsCallbacks2D), PhysicsCallbacks2D.SIZE);
      typeRegistry.Register(typeof(PhysicsCallbacks3D), PhysicsCallbacks3D.SIZE);
      typeRegistry.Register(typeof(PhysicsCollider2D), PhysicsCollider2D.SIZE);
      typeRegistry.Register(typeof(PhysicsCollider3D), PhysicsCollider3D.SIZE);
      typeRegistry.Register(typeof(PhysicsEngineState), PhysicsEngineState.SIZE);
      typeRegistry.Register(typeof(PhysicsJoints2D), PhysicsJoints2D.SIZE);
      typeRegistry.Register(typeof(PhysicsJoints3D), PhysicsJoints3D.SIZE);
      typeRegistry.Register(typeof(PhysicsQueryRef), PhysicsQueryRef.SIZE);
      typeRegistry.Register(typeof(PhysicsSceneSettings), PhysicsSceneSettings.SIZE);
      typeRegistry.Register(typeof(Quantum.PlayerInfo), Quantum.PlayerInfo.SIZE);
      typeRegistry.Register(typeof(PlayerRef), PlayerRef.SIZE);
      typeRegistry.Register(typeof(Ptr), Ptr.SIZE);
      typeRegistry.Register(typeof(QBoolean), QBoolean.SIZE);
      typeRegistry.Register(typeof(Quantum.Ptr), Quantum.Ptr.SIZE);
      typeRegistry.Register(typeof(QueryOptions), 2);
      typeRegistry.Register(typeof(RNGSession), RNGSession.SIZE);
      typeRegistry.Register(typeof(Shape2D), Shape2D.SIZE);
      typeRegistry.Register(typeof(Shape3D), Shape3D.SIZE);
      typeRegistry.Register(typeof(SpringJoint), SpringJoint.SIZE);
      typeRegistry.Register(typeof(SpringJoint3D), SpringJoint3D.SIZE);
      typeRegistry.Register(typeof(Quantum.State), 4);
      typeRegistry.Register(typeof(Quantum.StateMachine), Quantum.StateMachine.SIZE);
      typeRegistry.Register(typeof(Transform2D), Transform2D.SIZE);
      typeRegistry.Register(typeof(Transform2DVertical), Transform2DVertical.SIZE);
      typeRegistry.Register(typeof(Transform3D), Transform3D.SIZE);
      typeRegistry.Register(typeof(View), View.SIZE);
      typeRegistry.Register(typeof(Quantum._globals_), Quantum._globals_.SIZE);
    }
    static partial void InitComponentTypeIdGen() {
      ComponentTypeId.Reset(ComponentTypeId.BuiltInComponentCount + 3)
        .AddBuiltInComponents()
        .Add<Quantum.Dash>(Quantum.Dash.Serialize, null, null, ComponentFlags.None)
        .Add<Quantum.PlayerInfo>(Quantum.PlayerInfo.Serialize, null, null, ComponentFlags.None)
        .Add<Quantum.StateMachine>(Quantum.StateMachine.Serialize, null, null, ComponentFlags.None)
        .Finish();
    }
    [Preserve()]
    public static void EnsureNotStrippedGen() {
      FramePrinter.EnsureNotStripped();
      FramePrinter.EnsurePrimitiveNotStripped<CallbackFlags>();
      FramePrinter.EnsurePrimitiveNotStripped<Quantum.InputButtons>();
      FramePrinter.EnsurePrimitiveNotStripped<QueryOptions>();
      FramePrinter.EnsurePrimitiveNotStripped<Quantum.State>();
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
