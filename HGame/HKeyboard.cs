using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace HGame
{
    internal static class HKeyboard
    {
        public static KeyboardState KeyState;
        private static KeyboardState _oldKeyState;

        public static bool ShiftStatus => KeyState.IsKeyDown(Keys.LeftShift) || KeyState.IsKeyDown(Keys.RightShift);

        public static bool CtrlStatus => KeyState.IsKeyDown(Keys.LeftControl) || KeyState.IsKeyDown(Keys.RightControl);

        public static bool AltStatus => KeyState.IsKeyDown(Keys.LeftAlt) || KeyState.IsKeyDown(Keys.RightAlt);

        public static bool IsMouseVisible => !AltStatus;

        public static void Update(KeyboardState keyState)
        {
            KeyState = keyState;
        }
    }
}
