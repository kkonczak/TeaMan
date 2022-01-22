using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaMan
{
    public static class DatabaseController
    {
        public static Task<List<Models.UserTask>> GetUserTasks(
            int calendarId,
            int? taskStatusId,
            int? taskTypeId,
            DateTime startDate,
            DateTime endDate)
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.UserTasks.Where(e =>
                        (!e.PlannedStart.HasValue || (e.PlannedStart.Value >= startDate && e.PlannedStart <= endDate)) &&
                        (!taskStatusId.HasValue || e.TaskStatusId == taskStatusId) &&
                        (!taskTypeId.HasValue || e.TaskTypeId == taskTypeId) &&
                        e.CalendarId == calendarId)
                    .ToListAsync();
            }
        }

        public static async Task InitializeDatabase()
        {
            using (var dbContext = new DatabaseContext())
            {
                await dbContext.Database.MigrateAsync();

                // Create default calendars
                if (dbContext.Calendars.AsQueryable().Count() == 0)
                {
                    var todoStatus = new Models.TaskStatus("To do", 1);
                    var inProgressStatus = new Models.TaskStatus("In progress", 2);
                    var doneStatus = new Models.TaskStatus("Done", 3);

                    var bugType = new Models.TaskType("Bug", 1);
                    var backlogType = new Models.TaskType("Backlog", 2);
                    var featureType = new Models.TaskType("Feature", 3);

                    var calendar = new Models.Calendar("Your first calendar", 1)
                    {
                        TaskStatuses = { todoStatus, inProgressStatus, doneStatus },
                        TaskTypes = { bugType, backlogType, featureType },
                        UserTasks =
                            {
                                new Models.UserTask
                                {
                                    Name = "This is the test task. You can add new Task using button \"Add Task\"",
                                    TaskStatus = todoStatus,
                                    TaskType = bugType,
                                    PlannedStart = DateTime.Now,
                                    PlannedEnd = DateTime.Now.AddHours(8)
                                },
                                new Models.UserTask
                                {
                                    Name = "This is the second test task. You can add new Task using button \"Add Task\"",
                                    TaskStatus = todoStatus,
                                    TaskType = bugType,
                                    PlannedStart = DateTime.Now.AddHours(10),
                                    PlannedEnd = DateTime.Now.AddHours(12)
                                }
                            }
                    };

                    var todoStatus2 = new Models.TaskStatus("To do", 1);
                    var inProgressStatus2 = new Models.TaskStatus("In progress", 2);
                    var doneStatus2 = new Models.TaskStatus("Done", 3);

                    var homeType = new Models.TaskType("Home", 1);
                    var companyType = new Models.TaskType("Company", 2);

                    var calendar2 = new Models.Calendar("Your second calendar", 1)
                    {
                        TaskStatuses = { todoStatus2, inProgressStatus2, doneStatus2 },
                        TaskTypes = { homeType, companyType },
                        UserTasks =
                            {
                                new Models.UserTask
                                {
                                    Name = "This is the test task in second calendar. You can add new Task using button \"Add Task\"",
                                    TaskStatus = todoStatus2,
                                    TaskType = homeType,
                                    PlannedStart = DateTime.Now,
                                    PlannedEnd = DateTime.Now.AddHours(2)
                                },
                                new Models.UserTask
                                {
                                    Name = "This is the second test task in second calendar. You can add new Task using button \"Add Task\"",
                                    TaskStatus = todoStatus2,
                                    TaskType = companyType,
                                    PlannedStart = DateTime.Now.AddHours(2),
                                    PlannedEnd = DateTime.Now.AddHours(1)
                                }
                            }
                    };

                    dbContext.Calendars.Add(calendar);
                    dbContext.Calendars.Add(calendar2);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public static Task<List<Models.Calendar>> GetCalendarsWithIncludedCollections()
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.Calendars.AsQueryable()
                    .Include(e => e.TaskStatuses)
                    .Include(e => e.TaskTypes)
                    .ToListAsync();
            }
        }

        public static Task AddUserTask(Models.UserTask userTask)
        {
            using (var dbContext = new DatabaseContext())
            {
                dbContext.UserTasks.Add(userTask);
                return dbContext.SaveChangesAsync();
            }
        }

        public static async Task AddCalendar(Models.Calendar calendar)
        {
            using (var dbContext = new DatabaseContext())
            {
                var orderOfLastCalendar = await dbContext.Calendars.Select(e => e.Order).MaxAsync();
                calendar.Order = orderOfLastCalendar + 1;
                dbContext.Calendars.Add(calendar);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task AddTaskType(Models.TaskType taskType)
        {
            using (var dbContext = new DatabaseContext())
            {
                var orderOfLastTaskType = await dbContext.TaskTypes.Select(e => e.Order).MaxAsync();
                taskType.Order = orderOfLastTaskType + 1;
                dbContext.TaskTypes.Add(taskType);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task AddTaskStatus(Models.TaskStatus taskStatus)
        {
            using (var dbContext = new DatabaseContext())
            {
                var orderOfLastTaskStatus = await dbContext.TaskStatuses.Select(e => e.Order).MaxAsync();
                taskStatus.Order = orderOfLastTaskStatus + 1;
                dbContext.TaskStatuses.Add(taskStatus);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
