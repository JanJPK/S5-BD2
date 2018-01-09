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

            // Event handling.
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            // Database context.
            builder.RegisterType<WarlordDbContext>().AsSelf();
            
            // Main window.
            builder.RegisterType<MainVM>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();            

            // Services.
            builder.RegisterType<ManufacturerRepository>().As<IManufacturerRepository>();
            builder.RegisterType<VehicleModelRepository>().As<IVehicleModelRepository>();
            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>();
            builder.RegisterType<OrderRepository>().As <IOrderRepository>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<LookupService>().AsImplementedInterfaces();
            builder.RegisterType<MessageService>().As<IMessageService>();

            // View models.
            builder.RegisterType<ManufacturerDetailVM>().Keyed<IDetailVM>(nameof(ManufacturerDetailVM));
            builder.RegisterType<ManufacturerBrowseVM>().Keyed<IDetailVM>(nameof(ManufacturerBrowseVM));
            builder.RegisterType<VehicleModelDetailVM>().Keyed<IDetailVM>(nameof(VehicleModelDetailVM));
            builder.RegisterType<VehicleModelBrowseVM>().Keyed<IDetailVM>(nameof(VehicleModelBrowseVM));
            builder.RegisterType<VehicleDetailVM>().Keyed<IDetailVM>(nameof(VehicleDetailVM));
            builder.RegisterType<VehicleBrowseVM>().Keyed<IDetailVM>(nameof(VehicleBrowseVM));
            builder.RegisterType<OrderDetailVM>().Keyed<IDetailVM>(nameof(OrderDetailVM));
            builder.RegisterType<OrderBrowseVM>().Keyed<IDetailVM>(nameof(OrderBrowseVM));
            builder.RegisterType<CustomerDetailVM>().Keyed<IDetailVM>(nameof(CustomerDetailVM));
            builder.RegisterType<CustomerBrowseVM>().Keyed<IDetailVM>(nameof(CustomerBrowseVM));

            return builder.Build();
        }
    }
}
