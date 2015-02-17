using StoryTimeDevKit.Controls.Dialogs;
using StoryTimeDevKit.Controls.Puppeteer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace StoryTimeDevKit.Commands.UICommands.Puppeteer
{
    public class SaveAnimatedSkeletonCommand : BaseCommand
    {
        private IPuppeteerEditorControl _control;
        private Window _window;

        public SaveAnimatedSkeletonCommand(IPuppeteerEditorControl control, Window window)
        {
            _control = control;
            _window = window;
        }

        public override bool CanExecute(object parameter)
        {
            var hasBones = _control.PuppeteerController.Skeleton.RootBones.Any();
            if (!hasBones) return false;
            return _control.PuppeteerController.HasAnimations;
        }

        public override void Execute(object parameter)
        {
            bool? accepted = true;
            if (string.IsNullOrWhiteSpace(_control.PuppeteerController.SavedPuppeteerItemModel.FileNameWithoutExtension))
            {
                var dialog = new CreatePuppeteerItemDialog(_control.PuppeteerController.SavedPuppeteerItemModel);
                dialog.Owner = _window;
                accepted = dialog.ShowDialog();
            }

            if (accepted == true)
            {
                _control.PuppeteerController.SaveAnimatedSkeleton();
            }
        }
    }
}
