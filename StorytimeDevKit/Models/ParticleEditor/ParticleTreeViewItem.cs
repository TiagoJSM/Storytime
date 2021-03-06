﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleTreeViewItem : TreeViewItemViewModel
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

        public object EditableObject { get; private set; }

        public ParticleTreeViewItem(string name, INodeAddedCallback nodeAddCB, object editableObject)
            : this(name, null, nodeAddCB, editableObject)
        {

        }

        public ParticleTreeViewItem(string name, TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, object editableObject)
            : base(parent, nodeAddCB, false)
        {
            Name = name;
            EditableObject = editableObject;
        }
    }
}
