using System;
using System.Linq;
using Bridge.Models.ClientServer.Battles;
using UnityEngine;

namespace ApiTests.BattleTests
{
    internal sealed class VoteForBattlesTest: AuthorizedUserApiTestBase
    {
        public long TaskId;
        public Vote[] VoteModels;
        
        protected override async void RunTestAsync()
        {
            var res = await Bridge.Vote(TaskId, VoteModels.Select(x=> new BattleVoteModel
            {
                BattleId = x.BattleId,
                VotedVideoId = x.VotedVideoId
            }).ToArray());
            if (res.IsError)
            {
                Debug.LogError(res.ErrorMessage);
            }
            else
            {
                Debug.Log("Voting succeed");
            }
        }
        
        //for serializing BattleVoteModel in inspector without changing service related scripts
        [Serializable]
        internal struct Vote
        {
            public long BattleId;
            public long VotedVideoId;
        } 
    }
}