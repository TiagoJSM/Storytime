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
            using (GamePipeline game = new GamePipeline())
            {
                game.Run();
            }
        }
    }
#endif
}

