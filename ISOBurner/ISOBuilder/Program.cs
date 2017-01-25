using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
namespace ISOBuilder
{

    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ISOBuilderForm dlg = new ISOBuilderForm();
            // Pass options to dialog
            int showHelp = 0;
            foreach (string a in args)
            {
                if (a.StartsWith("--automate"))
                {
                    dlg.SetAutomation(true);
                }
                else if (a.StartsWith("--completionaction="))
                {
                    dlg.SetCompletionAction(Convert.ToInt32(a.Substring(19)));
                }
                else if (a.StartsWith("--statusfile="))
                {
                    dlg.SetStatusFile(a.Substring(13));
                }
                else if (a.StartsWith("--isofile="))
                {
                    dlg.SetISOFile(a.Substring(10));
                }
                else if (a.StartsWith("--burner="))
                {
                    dlg.SetBurnerDrive(a.Substring(9));
                }
                else if (a.StartsWith("--speed="))
                {
                    dlg.SetBurnerSpeed(Convert.ToInt32(a.Substring(8)));
                }
                    /***
                else if (a.StartsWith("--media="))
                {
                    dlg.SetMediaType(a.Substring(8));
                }
                     ***/
                else if (a.StartsWith("--help"))
                {
                    showHelp = 1;
                }
                else
                {
                    showHelp += 2;
                }
            }
            if (showHelp > 0)
            {
                string msg;
                string title;
                msg = "ISOBurner v" + dlg._version + "\n\n";
                if (showHelp > 1)
                {
                    title = "Invalid option specified";
                    msg += "One or more invalid options specified\n";
                }
                else
                {
                    title = "ISOBurner command line options";
                }
                msg += "General options\n";
                msg += "  --isofile=<filename>\tISO file to burn\n";
                msg += "  --burner=<driveletter>\tDrive letter of burner to use\n";
                msg += "  --speed=<rate>\t\tMaximum data rate to use\n\t\t\t(default=0 for max supported)\n";
                msg += "Automation (options marked * work only if --automate specified):\n";
                msg += "  --automate\t\tStart burning specified iso\n";
                msg += "* --statusfile=<filename>\tWrite status messages to specified file\n";
                // msg += "--media=<type>      Type can be dvd-r, dvd+r, dvd+dl, cd-r\n";
                // msg += "--completionaction=<code> (default=0) 0 = take no action; 1 = exit\n";
                MessageBox.Show(msg, title);
            }
            else
            {
                Application.Run(dlg);
            }
        }
    }
}
