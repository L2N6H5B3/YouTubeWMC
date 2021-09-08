using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeWMC {
    static class Program {

        // Create variable to hold original Cursor position
        private static Point originalCursorPosition;

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Hide the cursor
            Cursor.Hide();
            // Save the original Cursor position
            originalCursorPosition = Cursor.Position;
            // Set the new Cursor position
            Cursor.Position = new Point(0, 0);
            // Run the Application
            Application.Run(new YouTubeTV());
            // Restore original Cursor position
            Cursor.Position = originalCursorPosition;
        }
    }
}
