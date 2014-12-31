using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.DataStructures;
using System.Windows.Input;
using StoryTimeDevKit.Models.Puppeteer;
using Puppeteer.Armature;
using StoryTimeDevKit.Controllers.Puppeteer;

namespace StoryTimeDevKit.Commands.UICommands.Puppeteer
{
    public class AttachToBoneCommand : BaseCommand
    {
        PuppeteerController _controlData;

        public AttachToBoneCommand(PuppeteerController controlData)
        {
            _controlData = controlData;
        }

        public override bool CanExecute(object parameter)
        {
            return _controlData.SelectedBoneRenderableAsset != null;
        }

        public override void Execute(object parameter)
        {
            var model = parameter as BoneViewModel;
            var bone = _controlData.GetBoneFrom(model);
            _controlData.SelectedBoneRenderableAsset.Bone = bone;
        }
    }
}
