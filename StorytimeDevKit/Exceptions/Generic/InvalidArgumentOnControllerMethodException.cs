using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers;
using StoryTimeDevKit.Resources.Exceptions;

namespace StoryTimeDevKit.Exceptions.Generic
{
    public class InvalidArgumentOnControllerMethodException : ApplicationBaseException
    {
        public InvalidArgumentOnControllerMethodException(
            IController controller, 
            string actionName, 
            string parameterName, 
            Type parameterType, string userText)
            : base(
                userText, 
                FormatLogText(
                    controller, 
                    actionName, 
                    parameterName, 
                    parameterType
                )
            )
        {
        }

        private static string FormatLogText(
            IController controller, 
            string actionName, 
            string parameterName, 
            Type parameterType)
        {
            return
                string.Format(
                    LocalizedTexts.InvalidArgumentOnControllerMethodogTextTemplate,
                    parameterName,
                    parameterType.Name,
                    actionName,
                    controller.GetType().Name
                );
        }
    }
}
