using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Ionic.Zip;
using Microsoft.Win32;

/*
 * Class implements the partial window form for the Lords of Xulima saved game
 * editor.  Will load the saved game XML into a text box to edit and provides
 * a search function to find specific XML elements.  Will unpack and pack the game
 * files when opening and saving changes, with password as needed.
 * Eventually, will have character edit dialogs to provide a clean interface for
 * editing common character fields (stats, skills, etc.) and party fields (mainly
 * gold).  May include item editing, but that may be a stretch.
 * 
 * Author: Michael G. Slack
 * Date Written: 2022-03-19
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: 2022-03-24 - Reset search position when loading a new save file.
 *          2022-03-25 - Minor tweaks, allow for enter from search box and
 *                       prompt on save if no changes.
 *          2022-03-31 - Added simple character edit dialog to view/edit
 *                       the saved characters through instead of via the raw
 *                       XML.
 * 
 */
namespace LoX_Editor
{
    public partial class MainWin : Form
    {
        #region Private Consts
        private const string HTML_HELP_FILE = "LoX_Editor_help.html";
        private const string SG_ZIP_PW = "ADF788ASDF78FAE340DFEE32668384901290";
        private const string NO_SAVE_OPEN = "<>";
        private const string INFO_EXT = ".jxsavinfo";
        private const string ZIP_EXT = ".zip";

        private const string REG_NAME = @"HKEY_CURRENT_USER\Software\Slack and Associates\Tools\LoX_Editor";
        private const string REG_KEY1 = "PosX";
        private const string REG_KEY2 = "PosY";
        private const string REG_KEY3 = "WinWidth";
        private const string REG_KEY4 = "WinHeight";
        private const string REG_KEY5 = "LastDirectory";
        #endregion

        #region Private Variables
        private readonly string tempDir = System.IO.Path.GetTempPath();
        private string curSaveFn = NO_SAVE_OPEN;
        private string curSaveUnpFn = "";
        private string curSaveZipFn = "";
        private string curInfoFn = NO_SAVE_OPEN;
        private string curInfoUnpFn = "";
        private string curInfoZipFn = "";
        private bool saveChanged = false;
        private int searchPos = 0;
        #endregion

        // --------------------------------------------------------------------

