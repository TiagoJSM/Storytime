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
        public static IKernel Kernel
        {
            get 
            { 
                return Application
                        .Current
                        .Properties[ApplicationProperties.DependencyInjectorKey] as IKernel;
            }
        }
    }
}
