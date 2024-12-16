using System.Security.Cryptography.X509Certificates;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using Framework.Application;
using Framework.Domain;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthHelper _authHelper;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _authHelper = authHelper;
            _roleRepository = roleRepository;
        }

        public OperationResult Register(RegisterAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var password = _passwordHasher.Hash(command.Password);
            var picturePath = _fileUploader.Upload(command.ProfilePhoto, $"profilePhotos/{command.FullName}");
            var account = new Account(command.FullName, command.Username, password, command.Mobile,
                command.RoleId, picturePath);

            _accountRepository.Create(account);
            _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (_accountRepository.Exists(x => (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var picturePath = _fileUploader.Upload(command.ProfilePhoto, $"profilePhotos/{command.FullName}");

            account.Edit(command.FullName, command.Username, command.Mobile, command.RoleId, picturePath);
            _accountRepository.SaveChanges();
            return operation.Succeeded();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.NewPassword)
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);


            var password = _passwordHasher.Hash(command.Password);

            account.ChangePassword(password);
            _accountRepository.SaveChanges();

            return operation.Succeeded();
        }

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetByUserName(command.Username);
            if (account == null)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var (isVerified, needsUpgrade) = _passwordHasher.Check(account.Password, command.Password);
            if (!isVerified)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var permissions = _roleRepository
                .Get(account.RoleId)
                .Permissions
                .Select(x => x.Code)
                .ToList();
            var authViewModel = new AuthViewModel(account.Id,account.RoleId, account.FullName, account.Username, permissions);
            _authHelper.SignIn(authViewModel);
            return operation.Succeeded();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public List<AccountViewModel> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }

        public void Logout()
        {
            _authHelper.SignOut();
        }
    }
}