        #region Unzip/Zip Methods
        private string Unzip(string zipFn, bool usePW)
        {
            byte[] buffer = new byte[2048];
            int n;
            string ret = "";
            using (var raw = File.Open(zipFn, FileMode.Open, FileAccess.Read))
            {
                using (var input = new ZipInputStream(raw))
                {
                    if (usePW) input.Password = SG_ZIP_PW;
                    ZipEntry e;
                    while ((e = input.GetNextEntry()) != null)
                    {
                        if (e.IsDirectory) continue;
                        ret = Path.Combine(tempDir, e.FileName);
                        using (var output = File.Open(ret, FileMode.Create, FileAccess.ReadWrite))
                        {
                            while ((n = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                output.Write(buffer, 0, n);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private bool Zip(string zipFn, string fileToZip, bool usePW)
        {
            bool ret = true;
            try
            {
                using (ZipFile zip = new ZipFile(zipFn))
                {
                    if (usePW) zip.Password = SG_ZIP_PW;
                    ZipEntry ze = zip.AddFile(fileToZip, "");
                    zip.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error saving game file (zip) - " + ex.Message, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = false;
            }
            return ret;
        }
        #endregion

        // --------------------------------------------------------------------

        #region Private Methods
        private void LoadRegistryValues()
        {
            int winX = -1, winY = -1, wWid = -1, wHgt = -1;
            string lastDirectory = "";

            try
            {
                winX = (int)Registry.GetValue(REG_NAME, REG_KEY1, winX);
                winY = (int)Registry.GetValue(REG_NAME, REG_KEY2, winY);
                wWid = (int)Registry.GetValue(REG_NAME, REG_KEY3, wWid);
                wHgt = (int)Registry.GetValue(REG_NAME, REG_KEY4, wHgt);
                lastDirectory = (string)Registry.GetValue(REG_NAME, REG_KEY5, "");
            }
            catch (Exception) { /* ignore, go with defaults, but could use MessageBox.Show(e.Message); */ }

            // set window/form values read from registry
            if (winX != -1 && winY != -1) this.SetDesktopLocation(winX, winY);
            if (wWid != -1 && wHgt != -1) this.Size = new System.Drawing.Size(wWid, wHgt);
            // set last directory used read from registry
            if (lastDirectory != "") openDlg.InitialDirectory = lastDirectory;
        }

        private string GetSaveFileName()
        {
            curInfoUnpFn = Unzip(curInfoFn, false);

            string[] lines = File.ReadAllLines(curInfoUnpFn);
            int idx = lines[3].LastIndexOf('"');
            string ret = "[ " + lines[3].Substring(31, idx-31) + " ]: " + curSaveFn;

            return ret;
        }

        private void OpenSaveFile()
        {
            try
            {
                curInfoFn = Path.ChangeExtension(curSaveFn, INFO_EXT);
                lblSaveFn.Text = GetSaveFileName();
                curSaveUnpFn = Unzip(curSaveFn, true);
                tbGameXML.Text = File.ReadAllText(curSaveUnpFn);
                saveChanged = false;
                searchPos = 0;
                tsbSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not open LoX save file - " + ex.Message, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteTempFiles()
        {
            if (curInfoUnpFn != "")
            {
                try { File.Delete(curInfoUnpFn); } catch { }
            }
            if (curSaveUnpFn != "")
            {
                try { File.Delete(curSaveUnpFn); } catch { }
            }
            if (curInfoZipFn != "")
            {
                try { File.Delete(curInfoZipFn); } catch { }
            }
            if (curSaveZipFn != "")
            {
                try { File.Delete(curSaveZipFn); } catch { }
            }
        }

        private bool UpdateAndSaveInfo()
        {
            bool ret;
            StringCollection sc = new StringCollection();
            curInfoZipFn = Path.Combine(tempDir + Path.GetFileNameWithoutExtension(curInfoFn)) + "_I" + ZIP_EXT;

            try
            {
                string[] lines = File.ReadAllLines(curInfoUnpFn);

                foreach (string line in lines)
                {
                    if (!line.Contains("Simple name=\"Code\"")) sc.Add(line);
                }
                string[] newLines = new string[sc.Count];
                sc.CopyTo(newLines, 0);
                File.WriteAllLines(curInfoUnpFn, newLines);

                ret = Zip(curInfoZipFn, curInfoUnpFn, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error saving game file (info) - " + ex.Message, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = false;
            }

            return ret;
        }

        private bool SaveEditedText()
        {
            bool ret = true;

            try
            {
                File.WriteAllText(curSaveUnpFn, tbGameXML.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error saving game file (text) - " + ex.Message, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = false;
            }

            return ret;
        }

        private bool CopyFiles()
        {
            bool ret = true;
            try
            {
                File.Copy(curSaveZipFn, curSaveFn, true);
                File.Copy(curInfoZipFn, curInfoFn, true);
                saveChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error copying save game to LoX save folder - " + ex.Message,
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ret = false;
            }
            return ret;
        }

        private void SaveSaveFile()
        {
            if (UpdateAndSaveInfo())
            {
                if (SaveEditedText())
                {
                    curSaveZipFn = Path.Combine(tempDir + Path.GetFileNameWithoutExtension(curSaveFn)) + ZIP_EXT;
                    if (Zip(curSaveZipFn, curSaveUnpFn, true))
                    {
                        if (CopyFiles())
                        {
                            System.Media.SystemSounds.Beep.Play();
                            MessageBox.Show(this, "Game edits saved successfully.", this.Text,
                                MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                }
            }
        }
        #endregion

        // --------------------------------------------------------------------

        public MainWin()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------

        #region Form Events
        private void MainWin_Load(object sender, EventArgs e)
        {
            LoadRegistryValues();
        }

        private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saveChanged)
            {
                DialogResult res = MessageBox.Show("Do you need to save open LoX game file?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                e.Cancel = res == DialogResult.Yes;
            }
            if (!e.Cancel && this.WindowState == FormWindowState.Normal)
            {
                Registry.SetValue(REG_NAME, REG_KEY1, this.Location.X);
                Registry.SetValue(REG_NAME, REG_KEY2, this.Location.Y);
                Registry.SetValue(REG_NAME, REG_KEY3, this.Size.Width);
                Registry.SetValue(REG_NAME, REG_KEY4, this.Size.Height);
            }
        }

        private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteTempFiles();
        }

        private void TbGameXML_TextChanged(object sender, EventArgs e)
        {
            saveChanged = true;
        }
        #endregion

        // --------------------------------------------------------------------

        #region ToolStrip Events
        private void TsbOpen_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;

            if (saveChanged)
            {
                res = MessageBox.Show(this, "Save game file already opened and changed?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (res == DialogResult.No && openDlg.ShowDialog(this) == DialogResult.OK)
            {
                Registry.SetValue(REG_NAME, REG_KEY5, Path.GetFullPath(openDlg.FileName));
                DeleteTempFiles();
                curSaveFn = openDlg.FileName;
                OpenSaveFile();
            }
        }

        private void TsbSave_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.Yes;
            if (!saveChanged)
                res = MessageBox.Show(this, "You haven't modified the file, are you sure you want to save?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes) SaveSaveFile();
        }

        private void TsbAbout_Click(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();
            _ = dlg.ShowDialog(this);
        }

        private void TsbHelp_Click(object sender, EventArgs e)
        {
            var asm = Assembly.GetEntryAssembly();
            var asmLocation = Path.GetDirectoryName(asm.Location);
            var htmlPath = Path.Combine(asmLocation, HTML_HELP_FILE);

            try
            {
                Process.Start(htmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot load help: " + ex.Message, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TsbExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TsbSearch_Click(object sender, EventArgs e)
        {
            string searchFor = tstbSearch.Text;

            if (searchPos == -1) searchPos = 0;
            if (searchPos > 0) searchPos++;
            if (searchFor != "")
            {
                searchPos = tbGameXML.Text.IndexOf(searchFor, searchPos, StringComparison.OrdinalIgnoreCase);
                if (searchPos != -1)
                {
                    tbGameXML.SelectionStart = searchPos;
                    tbGameXML.SelectionLength = searchFor.Length;
                    tbGameXML.ScrollToCaret();
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show(this, "No more " + searchFor + " found!", this.Text,
                        MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
        }

        private void TstbSearch_TextChanged(object sender, EventArgs e)
        {
            searchPos = 0; // reset search position
        }

        private void TstbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                TsbSearch_Click(this, new EventArgs());
                _ = tbGameXML.Focus();
            }
        }

        private void TsbEditDlg_Click(object sender, EventArgs e)
        {
            EditCharDlg dlg = new EditCharDlg();

            dlg.SaveGameXML = tbGameXML.Text;
            DialogResult res = dlg.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                tbGameXML.Text = dlg.SaveGameXML;
                searchPos = 0;
            }
            dlg.Dispose();
        }
        #endregion
    }
}
