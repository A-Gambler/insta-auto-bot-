using System;
using System.Collections.Generic;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using InstaAutoBot.EntityFrameworkCore.Seed;
using InstaAutoBot.MultiTenancy;

namespace InstaAutoBot.EntityFrameworkCore
{
    public class MultiTenantMigrateExecuter : ITransientDependency
    {
        // public I// Logger // Log { get; set; }

        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;

        public MultiTenantMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            // ILogger Log,
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            // // Log = // Log;
            _migrator = migrator;
            _tenantRepository = tenantRepository;
            _connectionStringResolver = connectionStringResolver;
        }

        public void Run()
        {
            var hostConnStr = _connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host));
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                // // Log.Error("Configuration file should contain a connection string named 'Default'");
                return;
            }
            // Log.Info("Host database: " + ConnectionStringHelper.GetConnectionString(hostConnStr));

            // Log.Info("HOST database migration started...");
            try
            {
                _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                // Log.Error("Canceled migrations - An error occured during migration of host database.", ex);
                return;
            }
            // Log.Info("HOST database migration completed.");

            var migratedDatabases = new HashSet<string>();
            var tenants = _tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                // Log.Info($"Tenant database migration started... ({(i + 1)} / {tenants.Count})");
                // Log.Info("Name              : " + tenant.Name);
                // Log.Info("TenancyName       : " + tenant.TenancyName);
                // Log.Info("Tenant Id         : " + tenant.Id);
                // Log.Info("Connection string : " + SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString));

                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        _migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        // Log.Error("An error occured during migration of tenant database. Skipped this tenant and will continue for others...", ex);
                    }
                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    // Log.Info("This database has already migrated before (you have more than one tenant in same database). Skipping it....");
                }

                // Log.Info($"Tenant database migration completed. ({i + 1} / {tenants.Count})");
            }

            // Log.Info("All databases have been migrated.");
        }
    }
}