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
    class CommentsServiceTests
    {

        [Test]
        public void GetAllShouldReturnCorrectNumberOfPagesForComments()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPagesForComments))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var taskService = new TasksService(context);
                var commentService = new CommentsService(context);
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
                            Owner = null
                        }
                    },

                };

                var current = taskService.Create(added, null);

                var allComments = commentService.GetAll(string.Empty, 1);
                Assert.AreEqual(1, allComments.NumberOfPages);

            }
        }

        [Test]
        public void CreateShouldAddAndReturnTheCreatedComment()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheCreatedComment))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var commentsService = new CommentsService(context);
                var toAdd = new CommentPostDTO ()

                {
  
                     Important = true,
                     Text = "A nice task...",
 
                };

                var added = commentsService.Create(toAdd, null);


                Assert.IsNotNull(added);
                Assert.AreEqual("A nice task...", added.Text);
                Assert.True(added.Important);
            }
        }

        [Test]
        public void UpsertShouldModifyTheGivenComment()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyTheGivenComment))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var commentsService = new CommentsService(context);
                var toAdd = new CommentPostDTO()

                {

                    Important = true,
                    Text = "A nice task...",

                };

                var added = commentsService.Create(toAdd, null);
                var update = new CommentPostDTO()
                {
                    Important = false
                };

                var toUp = commentsService.Create(update, null);
                var updateResult = commentsService.Upsert(added.Id, added);
                Assert.IsNotNull(updateResult);
                Assert.False(toUp.Important);
                
            }
        }

        [Test]
        public void DeleteShouldDeleteAGivenComment()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteShouldDeleteAGivenComment))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var commentsService = new CommentsService(context);
                var toAdd = new CommentPostDTO()

                {

                    Important = true,
                    Text = "A nice task...",

                };

                
                var actual = commentsService.Create(toAdd, null);
                var afterDelete = commentsService.Delete(actual.Id);
                int numberOfCommentsInDb = context.Comments.CountAsync().Result;
                var resultComment = context.Comments.Find(actual.Id);


                Assert.IsNotNull(afterDelete);
                Assert.IsNull(resultComment);
                Assert.AreEqual(0, numberOfCommentsInDb);
            }
        }

        [Test]
        public void GetByIdShouldReturnCommentWithCorrectId()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnCommentWithCorrectId))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var commentsService = new CommentsService(context);
                var toAdd = new CommentPostDTO()

                {

                    Important = true,
                    Text = "A nice task...",

                };


                var current = commentsService.Create(toAdd, null);
                var expected = commentsService.GetById(current.Id);



                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Text, current.Text);
                Assert.AreEqual(expected.Id, current.Id);
            }
        }

    }
}
