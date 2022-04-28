using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System.Net;

namespace Cognito {
    public class CognitoUserManagement {
        private readonly AmazonCognitoIdentityProviderClient cognitoIdPClient;

        public CognitoUserManagement(string profileName, RegionEndpoint regionEndpoint) {
            var awsCredentials = new BasicAWSCredentials("", "");
            cognitoIdPClient = new AmazonCognitoIdentityProviderClient(
                awsCredentials,
                RegionEndpoint.APSoutheast1);

        }

        public async Task<string?> CreateUser(
            string username,
            string password,
            string userPoolId) {
            var request = new AdminCreateUserRequest {
                Username = username,
                TemporaryPassword = password,
                UserPoolId = userPoolId,
                UserAttributes = new List<AttributeType> {
                    new AttributeType {Name="name",  Value=username}
                },
                ForceAliasCreation = false,
                MessageAction = MessageActionType.SUPPRESS,
                DesiredDeliveryMediums = new List<string> { "EMAIL" },
            };
            var adminCreateUserResponse = await cognitoIdPClient
                .AdminCreateUserAsync(request)
                .ConfigureAwait(false);

            if (adminCreateUserResponse.HttpStatusCode == HttpStatusCode.OK) {
                await LinkProviderForUser(username, userPoolId);
            }
            return adminCreateUserResponse.User.Attributes.FirstOrDefault(a => a.Name == "sub")?.Value;

        }

        public async Task LinkProviderForUser(string username, string userPoolId) {
            var request = new AdminLinkProviderForUserRequest {
                DestinationUser = new ProviderUserIdentifierType {
                    ProviderAttributeValue = username,
                    ProviderName = "Cognito"
                },
                SourceUser = new ProviderUserIdentifierType {
                    ProviderAttributeName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
                    ProviderAttributeValue = username,
                    ProviderName = "Azure-AD"
                },
                UserPoolId = userPoolId,
            };
            await cognitoIdPClient
                 .AdminLinkProviderForUserAsync(request)
                 .ConfigureAwait(false);
        }

        public async Task UpdateUserAttributes(
           string username,
           string userPoolId,
           List<AttributeType> attributeTypes) {

            var request = new AdminUpdateUserAttributesRequest {
                Username = username,
                UserPoolId = userPoolId,
                UserAttributes = attributeTypes
            };

            await cognitoIdPClient
               .AdminUpdateUserAttributesAsync(request)
               .ConfigureAwait(false);
        }


        public async Task AddUserToGroup(
            string username,
            string userPoolId,
            string groupName) {
            var request = new AdminAddUserToGroupRequest {
                Username = username,
                UserPoolId = userPoolId,
                GroupName = groupName
            };

            await cognitoIdPClient
                .AdminAddUserToGroupAsync(request)
                .ConfigureAwait(false);
        }

        public async Task RemoveUserFromGroup(
            string username,
            string userPoolId,
            string groupName) {
            var request = new AdminRemoveUserFromGroupRequest {
                Username = username,
                UserPoolId = userPoolId,
                GroupName = groupName
            };

            await cognitoIdPClient
                .AdminRemoveUserFromGroupAsync(request)
                .ConfigureAwait(false);
        }

        public async Task DisableUser(
            string username,
            string userPoolId) {
            AdminDisableUserRequest adminDisableUserRequest = new AdminDisableUserRequest {
                Username = username,
                UserPoolId = userPoolId
            };

            await cognitoIdPClient
                .AdminDisableUserAsync(adminDisableUserRequest)
                .ConfigureAwait(false);
        }

        public async Task DeleteUser(
            string username,
            string userPoolId) {
            var request = new AdminDeleteUserRequest {
                Username = username,
                UserPoolId = userPoolId
            };

            await cognitoIdPClient
                .AdminDeleteUserAsync(request)
                .ConfigureAwait(false);
        }
    }
}
