using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.DataStructures;
using System.Windows.Input;
using StoryTimeDevKit.Models.Puppeteer;
using Puppeteer.Armature;

namespace StoryTimeDevKit.Commands.UICommands.Puppeteer
{
    public class AttachToBoneCommand : BaseCommand
    {
        PuppeteerEditorControlData _controlData;

        public AttachToBoneCommand(PuppeteerEditorControlData controlData)
        {
            _controlData = controlData;
        }

        public override bool CanExecute(object parameter)
        {
            return _controlData.SelectedBoneRenderableAsset != null;
        }

        public override void Execute(object parameter)
        {
            BoneViewModel model = parameter as BoneViewModel;
            Bone bone = _controlData.GetBoneFrom(model);
            _controlData.SelectedBoneRenderableAsset.Bone = bone;
        }
    }
}
