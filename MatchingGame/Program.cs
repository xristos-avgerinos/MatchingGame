using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    static class Program
    {
        public static List<String> defaultImages = new List<string>();//η λιστα αυτη θα περιεχει τις default φωτος(paths) που θα φορτωθουν στα pictureboxes του panel1
        public static List<String> imageList = new List<string>();//η λιστα αυτη κραταει ολες τις φωτο(pictureboxes) μετα το shuffle που γινεται καθε φορα με τη σειρα που δοθηκαν ανακατεμενα

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
