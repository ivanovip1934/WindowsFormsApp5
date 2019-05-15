using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Xml;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; // For the P/Invoke signatures.


namespace WindowsFormsApp5
{
    

    public static class PositionWindowDemo
    {

        // P/Invoke declarations.

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const int SW_MAXIMIZE = 3;

        public static void Main()
        {
            string Type = "";
            string Name = "";
            try{
                XmlReader xmlReader = XmlReader.Create("config.xml");
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name.ToLower())
                        {
                            case "ipclassname":
                                Type = "IpClassName";
                                Name = xmlReader.ReadInnerXml();
                                break;
                            case "ipwindowname":
                                Type = "IpWindowName";
                                Name = xmlReader.ReadInnerXml();
                                break;
                        }

                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
            }

            if ((Type == "") || (Name == ""))
            {
                MessageBox.Show("Not set application from config.xml", "Warning");
                Environment.Exit(1);
            }

            if (Screen.AllScreens.Length == 1)            {
                MessageBox.Show("You have one monitor", "Message");
                Environment.Exit(0);
            }
                
            var area = Screen.AllScreens[0].Primary ? Screen.AllScreens[1].WorkingArea : Screen.AllScreens[0].WorkingArea;
            int scStartWidth = area.X;
            int scStartHight = area.Y;
          
            
            IntPtr hWnd = IntPtr.Zero;
            switch (Type) {
                case "IpClassName":
                    hWnd = FindWindow(Name, null);
                    break;
                case "IpWindowName":
                    hWnd = FindWindow(null, Name);
                    break;
            }
            
            
            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                // Move the window to (0,0) without changing its size or position
                // in the Z order.
                SetWindowPos(hWnd, IntPtr.Zero, scStartWidth, scStartHight, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                ShowWindow(hWnd, SW_MAXIMIZE);
            }
        }

    }
}
