using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLua;

namespace InfiniEditor
{
    public partial class FormScripting : Form
    {
        public FormScripting()
        {
            InitializeComponent();
        }

        private void ScriptingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            BlocksCollection ClipboardBlocks = ((FormMain)Owner).ClipboardBlocks;
            BlocksCollection PuzzleBlocks = ((FormMain)Owner).OpenedLevel == null ? null : ((FormMain)Owner).OpenedLevel.Blocks;
            string res = "";
            try
            {
                Lua state = new Lua();
                state.LoadCLRPackage();
                state.DoString(@" import ('InfiniEditor', 'InfiniEditor')");
                state.DoString(@"import = function () end");
                state["Clipboard"] = ClipboardBlocks;
                state["Puzzle"] = PuzzleBlocks;
                state.DoString(@"enumDict = function(dict)
                                               local e = dict:GetEnumerator()
                                               return function()
                                                  if e:MoveNext() then
                                                    return e.Current.Key, e.Current.Value
                                                    end
                                               end
                                             end");
                BlockInfosManager bim = new BlockInfosManager();
                state.RegisterFunction("BlockInfo", bim, typeof(BlockInfosManager).GetMethod("BlockInfo"));
                state.RegisterFunction("DecalInfo", bim, typeof(BlockInfosManager).GetMethod("DecalInfo"));
                state["BIM"] = bim;
                state.DoString(@"dictToTable = function (dict) 
                                  table = {}
                                  for k, v in enumDict(dict) do
                                    table[k]=v
                                  end                                  
                                  return table
                                end");
                state.DoString(@"BlockInfos = function(cond) return dictToTable(BIM:BlockInfos(cond)) end");
                state.DoString(@"DecalInfos = function(cond) return dictToTable(BIM:DecalInfos(cond)) end");
                state["World"] = Block.Roles.World;
                state["Input"] = Block.Roles.In;
                state["Output"] = Block.Roles.Out;
                state["NegX"] = Direction.NegX;
                state["PosX"] = Direction.PosX;
                state["NegZ"] = Direction.NegZ;
                state["PosZ"] = Direction.PosZ;
                state["PosY"] = Direction.PosY;
                state["NegY"] = Direction.NegY;
                state.DoString(@"Directions = {PosX, NegX, PosY, NegY, PosZ, NegZ}");
                state.DoString(@"BlockDirections = {PosX, NegX, PosZ, NegZ}");
                state.DoString(@"Blocks = function(x) return enumDict(x.blocksDict) end");
                state.DoString(@"Decals = function(x) return enumDict(x.Decals) end");
                state.RegisterFunction("print", this, typeof(FormScripting).GetMethod("LogConsole"));
                state.DoString(richTextBoxCode.Text);
                ClipboardBlocks.ToClipboard();
                res = "Code ran successfully.";
                ((FormMain)Owner).SaveButtonUpdate(true);
                ((FormMain)Owner).UpdateBlockCount();

            }
            catch (NLua.Exceptions.LuaScriptException ex)
            {
                res = "There was an error executing the code. " + ex.Message.Replace("[string \"chunk\"]:", "line ");
            }
            ((FormMain)Owner).Status(res);
            LogConsole(res);
        }

        public static IEnumerable<string> TableToEnumerable(LuaTable table)
        {
            foreach(var v in table.Values)
            {
                yield return (string)v;
            }
        }
        public static string[] TableToArray(LuaTable table)
        {
            return TableToEnumerable(table).ToArray();
        }

        public void LogConsole(params Object[] o)
        {
            if (richTextBoxConsole.Text != "")
            {
                richTextBoxConsole.AppendText("\n");
            }
            string text = String.Join(" ", o.Select(i => i == null ? "null" : i.ToString()));
            richTextBoxConsole.AppendText(text);
        }

        private void buttonRunAndExit_Click(object sender, EventArgs e)
        {
            Run();
            Hide();
        }

        private void buttonShoTutorial_Click(object sender, EventArgs e)
        {
            richTextBoxCode.Text = Properties.Resources.scripting_lua;
        }
    }
}
