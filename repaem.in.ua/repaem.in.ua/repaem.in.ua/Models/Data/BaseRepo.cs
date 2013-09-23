using System.Data.Common;
using Dapper.Data;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Models.Data
{
	public abstract class BaseRepo<T> : DbContext, IRepo<T> where T : class, new()
	{
		private const string connection = "localhost";

		public BaseRepo()
			: base(connection)
		{
		}

		public virtual T Create()
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				T t = new T();
				cn.Insert(t);
				return t;
			}
		}

		public virtual void Insert(T t)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(t);
			}
		}

		public virtual void Update(T t)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Update(t);
			}
		}

		public virtual T Get(int id)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				return cn.Get<T>(id);
			}
		}

		public virtual void Delete(T t)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Delete(t);
			}
		}

		public virtual void Delete(int d)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				var t = Get(d);
				cn.Delete(t);
			}
		}
	}
}