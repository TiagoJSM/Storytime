﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Enums;
using StoryTimeDevKit.Controllers.Puppeteer;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public delegate void OnAssetListItemViewModelDrop(AssetListItemViewModel model, Vector2 dropPosition);

    public interface IPuppeteerEditorControl : IMouseInteractiveControl
    {
        event Action<IPuppeteerEditorControl> OnLoaded;
        event Action<IPuppeteerEditorControl> OnUnloaded;
        event Action<PuppeteerWorkingModeType> OnWorkingModeChanges;
        event OnAssetListItemViewModelDrop OnAssetListItemViewModelDrop;

        IPuppeteerController PuppeteerController { get; }
    }
}
