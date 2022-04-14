using System;
using System.IO;
using System.Text;
using System.Xml;

/*
 * Class used to read the XML and create the character records to edit via a dialog
 * for the LoX save game editor.  Also has routines to update the XML once done
 * editting.
 * 
 * Author: Michael G. Slack
 * Date Written: 2022-03-26
 * 
 * ----------------------------------------------------------------------------
 * 
 * Updated: 2022-04-13 - Added a static validate XML syntax method to use to
 *                       make sure the XML is well-formed before saving.
 * 
 */
namespace LoX_Editor
{
    #region LoXCharacter struct
    public struct LoXCharacter
    {
        public string name;
        public int[] resistances;  // dm, heat, org, mm, elec, cold
        public int[] attributes;   // str, con, spe, agi, pow (en)
        public int hpBonus;
        public int mpBonus;
        public int timesKilled;
        public int experience;
        public int devPoints;
    }
    #endregion

    class ProcessXML
    {
        #region Private/Public consts
        private const string FIRST_CHAR = "SOL.Characters.Gaulen";
        private const string OTHER_CHARS = "JX.JXCustomCharacter";
        private const string OTHER_NAME = "<Simple name=\"Name\" value=\"";
        private const string RESISTANCES = "<Dictionary name=\"ResistancesBase\"";
        private const string XML_QUOTE = "\"";
        private const string XML_VALUE = "value=\"";
        private const string HPBONUS = "<Simple name=\"HPBonus\" value=\"";
        private const string MPBONUS = "<Simple name=\"MPBonus\" value=\"";
        private const string TIMESKILLED = "<Simple name=\"Stats_TimesKilled\" value=\"";
        private const string EXPERIENCE = "<Simple name=\"Experience\" value=\"";
        private const string DEVPOINTS = "<Simple name=\"DevelopmentPoints\" value=\"";
        private const string XML_LINE_END = "\" />";

        public const int MAX_CHARS = 6;
        public const int MAX_RESISTANCES = 6;
        public const int MAX_ATTRIBUTES = 5;

        private readonly string[] RES_XXX = new string[MAX_RESISTANCES]
        {
            "SOL.Resistances.DivineMagic", "SOL.Resistances.Heat", "SOL.Resistances.Organic",
            "SOL.Resistances.MentalMagic", "SOL.Resistances.Electricity", "SOL.Resistances.Cold"
        };
        private readonly string[] ATTR_XXX = new string[MAX_ATTRIBUTES]
        {
            "Attribute_STR", "Attribute_CON", "Attribute_SPE", "Attribute_AGI", "Attribute_POW"
        };
        #endregion

        // --------------------------------------------------------------------

        #region Private structs
        private struct LineAndPos
        {
            public int lineNum;
            public int linePos;

            public LineAndPos(int line, int pos)
            {
                lineNum = line; linePos = pos;
            }
        }

        private struct LoXCharScratchPad
        {
            public LineAndPos name;
            public LineAndPos[] resistances;
            public LineAndPos[] attributes;
            public LineAndPos hpBonus;
            public LineAndPos mpBonus;
            public LineAndPos timesKilled;
            public LineAndPos experience;
            public LineAndPos devPoints;
        }
        #endregion

        // --------------------------------------------------------------------

        #region Private variables
        private bool charsLoaded = false;
        private string[] xmlLines;
        LoXCharacter[] chars = new LoXCharacter[MAX_CHARS];
        private LoXCharScratchPad[] scratchPad = new LoXCharScratchPad[MAX_CHARS];
        private int curSearchLine = -1;
        private int curCharLoading = 0;
        #endregion

        // --------------------------------------------------------------------

        #region Properties
        private string _saveGameXML = "";
        public string SaveGameXML { get => _saveGameXML; set => _saveGameXML = value; }
        #endregion

        // --------------------------------------------------------------------

