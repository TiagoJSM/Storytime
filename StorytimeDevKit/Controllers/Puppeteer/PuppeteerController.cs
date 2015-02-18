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
        private IRenderableAssetOrderControl _renderableAssetOrderControl;

        private SceneBonesDataSource _sceneBoneData;
        private SkeletonViewDataSource _skeletonTreeViewData;
        private AnimationTimeLineDataSource _animationTimeLineData;

        private ISceneObjectFactory _sceneObjectFactory;
        private SavedPuppeteerLoadFactory _loadSaveFilesfatory;

        private Dictionary<PuppeteerWorkingModeType, WorkingMode> _workingModes;
        private Dictionary<BoneAttachedRenderableAsset, AssetListItemViewModel> _assetMapping;
        private PuppeteerWorkingModesModel _workingModesModel;

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
        public ObservableCollection<AssetViewModel> RenderableAssetOrderModels { get { return _skeletonTreeViewData.RenderableAssetOrderModels; } }

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
        public TimeSpan AnimationTotalTime
        {
            get { return _animationTimeLineData.AnimationTotalTime; }
        }

        public IRenderableAssetOrderControl RenderableAssetOrderControl
        {
            get
            {
                return _renderableAssetOrderControl;
            }
            set
            {
                if (_renderableAssetOrderControl == value) return;
                if (_renderableAssetOrderControl != null)
                    UnassignRenderableAssetOrderControlEvents();
                _renderableAssetOrderControl = value;
                if (value != null)
                    AssignRenderableAssetOrderControlEvents();
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

            var armatureActor = Scene.AddWorldEntity<ArmatureActor>();
            Skeleton = armatureActor.SkeletonComponent.Skeleton;
            SavedPuppeteerItemModel = new SavePuppeteerItemDialogModel();
            _sceneObjectFactory = new PuppeteerSceneObjectFactory(this);
            _loadSaveFilesfatory = new SavedPuppeteerLoadFactory();
            _sceneBoneData = new SceneBonesDataSource(Skeleton, Scene);
            _animationTimeLineData = new AnimationTimeLineDataSource(Skeleton);
            _skeletonTreeViewData = new SkeletonViewDataSource(this, new AttachToBoneCommand(this), armatureActor);
            _assetMapping = new Dictionary<BoneAttachedRenderableAsset, AssetListItemViewModel>();
            _workingModesModel = workingModesModel;
            ConfigureSceneUI();
        }

        public BoneActor AddBone(Vector2 boneStartPosition)
        {
            var boneActor = Selected as BoneActor;
            var child = AddBone(boneStartPosition, boneActor);
            return child;
        }

        public BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition)
        {
            var boneActor = Selected as BoneActor;
            var child = AddBone(boneStartPosition, boneEndPosition, boneActor);
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
            _skeletonTreeViewData.AttachAssetToBone(asset, model);
            //var assetViewModel = new AssetViewModel(model, this, viewModel.Name);

            //model.Children.Add(assetViewModel);
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
                        SavedAnimations = new SavedAnimations()
                        {
                            SavedAnimationCollection = new SavedAnimation[]
                            {
                                _animationTimeLineData.Animation.FramesMapping.ToSavedAnimation()
                            }
                        }
                    },
                    FileNameWithoutExtension = SavedPuppeteerItemModel.FileNameWithoutExtension
                });
        }

        public void SynchronizeBoneChain(Bone bone)
        {
            _sceneBoneData.SynchronizeBoneChain(bone);
        }

        public void AddAnimationFrameFor(BoneActor actor, BoneState fromState, BoneState toState)
        {
            if (Seconds == null) return;
            _animationTimeLineData.AddAnimationFrame(actor, Seconds.Value, fromState, toState);
        }

        public void Load(FileInfo file)
        {
            ClearAll();
            var loadedContent = _loadSaveFilesfatory.Load(file);
            if (loadedContent.SavedSkeleton != null)
            {
                LoadSavedSkeleton(loadedContent.SavedSkeleton);
            }
            if (loadedContent.SavedAnimations != null)
            {
                LoadSavedAnimations(loadedContent.SavedAnimations);
            }
        }

        public void ClearAll()
        {
            var boneActors = Scene.GetAll<BoneActor>();
            foreach (var boneActor in boneActors)
                Scene.RemoveWorldEntity(boneActor);
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

        private void AssignRenderableAssetOrderControlEvents()
        {
            _renderableAssetOrderControl.OnAssetOrderChange += OnAssetOrderChangeHandler;
        }

        private void UnassignRenderableAssetOrderControlEvents()
        {
            _renderableAssetOrderControl.OnAssetOrderChange += OnAssetOrderChangeHandler;
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
            var assetViewModel = new AssetViewModel(this, asset, viewModel.Name);
            _skeletonTreeViewData.AddAssetToArmature(assetViewModel);
            _assetMapping.Add(asset, viewModel);
        }

        private void OnTimeMarkerChangeHandler(double seconds)
        {
            Seconds = seconds;
            _animationTimeLineData.Animation.SetTime(TimeSpan.FromSeconds(seconds));
            _sceneBoneData.SynchronizeFullBoneChain();
        }

        private BoneActor AddBone(Vector2 boneStartPosition, BoneActor parentActor = null)
        {
            var parentBone = parentActor == null ? null : parentActor.AssignedBone;
            var actor = _sceneBoneData.Add(boneStartPosition, parentBone);
            actor.Parent = parentActor;
            HandleNewBoneAdded(actor);
            return actor;
        }

        private BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition, BoneActor parentActor = null)
        {
            var parentBone = parentActor == null ? null : parentActor.AssignedBone;
            var actor = _sceneBoneData.Add(boneStartPosition, boneEndPosition, parentBone);
            actor.Parent = parentActor;
            HandleNewBoneAdded(actor);
            var bone = actor.AssignedBone;
            _sceneBoneData.SynchronizeBoneChain(bone);
            return actor;
        }

        private void HandleNewBoneAdded(BoneActor actor)
        {
            _skeletonTreeViewData.AddBone(actor);
            _animationTimeLineData.AddTimeLineFor(actor);

            _timeLineControl.AddTimeLine(
                _skeletonTreeViewData.GetBoneViewModelFromActor(actor),
                _animationTimeLineData.GetCollectionBoundToActor(actor));
            _timeLineControl.AddFrame(null, 0, Vector2.Zero);
        }

        private void LoadSavedSkeleton(SavedSkeleton savedSkeleton)
        {
            var rootBones = savedSkeleton.RootBones;

            foreach (var rootBone in rootBones)
            {
                var boneActor = AddBone(rootBone.AbsolutePosition.GetVector2(), rootBone.AbsoluteEnd.GetVector2());
                LoadSavedBone(rootBone, boneActor);
            }
        }

        private void LoadSavedAnimations(SavedAnimations savedAnimations)
        {
            //ToDo: in the future the editor will support multiple animations, then this has to be changed
            var savedAnimation = savedAnimations.SavedAnimationCollection.First();
            
            foreach(var boneAnimationFrames in savedAnimation.BoneAnimationFrames)
            {
                LoadSavedBoneAnimations(boneAnimationFrames);
            }
        }

        private void LoadSavedBoneAnimations(SavedBoneAnimation savedBoneAnimation)
        {
            var actor = _sceneBoneData.GetBoneActorByName(savedBoneAnimation.BoneName);
            foreach(var frame in savedBoneAnimation.AnimationFrames)
            {
                var seconds = frame.EndTime.TotalSeconds;
                var fromState = new BoneState()
                {
                    Rotation = frame.StartRotation,
                    Translation = frame.StartTranslation
                };
                var toState = new BoneState()
                {
                    Rotation = frame.EndRotation,
                    Translation = frame.EndTranslation
                };
                _animationTimeLineData.AddAnimationFrame(actor, seconds, fromState, toState);
            }
        }

        private void LoadSavedBone(SavedBone boneParent, BoneActor parent = null)
        {
            foreach (var child in boneParent.Children)
            {
                var boneActor = AddBone(child.AbsolutePosition.GetVector2(), child.AbsoluteEnd.GetVector2(), parent);
                LoadSavedBone(child, boneActor);
            }
        }

        private void OnAssetOrderChangeHandler(AssetViewModel model, int index)
        {
            _skeletonTreeViewData.Move(model, index);
        }

        public void NodeAddedCallback(TreeViewItemViewModel parent, IEnumerable<TreeViewItemViewModel> newModels)
        {
        }
    }
}
