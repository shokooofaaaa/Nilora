using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOS.Auth;
using Nilora.Application.Interfaces;
using Nilora.Domain.Entities;
using Nilora.Domain.Enums;
using Nilora.Infrastructure.Persistence;
using System.Security.Cryptography;
using static QRCoder.PayloadGenerator;
namespace Nilora.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly NiloraDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            NiloraDbContext context,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }
        public async Task<CurrentUserResponse?> GetCurrentUserAsync(
                         int userId,
                  CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
    .Include(x => x.Technician)
    .FirstOrDefaultAsync(
        x => x.Id == userId,
        cancellationToken);

            if (user is null)
            {
                return null;
            }

            return new CurrentUserResponse
            {
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                FirstName = user.Technician?.FirstName,
                LastName = user.Technician?.LastName
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {

            var phoneNumber = request.PhoneNumber.Trim();

            var user = await _context.Users
                .FirstOrDefaultAsync(
                    x => x.PhoneNumber == phoneNumber,
                    cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid phone number or password.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Invalid phone number or password.");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    "Invalid phone number or password.");
            }

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var newPasswordHash = _passwordHasher.HashPassword(
                    user,
                    request.Password);

                user.ChangePassword(newPasswordHash);

            }

            user.RecordLogin();
            var tokenResult = _tokenService.GenerateToken(user);
            await _context.SaveChangesAsync(cancellationToken);
            return new AuthResponse
            {
                UserId = user.Id,
                Token = tokenResult.Token,
                ExpiresAtUtc = tokenResult.ExpiresAtUtc
            };
        }

        public async Task<AuthResponse> RegisterAsync(
         RegisterRequest request,
         CancellationToken cancellationToken = default)
        {
            var phoneNumber = request.PhoneNumber.Trim();

            var existingUser = await _context.Users
                .AnyAsync(
                    x => x.PhoneNumber == phoneNumber,
                    cancellationToken);

            if (existingUser)
            {
                throw new InvalidOperationException(
                    "This phone number is already registered.");
            }

            // Create a temporary user to generate the password hash
            var user = new User(phoneNumber, "TEMPORARY_HASH");

            // Hash the user's actual password
            var passwordHash = _passwordHasher.HashPassword(
                user,
                request.Password);

            // Replace the temporary hash with the real hash
            user.ChangePassword(passwordHash);
            await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            var qrCodeToken = Convert.ToHexString(
            RandomNumberGenerator.GetBytes(32));

            var technician = new Technician(
                user.Id,
                request.FirstName,
                request.LastName,
                qrCodeToken);

            _context.Technicians.Add(technician);

            await _context.SaveChangesAsync(cancellationToken);
            var tokenResult = _tokenService.GenerateToken(user);

            await transaction.CommitAsync(cancellationToken);

            return new AuthResponse
            {
                UserId = user.Id,
                Token = tokenResult.Token,
                ExpiresAtUtc = tokenResult.ExpiresAtUtc
            };
        }

        
    }
}
