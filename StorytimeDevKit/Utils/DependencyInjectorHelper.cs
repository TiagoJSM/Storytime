using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using System.Windows;
using StoryTimeDevKit.Configurations;

namespace StoryTimeDevKit.Utils
{
    public static class DependencyInjectorHelper
    {
        public static IKernel MainWindowKernel
        {
            get 
            { 
                return Application
                        .Current
                        .Properties[ApplicationProperties.MainWindowDependencyInjectorKey] as IKernel;
            }
        }

        public static IKernel PuppeteerKernel
        {
            get
            {
                return Application
                        .Current
                        .Properties[ApplicationProperties.PuppeteerDependencyInjectorKey] as IKernel;
            }
        }

        public static IKernel ParticleEditorKernel
        {
            get
            {
                return Application
                        .Current
                        .Properties[ApplicationProperties.ParticleEditorInjectorKey] as IKernel;
            }
        }
    }
}
