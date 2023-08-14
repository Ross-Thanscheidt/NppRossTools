using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    static class Main
    {
        internal const string PluginName = "Ross Tools";
        //static string iniFilePath = null;
        //static bool someSetting = false;
        //static frmMyDlg frmMyDlg = null;
        //static int idMyDlg = -1;
        //static Bitmap tbBmp = NppRossTools.Properties.Resources.star;
        //static Bitmap tbBmp_tbTab = NppRossTools.Properties.Resources.star_bmp;
        //static Icon tbIcon = null;
        static readonly IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static readonly INotepadPPGateway notepad = new NotepadPPGateway();

        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            //StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            //Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            //iniFilePath = sbIniFilePath.ToString();
            //if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            //iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            //someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);

            PluginBase.SetCommand(0, "Update Ages", UpdateAgesCommand, new ShortcutKey(false, false, false, Keys.None));
            //PluginBase.SetCommand(1, "MyDockableDialog", myDockableDialog); idMyDlg = 1;
        }

        internal static void SetToolBarIcon()
        {
            //toolbarIcons tbIcons = new toolbarIcons();
            //tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            //IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            //Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            //Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            //Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
            //Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
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

    //    internal static void myDockableDialog()
    //    {
    //        if (frmMyDlg == null)
    //        {
    //            frmMyDlg = new frmMyDlg();

    //            using (Bitmap newBmp = new Bitmap(16, 16))
    //            {
    //                Graphics g = Graphics.FromImage(newBmp);
    //                ColorMap[] colorMap = new ColorMap[1];
    //                colorMap[0] = new ColorMap();
    //                colorMap[0].OldColor = Color.Fuchsia;
    //                colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
    //                ImageAttributes attr = new ImageAttributes();
    //                attr.SetRemapTable(colorMap);
    //                g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
    //                tbIcon = Icon.FromHandle(newBmp.GetHicon());
    //            }

    //            NppTbData _nppTbData = new NppTbData();
    //            _nppTbData.hClient = frmMyDlg.Handle;
    //            _nppTbData.pszName = "My dockable dialog";
    //            _nppTbData.dlgID = idMyDlg;
    //            _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
    //            _nppTbData.hIconTab = (uint)tbIcon.Handle;
    //            _nppTbData.pszModuleName = PluginName;
    //            IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
    //            Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

    //            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
    //        }
    //        else
    //        {
    //            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMSHOW, 0, frmMyDlg.Handle);
    //        }
    //    }
    }
}