        #region Private methods
        private string[] ConvertToLines(string text)
        {
            return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        private string ConvertToText(string[] lines)
        {
            return string.Join(Environment.NewLine, lines);
        }

        private bool SearchLinesFor(string searchStr)
        {
            bool found = false;

            while (!found && curSearchLine < xmlLines.Length)
            {
                string line = xmlLines[++curSearchLine];
                if (line.Contains(searchStr))
                {
                    found = true;
                }
            }

            return found;
        }

        private bool FindNextChar(string charType)
        {
            return SearchLinesFor(charType);
        }

        private bool FindResistances()
        {
            return SearchLinesFor(RESISTANCES);
        }

        private string GetValue(int idx)
        {
            return xmlLines[curSearchLine].Substring(idx, xmlLines[curSearchLine].LastIndexOf(XML_QUOTE) - idx);
        }

        private bool LoadResistances()
        {
            bool loaded = false;

            // loads divinemagic, heat, organic, mentalmagic, electric, cold
            for (int i = 0; i < MAX_RESISTANCES; i++)
            {
                loaded = SearchLinesFor(RES_XXX[i]);
                if (loaded)
                {
                    curSearchLine++; // value on next line
                    int idx = xmlLines[curSearchLine].IndexOf(XML_QUOTE) + 1;
                    chars[curCharLoading].resistances[i] = int.Parse(GetValue(idx));
                    scratchPad[curCharLoading].resistances[i] = new LineAndPos(curSearchLine, idx);
                }
                else
                {
                    break;
                }
            }

            return loaded;
        }

        private int GetValueIdx()
        {
            return xmlLines[curSearchLine].IndexOf(XML_VALUE) + 7;
        }

        private bool LoadAttributes()
        {
            bool loaded = false;

            // loads str, con, spe, agi, pow
            for (int i = 0; i < MAX_ATTRIBUTES; i++)
            {
                loaded = SearchLinesFor(ATTR_XXX[i]);
                if (loaded)
                {
                    curSearchLine += 2; // value 2 lines down
                    int idx = GetValueIdx();
                    chars[curCharLoading].attributes[i] = int.Parse(GetValue(idx));
                    scratchPad[curCharLoading].attributes[i] = new LineAndPos(curSearchLine, idx);
                }
            }

            return loaded;
        }

        private bool LoadOtherValues()
        {
            bool loaded = false;
            int idx;

            loaded = SearchLinesFor(HPBONUS);
            if (loaded)
            {
                idx = GetValueIdx();
                chars[curCharLoading].hpBonus = int.Parse(GetValue(idx));
                scratchPad[curCharLoading].hpBonus = new LineAndPos(curSearchLine, idx);
            }
            if (loaded) loaded = SearchLinesFor(MPBONUS);
            if (loaded)
            {
                idx = GetValueIdx();
                chars[curCharLoading].mpBonus = int.Parse(GetValue(idx));
                scratchPad[curCharLoading].mpBonus = new LineAndPos(curSearchLine, idx);
            }
            if (loaded) loaded = SearchLinesFor(TIMESKILLED);
            if (loaded)
            {
                idx = GetValueIdx();
                chars[curCharLoading].timesKilled = int.Parse(GetValue(idx));
                scratchPad[curCharLoading].timesKilled = new LineAndPos(curSearchLine, idx);
            }
            if (loaded) loaded = SearchLinesFor(EXPERIENCE);
            if (loaded)
            {
                idx = GetValueIdx();
                chars[curCharLoading].experience = int.Parse(GetValue(idx));
                scratchPad[curCharLoading].experience = new LineAndPos(curSearchLine, idx);
            }
            if (loaded) loaded = SearchLinesFor(DEVPOINTS);
            if (loaded)
            {
                idx = GetValueIdx();
                chars[curCharLoading].devPoints = int.Parse(GetValue(idx));
                scratchPad[curCharLoading].devPoints = new LineAndPos(curSearchLine, idx);
            }

            return loaded;
        }

        private void LoadGaulen()
        {
            if (FindNextChar(FIRST_CHAR))
            {
                // order is everything, find/load resis, load attrs, then other value (in order)
                if (FindResistances() && LoadResistances() && LoadAttributes() && LoadOtherValues())
                {
                    chars[curCharLoading].name = "Gaulen";
                    scratchPad[curCharLoading].name = new LineAndPos(-1, -1);
                    curCharLoading++;
                }
                else { charsLoaded = false; }
            }
            else { charsLoaded = false; }
        }

        private bool FindName()
        {
            bool found = SearchLinesFor(OTHER_NAME);

            if (found)
            {
                int idx = GetValueIdx();
                chars[curCharLoading].name = GetValue(idx);
                scratchPad[curCharLoading].name = new LineAndPos(curSearchLine, idx);
            }

            return found;
        }

        private void LoadOtherChars()
        {
            while (curCharLoading < MAX_CHARS && FindNextChar(OTHER_CHARS))
            {
                if (FindName())
                {
                    if (FindResistances() && LoadResistances() && LoadAttributes() && LoadOtherValues())
                    {
                        curCharLoading++;
                    }
                }
            }
            if (curCharLoading < MAX_CHARS) charsLoaded = false;
        }

        private void LoadTheChars()
        {
            if (_saveGameXML != "")
            {
                charsLoaded = true;
                LoadGaulen();
                if (charsLoaded) LoadOtherChars();
            }
        }

        private void SaveValue(string value, LineAndPos lp)
        {
            string line = xmlLines[lp.lineNum].Substring(0, lp.linePos);

            line += value + XML_LINE_END;
            xmlLines[lp.lineNum] = line;
        }

        private void SaveIntValue(int value, LineAndPos lp)
        {
            SaveValue("" + value, lp);
        }


        private void SaveResistances(int curChar, LoXCharacter[] eChars)
        {
            for (int i = 0; i < MAX_RESISTANCES; i++)
                SaveIntValue(eChars[curChar].resistances[i], scratchPad[curChar].resistances[i]);
        }

        private void SaveAttributes(int curChar, LoXCharacter[] eChars)
        {
            for (int i = 0; i < MAX_ATTRIBUTES; i++)
                SaveIntValue(eChars[curChar].attributes[i], scratchPad[curChar].attributes[i]);
        }
        #endregion

        // --------------------------------------------------------------------

        public ProcessXML() { }

        // --------------------------------------------------------------------

        #region Public methods
        public LoXCharacter[] LoadCharacters()
        {
            for (int i = 0; i < MAX_CHARS; i++)
            {
                chars[i].resistances = new int[MAX_RESISTANCES];
                chars[i].attributes = new int[MAX_ATTRIBUTES];
                scratchPad[i] = new LoXCharScratchPad();
                scratchPad[i].resistances = new LineAndPos[MAX_RESISTANCES];
                scratchPad[i].attributes = new LineAndPos[MAX_ATTRIBUTES];
            }

            xmlLines = ConvertToLines(_saveGameXML);
            LoadTheChars();

            return chars;
        }

        public void SaveCharacters(LoXCharacter[] eChars)
        {
            if (_saveGameXML != "" && charsLoaded)
            {
                for (int i = 0; i < MAX_CHARS; i++)
                {
                    if (scratchPad[i].name.lineNum != -1) SaveValue(eChars[i].name, scratchPad[i].name);
                    SaveResistances(i, eChars);
                    SaveAttributes(i, eChars);
                    SaveIntValue(eChars[i].hpBonus, scratchPad[i].hpBonus);
                    SaveIntValue(eChars[i].mpBonus, scratchPad[i].mpBonus);
                    SaveIntValue(eChars[i].timesKilled, scratchPad[i].timesKilled);
                    SaveIntValue(eChars[i].experience, scratchPad[i].experience);
                    SaveIntValue(eChars[i].devPoints, scratchPad[i].devPoints);
                }
                _saveGameXML = ConvertToText(xmlLines);
            }
        }

        public static string ValidateXMLSyntax(string gameXML)
        {
            string retMsg = "";

            // Based on code found on Stack Overflow
            //  from: https://stackoverflow.com/questions/8143732/stringstream-in-c-sharp
            //  - NH. Aug 18, 2017 at 15:19
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(gameXML)))
                {
                    var settings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Ignore,
                        XmlResolver = null
                    };

                    using (var reader = XmlReader.Create(new StreamReader(stream), settings))
                    {
                        var document = new XmlDocument();
                        document.Load(reader);
                    }
                }
            }
            catch (Exception exc)
            {
                retMsg = exc.Message;
            }

            return retMsg;
        }
        #endregion
    }
}
