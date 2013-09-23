using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Security;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Models.Data
{
	public class RepetitionRepo : BaseProtectedRepo<Repetition>
	{
		public RepetitionRepo(IUserService us) : base(us)
		{
		}

		public override bool CheckAccess(Repetition t, User u)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"SELECT rb.ManagerId 
FROM Repetitions r 
INNER JOIN RepBases rb ON rb.Id = r.RebBaseId
WHERE r.Id = @Id";
				return Query<int>(sql, new {Id = t.Id}).FirstOrDefault() == u.Id;
			}
		}

		public void CreateNewDemoRepetitions()
		{
			string sql = "DELETE FROM Repetitions";
			Execute(sql);
			sql = "SELECT TOP 1 * FROM Rooms";
			var r1 = Query<Room>(sql).FirstOrDefault();
			sql = "SELECT TOP 1 * FROM RepBases WHERE Id = @Id";
			var rb1 = Query<RepBase>(sql, new {Id = r1.Id}).FirstOrDefault();
			sql = "SELECT TOP 1 * FROM Users WHERE Role = 'Musician'";
			var m1 = Query<User>(sql).FirstOrDefault();

			var rep1 = new Repetition
			{
				Comment = "23",
				MusicianId = m1.Id,
				RepBaseId = rb1.Id,
				RoomId = r1.Id,
				Status = (int)Status.ordered,
				Sum = 90,
				TimeEnd = 4,
				TimeStart = 1,
				Date = DateTime.Today
			};
			var rep2 = new Repetition
			{
				Comment = "23",
				MusicianId = m1.Id,
				RepBaseId = rb1.Id,
				RoomId = r1.Id,
				Status = (int)Status.ordered,
				Sum = 90,
				TimeEnd = 6,
				TimeStart = 2,
				Date = DateTime.Today
			};
			var rep3 = new Repetition
			{
				Comment = "23",
				MusicianId = m1.Id,
				RepBaseId = rb1.Id,
				RoomId = r1.Id,
				Status = (int)Status.ordered,
				Sum = 90,
				TimeEnd = 3,
				TimeStart = 2,
				Date = DateTime.Today
			};
			var rep4 = new Repetition
			{
				Comment = "23",
				MusicianId = m1.Id,
				RepBaseId = rb1.Id,
				RoomId = r1.Id,
				Status = (int)Status.ordered,
				Sum = 90,
				TimeEnd = 10,
				TimeStart = 8,
				Date = DateTime.Today
			};

			Insert(rep1);
			Insert(rep2);
			Insert(rep3);
			Insert(rep4);
		}
	}
}