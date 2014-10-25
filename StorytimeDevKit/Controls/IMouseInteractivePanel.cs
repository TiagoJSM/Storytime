using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controls
{
    public delegate void OnPanelMouseUp(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions);
    public delegate void OnPanelMouseDown(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions);
    public delegate void OnPanelMouseClick(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions);
    public delegate void OnPanelMouseMove(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions, System.Windows.Forms.MouseButtons buttons);

    public interface IMouseInteractivePanel
    {
        event OnPanelMouseUp OnMouseUp;
        event OnPanelMouseDown OnMouseDown;
        event OnPanelMouseClick OnMouseClick;
        event OnPanelMouseMove OnMouseMove;
    }
}
