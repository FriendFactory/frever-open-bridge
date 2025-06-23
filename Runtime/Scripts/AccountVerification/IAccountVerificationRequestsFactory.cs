using System;
using Bridge.AccountVerification.Models;

namespace Bridge.AccountVerification
{
    public interface IAccountVerificationRequestsFactory
    {
        AddCredentialsRequest CreateAddCredentialsRequest(VerifiableCredentialType type, string credential, string verificationCode);
        AddCredentialsRequest CreateAddCredentialsRequest(LinkableCredentialType type, string identityToken,
            string identityId);
        AddCredentialsRequest CreateAddCredentialsRequest(string password);
        VerifyCredentialRequest CreateVerifyCredentialRequest(VerifiableCredentialType type, string credential, bool isNew);
        VerifyUserRequest CreateVerifyUserRequest(CredentialType type, string verification);
        UpdateCredentialsRequest CreateUpdateCredentialsRequest(VerifiableCredentialType type, string credential, string verificationCode, string verificationToken);
        UpdateCredentialsRequest CreateUpdateCredentialsRequest(string password, string verificationToken);
        UpdateCredentialsRequest CreateRemoveCredentialsRequest(CredentialType type, string verificationCode,
            string verificationToken);
    }

    internal class AccountVerificationRequestsFactory : IAccountVerificationRequestsFactory
    {
        public AddCredentialsRequest CreateAddCredentialsRequest(VerifiableCredentialType type, string credential, string verificationCode)
        {
            switch (type)
            {
                case VerifiableCredentialType.Email:
                    return new AddCredentialsRequest()
                    {
                        Type = CredentialType.Email,
                        Email = credential,
                        VerificationCode = verificationCode,
                    };
                case VerifiableCredentialType.PhoneNumber:
                    return new AddCredentialsRequest()
                    {
                        Type = CredentialType.PhoneNumber,
                        PhoneNumber = credential,
                        VerificationCode = verificationCode,
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public AddCredentialsRequest CreateAddCredentialsRequest(LinkableCredentialType type, string identityToken,
            string identityId)
        {
            return type switch
            {
                LinkableCredentialType.Apple => new AddCredentialsRequest()
                {
                    Type = CredentialType.AppleId, AppleIdentityToken = identityToken, AppleId = identityId,
                },
                LinkableCredentialType.Google => new AddCredentialsRequest()
                {
                    Type = CredentialType.GoogleId, IdentityToken = identityToken, GoogleId = identityId,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public AddCredentialsRequest CreateAddCredentialsRequest(string password)
        {
            return new AddCredentialsRequest()
            {
                Type = CredentialType.Password,
                Password = password,
            };
        }

        public VerifyCredentialRequest CreateVerifyCredentialRequest(VerifiableCredentialType type, string credential, bool isNew)
        {
            switch (type)
            {
                case VerifiableCredentialType.Email:
                    return new VerifyCredentialRequest() { Email = credential, IsNew = isNew };
                case VerifiableCredentialType.PhoneNumber:
                    return new VerifyCredentialRequest() { PhoneNumber = credential, IsNew = isNew };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
        }

        public VerifyUserRequest CreateVerifyUserRequest(CredentialType type, string verification)
        {
            switch (type)
            {
                case CredentialType.Email:
                    return new VerifyUserRequest() { Type = type, VerificationCode = verification };
                case CredentialType.PhoneNumber:
                    return new VerifyUserRequest() { Type = type, VerificationCode = verification };
                case CredentialType.Password:
                    return new VerifyUserRequest() { Type = type, Password = verification };
                case CredentialType.AppleId:
                    return new VerifyUserRequest() { Type = type, AppleIdentityToken = verification };
                case CredentialType.GoogleId:
                    return new VerifyUserRequest() { Type = type, IdentityToken = verification };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public UpdateCredentialsRequest CreateUpdateCredentialsRequest(VerifiableCredentialType type, string credential, string verificationCode, string verificationToken)
        {
            switch (type)
            {
                case VerifiableCredentialType.Email:
                    return new UpdateCredentialsRequest()
                    {
                        Type = CredentialType.Email,
                        Email = credential,
                        VerificationToken = verificationToken,
                        VerificationCode = verificationCode
                    };
                case VerifiableCredentialType.PhoneNumber:
                    return new UpdateCredentialsRequest()
                    {
                        Type = CredentialType.PhoneNumber,
                        PhoneNumber = credential,
                        VerificationToken = verificationToken,
                        VerificationCode = verificationCode
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public UpdateCredentialsRequest CreateUpdateCredentialsRequest(string password, string verificationToken)
        {
            return new UpdateCredentialsRequest() { Type = CredentialType.Password, VerificationToken = verificationToken, Password = password };
        }

        public UpdateCredentialsRequest CreateRemoveCredentialsRequest(CredentialType type, string verificationCode, string verificationToken)
        {
            switch (type)
            {
                case CredentialType.Email:
                    return new UpdateCredentialsRequest()
                    {
                        Type = CredentialType.Email,
                        Email = null, 
                        VerificationCode = verificationCode, 
                        VerificationToken = verificationToken
                    };
                case CredentialType.PhoneNumber:
                    return new UpdateCredentialsRequest()
                    {
                        Type = CredentialType.PhoneNumber,
                        PhoneNumber = null, 
                        VerificationCode = verificationCode, 
                        VerificationToken = verificationToken
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}