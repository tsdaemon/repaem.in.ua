using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Data;

namespace aspdev.repaem.Models.Data
{
	interface IRepo<T>
	{
		T Create();

		void Insert(T t);

		void Update(T t);

		void Delete(T t);

		void Delete(int d);

		T Get(int id);
	}
}
