using DryIoc;

namespace md_dbdocs.core.StartUp
{
    public class Bootstrapper
    {
        public IContainer BootStrap()
        {
            var container = new Container();

            return container;
        }
    }
}
