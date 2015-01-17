using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Controls.Dialogs;
using StoryTimeDevKit.Controls.Puppeteer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace StoryTimeDevKit.Commands.UICommands.Puppeteer
{
    public class SaveSkeletonCommand : BaseCommand
    {
        private IPuppeteerEditorControl _control;
        private Window _window;

        public SaveSkeletonCommand(IPuppeteerEditorControl control, Window window)
        {
            _control = control;
            _window = window;
        }

        public override bool CanExecute(object parameter)
        {
            return _control.PuppeteerController.Skeleton.RootBones.Any();
        }

        public override void Execute(object parameter)
        {
            bool? accepted = true;
            if (string.IsNullOrWhiteSpace(_control.PuppeteerController.SavedSkeletonModel.FileNameWithoutExtension))
            {
                var dialog = new CreateSkeletonDialog(_control.PuppeteerController.SavedSkeletonModel);
                dialog.Owner = _window;
                accepted = dialog.ShowDialog();
            }

            if (accepted == true)
            {
                _control.PuppeteerController.SaveSkeleton();
            }
        }
    }
}
