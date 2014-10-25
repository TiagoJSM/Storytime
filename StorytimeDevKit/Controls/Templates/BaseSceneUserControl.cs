using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Delegates;
using StoryTimeFramework.WorldManagement;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Extensions;

namespace StoryTimeDevKit.Controls.Templates
{
    public class BaseSceneUserControl : UserControl, IMouseInteractiveControl
    {
        public event OnMouseMove OnMouseMove;
        public event OnMouseClick OnMouseClick;
        public event OnMouseDown OnMouseDown;
        public event OnMouseUp OnMouseUp;

        private Vector2 _clickPosition;

        protected void AssignPanelEventHandling(IMouseInteractivePanel panel)
        {
            panel.OnMouseClick += Panel_OnMouseClick;
            panel.OnMouseDown += Panel_OnMouseDown;
            panel.OnMouseMove += Panel_OnMouseMove;
            panel.OnMouseUp += Panel_OnMouseUp;
        }

        protected virtual Scene GetScene() { return null; }

        private void Panel_OnMouseClick(
            System.Drawing.Point pointInGamePanel,
            System.Drawing.Point gamePanelDimensions)
        {
            Scene scene = GetScene();
            if (scene == null) return;

            _clickPosition = scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseClick != null)
                OnMouseClick(scene, _clickPosition);
        }

        private void Panel_OnMouseDown(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            Scene scene = GetScene();
            if (scene == null) return;

            Vector2 clickPosition = scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseDown != null)
                OnMouseDown(scene, clickPosition);
        }

        private void Panel_OnMouseMove(
            System.Drawing.Point pointInGamePanel,
            System.Drawing.Point gamePanelDimensions,
            System.Windows.Forms.MouseButtons buttons)
        {
            Scene scene = GetScene();
            if (scene == null) return;

            Vector2 point = scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseMove != null)
                OnMouseMove(scene, point);
        }

        private void Panel_OnMouseUp(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            Scene scene = GetScene();
            if (scene == null) return;

            Vector2 mouseDownPosition = scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseUp != null)
                OnMouseUp(scene, mouseDownPosition);
        }
    }
}
