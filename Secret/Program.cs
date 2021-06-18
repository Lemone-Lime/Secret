using Microsoft.Xna.Framework;
using System;
using Hasel;


namespace Secret
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Secret(1024, 576, "Secret - Hasel Engine", Color.Black, false))
                game.Run();
        }
    }
}
