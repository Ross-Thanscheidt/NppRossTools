using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    static class Main
    {
        internal const string PluginName = "Ross Tools";
        static readonly IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static readonly INotepadPPGateway notepad = new NotepadPPGateway();

        public static void OnNotification(ScNotification notification)
        {
            // This method is invoked whenever something is happening in notepad++
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
        }

        internal static void CommandMenuInit()
        {
            PluginBase.SetCommand(0, "Update Ages", UpdateAgesCommand, new ShortcutKey(false, false, false, Keys.None));
        }

        internal static void SetToolBarIcon()
        {
        }

        internal static void PluginCleanUp()
        {
        }


        internal static void UpdateAgesCommand()
        {
            var regex = new Regex(@"^(\d{2}\/.{2}\/(\d{4}).*)\(\d+ in \d{4}\)(.*)$");
            int currentYear = DateTime.Now.Year;

            int lineCount = editor.GetLineCount();
            if (lineCount > 0)
            {
                editor.BeginUndoAction();
                for (int lineNumber = 0; lineNumber < lineCount; lineNumber++)
                {
                    editor.GotoLine(lineNumber);
                    string line = editor.GetLine(lineNumber);
                    MatchCollection matches = regex.Matches(line);
                    if (matches.Count > 0)
                    {
                        int age = currentYear - int.Parse(matches[0].Groups[2].Value);
                        string newLine = regex.Replace(line, $"$1({age} in {currentYear})$3");
                        editor.SelectCurrentLine();
                        editor.ReplaceSel(newLine);
                    }
                }
                editor.GotoLine(0);
                editor.EndUndoAction();
            }
        }
    }
}