using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FinalProject {
    static class KeyboardHandler {
        /// <summary>
        /// Enum for not pressing a key and for pressing a key
        /// </summary>
        private enum KeyStates { None = 0, Down = 1, }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        /// <summary>
        /// Method used to return the state of the registered key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static KeyStates GetKeyState(Keys key) {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            if ((retVal & 0x8000) == 0x8000) {
                state = KeyStates.Down;
            }

            return state;
        }

        /// <summary>
        /// Method used to return the state of the registered key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key) {
            return GetKeyState(key) == KeyStates.Down;
        }
    }
}