using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace ECS.Movement.Services
{
    [StructLayout(LayoutKind.Explicit)]
    public struct NetworkButtons
    {
        [FieldOffset(0)] private int _bits;

        public int Bits => _bits;

        public NetworkButtons(int buttons) => _bits = buttons;

        public bool IsSet(int button) => (_bits & 1 << button) != 0;

        public void SetDown(int button) => _bits |= 1 << button;

        public void SetUp(int button) => _bits &= ~(1 << button);

        public void Set(int button, bool state)
        {
            if (state)
                SetDown(button);
            else
                SetUp(button);
        }

        public void SetAllUp() => _bits = 0;

        public void SetAllDown() => _bits = -1;

        public bool IsSet<T>(T button) where T : unmanaged, Enum => IsSet(UnsafeUtility.EnumToInt(button));

        public void SetDown<T>(T button) where T : unmanaged, Enum => SetDown(UnsafeUtility.EnumToInt(button));

        public void SetUp<T>(T button) where T : unmanaged, Enum => SetDown(UnsafeUtility.EnumToInt(button));

        public void Set<T>(T button, bool state) where T : unmanaged, Enum =>
            Set(UnsafeUtility.EnumToInt(button), state);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(NetworkButtons buttons) => buttons.Bits;
        public static implicit operator NetworkButtons(int buttons) => new NetworkButtons(buttons);

    }
}