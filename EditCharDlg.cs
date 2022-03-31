using System;
using System.Windows.Forms;

/*
 * Partial class representing the LoX Editor character edit dialog.  Can use this dialog
 * to edit some fields of the character rather than editting the raw XML.
 * 
 * Author: Michael G. Slack
 * Date Written: 2022-03-29
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: yyyy-mm-dd - xxxx.
 * 
 */
namespace LoX_Editor
{
    public partial class EditCharDlg : Form
    {
        #region Private statics/variables
        private static int FIRST_CHAR = 0;
        private static int LAST_CHAR = ProcessXML.MAX_CHARS - 1;

        private ProcessXML procXML = new ProcessXML();
        private LoXCharacter[] chars;
        private int curChar = FIRST_CHAR;
        #endregion

        // --------------------------------------------------------------------

        #region Properties
        private string _saveGameXML = "";
        public string SaveGameXML { get => _saveGameXML; set => _saveGameXML = value; }
        #endregion

        // --------------------------------------------------------------------

        #region Private methods
        private void LoadDialog()
        {
            tbName.Text = chars[curChar].name;
            if (curChar == FIRST_CHAR)
                tbName.ReadOnly = true;
            else
                tbName.ReadOnly = false;
            // resistances 
            nudDM.Value = chars[curChar].resistances[0];
            nudHeat.Value = chars[curChar].resistances[1];
            nudOrg.Value = chars[curChar].resistances[2];
            nudMM.Value = chars[curChar].resistances[3];
            nudElec.Value = chars[curChar].resistances[4];
            nudCold.Value = chars[curChar].resistances[5];
            // attributes
            nudStr.Value = chars[curChar].attributes[0];
            nudCon.Value = chars[curChar].attributes[1];
            nudSpe.Value = chars[curChar].attributes[2];
            nudAgi.Value = chars[curChar].attributes[3];
            nudEnd.Value = chars[curChar].attributes[4];
            // other values
            nudHPB.Value = chars[curChar].hpBonus;
            nudMPB.Value = chars[curChar].mpBonus;
            nudKilled.Value = chars[curChar].timesKilled;
            nudExp.Value = chars[curChar].experience;
            nudDevP.Value = chars[curChar].devPoints;
        }

        private void SaveDialog()
        {
            if (curChar != FIRST_CHAR) chars[curChar].name = tbName.Text;
            // resistances
            chars[curChar].resistances[0] = (int)nudDM.Value;
            chars[curChar].resistances[1] = (int)nudHeat.Value;
            chars[curChar].resistances[2] = (int)nudOrg.Value;
            chars[curChar].resistances[3] = (int)nudMM.Value;
            chars[curChar].resistances[4] = (int)nudElec.Value;
            chars[curChar].resistances[5] = (int)nudCold.Value;
            // attributes
            chars[curChar].attributes[0] = (int)nudStr.Value;
            chars[curChar].attributes[1] = (int)nudCon.Value;
            chars[curChar].attributes[2] = (int)nudSpe.Value;
            chars[curChar].attributes[3] = (int)nudAgi.Value;
            chars[curChar].attributes[4] = (int)nudEnd.Value;
            // other values
            chars[curChar].hpBonus = (int)nudHPB.Value;
            chars[curChar].mpBonus = (int)nudMPB.Value;
            chars[curChar].timesKilled = (int)nudKilled.Value;
            chars[curChar].experience = (int)nudExp.Value;
            chars[curChar].devPoints = (int)nudDevP.Value;
        }
        #endregion

        // --------------------------------------------------------------------

        public EditCharDlg()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------

        #region Form events
        private void EditCharDlg_Load(object sender, EventArgs e)
        {
            procXML.SaveGameXML = _saveGameXML;
            chars = procXML.LoadCharacters();
            LoadDialog();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveDialog();
            procXML.SaveCharacters(chars);
            _saveGameXML = procXML.SaveGameXML;
            DialogResult = DialogResult.OK;
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            SaveDialog();
            curChar++;
            if (curChar > LAST_CHAR) curChar = FIRST_CHAR;
            LoadDialog();
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            SaveDialog();
            curChar--;
            if (curChar < FIRST_CHAR) curChar = LAST_CHAR;
            LoadDialog();
        }
        #endregion
    }
}
