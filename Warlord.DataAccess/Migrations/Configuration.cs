using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Warlord.Model;

namespace Warlord.DataAccess.Migrations
{
    public class Configuration : DbMigrationsConfiguration<WarlordDbContext>
    {
        #region Constructors and Destructors

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        #endregion

        #region Methods

        protected override void Seed(WarlordDbContext context)
        {
            context.Manufacturers.AddOrUpdate(
                m => m.ShortName,
                new Manufacturer
                {
                    ShortName = "UVZ",
                    FullName = "Uralvagonzavod",
                    Country = "Russia"
                },
                new Manufacturer
                {
                    ShortName = "KhPZ",
                    FullName = "Kharkiv Locomotive Factory",
                    Country = "Russia"
                },
                new Manufacturer
                {
                    ShortName = "KMZ",
                    FullName = "Kurganmashzavod",
                    Country = "Russia"
                },
                new Manufacturer
                {
                    ShortName = "FIMMF",
                    FullName = "First Inner Mongolia Machinery Factory",
                    Country = "China"
                },
                new Manufacturer
                {
                    ShortName = "ROF",
                    FullName = "Royal Ordnance Factory",
                    Country = "United Kingdom"
                }
            );

            context.SaveChanges();

            context.VehicleModels.AddOrUpdate(
                vm => vm.Name,
                new VehicleModel
                {
                    Name = "T-72",
                    MainArmament = "125mm 2A46",
                    SecondaryArmament = "7.62 PKT, 12.7mm NSVT",
                    Crew = 3,
                    Weight = 45.5f,
                    Engine = "V-92S2f, V-12, diesel",
                    EnginePower = 700,
                    ManufacturerId = context.Manufacturers.Single(m => m.ShortName == "UVZ").Id
                },
                new VehicleModel
                {
                    Name = "T-54",
                    MainArmament = "100mm D-10T",
                    SecondaryArmament = "7.62 PKT, 12.7mm NSVT",
                    Crew = 4,
                    Weight = 39.7f,
                    Engine = "V-55, V-12, diesel",
                    EnginePower = 500,
                    ManufacturerId = context.Manufacturers.Single(m => m.ShortName == "KhPZ").Id
                },
                new VehicleModel
                {
                    Name = "BMP-2",
                    MainArmament = "30mm 2A42",
                    SecondaryArmament = "9M113 Konkurs",
                    Crew = 3,
                    Weight = 15.8f,
                    Engine = "UTD-20, V-6, diesel",
                    EnginePower = 300,
                    ManufacturerId = context.Manufacturers.Single(m => m.ShortName == "KMZ").Id
                },
                new VehicleModel
                {
                    Name = "T-64",
                    MainArmament = "125mm 2A46",
                    SecondaryArmament = "7.62 PKMT, 12.7 NSVT",
                    Crew = 3,
                    Weight = 42.0f,
                    Engine = "5DTF, opposed piston, multi-fuel",
                    EnginePower = 700,
                    ManufacturerId = context.Manufacturers.Single(m => m.ShortName == "KhPZ").Id
                },
                new VehicleModel
                {
                    Name = "Type 59",
                    MainArmament = "100mm Type 59",
                    SecondaryArmament = "7.62 Type 59T",
                    Crew = 4,
                    Weight = 40.0f,
                    Engine = "12150L, V-12, diesel",
                    EnginePower = 520,
                    ManufacturerId = context.Manufacturers.Single(m => m.ShortName == "FIMMF").Id
                },
                new VehicleModel
                {
                    Name = "Challenger 1",
                    MainArmament = "120mm L11A5",
                    SecondaryArmament = "7.62 L8A2, 7.62 L37A2",
                    Crew = 4,
                    Weight = 62.0f,
                    Engine = "CV12, V-12, diesel",
                    EnginePower = 1200,
                    ManufacturerId = context.Manufacturers.Single(m => m.ShortName == "ROF").Id
                });

            context.SaveChanges();

            context.Vehicles.AddOrUpdate(
                v => v.Id,
                new Vehicle
                {
                    Id = 1,
                    VehicleModelId = 1,
                    Color ="Green with black blob camo pattern.",
                    Condition="Perfect condition; fully renovated and displayed in museum.",
                    DateOfManufacture = new DateTime(1980, 11, 20),
                    Price = 100000
                },
                new Vehicle
                {
                    Id = 2,
                    VehicleModelId = 1,
                    Color = "Plain tan paint.",
                    Condition = "Bent mudguards, missing splash screen and rear storage box.",
                    DateOfManufacture = new DateTime(1991, 5, 15),
                    Price = 85000
                },
                new Vehicle
                {
                    Id = 3,
                    VehicleModelId = 1,
                    Color = "Plain sand paint.",
                    Condition = "Mudguards and side skirts either damaged or missing; most storage boxes completely destroyed. Diving snorkel instead of left storage box.",
                    DateOfManufacture = new DateTime(1989, 1, 25),
                    Price = 70000
                },
                new Vehicle
                {
                    Id = 4,
                    VehicleModelId = 5,
                    Color = "Tan, light and dark green stripe camouflage.",
                    Condition = "Normal condition; used by a collector.",
                    DateOfManufacture = new DateTime(1967, 6, 20),
                    Price = 50000
                },
                new Vehicle
                {
                    Id = 5,
                    VehicleModelId = 2,
                    Color = "Plain olivgrun paint with DDR markings.",
                    Condition = "Good condition; never used in combat. Used to be a part of private collection for many years before we acquired it.",
                    DateOfManufacture = new DateTime(1970, 2, 10),
                    Price = 58000
                },
                new Vehicle
                {
                    Id = 6,
                    VehicleModelId = 2,
                    Color = "Plain white.",
                    Condition = "Good condition; lacks secondary armament. Israeli Tiran-4 modification package.",
                    DateOfManufacture = new DateTime(1965, 12, 12),
                    Price = 50000
                });

            // Block which allows to catch and display exceptions when running "Update-Database" command through Package Manager Console.
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbValEx)
            {
                var outputLines = new StringBuilder();
                foreach (var eve in dbValEx.EntityValidationErrors)
                {
                    outputLines.AppendFormat("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:"
                        , DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.AppendFormat("- Property: \"{0}\", Error: \"{1}\""
                            , ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw new DbEntityValidationException(string.Format("Validation errors\r\n{0}"
                    , outputLines.ToString()), dbValEx);
            }

        }

        #endregion

        // 0. Enable-Migrations
        // 1. Add-Migration name
        // 2. Update-Database
    }
}