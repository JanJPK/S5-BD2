using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Prism.Events;
using Warlord.DataAccess;
using Warlord.UI.Service.Lookups;
using Warlord.UI.Service.Message;
using Warlord.UI.Service.Repositories;
using Warlord.UI.ViewModel;
using Warlord.UI.ViewModel.Detail;
using Warlord.UI.ViewModel.Detail.Browse;

namespace Warlord.UI.Startup
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
            builder.RegisterType<LookupService>().AsImplementedInterfaces();

            builder.RegisterType<VehicleModelDetailVM>().Keyed<IDetailVM>(nameof(VehicleModelDetailVM));
            builder.RegisterType<VehicleModelBrowseDetailVM>().Keyed<IDetailVM>(nameof(VehicleModelBrowseDetailVM));

            builder.RegisterType<MessageService>().As<IMessageService>();

            return builder.Build();
        }
    }
}
