/// SE encarga de establecer el vinculo entre las VISTA y los View Model
namespace ForeignExhange.Infrastructure
{

    using ViewModels;

    public class InstanceLocator
    {

        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
