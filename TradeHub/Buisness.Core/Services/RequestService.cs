﻿using Buisness.Contracts;
using Common.Enums;
using Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buisness.Contracts.Models;
using Common.Filters;
using Buisness.Core.Mappers;

namespace Buisness.Core.Services
{
    public class RequestService
    {
        private static readonly string CommunityNotExistsMessage = "Such community does not exist";
        private static readonly string UserNotExistsMessage = "Such user does not exist";
        private static readonly string alreadyMemberMessage = "You have already joined this community";
        private static readonly string notYetMemberMessage = "You have not joined this community yet";

        public WResult RequestToJoin( long communityId, long requesterId )
        {
            using ( var uow = new UnitOfWork() )
            {
                if ( uow.Requests.RequestExsits( requesterId, communityId ) != null )
                {
                    return new WResult( ValidationStatus.Failed, "Request already Exists" );
                }

                var community = uow.Communities.GetById( communityId );
                var requesterUser = uow.Users.GetById( requesterId );

                if ( community == null )
                {
                    return new WResult( ValidationStatus.Failed, CommunityNotExistsMessage );
                }

                if ( requesterUser == null )
                {
                    return new WResult( ValidationStatus.Failed, UserNotExistsMessage );
                }

                if ( community.CommunityUsers.Contains( requesterUser ) )
                {
                    return new WResult( ValidationStatus.Failed, "User is already in this ocmmunity" );
                }

                var joinRequest = new Request
                {
                    Type = (int)RequestType.Request,
                    User = requesterUser,
                    Community = community
                };

                uow.Requests.Add( joinRequest );
                uow.Complete();
            }

            return new WResult( ValidationStatus.Succeded );
        }

        public WResult InviteToJoin( long communityId, long inviterId, long requesterId )
        {
            using ( var uow = new UnitOfWork() )
            {
                if(uow.Requests.InvitationExists(requesterId, communityId) != null)
                {
                    return new WResult( ValidationStatus.Failed, "Invitation already Exists" );
                }

                var community = uow.Communities.GetById( communityId );
                var inviterUser = uow.Users.GetById( inviterId );
                var requesterUser = uow.Users.GetById( requesterId );

                if ( community == null )
                {
                    return new WResult( ValidationStatus.Failed, CommunityNotExistsMessage );
                }

                if ( inviterUser == null || requesterUser == null )
                {
                    return new WResult( ValidationStatus.Failed, UserNotExistsMessage );
                }

                if ( !community.CommunityUsers.Contains( inviterUser ) )
                {
                    return new WResult( ValidationStatus.Failed, "User is not in community and can't invite others to join" );
                }

                var joinRequest = new Request
                {
                    Type = (int)RequestType.Invitation,
                    User = requesterUser,
                    Community = community
                };

                uow.Requests.Add( joinRequest );
                uow.Complete();
            }

            return new WResult( ValidationStatus.Succeded );
        }

        public WResult AcceptRequestToJoin( long requestId, long communityUserId )
        {
            using ( var uow = new UnitOfWork() )
            {
                var request = uow.Requests.GetById( requestId );
                var communityUser = uow.Users.GetById( communityUserId );

                if ( request == null )
                {
                    return new WResult( ValidationStatus.Failed, "Request does not exist" );
                }

                if ( communityUser == null )
                {
                    return new WResult( ValidationStatus.Failed, UserNotExistsMessage );
                }

                if ( !request.Community.CommunityUsers.Contains( communityUser ) )
                {
                    return new WResult( ValidationStatus.Failed, "User is not in community and can't accept invitations from other users" );
                }

                request.Community.CommunityUsers.Add( request.User );
                uow.Requests.Remove( request );

                var invitation = uow.Requests.InvitationExists( request.UserId, request.CommunityId );
                if ( invitation != null )
                    uow.Requests.Remove( invitation );

                uow.Complete();
            }

            return new WResult( ValidationStatus.Succeded );
        }

