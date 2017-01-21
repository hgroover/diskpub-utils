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
                if (showHelp > 1)
                    System.Console.Out.WriteLine("One or more invalid options specified");
                System.Console.Out.WriteLine("Syntax:");
                System.Console.Out.WriteLine("--automate  Start burning specified iso");
                System.Console.Out.WriteLine("--statusfile=<filename> Write status messages to specified file");
                System.Console.Out.WriteLine("--completionaction=<code> (default=0) 0 = take no action; 1 = exit");
                System.Console.Out.WriteLine("--iso=<filename>  ISO file to burn");
                System.Console.Out.WriteLine("--burner=<driveletter> Drive letter of burner to use");
                System.Console.Out.WriteLine("--speed=<rate>  Maximum data rate to use (default=0 for max supported)");
                System.Console.Out.WriteLine("--media=<type>  Type can be dvd-r, dvd+r, dvd+dl, cd-r");
            }
            else
            {
                Application.Run(dlg);
            }
        }
    }
}
