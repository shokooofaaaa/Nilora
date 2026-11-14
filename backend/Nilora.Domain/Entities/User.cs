using Nilora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{

    public class User
    {
        public int Id { get; private set; }

        public string PhoneNumber { get; private set; } = null!;

        public string PasswordHash { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public bool IsPhoneNumberVerified { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? LastLoginAt { get; private set; }

        public Technician? Technician { get; private set; }

        public UserRole Role { get; private set; }
        private User() { }

        public User(string phoneNumber, string passwordHash)
        {
            SetPhoneNumber(phoneNumber);
            SetPasswordHash(passwordHash);
            Role = UserRole.Technician;
            IsActive = true;
            IsPhoneNumberVerified = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangePhoneNumber(string phoneNumber)
        {
            SetPhoneNumber(phoneNumber);
            IsPhoneNumberVerified = false;
        }

        public void ChangePassword(string passwordHash)
        {
            SetPasswordHash(passwordHash);
        }

        public void VerifyPhoneNumber()
        {
            IsPhoneNumberVerified = true;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void RecordLogin()
        {
            if (!IsActive)
                throw new InvalidOperationException("User is inactive.");

            LastLoginAt = DateTime.UtcNow;
        }

        private void SetPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.");

            PhoneNumber = phoneNumber.Trim();
        }

        private void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required.");

            PasswordHash = passwordHash;
        }
    }
}
