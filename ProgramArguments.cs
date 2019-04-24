using System;
using System.Linq;

namespace AsyncDemo
{
    public class ProgramArguments
    {
        public bool IsHelp { get; }
        public bool IsAsync { get; }
        public bool IsSync { get; }
        public UInt16 Iterations { get; }
        public bool ArgsValid { get; }
        public string ErrorMessage { get; }

        public ProgramArguments(string[] args)
        {
            ArgsValid = true;

            IsHelp = args.Contains("-help");
            IsAsync = args.Contains("-a");
            IsSync = args.Contains("-s");

            if (IsAsync && IsSync)
            {
                ArgsValid = false;
                ErrorMessage = "Argument '-s' or 'a' but not both can be used. Run with -help for more information.";
            }
            else
            {
                UInt16 iterationArgument = 1;
                try
                {
                    if (Array.IndexOf(args, "-i") > -1)
                    {
                        UInt16.TryParse(args[Array.IndexOf(args, "-i") + 1], out iterationArgument);
                    }
                }
                catch (Exception ex)
                {
                    ArgsValid = false;
                    ErrorMessage = "Error parsing -i argument value..." + Environment.NewLine;
                    ErrorMessage += ex.Message;
                }
                Iterations = iterationArgument;
            }
        }
    }
}
