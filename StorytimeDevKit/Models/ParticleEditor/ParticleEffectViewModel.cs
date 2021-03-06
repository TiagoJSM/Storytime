﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands.ParticleEditor;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleEffectViewModel : ParticleTreeViewItem
    {
        public ICommand AddParticleEmitter { get; private set; }

        public ParticleEffectViewModel(string name, INodeAddedCallback nodeAddCB, ICommand addParticleEmitterCommand)
            : base(name, nodeAddCB, null)
        {
            AddParticleEmitter = addParticleEmitterCommand;
        }

    }
}
