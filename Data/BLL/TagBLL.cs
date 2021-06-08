using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class TagBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;

        public TagBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public TagBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private TagInfo ToTagInfo(Tag tag)
        {
            if (tag == null)
                return null;
            return new TagInfo
            {
                ID = tag.ID,
                name = tag.name,
                description = tag.description,
                createAt = tag.createAt,
                updateAt = tag.updateAt
            };
        }

        private Tag ToTag(TagCreation tagCreation)
        {
            if (tagCreation == null)
                throw new Exception("@'categoryCreation' must be not null");
            return new Tag
            {
                name = tagCreation.name,
                description = tagCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Tag ToTag(TagUpdate tagUpdate)
        {
            if (tagUpdate == null)
                throw new Exception("");
            return new Tag
            {
                ID = tagUpdate.ID,
                name = tagUpdate.name,
                description = tagUpdate.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<TagInfo>> GetTagsAsync()
        {
            List<TagInfo> tags = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                tags = (await db.Tags.ToListAsync())
                    .Select(t => ToTagInfo(t)).ToList();
            else
                tags = (await db.Tags.ToListAsync())
                    .Select(t => ToTagInfo(t)).ToList();
            return tags;
        }

        public List<TagInfo> GetTags()
        {
            List<TagInfo> tags = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                tags = db.Tags.ToList()
                    .Select(t => ToTagInfo(t)).ToList();
            else
                tags = db.Tags.ToList()
                    .Select(t => ToTagInfo(t)).ToList();
            return tags;
        }

        public async Task<PagedList<TagInfo>> GetTagsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Tag> pagedList = null;
            Expression<Func<Tag, object>> orderBy = t => new { t.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Tags.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Tags.ToPagedListAsync(
                    t => new { t.ID, t.name, t.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<TagInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(t => ToTagInfo(t)).ToList()
            };
        }

        public PagedList<TagInfo> GetTags(int pageIndex, int pageSize)
        {
            SqlPagedList<Tag> pagedList = null;
            Expression<Func<Tag, object>> orderBy = t => new { t.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Tags.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Tags.ToPagedList(
                    t => new { t.ID, t.name, t.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<TagInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(t => ToTagInfo(t)).ToList()
            };
        }

        public async Task<TagInfo> GetTagAsync(long tagId)
        {
            if (tagId <= 0)
                throw new Exception("");
            Tag tag = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                tag = await db.Tags
                     .SingleOrDefaultAsync(t => t.ID == tagId);
            else
                tag = await db.Tags
                    .SingleOrDefaultAsync(t => new { t.ID, t.name, t.description }, t => t.ID == tagId);

            return ToTagInfo(tag);
        }

        public TagInfo GetTag(long tagId)
        {
            if (tagId <= 0)
                throw new Exception("");
            Tag tag = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                tag = db.Tags
                     .SingleOrDefault(t => t.ID == tagId);
            else
                tag = db.Tags
                    .SingleOrDefault(t => new { t.ID, t.name, t.description }, t => t.ID == tagId);

            return ToTagInfo(tag);
        }

        public async Task<List<TagInfo>> GetTagsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Tag].* from [TagDistribution], [Tag]
                                where [TagDistribution].[tagID] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Tag].[ID], [Tag].[name], [Tag].[description] from [TagDistribution], [Tag]
                                where [TagDistribution].[tagID] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<TagInfo>>(sqlCommand);
        }

        public List<TagInfo> GetTagsByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Tag].* from [TagDistribution], [Tag]
                                where [TagDistribution].[tagID] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Tag].[ID], [Tag].[name], [Tag].[description] from [TagDistribution], [Tag]
                                where [TagDistribution].[tagID] = [Tag].[ID]
                                    and [TagDistribution].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return db.ExecuteReader<List<TagInfo>>(sqlCommand);
        }

        public async Task<StateOfCreation> CreateTagAsync(TagCreation tagCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Tag tag = ToTag(tagCreation);
            if (tag.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Tags.CountAsync(t => t.name == tag.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (tag.description == null)
                affected = await db.Tags.InsertAsync(tag, new List<string> { "ID", "description" });
            else
                affected = await db.Tags.InsertAsync(tag, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateTagAsync(TagUpdate tagUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Tag tag = ToTag(tagUpdate);
            if (tag.name == null)
                throw new Exception("");

            int affected;
            if (tag.description == null)
                affected = await db.Tags.UpdateAsync(
                    tag,
                    t => new { t.name, t.updateAt },
                    t => t.ID == tag.ID
                );
            else
                affected = await db.Tags.UpdateAsync(
                    tag,
                    t => new { t.name, t.description, t.updateAt },
                    t => t.ID == tag.ID
                );

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteTagAsync(long tagId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (tagId <= 0)
                throw new Exception("");

            long tagDistributionNumber = await db.TagDistributions
                .CountAsync(td => td.tagId == tagId);
            if (tagDistributionNumber > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Tags.DeleteAsync(t => t.ID == tagId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Tags.CountAsync();
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
