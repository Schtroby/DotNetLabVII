using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{
    public class CommentFilterDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public int TaskId { get; set; }

        public static CommentFilterDTO FromComment(Comment comment)
        {

            return new CommentFilterDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                TaskId = comment.Task.Id,
                Important = comment.Important
            };
        }
    }
       
}
