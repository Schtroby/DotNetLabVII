using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPITests
{
    class TasksServiceTests
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPagesForTasks()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPagesForTasks))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var taskService = new TasksService(context);
                var added = taskService.Create(new LabIV.DTO.TaskPostDTO
                
                {
                    Title = "Booking Commision",
                    Description = "Verify booking commision",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    Deadline = DateTime.Parse("2019-06-17T00:00:00"),
                    TaskImportance = "High",
                    TaskState = "Closed",
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice task...",
                           
                        }
                    },
                    
                }, null);

                DateTime from = DateTime.Parse("2019-06-13T00:00:00");
                DateTime to = DateTime.Parse("2019-06-19T00:00:00");

                var allTasks = taskService.GetAll(1,from, to);
                Assert.AreEqual(1, allTasks.Entries.Count);
                Assert.IsNotNull(allTasks);
            }
        }

        

        [Test]
        public void CreateShouldAddAndReturnTheCreatedTask()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheCreatedTask))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var taskService = new TasksService(context);
                var added = taskService.Create(new LabIV.DTO.TaskPostDTO

                {
                    Title = "Booking1010",
                    Description = "Verify booking commision",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    Deadline = DateTime.Parse("2019-06-17T00:00:00"),
                    TaskImportance = "High",
                    TaskState = "Closed",
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice task...",

                        }
                    },

                }, null);


                Assert.IsNotNull(added);
                Assert.AreEqual("Booking1010", added.Title);
                Assert.AreNotEqual("BookingNOW", added.Title);

            }
        }

        [Test]
        public void UpsertShouldModifyTheGivenTask()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyTheGivenTask))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var taskService = new TasksService(context);
                var added = new TaskPostDTO()

                {
                    Title = "Booking1010",
                    Description = "Verify booking commision",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    Deadline = DateTime.Parse("2019-06-17T00:00:00"),
                    TaskImportance = "High",
                    TaskState = "Closed",
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice task...",

                        }
                    },

                };

                var toAdd = taskService.Create(added, null);
                var update = new TaskPostDTO()
                {
                    Title = "Updated"
                };

                var toUp = taskService.Create(update, null);
                var updateResult = taskService.Upsert(toUp.Id, toUp);


                Assert.IsNotNull(updateResult);
                Assert.AreEqual(toUp.Title, updateResult.Title);
                
            }
        }

        [Test]
        public void DeleteTaskWithCommentsShouldDeleteTasksAndComments()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteTaskWithCommentsShouldDeleteTasksAndComments))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var tasksService = new TasksService(context);

                var expected = new TaskPostDTO()
                {
                    Title = "Booking1010",
                    Description = "Verify booking commision",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    Deadline = DateTime.Parse("2019-06-17T00:00:00"),
                    TaskImportance = "High",
                    TaskState = "Closed",
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice task...",

                        }
                    },

                };
                
                var actual = tasksService.Create(expected, null);
                var afterDelete = tasksService.Delete(actual.Id);
                int numberOfCommentsInDb = context.Comments.CountAsync().Result;
                var resultExpense = context.Tasks.Find(actual.Id);

                Assert.IsNotNull(afterDelete);
                Assert.IsNull(resultExpense);
                Assert.AreEqual(0, numberOfCommentsInDb);
            }
        }

        [Test]
        public void GetByIdShouldReturnTaskWithCorrectId()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnTaskWithCorrectId))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var taskService = new TasksService(context);
                var added = new TaskPostDTO()

                {
                    Title = "BookingNOW",
                    Description = "Verify booking commision",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    Deadline = DateTime.Parse("2019-06-17T00:00:00"),
                    TaskImportance = "High",
                    TaskState = "Closed",
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice task...",

                        }
                    },

                };

                var current = taskService.Create(added, null);
                var expected = taskService.GetById(current.Id);

                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Title, current.Title);
                Assert.AreEqual(expected.Description, current.Description);
                Assert.AreEqual(expected.TaskImportance, current.TaskImportance);
                Assert.AreEqual(expected.TaskState, current.TaskState);
                Assert.AreEqual(expected.Id, current.Id);

            }
        }

    }
}
