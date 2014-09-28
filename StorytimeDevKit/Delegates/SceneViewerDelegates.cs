using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Models.SceneViewer;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Delegates
{
    public delegate void OnDropActor(ActorViewModel actorModel, SceneTabViewModel sceneTabModel, Vector2 position);
    public delegate void OnMouseMove(SceneTabViewModel model, Vector2 position);
    public delegate void OnMouseClick(SceneTabViewModel model, Vector2 position);
    public delegate void OnMouseDown(SceneTabViewModel model, Vector2 position);
    public delegate void OnMouseUp(SceneTabViewModel model, Vector2 position);
    public delegate void OnSceneAdded(SceneTabViewModel model);
}
