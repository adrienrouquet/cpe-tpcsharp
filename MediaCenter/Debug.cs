using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaCenter
{
    class Debug
    {
        private static Boolean _DebugMode = true;
        private static DebugWindow _debugWindow = null;

        public Debug()
        {
            if (_debugWindow == null)
                _debugWindow = new DebugWindow();

        }

        public void Show(String str)
        {
            _debugWindow.Show(str);
            if(_DebugMode)
                _debugWindow.Show();
        }
    }
}
