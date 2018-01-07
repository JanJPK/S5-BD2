using Autofac;
using Prism.Events;
using Warlord.DataAccess;
using Warlord.Service.Lookups;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.ViewModel;
using Warlord.ViewModel.Detail;
using Warlord.ViewModel.Detail.Browse;

namespace Warlord.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<WarlordDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainVM>().AsSelf();

            builder.RegisterType<VehicleModelRepository>().As<IVehicleModelRepository>();
            builder.RegisterType<LookupService>().As<IVehicleModelLookupService>();

            builder.RegisterType<VehicleModelDetailVM>().Keyed<IDetailVM>(nameof(VehicleModelDetailVM));
            builder.RegisterType<VehicleModelBrowseDetailVM>().Keyed<IDetailVM>(nameof(VehicleModelBrowseDetailVM));

            builder.RegisterType<MessageService>().As<IMessageService>();

            return builder.Build();
        }
    }
}
