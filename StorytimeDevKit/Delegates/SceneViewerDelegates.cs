using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Models.SceneViewer;
using Microsoft.Xna.Framework;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeDevKit.Delegates
{
    public delegate void OnDropActor(ActorViewModel actorModel, SceneTabViewModel sceneTabModel, Vector2 position);
    public delegate void OnMouseMove(Scene scene, Vector2 position);
    public delegate void OnMouseClick(Scene scene, Vector2 position);
    public delegate void OnMouseDown(Scene scene, Vector2 position);
    public delegate void OnMouseUp(Scene scene, Vector2 position);
    public delegate void OnSceneAdded(SceneTabViewModel model);
    public delegate void OnSceneChanged(SceneTabViewModel model);
}
