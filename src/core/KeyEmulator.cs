
using System.Runtime.InteropServices;

namespace NotifiableTools
{

    public static class KeyEmulator
    {

        //=====================================================================


        public static void PressKey(Keys key)
        {
            int vsc = NativeMethod.GetScanCodeFromKey(key);

            Input input = new Input();
            input.Type = 1;
            input.ui.Keyboard.VirtualKey = (short)key;
            input.ui.Keyboard.ScanCode = (short)vsc;
            input.ui.Keyboard.Flags = 0;
            input.ui.Keyboard.Time = 0;
            input.ui.Keyboard.ExtraInfo = IntPtr.Zero;

            NativeMethod.SendInput(input);
        }

        public static void ReleaseKey(Keys key)
        {
            int vsc = NativeMethod.GetScanCodeFromKey(key);

            Input input = new Input();
            input.Type = 1;
            input.ui.Keyboard.VirtualKey = (short)key;
            input.ui.Keyboard.ScanCode = (short)vsc;
            input.ui.Keyboard.Flags = (int)KeyEventCode.KeyUp;
            input.ui.Keyboard.Time = 0;
            input.ui.Keyboard.ExtraInfo = IntPtr.Zero;

            NativeMethod.SendInput(input);
        }

        public static void PressAndReleaseKey(Keys key)
        {
            int vsc = NativeMethod.GetScanCodeFromKey(key);

            Input[] inputs = new Input[2];

            inputs[0] = KeyEmulator.CreateInput(key, (short) vsc, KeyEventCode.KeyDown);
            inputs[1] = KeyEmulator.CreateInput(key, (short)vsc, KeyEventCode.KeyUp);

            NativeMethod.SendInput(inputs);
        }

        public static void InputText(string text)
        {
            Input[] inputs = text.ToCharArray()
                .SelectMany(c => new Input[] {

                    KeyEmulator.CreateInputFromScanCode((short) c, KeyEventCode.Unicode, KeyEventCode.KeyDown),
                    KeyEmulator.CreateInputFromScanCode((short) c, KeyEventCode.Unicode, KeyEventCode.KeyUp)

                }).ToArray();

            NativeMethod.SendInput(inputs);
        }





        





        //=====================================================================
        //参考: https://qiita.com/kob58im/items/ca3b61e72111dfbab8a8



        private enum KeyEventCode
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            ScanCode = 0x0008,
            Unicode = 0x0004,
        }


        private class NativeMethod
        {

            [DllImport("user32.dll", SetLastError = true)]
            private extern static void SendInput(int nInputs, Input[] pInputs, int cbsize);

            [DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
            private extern static int MapVirtualKey(int wCode, int wMapType);



            private const int MapVkToVsc = 0;


            public static void SendInput(params Input[] inputs)
            {
                NativeMethod.SendInput(inputs.Length, inputs, Marshal.SizeOf(inputs[0]));
            }

            public static int GetScanCodeFromKey(Keys key)
            {
                return NativeMethod.MapVirtualKey((int)key, NativeMethod.MapVkToVsc);
            }
        }



        private static Input CreateInput(Keys key, short vScanCode, params KeyEventCode[] flags)
        {
            Input input = new Input();
            input.Type = 1;
            input.ui.Keyboard.VirtualKey = (short)key;
            input.ui.Keyboard.ScanCode = vScanCode;
            input.ui.Keyboard.Flags = (short) flags.Aggregate((a, b) => a | b);
            input.ui.Keyboard.Time = 0;
            input.ui.Keyboard.ExtraInfo = IntPtr.Zero;

            return input;
        }

        private static Input CreateInputFromKey(Keys key, params KeyEventCode[] flags)
        {
            Input input = new Input();
            input.Type = 1;
            input.ui.Keyboard.VirtualKey = (short)key;
            input.ui.Keyboard.Flags = (short)flags.Aggregate((a, b) => a | b);
            input.ui.Keyboard.Time = 0;
            input.ui.Keyboard.ExtraInfo = IntPtr.Zero;

            return input;
        }

        private static Input CreateInputFromScanCode(short vScanCode, params KeyEventCode[] flags)
        {
            Input input = new Input();
            input.Type = 1;
            input.ui.Keyboard.ScanCode = vScanCode;
            input.ui.Keyboard.Flags = (short)flags.Aggregate((a, b) => a | b);
            input.ui.Keyboard.Time = 0;
            input.ui.Keyboard.ExtraInfo = IntPtr.Zero;

            return input;
        }



        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public int X;
            public int Y;
            public int Data;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public short VirtualKey;
            public short ScanCode;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Input
        {
            public int Type;
            public InputUnion ui;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public MouseInput Mouse;
            [FieldOffset(0)]
            public KeyboardInput Keyboard;
            [FieldOffset(0)]
            public HardwareInput Hardware;
        }


        //=====================================================================

    }




}
