import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Task } from 'src/app/models/tasks';
import { TaskService } from 'src/app/services/tasks.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent implements OnInit {

    public tasks: any = null;
  //public displayedColumns: string[] = ['Title', 'Description', 'DateAdded', 'Deadline', 'TaskImportance', 'TaskState', 'DateClosed', 'NumberOfComments'];
  public displayedColumns: string[] = ['Title', 'Description', 'TaskImportance', 'TaskState', 'NumberOfComments'];

    constructor(private taskService: TaskService) {
        this.getAllTasks();
      }

    ngOnInit() {
    }

    getAllTasks() {
        // this.tasks = []
        this.taskService.getAllTasks().subscribe(t => {
            this.tasks = t;
            console.log(t);
        });
    }
}
