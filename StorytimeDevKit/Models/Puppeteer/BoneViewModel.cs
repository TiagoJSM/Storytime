using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Controls;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Entities.Actors;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class BoneViewModel : TreeViewItemViewModel
    {
        private string _name;
        private bool _editMode;

        public string Name
        {
            get
            {
                return BoneActor.AssignedBone.Name;
            }
            set
            {
                if (BoneActor.AssignedBone.Name == value) return;
                BoneActor.AssignedBone.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                if (_editMode == value) return;
                _editMode = value;
                OnPropertyChanged("EditMode");
            }
        }

        public ICommand AttachToBoneCommand { get; private set; }

        public BoneActor BoneActor { get; set; }

        public BoneViewModel(INodeAddedCallback nodeAddCB, ICommand attachToBoneCommand, BoneActor actor)
            : this(null, nodeAddCB, attachToBoneCommand, actor)
        {
        }

        public BoneViewModel(
            TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, ICommand attachToBoneCommand, BoneActor actor)
            : base(parent, nodeAddCB, false)
        {
            BoneActor = actor;
            AttachToBoneCommand = attachToBoneCommand;
        }
    }
}
