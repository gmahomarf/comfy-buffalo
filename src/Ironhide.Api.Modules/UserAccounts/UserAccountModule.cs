using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Api.Modules.UserAccounts.Facebook;
using Ironhide.Api.Modules.UserAccounts.Google;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Nancy;
using Nancy.ModelBinding;

namespace Ironhide.Api.Modules.UserAccounts
{
    public class UserAccountModule : NancyModule
    {
        public UserAccountModule(IReadOnlyRepository readOnlyRepository, ICommandDispatcher commandDispatcher,
            IPasswordEncryptor passwordEncryptor, IMappingEngine mappingEngine, IUserSessionFactory userSessionFactory)
        {
            Post["/register"] =
                _ =>
                {
                    var req = this.Bind<NewUserRequest>();
                    IEnumerable<UserAbility> abilities =
                        mappingEngine.Map<IEnumerable<UserAbilityRequest>, IEnumerable<UserAbility>>(req.Abilities);
                    commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                        new CreateEmailLoginUser(req.Email, passwordEncryptor.Encrypt(req.Password), req.Name,
                            req.PhoneNumber, abilities));
                    return null;
                };


            Post["/register/facebook"] =
                _ =>
                {
                    var req = this.Bind<FacebookRegisterRequest>();
                    commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                        new CreateFacebookLoginUser(req.id, req.email, req.first_name, req.last_name, req.link, req.name,
                            req.url_image));
                    return null;
                };

            Post["/register/google"] =
                _ =>
                {
                    var req = this.Bind<GoogleRegisterRequest>();
                    commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                        new CreateGoogleLoginUser(req.id, req.email, req.name.givenName, req.name.familyName, req.url,
                            req.displayName, req.image.url));
                    return null;
                };

            Post["/password/requestReset"] =
                _ =>
                {
                    var req = this.Bind<ResetPasswordRequest>();
                    commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                        new CreatePasswordResetToken(req.Email));
                    return null;
                };

            Put["/password/reset/{token}"] =
                p =>
                {
                    var newPasswordRequest = this.Bind<NewPasswordRequest>();
                    Guid token = Guid.Parse((string) p.token);
                    commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                        new ResetPassword(token, passwordEncryptor.Encrypt(newPasswordRequest.Password)));
                    return null;
                };

            Post["/user/abilites"] = p =>
                                     {
                                         var requestAbilites = this.Bind<UserAbilitiesRequest>();
                                         commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                             new AddAbilitiesToUser(requestAbilites.UserId,
                                                 requestAbilites.Abilities.Select(x => x.Id)));

                                         return null;
                                     };

            Get["/abilities", true] = async (_, c) =>
                                            {
                                                IEnumerable<UserAbility> abilites =
                                                    await readOnlyRepository.GetAll<UserAbility>();

                                                IEnumerable<UserAbilityRequest> mappedAbilites =
                                                    mappingEngine
                                                        .Map<IEnumerable<UserAbility>, IEnumerable<UserAbilityRequest>>(
                                                            abilites);

                                                return mappedAbilites;
                                            };
        }
    }
}