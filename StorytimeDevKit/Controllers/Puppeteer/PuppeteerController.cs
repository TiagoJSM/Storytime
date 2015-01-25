using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.TemplateControllers;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeDevKit.Models.SceneViewer;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Controllers.Puppeteer.WorkingModes;
using StoryTime;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Resources.Graphic;
using FarseerPhysicsWrapper;
using FarseerPhysics.Factories;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Entities.Renderables;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.DataStructures;
using StoryTimeDevKit.SceneWidgets.Transformation;
using Ninject;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Commands.ReversibleCommands;
using Puppeteer.Armature;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using Puppeteer.Resources;
using System.Collections.ObjectModel;
using TimeLineTool;
using StoryTimeDevKit.DataStructures.Factories;
using StoryTimeDevKit.Commands.UICommands.Puppeteer;
using StoryTimeDevKit.Entities.Renderables.Puppeteer;
using StoryTimeDevKit.Enums;
using StoryTimeDevKit.Models.SavedData.Bones;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Extensions;
using System.IO;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public class PuppeteerController
        : EditableSceneController<IPuppeteerEditorControl, PuppeteerWorkingModeType>, 
          IPuppeteerController, 
          ISkeletonViewerController,
          IPuppeteerWorkingModeContext,
          IAnimationTimeLineController,
          IPuppeteerSceneOjectActionContext,
          INodeAddedCallback
    {
        private IPuppeteerEditorControl _puppeteerEditorControl;
        private ISkeletonTreeViewControl _skeletonTreeViewControl;
        private IAnimationTimeLineControl _timeLineControl;

        private SceneBonesDataSource _sceneBoneData;
        private SkeletonTreeViewDataSource _skeletonTreeViewData;
        private AnimationTimeLineDataSource _animationTimeLineData;

        private ISceneObjectFactory _sceneObjectFactory;
        private SavedPuppeteerLoadFactory _loadSaveFilesfatory;

        private Dictionary<PuppeteerWorkingModeType, WorkingMode> _workingModes;
        private Dictionary<BoneAttachedRenderableAsset, AssetListItemViewModel> _assetMapping;
        private PuppeteerWorkingModesModel _workingModesModel;
        private ArmatureActor _armatureActor;

        public IPuppeteerEditorControl PuppeteerControl 
        {
            get
            {
                return _puppeteerEditorControl;
            }
            set
            {
                if (_puppeteerEditorControl == value) return;
                if (_puppeteerEditorControl != null)
                    UnassignPuppeteerEditorControlEvents();
                _puppeteerEditorControl = value;
                if (value != null)
                    AssignPuppeteerEditorControlEvents();
            }
        }
        public object Selected 
        {
            get
            {
                return SelectedObject;
            }
            set
            {
                if (SelectedObject == value) return;
                SelectedObject = value;

                if (SelectedObject != null)
                {
                    TransformModeModel.HasActor = true;
                }
                else
                {
                    TransformModeModel.HasActor = false;
                }
            }
        }
        public Skeleton Skeleton { get; private set; }
        public bool HasAnimations
        {
            get { return _animationTimeLineData.HasAnimations(); }
        }

        public ISkeletonTreeViewControl SkeletonTreeViewControl
        {
            get
            {
                return _skeletonTreeViewControl;
            }
            set
            {
                if (_skeletonTreeViewControl == value) return;
                if (_skeletonTreeViewControl != null)
                    UnassignSkeletonTreeViewControlEvents();
                _skeletonTreeViewControl = value;
                if (value != null)
                    AssignSkeletonTreeViewControlEvents();
            }
        }
        public SkeletonViewModel SkeletonViewModel { get { return _skeletonTreeViewData.SkeletonViewModel; } }

        public SavePuppeteerItemDialogModel SavedPuppeteerItemModel { get; private set; }

        public IAnimationTimeLineControl TimeLineControl
        {
            get
            {
                return _timeLineControl;
            }
            set
            {
                if (_timeLineControl == value) return;
                if (_timeLineControl != null)
                    UnassignAnimationTimeLineControlEvents();
                _timeLineControl = value;
                if (value != null)
                    AssignAnimationTimeLineControlEvents();
            }
        }
        public double? Seconds { get; private set; }
        public bool BoneAttachedRenderableAssetSelected
        {
            get
            {
                if (SceneObjectViewModel.SceneObject == null) return false;
                return SceneObjectViewModel.SceneObject.Object is BoneAttachedRenderableAsset;
            }
        }

        protected override Dictionary<PuppeteerWorkingModeType, WorkingMode> WorkingModeMapping
        {
            get { return _workingModes; }
        }
        protected override ISceneObjectFactory SceneObjectFactory
        {
            get { return _sceneObjectFactory; }
        }

        public PuppeteerController(GameWorld gameWorld, TransformModeViewModel transformModeModel, PuppeteerWorkingModesModel workingModesModel)
            : base(gameWorld, transformModeModel)
        {
            _workingModes =
                new Dictionary<PuppeteerWorkingModeType, WorkingMode>()
                {
                    { PuppeteerWorkingModeType.BoneSelectionMode, new BoneSelectionMode(this) },
                    { PuppeteerWorkingModeType.AssetSelectionMode, new AssetSelectionMode(this) },
                    { PuppeteerWorkingModeType.AddBoneMode, new AddBoneMode(this) }
                };

            Skeleton = new Skeleton();
            SavedPuppeteerItemModel = new SavePuppeteerItemDialogModel();
            _sceneObjectFactory = new PuppeteerSceneObjectFactory(this);
            _loadSaveFilesfatory = new SavedPuppeteerLoadFactory();
            _armatureActor = Scene.AddWorldEntity<ArmatureActor>();
            _sceneBoneData = new SceneBonesDataSource(Skeleton);
            _animationTimeLineData = new AnimationTimeLineDataSource(Skeleton);
            _skeletonTreeViewData = new SkeletonTreeViewDataSource(this, new AttachToBoneCommand(this));
            _assetMapping = new Dictionary<BoneAttachedRenderableAsset, AssetListItemViewModel>();
            _workingModesModel = workingModesModel;
            ConfigureSceneUI();
        }

        public BoneActor AddBone(Vector2 boneStartPosition)
        {
            var boneActor = Selected as BoneActor;
            var child = AddBone(boneStartPosition, boneActor);
            //HandleNewBoneAdded(child);
            return child;
        }

        public BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition)
        {
            var boneActor = Selected as BoneActor;
            var child = AddBone(boneStartPosition, boneEndPosition, boneActor);
            //HandleNewBoneAdded(child);
            return child;
        }

        public BaseActor GetIntersectedActor(Vector2 position)
        {
            return Scene.Intersect(position).OfType<BaseActor>().FirstOrDefault();
        }

        public void EnableTransformationUI(bool enable)
        {
            EnableUI = enable;
        }

        public BoneViewModel GetBoneViewModelByName(string name)
        {
            return _skeletonTreeViewData.GetBoneViewModelByName(name);
        }

        public void SelectBone(BoneViewModel model)
        {
            Selected = model.BoneActor;
            _workingModesModel.WorkingMode = PuppeteerWorkingModeType.BoneSelectionMode;
        }

        public void AttachBoneToSelectedRenderableAsset(BoneViewModel model)
        {
            if (!BoneAttachedRenderableAssetSelected) return;
            var asset = SceneObjectViewModel.SceneObject.Object as BoneAttachedRenderableAsset;
            asset.Bone = model.BoneActor.AssignedBone;
            var viewModel = _assetMapping[asset];
            var assetViewModel = new AssetViewModel(model, this, viewModel.Name);
            model.Children.Add(assetViewModel);
        }

        public void SaveSkeleton()
        {
            PuppeteerUtils.SaveSkeleton(
                new SavedSkeletonFile()
                {
                    SavedSkeleton = Skeleton.ToSavedSkeleton(),
                    FileNameWithoutExtension = SavedPuppeteerItemModel.FileNameWithoutExtension
                });
        }

        public void SaveAnimatedSkeleton()
        {
            PuppeteerUtils.SaveAnimatedSkeleton(
                new SavedAnimatedSkeletonFile()
                {
                    SavedAnimatedSkeleton = new SavedAnimatedSkeleton()
                    {
                        SavedSkeleton = Skeleton.ToSavedSkeleton(),
                    },
                    FileNameWithoutExtension = SavedPuppeteerItemModel.FileNameWithoutExtension
                });
        }

        public void SynchronizeBoneChain(Bone bone)
        {
            _sceneBoneData.SynchronizeBoneChain(bone);
        }

        public void AddAnimationFrameFor(BoneActor actor)
        {
            if (Seconds == null)
                _animationTimeLineData.AddBoneInitialSate(actor);
            else
                _animationTimeLineData.AddAnimationFrame(actor, Seconds.Value);
        }

        public void Load(FileInfo file)
        {
            ClearAll();
            var loadedContent = _loadSaveFilesfatory.Load(file);
            if (loadedContent.Skeleton != null)
            {
                //LoadSkeleton(loadedContent.Skeleton);
            }
        }

        public void ClearAll()
        {
            var boneActors = Scene.GetAll<BoneActor>();
            foreach (var boneActor in boneActors)
                Scene.RemoveActor(boneActor);
            foreach (var bone in Skeleton.RootBones.ToList())
                Skeleton.RemoveBone(bone);
            _sceneBoneData.Clear();
            _skeletonTreeViewData.Clear();
            _animationTimeLineData.Clear();
            _skeletonTreeViewControl.Clear();
            _timeLineControl.Clear();
            Selected = null;
        }

        private void AssignPuppeteerEditorControlEvents()
        {
            BindControlEvents(_puppeteerEditorControl);
            _puppeteerEditorControl.OnLoaded += PuppeteerOnLoadedHandler;
            _puppeteerEditorControl.OnUnloaded += PuppeteerOnUnloadedHandler;
            _puppeteerEditorControl.OnWorkingModeChanges += OnWorkingModeChangesHandler;
            _puppeteerEditorControl.OnAssetListItemViewModelDrop += OnAssetListItemViewModelDropHandler;
        }

        private void UnassignPuppeteerEditorControlEvents()
        {
            BindControlEvents(null);
            _puppeteerEditorControl.OnLoaded -= PuppeteerOnLoadedHandler;
            _puppeteerEditorControl.OnUnloaded -= PuppeteerOnUnloadedHandler;
            _puppeteerEditorControl.OnWorkingModeChanges -= OnWorkingModeChangesHandler;
            _puppeteerEditorControl.OnAssetListItemViewModelDrop -= OnAssetListItemViewModelDropHandler;
        }

        private void AssignSkeletonTreeViewControlEvents()
        {
            _skeletonTreeViewControl.OnLoaded += SkeletonViewerOnLoadedHandler;
            _skeletonTreeViewControl.OnUnloaded += SkeletonViewerOnUnloadedHandler;
        }

        private void UnassignSkeletonTreeViewControlEvents()
        {
            _skeletonTreeViewControl.OnLoaded -= SkeletonViewerOnLoadedHandler;
            _skeletonTreeViewControl.OnUnloaded -= SkeletonViewerOnUnloadedHandler;
        }

        private void AssignAnimationTimeLineControlEvents()
        {
            _timeLineControl.OnTimeMarkerChange += OnTimeMarkerChangeHandler;
        }

        private void UnassignAnimationTimeLineControlEvents()
        {
            _timeLineControl.OnTimeMarkerChange -= OnTimeMarkerChangeHandler;
        }

        private void PuppeteerOnLoadedHandler(IPuppeteerEditorControl control)
        {
        }

        private void PuppeteerOnUnloadedHandler(IPuppeteerEditorControl control)
        {
            UnassignPuppeteerEditorControlEvents();
        }

        private void OnWorkingModeChangesHandler(PuppeteerWorkingModeType mode)
        {
            SetWorkingMode(mode);
        }

        private void SkeletonViewerOnLoadedHandler(ISkeletonTreeViewControl control)
        {
        }

        private void SkeletonViewerOnUnloadedHandler(ISkeletonTreeViewControl control)
        {
            UnassignSkeletonTreeViewControlEvents();
        }

        private void ConfigureSceneUI()
        {
            TranslateWidget.OnTranslationComplete += OnTranslationCompleteHandler;
            RotateWidget.OnStopRotation += OnStopRotationHandler;
        }

        private void OnTranslationCompleteHandler(Vector2 fromPosition, Vector2 toPosition)
        {
            if (base.SelectedObject == null) return;
            //IReversibleCommand command = new MoveActorCommand(_puppeteerEdControlData._transformSceneObjectModel.Actor, fromPosition, toPosition);
            //Commands.Push(command);
        }

        private void OnStopRotationHandler(float originalRotation, float finalRotation)
        {
            if (base.SelectedObject == null) return;
            //IReversibleCommand command = new RotateActorCommand(_puppeteerEdControlData._transformSceneObjectModel.Actor, originalRotation, finalRotation);
            //Commands.Push(command);
        }

        private void OnAssetListItemViewModelDropHandler(AssetListItemViewModel viewModel, Vector2 dropPosition)
        {
            var texture = Scene.GraphicsContext.CreateTexture2D(viewModel.FullPath);
            var asset = new BoneAttachedRenderableAsset() 
            { 
                Texture2D = texture,
                RenderingOffset = dropPosition
            };
            _armatureActor.ArmatureRenderableAsset.Add(asset);
            _assetMapping.Add(asset, viewModel);
        }

        private void OnTimeMarkerChangeHandler(double seconds)
        {
            Seconds = seconds;
            _animationTimeLineData.Animation.SetTime(TimeSpan.FromSeconds(seconds));
            _sceneBoneData.SynchronizeFullBoneChain();
        }

        private void HandleNewBoneAdded(BoneActor actor)
        {
            _timeLineControl.AddTimeLine(
                _skeletonTreeViewData.GetBoneViewModelFromActor(actor),
                _animationTimeLineData.GetCollectionBoundToActor(actor));
            _timeLineControl.AddFrame(null, 0, Vector2.Zero);
        }

        private BoneActor AddBone(Vector2 boneStartPosition, BoneActor parent)
        {
            var actor = Scene.AddWorldEntity<BoneActor>();
            actor.Parent = parent;
            actor.Body.Position = boneStartPosition;

            _sceneBoneData.Add(actor);
            _skeletonTreeViewData.AddBone(actor);
            _animationTimeLineData.AddTimeLineFor(actor);
            HandleNewBoneAdded(actor);
            return actor;
        }

        private BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition, BoneActor parent)
        {
            var actor = AddBone(boneStartPosition, parent);
            var bone = actor.AssignedBone;
            bone.AbsoluteEnd = boneEndPosition;
            _sceneBoneData.SynchronizeBoneChain(bone);
            return actor;
        }

        public void NodeAddedCallback(TreeViewItemViewModel parent, IEnumerable<TreeViewItemViewModel> newModels)
        {
        }
    }
}
