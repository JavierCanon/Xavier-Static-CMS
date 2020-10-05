using System.CommandLine;

namespace AcBlog.Tools.Sdk.Commands
{
    public class ListCommand : BaseCommand<ListCommand.CArgument>
    {
        public override string Name => "list";

        public override string Description => "List blog contents.";

        protected override bool DisableHandler => true;

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddCommand(new Lists.PostCommand().Build());
            return result;
        }

        public class CArgument
        {
        }
    }
}
