#region File Description
/*
 * Author: Hristo Hristov
 * Date: 10.02.12
 * Revision: 1 (10.02.12)
 *
 * **********************************
 * License: Microsoft Public License (Ms-PL)
 * -----------------------------------------
 * This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
 *
 * 1. Definitions
 *
 * The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
 *
 * A "contribution" is the original software, or any additions or changes to the software.
 *
 * A "contributor" is any person that distributes its contribution under this license.
 *
 * "Licensed patents" are a contributor's patent claims that read directly on its contribution.
 *
 * 2. Grant of Rights
 *
 * (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
 *
 * (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
 *
 * 3. Conditions and Limitations
 *
 * (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
 *
 * (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
 *
 * (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
 * 
 * (D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
 *
 * (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Controls.XNA
{
    public delegate void OnDropActor(ActorViewModel model, System.Drawing.Point positionGameWorld, System.Drawing.Point gamePanelDimensions);
    public delegate void OnDropData(object data, System.Drawing.Point positionGameWorld, System.Drawing.Point gamePanelDimensions);

    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class XnaControl : UserControl, IMouseInteractivePanel
    {
        public event OnDropActor OnDropActor;
        public event OnDropData OnDropData;
        public event OnPanelMouseUp OnMouseUp;
        public event OnPanelMouseDown OnMouseDown;
        public event OnPanelMouseClick OnMouseClick;
        public event OnPanelMouseMove OnMouseMove;

        public IntPtr Handle
        {
            get { return GamePanel.Handle; }
        }

        public XnaControl()
        {
            InitializeComponent();
        }

        private System.Drawing.Point GetPointInGamePanel()
        {
            var cursorScreenCoords = System.Windows.Forms.Cursor.Position;
            var controlRelatedCoords = this.GamePanel.PointToClient(cursorScreenCoords);

            controlRelatedCoords.Y = GamePanel.Height - controlRelatedCoords.Y;
            return controlRelatedCoords;
        }

        private System.Drawing.Point GetGamePanelDimensions()
        {
            var gamePanelDimensions =
                new System.Drawing.Point(GamePanel.Width, GamePanel.Height);

            return gamePanelDimensions;
        }

        private void GamePanel_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            var t = Type.GetType(e.Data.GetFormats()[0]);
            var o = GetDataOfType(t, e);
            
            var pGameWorld =  GetPointInGamePanel();
            var gamePanelDimensions = 
                new System.Drawing.Point(GamePanel.Width, GamePanel.Height);

            if (o is ActorViewModel && OnDropActor != null)
                OnDropActor(o as ActorViewModel, pGameWorld, gamePanelDimensions);
            if (OnDropData != null)
                OnDropData(o, pGameWorld, gamePanelDimensions);
        }

        private void GamePanel_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.Move;
        }

        private void GamePanel_DragLeave(object sender, EventArgs e)
        {

        }

        private void GamePanel_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            
        }

        //due to an incompatibility error between WPF and WinForms
        //the data has to be obtained through introspection hacking
        private object GetDataOfType(Type t, System.Windows.Forms.DragEventArgs e)
        {
            var innerDataFI = e.Data.GetType().GetField("innerData", BindingFlags.NonPublic | BindingFlags.Instance);

            var innerDataObject = innerDataFI.GetValue(e.Data);

            innerDataFI = innerDataObject.GetType().GetField("innerData", BindingFlags.NonPublic | BindingFlags.Instance);

            var dataObj = innerDataFI.GetValue(innerDataObject) as System.Windows.DataObject;

            innerDataFI = dataObj.GetType().GetField("_innerData", BindingFlags.NonPublic | BindingFlags.Instance);
            
            var item = dataObj.GetData(t); 

            return item;
        }

        private void GamePanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var pGamePanel = GetPointInGamePanel();
            var gamePanelDimensions = GetGamePanelDimensions();

            if (OnMouseUp != null)
                OnMouseUp(pGamePanel, gamePanelDimensions);
        }

        private void GamePanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var pGamePanel = GetPointInGamePanel();
            var gamePanelDimensions = GetGamePanelDimensions();

            if (OnMouseMove != null)
                OnMouseMove(pGamePanel, gamePanelDimensions, e.Button);
        }

        private void GamePanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var pGamePanel = GetPointInGamePanel();
            var gamePanelDimensions = GetGamePanelDimensions();

            if (OnMouseDown != null)
                OnMouseDown(pGamePanel, gamePanelDimensions);
        }

        private void GamePanel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var pGamePanel = GetPointInGamePanel();
            var gamePanelDimensions = GetGamePanelDimensions();

            if (OnMouseClick != null)
                OnMouseClick(pGamePanel, gamePanelDimensions);
        }
    }
}
