using MoonSharp.Interpreter;
using rim_twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TwitchLib.Client.Models;
using Verse;

namespace TwitchToolkit
{
    public class Command : Def
    {

        public void RunCommand(ChatMessage message)
        {
            if (command == null)
            {
                throw new Exception("Command is null");
            }

            CommandDriver driver = (CommandDriver)Activator.CreateInstance(commandDriver);
            driver.command = this;
            driver.RunCommand(message);
        }

        public string Label
        {
            get
            {
                if (label != null && label != "")
                {
                    return label;
                }

                return defName;
            }
        }

        public string command = null;

        public bool enabled = true;

        public bool shouldBeInSeparateRoom = false;

        public Type commandDriver = typeof(CommandDriver);

        public bool requiresMod = false;

        public bool requiresAdmin = false;

        public string outputMessage = "";

        public bool isCustomMessage = false;
    }

    public class Functions
    {
        public Viewer GetViewer(string username)
        {
            return Viewers.GetViewer(username);
        }

        public string ReturnString()
        {
            return "Hello World!";
        }
    }

    public class CommandDriver
    {
        public Command command = null;

        public virtual void RunCommand(ChatMessage message)
        {
            Helper.Log("filtering command");

            string output = FilterTags(message, command.outputMessage);

            Helper.Log("command filtered");

            

            if (!UserData.IsTypeRegistered<Functions>())
            {
                UserData.RegisterType<Functions>();
                UserData.RegisterType<Viewer>();
            }
            
            Helper.Log("creating script");

            Script script = new Script();
            script.DebuggerEnabled = true;
            DynValue functions = UserData.Create(new Functions());
            script.Globals.Set("functions", functions);

            Helper.Log("Parsing Script " + output);

            DynValue res = script.DoString(output);
            MessageQueue.messageQueue.Enqueue(res.CastToString());

            Log.Message(res.CastToString());            
        }

        public string FilterTags(ChatMessage message, string input)
        {
            Helper.Log("starting filter");

            Viewer viewer = Viewers.GetViewer(message.Username);

            StringBuilder output = new StringBuilder(input);
            output.Replace("{username}", viewer.username);
            output.Replace("{balance}", viewer.GetViewerCoins().ToString());
            output.Replace("{karma}", viewer.GetViewerKarma().ToString());
            output.Replace("{purchaselist}", ToolkitSettings.CustomPricingSheetLink);
            output.Replace("{coin-reward}", ToolkitSettings.CoinAmount.ToString());

            output.Replace("\n", "");

            Helper.Log("starting regex");

            Regex regex = new Regex(@"\[(.*?)\]");

            MatchCollection matches = regex.Matches(output.ToString());

            foreach (Match match in matches)
            {
                Helper.Log("found match " + match.Value);
                string code = match.Value;
                code = code.Replace("[", "");
                code = code.Replace("]", "");

                //Regex doubleReg = new Regex("double\\<(.*?)\\>");

                //foreach (Match innerMatch in doubleReg.Matches(match.Value.ToString()))
                //{
                //    Helper.Log("found match " + innerMatch.Value);

                //    string innerCode = innerMatch.Value;
                //    innerCode = innerCode.Replace("double<", "");
                //    innerCode = innerCode.Replace(">", "");

                //    Helper.Log("executing double " + innerCode);

                //    output.Replace(innerMatch.Value, MoonSharpDouble(code).ToString());
                //}

                // Helper.Log("finished inner code");

                output.Replace(match.Value, MoonSharpString(code));
            }

            return output.ToString();
        }

        public string MoonSharpString(string function)
        {
            string script = @function;

            DynValue res = Script.RunString(script);
            return res.String;
        }

        public double MoonSharpDouble(string function)
        {
            string script = @function;

            DynValue res = Script.RunString(script);
            return res.Number;
        }

        static string TokenizeObjects()
        {


            return "";
        }
    }

    //public static class LUATools
    //{
    //    public static double Double(string code)
    //    {
    //        try
    //        {
    //            string script = code;

    //            DynValue res = Script.RunString(script);

    //            return res.Number;
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Error(e.Message);

    //            return 0d;
    //        }
    //    }
    //}

    //public class LUAActivator
    //{
    //    public static object CreateInstance(string type)
    //    {
    //        Type classType = Type.GetType(type);
    //        object classObject = (object)Activator.CreateInstance(classType);

    //        return classObject;
    //    }
    //}
}
