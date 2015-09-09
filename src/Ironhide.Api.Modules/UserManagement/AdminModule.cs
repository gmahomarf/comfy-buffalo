using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AcklenAvenue.Commands;
using AutoMapper;
using Ironhide.Api.Infrastructure;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminModule : NancyModule
    {
        public AdminModule(IReadOnlyRepository readOnlyRepository, IMappingEngine mappingEngine,
            ICommandDispatcher commandDispatcher, IUserSessionFactory userSessionFactory)
        {
            Get["/users", true] =
                async (a, c) =>
                      {
                          this.RequiresClaims(new[] {"Administrator"});
                          var request = this.Bind<AdminUsersRequest>();

                          ParameterExpression parameter = Expression.Parameter(typeof (User), "User");
                          Expression<Func<User, object>> mySortExpression =
                              Expression.Lambda<Func<User, object>>(Expression.Property(parameter, request.Field),
                                  parameter);

                          IQueryable<User> users =
                              (await readOnlyRepository.Query<User>(x => x.Name != Context.CurrentUser.UserName))
                                  .AsQueryable();

                          IOrderedQueryable<User> orderedUsers = users.OrderBy(mySortExpression);

                          IQueryable<User> pagedUsers =
                              orderedUsers.Skip(request.PageSize*(request.PageNumber - 1)).Take(request.PageSize);

                          List<AdminUserResponse> usersList = mappingEngine
                              .Map<IQueryable<User>, IEnumerable<AdminUserResponse>>(pagedUsers).ToList();

                          return usersList;
                      };

            Post["/users/enable", true] =
                async (a, c) =>
                      {
                          this.RequiresClaims(new[] {"Administrator"});
                          var request = this.Bind<AdminEnableUsersRequest>();
                          if (request.Enable)
                          {
                              await
                                  commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                      new EnableUser(request.Id));
                          }
                          else
                          {
                              await
                                  commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                      new DisableUser(request.Id));
                          }

                          return null;
                      };

            Get["/user/{userId}", true] =
                async (_, c) =>
                      {
                          Guid userId = Guid.Parse((string) _.userId);
                          User user = await readOnlyRepository.GetById<User>(userId);
                          AdminUserResponse mappedUser = mappingEngine
                              .Map<User, AdminUserResponse>(user);
                          return mappedUser;
                      };

            Post["/user", true] =
                async (a, c) =>
                      {
                          var request = this.Bind<AdminUpdateUserRequest>();
                          await
                              commandDispatcher.Dispatch(userSessionFactory.Create(Context.CurrentUser),
                                  new UpdateUserProfile(request.Id, request.Name, request.Email));
                          return null;
                      };
        }
    }
}