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
        /// Получить две последние добавленные базы с рейтингом и количеством голосов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RepBaseListItem> GetNewBases()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"
SELECT TOP 2 rp.Id as Id, 
            rp.Name as Name, 
            rp.Description as Description,
			(SELECT CONVERT(nvarchar(50), AVG(cm.Rating))
					FROM Comments cm 
					WHERE cm.RepBaseId = Id 
					GROUP BY cm.RepBaseId) as Rating,
			(SELECT COUNT(cm.RepBaseId) FROM Comments cm 
					WHERE cm.RepBaseId = Id 
					GROUP BY cm.RepBaseId) as RatingCount,
			ph.ThumbnailSrc as ImageSrc,
            rp.Address as Address
FROM RepBases rp 
LEFT JOIN PhotoToRepBase phrb ON phrb.RepBaseId = rp.Id
LEFT JOIN Photos ph ON ph.Id = phrb.PhotoId AND ph.IsLogo = 1
ORDER BY rp.CreationDate DESC";
                var rp = cn.Query<RepBaseListItem>(sql);
                return rp;
            }
        }

        /// <summary>
        /// Получить список значений для словаря
        /// </summary>
        /// <param name="tableName">Название словаря</param>
        /// <param name="fKey">Внешний ключ</param>
        /// <returns></returns>
        internal List<System.Web.Mvc.SelectListItem> GetDictionary(string tableName, int fKey = 0)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = String.Format("SELECT Id, Name FROM {0}", tableName);

                //Обработка конкретных словарей
                switch (tableName)
                {
                    case "Rooms": sql += " WHERE RepBaseId = " + fKey.ToString();
                        break;
                    case "Distincts": sql += " WHERE CityId = " + fKey.ToString();
                        break;
                }
                var result = cn.Query<SelectListItem>(sql).ToList();
                return result;
            }
        }

        internal List<RepbaseInfo> GetAllBasesCoordinates()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"SELECT Id, Name, LEFT(Description, 256) Description, Lat, Long FROM RepBases";
                var result = cn.Query<RepbaseInfo>(sql).ToList();
                return result;
            }
        }

        internal void InsertComment(ViewModel.Comment cm)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var com = new Models.Data.Comment { 
                    Rating = cm.Rating, Text = cm.Text, Email = cm.Text, Name = cm.Name, 
                };
                //TODO: вытащить id пользователя
                cn.Insert<Models.Data.Comment>(com);
            }
        }
    }
}