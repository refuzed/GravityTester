using System;

namespace GravityTester
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GravityTester game = new GravityTester())
            {
                game.Run();
            }
        }
    }
#endif
}

