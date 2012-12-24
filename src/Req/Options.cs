//----------------------------------------------------------------------------------
// Command line options
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace Req
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    class Options : CommandLineOptionsBase
    {
        [OptionList("c", "connectEndPoints", Required = true, Separator = ';', HelpText = "List of end points to connect seperated by ';'")]
        public IList<string> connectEndPoints { get; set; }

        [OptionList("m", "alterMessages", Required = true, Separator = ';', HelpText = "List of alternavive messages to send seperated by ';'. It may contains macros: #nb# = number of the msg")]
        public IList<string> alterMessages { get; set; }

        [Option("x", "maxNbMessages", Required = false, HelpText = "Max nb message to send. Default -1 (unlimitted)")]
        public long maxMessage { get; set; }  

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int delay { get; set; }
     
        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "Req Client",              
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            this.HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine("Usage: Req.exe -c <connect endpoint list> -m <msgs to send> [-x <max nb msg>] [-d <time delay>]"); 
            help.AddOptions(this);

            return help;
        }

        private void HandleParsingErrorsInHelp(HelpText help)
        {
            if (this.LastPostParsingState.Errors.Count > 0)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }
        }
     
        public Options()
        {
            maxMessage = -1;
        }
    }
}
