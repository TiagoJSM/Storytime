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
            var dialog = new LoadSavedPuppeteerItemsDialog();
            dialog.Owner = _window;
            var accepted = dialog.ShowDialog();
            var file = dialog.FileInfo;

            if (accepted == true)
            {
                _control.PuppeteerController.Load(file);
            }
        }
    }
}
