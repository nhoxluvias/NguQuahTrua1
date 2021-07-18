using Data.DAL;
using System;

namespace Data.BLL
{
    public class UserReactionBLL : BusinessLogicLayer
    {
        private bool disposed;

        public UserReactionBLL()
            : base()
        {
            InitDAL();
            disposed = false;
        }

        public UserReactionBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
        }

        public bool Upvote(string filmId, string userId)
        {
            StateOfCreation creationState = AddUserReaction(filmId, userId, true);
            if (creationState == StateOfCreation.Failed)
                return false;

            if (creationState == StateOfCreation.AlreadyExists)
            {
                StateOfUpdate updateState = UpdateUserReaction(filmId, userId, true);
                if (updateState == StateOfUpdate.Success)
                    return true;
                return false;
            }

            return true;
        }

        public bool Downvote(string filmId, string userId)
        {
            StateOfCreation creationState = AddUserReaction(filmId, userId, false);
            if (creationState == StateOfCreation.Failed)
                return false;

            if (creationState == StateOfCreation.AlreadyExists)
            {
                StateOfUpdate updateState = UpdateUserReaction(filmId, userId, false);
                if (updateState == StateOfUpdate.Success)
                    return true;
                return false;
            }

            return true;
        }

        public StateOfCreation AddUserReaction(string filmId, string userId, bool isUpvote)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(userId))
                throw new Exception("");

            UserReaction userReaction = db.UserReactions.SingleOrDefault(ur => ur.filmId == filmId && ur.userId == userId);
            if (userReaction != null)
                return StateOfCreation.AlreadyExists;

            userReaction = new UserReaction
            {
                filmId = filmId,
                userId = userId,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };

            if (isUpvote)
            {
                userReaction.upvoted = true;
                userReaction.downvoted = false;
            }
            else
            {
                userReaction.upvoted = false;
                userReaction.downvoted = true;
            }

            int affected = db.UserReactions.Insert(userReaction);

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public StateOfDeletion DeleteUserReaction(string filmId, string userId)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(userId))
                throw new Exception("");

            int affected = db.UserReactions.Delete(ur => ur.filmId == filmId && ur.userId == userId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public StateOfUpdate UpdateUserReaction(string filmId, string userId, bool isUpvote)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(userId))
                throw new Exception("");

            UserReaction userReaction = new UserReaction
            {
                updateAt = DateTime.Now,
            };
            if (isUpvote)
            {
                userReaction.upvoted = true;
                userReaction.downvoted = false;
            }
            else
            {
                userReaction.upvoted = false;
                userReaction.downvoted = true;
            }

            int affected = db.UserReactions.Update(
                userReaction, ur => new { ur.upvoted, ur.downvoted, ur.updateAt },
                ur => ur.filmId == filmId && ur.userId == userId);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
