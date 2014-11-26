using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Controls;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class BoneViewModel : TreeViewItemViewModel
    {
        private string _name;

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

        public ICommand AttachToBoneCommand { get; private set; }

        public BoneViewModel(INodeAddedCallback nodeAddCB, ICommand attachToBoneCommand)
            : this(null, nodeAddCB, attachToBoneCommand)
        {
        }

        public BoneViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, ICommand attachToBoneCommand)
            : base(parent, nodeAddCB, false)
        {
            Name = "Bone";
            AttachToBoneCommand = attachToBoneCommand;
        }
    }
}
