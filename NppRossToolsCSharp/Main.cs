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
            PluginBase.SetCommand(0, "Remove Trailing Spaces", RemoveTrailingSpaces, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "Update Ages", UpdateAgesCommand, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(2, "Update Line Balances", UpdateLineBalances, new ShortcutKey(false, false, false, Keys.None));
        }

        internal static void SetToolBarIcon()
        {
        }

        internal static void PluginCleanUp()
        {
        }


        internal static void RemoveTrailingSpaces()
        {
            int lineCount = editor.GetLineCount();
            if (lineCount > 0)
            {
                editor.BeginUndoAction();
                for (int lineNumber = 0; lineNumber < lineCount; lineNumber++)
                {
                    editor.GotoLine(lineNumber);
                    string line = editor.GetLine(lineNumber);
                    bool endHasLF = line.Length > 0 && line[line.Length - 1] == '\n';

                    string newLine = line.TrimEnd();

                    if (endHasLF)
                    {
                        newLine = string.Concat(newLine, '\n');
                    }

                    if (!string.Equals(line, newLine))
                    {
                        editor.SelectCurrentLine();
                        editor.ReplaceSel(newLine);
                    }
                }
                editor.GotoLine(0);
                editor.EndUndoAction();
            }
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

        internal static void UpdateLineBalances()
        {
            var regexBalance = new Regex(@"^(?<prefix>\??)(?<balance>\-?[0-9]+(?>,?[0-9]{3})*(?>\.[0-9]{0,2})?)(?<eol>.*Balance.*)$");
            var regexTransaction = new Regex(@"^(?<prefix>\?*)(?<balance>-?[0-9]*(?>,?[0-9]{3})*(?>\.[0-9]{0,2})?)(?<suffix>\??) *(?<transall>(?>\(|\[|\\\[)(?<transamount>(?>\-|\+)?[0-9]*(?>,?[0-9]{3})*(?>\.[0-9]{0,2})?).*?(?>\)|\]|\\\]))(?<eol>.*)$");

            int lineCount = editor.GetLineCount();
            if (lineCount > 0)
            {
                editor.BeginUndoAction();
                decimal? currentBalance = null;
                for (int lineNumber = lineCount - 1; lineNumber >= 0; lineNumber--)
                {
                    editor.GotoLine(lineNumber);
                    string line = editor.GetLine(lineNumber);
                    if (line.Trim().Length > 0)
                    {
                        if (line.StartsWith("* * *"))
                        {
                            currentBalance = null;
                        }
                        else
                        {
                            MatchCollection matches = regexBalance.Matches(line);
                            if (matches.Count > 0)
                            {
                                if (decimal.TryParse(regexBalance.Replace(line, "${balance}"), out var balance))
                                {
                                    currentBalance = balance;
                                }
                            }

                            if (currentBalance.HasValue)
                            {
                                matches = regexTransaction.Matches(line);
                                if (matches.Count > 0)
                                {
                                    if (decimal.TryParse(regexTransaction.Replace(line, "${transamount}"), out var transAmount))
                                    {
                                        currentBalance = currentBalance + transAmount;
                                        string newBalanceString = currentBalance.Value.ToString("N2");
                                        string oldBalanceString = regexTransaction.Replace(line, "${balance}");
                                        if (!string.Equals(oldBalanceString, newBalanceString))
                                        {
                                            string newLine = regexTransaction.Replace(line, "${prefix}" + newBalanceString + "${suffix} ${transall}${eol}");
                                            editor.SelectCurrentLine();
                                            editor.ReplaceSel(newLine);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                editor.GotoLine(0);
                editor.EndUndoAction();
            }
        }
    }
}