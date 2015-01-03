using System;

namespace StoryTime
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new NormalGame())
            {
                game.Run();
            }
        }
    }
#endif
}

