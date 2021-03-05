using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TeaMan.Models;

namespace TeaMan.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        const string FirstCalendarName = "Test Calendar 1";

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            using (var context = new DatabaseContext())
            {
                context.Database.Migrate();
                context.Database.ExecuteSqlRaw("delete from Calendars");
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task AddCalendar()
        {
            using (var context = new DatabaseContext())
            {
                context.Calendars.Add(
                    new Calendar()
                    {
                        Name = FirstCalendarName,
                        Order = 0
                    });

                await context.SaveChangesAsync();

                var addedCalendarsCount = await context.Calendars.AsQueryable()
                    .Where(e => e.Name == FirstCalendarName)
                    .CountAsync();

                Assert.AreEqual(1, addedCalendarsCount);
            }
        }
    }
}
