using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsMedia
{
    public interface IController
    {
        void VolumeUp();
        void VolumeDown();
        void Prevoius();
        void Next();
        void PlayPause();
        void MoveLeft();
        void MoveTop();
        void MoveRight();
        void MoveBottom();
        void Click();

        void EmitScreenCapture();
    }
}
