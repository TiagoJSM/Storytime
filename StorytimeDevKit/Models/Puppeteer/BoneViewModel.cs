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
                return _name;
            }
            set
            {
                if (_name == value) return;
                _name = value;
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

        public BoneViewModel(INodeAddedCallback nodeAddCB, ICommand attachToBoneCommand, string name)
            : this(null, nodeAddCB, attachToBoneCommand, name)
        {
        }

        public BoneViewModel(
            TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, ICommand attachToBoneCommand, string name)
            : base(parent, nodeAddCB, false)
        {
            Name = name;
            AttachToBoneCommand = attachToBoneCommand;
        }
    }
}
