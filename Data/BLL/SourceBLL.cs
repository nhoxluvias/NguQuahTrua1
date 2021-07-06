using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class SourceBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;

        public SourceBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }
        public SourceBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
           : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private SourceInfo ToSourceInfo(Source source)
        {
            if (source == null)
                return null;
            return new SourceInfo
            {
                filmName = new FilmBLL(this, dataAccessLevel).GetFilm(source.filmId).name,
                mainSource = source.mainSource,
                secondarySource1 = source.secondarySource1,
                secondarySource2 = source.secondarySource2,
                secondarySource3 = source.secondarySource3,
                createAt = source.createAt,
                updateAt = source.updateAt
            };
        }

        private Source ToSource(SourceCreation sourceCreation)
        {
            if (sourceCreation == null)
                throw new Exception("");
            return new Source
            {
                filmId = sourceCreation.filmId,
                mainSource = sourceCreation.mainSource,
                secondarySource1 = sourceCreation.secondarySource1,
                secondarySource2 = sourceCreation.secondarySource2,
                secondarySource3 = sourceCreation.secondarySource3,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Source ToSource(SourceUpdate sourceUpdate)
        {
            if (sourceUpdate == null)
                throw new Exception("");
            return new Source
            {
                filmId = sourceUpdate.filmId,
                mainSource = sourceUpdate.mainSource,
                secondarySource1 = sourceUpdate.secondarySource1,
                secondarySource2 = sourceUpdate.secondarySource2,
                secondarySource3 = sourceUpdate.secondarySource3,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<SourceInfo>> GetSourcesAsync()
        {
            List<SourceInfo> sources = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sources = (await db.Sources.ToListAsync())
                    .Select(s => ToSourceInfo(s)).ToList();
            else
                sources = (await db.Sources.ToListAsync(s => new { s.filmId, s.mainSource, s.secondarySource1, s.secondarySource2, s.secondarySource3 }))
                    .Select(s => ToSourceInfo(s)).ToList();
            return sources;
        }

        public List<SourceInfo> GetSources()
        {
            List<SourceInfo> sources = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sources = db.Sources.ToList()
                    .Select(s => ToSourceInfo(s)).ToList();
            else
                sources = db.Sources.ToList(s => new { s.filmId, s.mainSource, s.secondarySource1, s.secondarySource2, s.secondarySource3 })
                    .Select(s => ToSourceInfo(s)).ToList();
            return sources;
        }

        public async Task<PagedList<SourceInfo>> GetSourcesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Source> pagedList = null;
            Expression<Func<Source, object>> orderBy = s => new { s.filmId };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Sources.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Sources.ToPagedListAsync(
                    s => new { s.filmId, s.mainSource, s.secondarySource1, s.secondarySource2, s.secondarySource3 },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<SourceInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(s => ToSourceInfo(s)).ToList()
            };
        }

        public PagedList<SourceInfo> GetSources(int pageIndex, int pageSize)
        {
            SqlPagedList<Source> pagedList = null;
            Expression<Func<Source, object>> orderBy = s => new { s.filmId };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Sources.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Sources.ToPagedList(
                    s => new { s.filmId, s.mainSource, s.secondarySource1, s.secondarySource2, s.secondarySource3 },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<SourceInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(s => ToSourceInfo(s)).ToList()
            };
        }

        public async Task<SourceInfo> GetSourceAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            Source source = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                source = await db.Sources
                      .SingleOrDefaultAsync(s => s.filmId == filmId);
            else
                source = await db.Sources.SingleOrDefaultAsync(
                        s => new { s.filmId, s.mainSource, s.secondarySource1, s.secondarySource2, s.secondarySource3 },
                        s => s.filmId == filmId
                    );
            return ToSourceInfo(source);
        }

        public SourceInfo GetSource(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            Source source = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                source = db.Sources
                      .SingleOrDefault(s => s.filmId == filmId);
            else
                source = db.Sources.SingleOrDefault(
                        s => new { s.filmId, s.mainSource, s.secondarySource1, s.secondarySource2, s.secondarySource3 },
                        s => s.filmId == filmId
                    );
            return ToSourceInfo(source);
        }

        public async Task<StateOfCreation> CreateCategoryAsync(SourceCreation sourceCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Source source = ToSource(sourceCreation);
            if (string.IsNullOrEmpty(source.filmId))
                throw new Exception("");

            int checkExists = (int)await db.Sources.CountAsync(s => s.filmId == source.filmId);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (
                string.IsNullOrEmpty(source.secondarySource1) || string.IsNullOrEmpty(source.secondarySource2)
                || string.IsNullOrEmpty(source.secondarySource3)
            )
            {
                if (
                    string.IsNullOrEmpty(source.secondarySource1) || string.IsNullOrEmpty(source.secondarySource2)
                    || string.IsNullOrEmpty(source.secondarySource3)    
                )
                {
                    affected = await db.Sources
                        .InsertAsync(source, new List<string> { "secondarySource1", "secondarySource2", "secondarySource3" }); ;
                }
                else if (string.IsNullOrEmpty(source.secondarySource1))
                {
                    affected = await db.Sources
                        .InsertAsync(source, new List<string> { "secondarySource1" }); ;
                }
                else if (string.IsNullOrEmpty(source.secondarySource2))
                {
                    affected = await db.Sources
                        .InsertAsync(source, new List<string> { "secondarySource2" }); ;
                }
                else
                {
                    affected = await db.Sources
                        .InsertAsync(source, new List<string> { "secondarySource3" }); ;
                }
            }
            else
            {
                affected = await db.Sources.InsertAsync(source);
            }

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

    }
}