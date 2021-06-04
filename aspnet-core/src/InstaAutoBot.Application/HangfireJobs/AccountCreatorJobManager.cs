using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Hangfire;
using InstaAutoBot.Authorization.Roles;
using InstaAutoBot.Authorization.Users;

namespace InstaAutoBot.HangfireJobs
{
    public class AccountCreatorJobManager : InstaAutoBotDomainServiceBase
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly UserManager _userManager;

        public AccountCreatorJobManager(
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            UserManager userManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 60)]
        [UnitOfWork]
        public virtual void Execute()
        {
            //var d = AsyncHelper.RunSync(() => _userManager.FindByNameAsync("admin"));
            //var dd = _roleRepository.GetAll().ToList();
            var totalUsers = _userRepository.GetAll().ToList();
            
            Logger.Debug("AccountCreatorJobManager.Executed " + totalUsers);
        }
    }
}