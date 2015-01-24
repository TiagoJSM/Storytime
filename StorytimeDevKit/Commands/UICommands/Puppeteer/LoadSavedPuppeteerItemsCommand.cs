using StoryTimeDevKit.Controls.Dialogs;
using StoryTimeDevKit.Controls.Puppeteer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace StoryTimeDevKit.Commands.UICommands.Puppeteer
{
    public class LoadSavedPuppeteerItemsCommand : BaseCommand
    {
        private IPuppeteerEditorControl _control;
        private Window _window;

        public LoadSavedPuppeteerItemsCommand(IPuppeteerEditorControl control, Window window)
        {
            _control = control;
            _window = window;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            bool? accepted = true;
            FileInfo file = null;
            if (string.IsNullOrWhiteSpace(_control.PuppeteerController.SavedPuppeteerItemModel.FileNameWithoutExtension))
            {
                var dialog = new LoadSavedPuppeteerItemsDialog();
                dialog.Owner = _window;
                accepted = dialog.ShowDialog();
                file = dialog.FileInfo;
            }

            if (accepted == true)
            {
                _control.PuppeteerController.Load(file);
                //_control.PuppeteerController.SaveSkeleton();
            }
        }
    }
}
