//----------------------------------------------------------------------------------
// Command line options
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace SyncPub
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
        [Option("e", "pubEndpoint", Required = true, HelpText = "Publisher endpoint")]
        public string pubEndpoint { get; set; }       

        [Option("p", "repEndpoint", Required = true, HelpText = "REP endpoint")]
        public string repEndpoint { get; set; }

        [Option("n", "nbExpectedSubscribers", Required = true, HelpText = "Number of expected subcribers")]
        public uint nbExpectedSubscribers { get; set; }

        [OptionList("m", "AlterMessages", Required = true, Separator = ';', HelpText = "List of alternavive messages to send seperated by ';'. It may contains macros: #nb# = number of the msg")]
        public IList<string> altMessages { get; set; }  

        [Option("x", "MaxNbMessages", Required = false, HelpText = "Max nb message to send. Default -1 (unlimitted)")]
        public long maxMessage { get; set; }  

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int delay { get; set; }            

        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "SyncPub",               
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            this.HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine("Usage: SyncPub.exe -e <PUB endpoint list> -p <REP endpoint> -n <nb Subscribers> -m <msgs to send> [-x <max nb msg>] [-d <time delay>]");          
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
            delay = 0;
            maxMessage = -1;
        }
    }
}
