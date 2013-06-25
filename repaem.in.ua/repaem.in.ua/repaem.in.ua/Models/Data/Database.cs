using aspdev.repaem.ViewModel;
using Dapper.Data;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace aspdev.repaem.Models.Data
{
    public class Database : DbContext
    {
        const string connection = "NBSTEA";

        public Database()
            : base(connection)
        {
        }

        /// <summary>
        /// Получить две последние добавленные базы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RepBaseListItem> GetNewBases()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"
SELECT TOP 2 MAX(rp.Id) as Id, 
            MAX(rp.Name) as Name, 
            MAX(rp.Description) as Description, 
            CONVERT(nvarchar(50), AVG(cm.Rating)) as Rating, 
            COUNT(cm.RepBaseId) as RatingCount, 
            MAX(ph.ThumbnailSrc) as ImageSrc,
            MAX(rp.Address) as Address
FROM RepBases rp 
LEFT JOIN Comments cm ON cm.RepBaseId = rp.Id
LEFT JOIN PhotoToRepBase phrb ON phrb.RepBaseId = rp.Id
LEFT JOIN Photos ph ON ph.Id = phrb.PhotoId AND ph.IsLogo = 1
GROUP BY cm.RepBaseId
ORDER BY MAX(rp.CreationDate) DESC";
                var rp = cn.Query<RepBaseListItem>(sql);
                return rp;
            }
        }

        /// <summary>
        /// Получить список значений для словаря
        /// </summary>
        /// <param name="tableName">Название словаря</param>
        /// <returns></returns>
        internal List<System.Web.Mvc.SelectListItem> GetDictionary(string tableName)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = String.Format("SELECT Id, Name FROM {0}", tableName);
                var result = cn.Query<SelectListItem>(sql).ToList();
                return result;
            }
        }
    }
}