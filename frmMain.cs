using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;

namespace AutoMouseControlFocus
{
    public partial class frmMain : AutoMouseControlFocus.CustomForm
    {
        string[] sz = null;
        string[] szC = null;

        public frmMain()
        {
            InitializeComponent();

            ///Properties.Settings.Default.Initialized = false;

            if (Properties.Settings.Default.Initialized && Properties.Settings.Default.MinimizeToTray)
            {
                this.Visible = false;
                WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }                   
        }

        private void frmMain_Load(object sender, EventArgs e)
        {            
            if (Properties.Settings.Default.CheckWeek)
            {
                UpdateHelper.InitializeCheckVersionWeek();
            }

            checkForNewVersionEachWeekToolStripMenuItem.Checked = Properties.Settings.Default.CheckWeek;
            minimizeToWindowsSystemTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinimizeToTray;
                        
            runAtWindowsStartupToolStripMenuItem.Checked = RunAtWndowsStartupManager.RunAtWindowsStartup;                        
            SetTitle();

            if (Properties.Settings.Default.Initialized && Properties.Settings.Default.MinimizeToTray)
            {
                this.Visible = false;
                this.Hide();
                WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }            

            AddLanguageMenuItems();

            AdjustSizeLocation();

            ResizeControls();                       

            if (!Properties.Settings.Default.Initialized)
            {
                RunAtWndowsStartupManager.RunAtWindowsStartup = true;

                frmMessageCheckbox fm = new frmMessageCheckbox();
                fm.Show(this);

            }

            if (!Properties.Settings.Default.Initialized)
            {
                Properties.Settings.Default.FocusClassNames = Properties.Settings.Default.DefaultClassNames;

                Properties.Settings.Default.ClickClassNames = Properties.Settings.Default.DefaultClickClassNames;                
            }

            string[] sz = Properties.Settings.Default.FocusClassNames.Split(new string[] { "|" }, StringSplitOptions.None);

            for (int k = 0; k < sz.Length; k++)
            {
                lstFocusClassNames.Items.Add(sz[k]);
            }

            string[] sz2 = Properties.Settings.Default.ClickClassNames.Split(new string[] { "|" }, StringSplitOptions.None);

            for (int k = 0; k < sz2.Length; k++)
            {
                lstClickClassNames.Items.Add(sz2[k]);
            }            

            if (Properties.Settings.Default.Initialized && Properties.Settings.Default.MinimizeToTray)
            {
                btnClose_Click(null, null);
            }

            Properties.Settings.Default.Initialized = true;
            Properties.Settings.Default.Save();
        }

        #region Help Menu

