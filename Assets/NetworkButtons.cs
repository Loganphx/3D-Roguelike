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

        public int Bits => this._bits;

        public NetworkButtons(int buttons) => this._bits = buttons;

        public bool IsSet(int button) => (this._bits & 1 << button) != 0;

        public void SetDown(int button) => this._bits |= 1 << button;

        public void SetUp(int button) => this._bits &= ~(1 << button);

        public void Set(int button, bool state)
        {
            if (state)
                this.SetDown(button);
            else
                this.SetUp(button);
        }

        public void SetAllUp() => this._bits = 0;

        public void SetAllDown() => this._bits = -1;

        public bool IsSet<T>(T button) where T : unmanaged, Enum => this.IsSet(UnsafeUtility.EnumToInt<T>(button));

        public void SetDown<T>(T button) where T : unmanaged, Enum => this.SetDown(UnsafeUtility.EnumToInt<T>(button));

        public void SetUp<T>(T button) where T : unmanaged, Enum => this.SetDown(UnsafeUtility.EnumToInt<T>(button));

        public void Set<T>(T button, bool state) where T : unmanaged, Enum =>
            this.Set(UnsafeUtility.EnumToInt<T>(button), state);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(NetworkButtons buttons) => buttons.Bits;
        public static implicit operator NetworkButtons(int buttons) => new NetworkButtons(buttons);

    }
}