        public WResult AcceptInvitationToCommunity( long requestId, long userId )
        {
            using ( var uow = new UnitOfWork() )
            {
                var invitation = uow.Requests.GetById( requestId );
                var user = uow.Users.GetById( userId );

                if ( invitation == null )
                {
                    return new WResult( ValidationStatus.Failed, "Request does not exist" );
                }

                if ( user == null )
                {
                    return new WResult( ValidationStatus.Failed, UserNotExistsMessage );
                }

                if ( invitation.User != user )
                {
                    return new WResult( ValidationStatus.Failed, "User can't accept invitations of other users" );
                }

                invitation.Community.CommunityUsers.Add( invitation.User );
                uow.Requests.Remove( invitation );

                var request = uow.Requests.InvitationExists( invitation.UserId, invitation.CommunityId );
                if ( request != null )
                    uow.Requests.Remove( request );

                uow.Complete();
            }

            return new WResult( ValidationStatus.Succeded );
        }

        public WResult DeclineRequestToJoin( long requestId, long communityUserId )
        {
            using ( var uow = new UnitOfWork() )
            {
                var request = uow.Requests.GetById( requestId );
                var communityUser = uow.Users.GetById( communityUserId );

                if ( request == null )
                {
                    return new WResult( ValidationStatus.Failed, "Request does not exist" );
                }

                if ( communityUser == null )
                {
                    return new WResult( ValidationStatus.Failed, UserNotExistsMessage );
                }

                if ( !request.Community.CommunityUsers.Contains( communityUser ) )
                {
                    return new WResult( ValidationStatus.Failed, "User is not in community and can't accept invitations from other users" );
                }

                uow.Requests.Remove( request );

                var invitation = uow.Requests.InvitationExists( request.UserId, request.CommunityId );
                if ( invitation != null )
                    uow.Requests.Remove( invitation );

                uow.Complete();
            }

            return new WResult( ValidationStatus.Succeded );
        }

        public WResult DeclineInvitationToCommunity( long requestId, long userId )
        {
            using ( var uow = new UnitOfWork() )
            {
                var invitation = uow.Requests.GetById( requestId );
                var user = uow.Users.GetById( userId );

                if ( invitation == null )
                {
                    return new WResult( ValidationStatus.Failed, "Request does not exist" );
                }

                if ( user == null )
                {
                    return new WResult( ValidationStatus.Failed, UserNotExistsMessage );
                }

                if ( invitation.User != user )
                {
                    return new WResult( ValidationStatus.Failed, "User can't accept invitations of other users" );
                }

                uow.Requests.Remove( invitation );

                var request = uow.Requests.InvitationExists( invitation.UserId, invitation.CommunityId );
                if ( request != null )
                    uow.Requests.Remove( request );

                uow.Complete();
            }

            return new WResult( ValidationStatus.Succeded );
        }

        public WResult<RequestIndexModel> GetUserInvitations(long userId)
        {
            using ( var uow = new UnitOfWork() )
            {
                var invitations = uow.Requests.GetUserInvitations( userId );
                uow.Complete();

                return new WResult<RequestIndexModel>( ValidationStatus.Succeded, errors: null, data: new RequestIndexModel
                {
                    Filters = null,
                    Requests = RequestMapper.Default.Map<List<RequestInfoModel>>( invitations )
                } );
            }
        }

        public WResult<RequestIndexModel> GetCommunityRequests(long communityId)
        {
            using ( var uow = new UnitOfWork() )
            {
                var requests = uow.Requests.GetCommunityJoinRequests( communityId );
                uow.Complete();

                return new WResult<RequestIndexModel>( ValidationStatus.Succeded, errors: null, data: new RequestIndexModel
                {
                    Filters = null,
                    Requests = RequestMapper.Default.Map<List<RequestInfoModel>>( requests )
                } );
            }
        }


    }
}