        private void helpGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Module.HelpURL);
        }

        private void pleaseDonateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/donate.php");
        }

        private void dotsSoftwarePRODUCTCATALOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/downloads/4dots-Software-PRODUCT-CATALOG.pdf");
        }

        private void checkForNewVersionEachWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckWeek = checkForNewVersionEachWeekToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void tiHelpFeedback_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));
        }

        private void checkForNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateHelper.CheckVersion(false);
        }

        private void followUsOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.twitter.com/4dotsSoftware");
        }

        private void visit4dotsSoftwareWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.4dots-software.com");
        }

        private void youtubeChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCovA-lld9Q79l08K-V1QEng");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout f = new frmAbout();
            f.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private void minimizeToWindowsSystemTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MinimizeToTray = minimizeToWindowsSystemTrayToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void notMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            this.Visible = true;
            this.Show();
            this.BringToFront();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            //3this.CenterToScreen();

            AdjustSizeLocation();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            string str = "";

            for (int k=0;k<lstFocusClassNames.Items.Count;k++)
            {
                if (k>0)
                {
                    str += "|";
                }

                str += lstFocusClassNames.Items[k].ToString();
            }

            Properties.Settings.Default.FocusClassNames = str;

            str = "";

            for (int k = 0; k < lstClickClassNames.Items.Count; k++)
            {
                if (k > 0)
                {
                    str += "|";
                }

                str += lstClickClassNames.Items[k].ToString();
            }

            Properties.Settings.Default.ClickClassNames = str;            

            this.Visible = !Properties.Settings.Default.MinimizeToTray;

            if (Properties.Settings.Default.MinimizeToTray)
            {
                this.Hide();
                WindowState = FormWindowState.Minimized;
            }

            this.notMain.Visible = Properties.Settings.Default.MinimizeToTray;            

            sz = Properties.Settings.Default.FocusClassNames.Split(new string[] { "|" }, StringSplitOptions.None);

            szC = Properties.Settings.Default.ClickClassNames.Split(new string[] { "|" }, StringSplitOptions.None);

            timFocus.Enabled = true;

            Properties.Settings.Default.Save();
        }               

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState==FormWindowState.Minimized)
            {
                if (Properties.Settings.Default.MinimizeToTray)
                {
                    notMain.Visible = true;
                    this.Visible = false;
                }
            }
        }                        

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notMain_MouseDoubleClick(null, null);
        }

        private void runAtWindowsStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunAtWndowsStartupManager.RunAtWindowsStartup = !runAtWindowsStartupToolStripMenuItem.Checked;

            runAtWindowsStartupToolStripMenuItem.Checked = RunAtWndowsStartupManager.RunAtWindowsStartup;

        }

        bool FreeForPersonalUse = false;
        bool FreeForPersonalAndCommercialUse = true;

        private void SetTitle()
        {
            string str = "";
                        
            if (FreeForPersonalUse)
            {
                str += " - " + TranslateHelper.Translate("Free for Personal Use Only - Please Donate !");
            }
            else if (FreeForPersonalAndCommercialUse)
            {
                str += " - " + TranslateHelper.Translate("Free for Personal and Commercial Use - Please Donate !");
            }

            this.Text = Module.ApplicationTitle + str.ToUpper();
        }        

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(System.Drawing.Point p);

        [DllImport("user32.dll")]
        static extern IntPtr ChildWindowFromPoint(IntPtr parentHwnd,System.Drawing.Point p);

        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetFocus(IntPtr hWnd);

        IntPtr lastFocusHwnd = IntPtr.Zero;

        [DllImport("USER32.DLL")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /*
        [StructLayout(LayoutKind.Sequential)]
    public struct RECT
{
     public int Left;        // x position of upper-left corner
     public int Top;         // y position of upper-left corner
     public int Right;       // x position of lower-right corner
     public int Bottom;      // y position of lower-right corner
}*/
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd,
            int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        enum GetAncestorFlags
        {
            GetParent = 1,
            GetRoot = 2,
            GetRootOwner = 3
        }


        private bool InTimFocus = false;

        private IntPtr lastHwnd = IntPtr.Zero;

        IntPtr focusedControlHandle = IntPtr.Zero;

        private int Wait = 0;
        private Stopwatch sw = new Stopwatch();
        private void timFocus_Tick(object sender, EventArgs e)
        {
            /*
            if (Wait!=0)
            {
                Wait--;
                return;
            }
            */

            if (sw.IsRunning && sw.ElapsedMilliseconds<(SystemInformation.DoubleClickTime+1000))
            {
                return;
            }
            else if (sw.IsRunning)
            {
                sw.Stop();
            }

            int maxWait = 50;

            if ((Control.MouseButtons & MouseButtons.Left)==MouseButtons.Left)
            {
                sw.Stop();
                sw.Reset();
                sw.Start();
                Wait = maxWait;
                return;
            }

            if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right)
            {
                sw.Stop();
                sw.Reset();
                sw.Start();
                Wait = maxWait;

                return;
            }

            if ((Control.MouseButtons & MouseButtons.Middle) == MouseButtons.Middle)
            {
                sw.Stop();
                sw.Reset();
                sw.Start();
                Wait = maxWait;

                return;
            }

            if ((Control.MouseButtons & MouseButtons.XButton1) == MouseButtons.XButton1)
            {
                sw.Stop();
                sw.Reset();
                sw.Start();

                Wait = maxWait;

                return;
            }


            if ((Control.MouseButtons & MouseButtons.XButton2) == MouseButtons.XButton2)
            {
                sw.Stop();
                sw.Reset();
                sw.Start();

                Wait = maxWait;

                return;
            }

            //3IntPtr hWnd = WindowFromPoint(System.Windows.Forms.Cursor.Position);
            if (InTimFocus) return;

            uint activeWindowThread = 0;
            uint thisWindowThread = 0;

            try
            {
                InTimFocus = true;

                IntPtr hWnd = GetForegroundWindow();                

                thisWindowThread = GetWindowThreadProcessId(this.Handle, IntPtr.Zero);
                
                IntPtr hw = WindowFromPoint(System.Windows.Forms.Cursor.Position);                               
                
                IntPtr ancestor = GetAncestor(hw, GetAncestorFlags.GetRoot);

                activeWindowThread =
                GetWindowThreadProcessId(ancestor, IntPtr.Zero);

                AttachThreadInput(activeWindowThread, thisWindowThread, true);
                
                StringBuilder className = new StringBuilder(100);
                int nret = GetClassName(hw, className, className.Capacity);
                                
                if (focusedControlHandle.ToString() != hw.ToString())                
                {                    
                    string cn = className.ToString().ToLower();

                    bool contains = false;
                                        
                    for (int k = 0; k < sz.Length; k++)
                    {
                        if (cn.Contains(sz[k].ToLower()))
                        {
                            contains = true;
                            break;
                        }
                    }

                    bool containsC = false;                                       

                    for (int k = 0; k < szC.Length; k++)
                    {
                        if (cn.Contains(szC[k].ToLower()))
                        {
                            containsC = true;
                            break;
                        }
                    }
                    
                    if (contains)
                    {                                                                       
                        SetFocus(hw);                        

                        focusedControlHandle = GetFocus();                        
                    }
                    else if (containsC)
                    {
                        MouseSimulator.Click(MouseButton.Left);
                        focusedControlHandle = GetFocus();
                    }                                      
                }
            }
            catch { }
            finally
            {
                AttachThreadInput(activeWindowThread, thisWindowThread, false);

                InTimFocus = false;
            }            
        }

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThread();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo,
   bool fAttach);

        [DllImport("user32.dll")]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private bool InAutoSetFocus = false;


        [DllImport("user32.dll")]
            private static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        /// <summary>Enumeration of the different ways of showing a window using
        /// ShowWindow</summary>
        private enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized
            /// or maximized, the system restores it to its original size and
            /// position. An application should specify this flag when displaying
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position.
            /// This value is similar to "ShowNormal", except the window is not
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is
            /// minimized or maximized, the system restores it to its original size
            /// and position. An application should specify this flag when restoring
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
            /// that owns the window is hung. This flag should only be used when
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

        public static void ShowWindow(IntPtr handle)
        {
            //3ShowWindow(handle, WindowShowStyle.Restore);

            ShowWindow(handle, WindowShowStyle.Show);
            SetForegroundWindowNative(handle);

            
        }

        void AutoSetFocus(IntPtr hwndTarget, string childClassName)
        {
            if (InAutoSetFocus) return; 

            try
            {
                InAutoSetFocus = true;

                // hwndTarget is the other app's main window 
                // ...
                uint targetThreadID = GetWindowThreadProcessId(hwndTarget, IntPtr.Zero); //target thread id
                uint myThreadID = GetCurrentThread(); // calling thread id, our thread id
                try
                {
                    bool lRet = AttachThreadInput(myThreadID, targetThreadID, true); // attach current thread id to target window

                    // if it's not already in the foreground...
                    //lRet = BringWindowToTop(hwndTarget);
                    //SetForegroundWindow(hwndTarget);

                    IntPtr hw = WindowFromPoint(System.Windows.Forms.Cursor.Position);
                    IntPtr focusHwnd = GetFocus();

                    if ((int)focusHwnd != 0)
                    {
                        if (focusHwnd.ToString() != hw.ToString())
                        {
                            SetFocus(hw);
                        }
                    }                    
                }
                finally
                {
                    bool lRet = AttachThreadInput(myThreadID, targetThreadID, false); //detach from foreground window
                }
            }
            finally
            {
                InAutoSetFocus = false;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lstFocusClassNames.SelectedItems.Count>0)
            {
                lstFocusClassNames.Items.RemoveAt(lstFocusClassNames.SelectedIndices[0]);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lstFocusClassNames.Items.Clear();

            Properties.Settings.Default.FocusClassNames = Properties.Settings.Default.DefaultClassNames;

            string[] sz = Properties.Settings.Default.FocusClassNames.Split(new string[] { "|" }, StringSplitOptions.None);

            for (int k = 0; k < sz.Length; k++)
            {
                lstFocusClassNames.Items.Add(sz[k]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtClassName.Text.Trim()==string.Empty)
            {
                Module.ShowMessage("Please enter a Class Name !");
                return;
            }

            string str = txtClassName.Text;

            bool found = false;

            for (int k=0;k<lstFocusClassNames.Items.Count;k++)
            {
                if (str.ToLower()==lstFocusClassNames.Items[k].ToString().ToLower())
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                lstFocusClassNames.Items.Add(str);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Target1 = true;
                        
            CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);
            DoDragDrop(this, DragDropEffects.All);

            
        }

        private void frmMain_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Drop || e.Action == DragAction.Cancel)
            {
                CursorManager.DestroyCursor(CursorManager.TargetCursor);

                IntPtr hw = Win32Api.WindowFromPoint(Win32Api.GetCursorPosition());

                StringBuilder className = new StringBuilder(100);
                int nret = GetClassName(hw, className, className.Capacity);

                if (Target1)
                {
                    txtClassName.Text = className.ToString();
                }
                else
                {
                    txtClickClassName.Text = className.ToString();
                }                

                CursorManager.ClearCursor();                
            }
            else if (e.Action == DragAction.Continue)
            {
                CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);

                IntPtr hwnd = Win32Api.WindowFromPoint(Win32Api.GetCursorPosition());
                RECT r;

                Win32Api.GetWindowRect(hwnd, out r);
                Rectangle re = r.ToRectangle();
                Win32Api.DrawReversibleFrame(re);

            }
        }

        private void frmMain_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            string str = "";

            for (int k=0;k<lstClickClassNames.Items.Count;k++)
            {
                if (k>0)
                {
                    str += "|";
                }

                str += lstClickClassNames.Items[k].ToString();
            }

            Properties.Settings.Default.ClickClassNames = str;

            str = "";

            for (int k = 0; k < lstFocusClassNames.Items.Count; k++)
            {
                if (k > 0)
                {
                    str += "|";
                }

                str+=lstFocusClassNames.Items[k].ToString();
            }

            Properties.Settings.Default.FocusClassNames = str;
            
            SaveSizeLocation();

            Properties.Settings.Default.Save();
        }

        private bool Target1 = true;

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            Target1 = false;
                        
            CursorManager.ChangeSystemCursor(Properties.Resources.target_red_321);
            
            DoDragDrop(this, DragDropEffects.All);
                        
        }

        private void btnAddClick_Click(object sender, EventArgs e)
        {
            if (txtClickClassName.Text.Trim() == string.Empty)
            {
                Module.ShowMessage("Please enter a Class Name !");
                return;
            }

            string str = txtClickClassName.Text;

            bool found = false;

            for (int k = 0; k < lstClickClassNames.Items.Count; k++)
            {
                if (str.ToLower() == lstClickClassNames.Items[k].ToString().ToLower())
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                lstClickClassNames.Items.Add(str);
            }
        }

        private void btnRemoveClick_Click(object sender, EventArgs e)
        {
            while (lstClickClassNames.SelectedItems.Count > 0)
            {
                lstClickClassNames.Items.RemoveAt(lstClickClassNames.SelectedIndices[0]);
            }
        }

        private void btnResetClick_Click(object sender, EventArgs e)
        {
            lstClickClassNames.Items.Clear();

            Properties.Settings.Default.ClickClassNames = Properties.Settings.Default.DefaultClickClassNames;

            string[] sz = Properties.Settings.Default.ClickClassNames.Split(new string[] { "|" }, StringSplitOptions.None);

            for (int k = 0; k < sz.Length; k++)
            {
                lstClickClassNames.Items.Add(sz[k]);
            }
        }

        #region Localization

        private void AddLanguageMenuItems()
        {
            for (int k = 0; k < frmLanguage.LangCodes.Count; k++)
            {
                ToolStripMenuItem ti = new ToolStripMenuItem();
                ti.Text = frmLanguage.LangDesc[k];
                ti.Tag = frmLanguage.LangCodes[k];
                ti.Image = frmLanguage.LangImg[k];

                if (Properties.Settings.Default.Language == frmLanguage.LangCodes[k])
                {
                    ti.Checked = true;
                }

                ti.Click += new EventHandler(tiLang_Click);

                if (k < 25)
                {
                    languages1ToolStripMenuItem.DropDownItems.Add(ti);
                }
                else
                {
                    languages2ToolStripMenuItem.DropDownItems.Add(ti);
                }

                //languageToolStripMenuItem.DropDownItems.Add(ti);
            }
        }

        void tiLang_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = (ToolStripMenuItem)sender;
            string langcode = ti.Tag.ToString();
            ChangeLanguage(langcode);

            //for (int k = 0; k < languageToolStripMenuItem.DropDownItems.Count; k++)
            for (int k = 0; k < languages1ToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languages1ToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }

            for (int k = 0; k < languages2ToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languages2ToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }
        }

        private bool InChangeLanguage = false;

        private void ChangeLanguage(string language_code)
        {
            try
            {
                InChangeLanguage = true;

                Properties.Settings.Default.Language = language_code;
                frmLanguage.SetLanguage();

                Properties.Settings.Default.Save();

                Module.ShowMessage("Please restart the application !");
                Application.Exit();

                return;

                bool maximized = (this.WindowState == FormWindowState.Maximized);
                this.WindowState = FormWindowState.Normal;

                /*
                RegistryKey key = Registry.CurrentUser;
                RegistryKey key2 = Registry.CurrentUser;

                try
                {
                    key = key.OpenSubKey("Software\\4dots Software", true);

                    if (key == null)
                    {
                        key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\4dots Software");
                    }

                    key2 = key.OpenSubKey(frmLanguage.RegKeyName, true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey(frmLanguage.RegKeyName);
                    }

                    key = key2;

                    //key.SetValue("Language", language_code);
                    key.SetValue("Menu Item Caption", TranslateHelper.Translate("Change PDF Properties"));
                }
                catch (Exception ex)
                {
                    Module.ShowError(ex);
                    return;
                }
                finally
                {
                    key.Close();
                    key2.Close();
                }
                */
                //1SaveSizeLocation();

                //3SavePositionSize();

                this.Controls.Clear();

                InitializeComponent();

                //3SetupOnLoad();

                if (maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }

                this.ResumeLayout(true);
            }
            finally
            {
                InChangeLanguage = false;
            }
        }

        #endregion
        private void AdjustSizeLocation()
        {
            if (Properties.Settings.Default.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {

                if (Properties.Settings.Default.Width == -1)
                {
                    this.CenterToScreen();
                    return;
                }
                else
                {
                    this.Width = Properties.Settings.Default.Width;
                }
                if (Properties.Settings.Default.Height != 0)
                {
                    this.Height = Properties.Settings.Default.Height;
                }

                if (Properties.Settings.Default.Left != -1)
                {
                    this.Left = Properties.Settings.Default.Left;
                }

                if (Properties.Settings.Default.Top != -1)
                {
                    this.Top = Properties.Settings.Default.Top;
                }

                if (this.Width < 300)
                {
                    this.Width = 300;
                }

                if (this.Height < 300)
                {
                    this.Height = 300;
                }

                if (this.Left < 0)
                {
                    this.Left = 0;
                }

                if (this.Top < 0)
                {
                    this.Top = 0;
                }
            }

        }

        private void SaveSizeLocation()
        {
            if (this.WindowState == FormWindowState.Minimized) return;

            Properties.Settings.Default.Maximized = (this.WindowState == FormWindowState.Maximized);
            Properties.Settings.Default.Save();

            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Save();
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            
        }        
    }
}
