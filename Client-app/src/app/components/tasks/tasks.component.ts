import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material';
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
  totalTasks = 10;
  totalTasksPerPage = 3;
  pageSizeOptions = [1, 3, 5, 7, 9];
  public displayedColumns: string[] = ['Title', 'Description', 'DateAdded', 'Deadline', 'TaskImportance', 'TaskState', 'DateClosed', 'NumberOfComments'];
 // public displayedColumns: string[] = ['Title', 'Description', 'TaskImportance', 'TaskState', 'NumberOfComments'];

  constructor(private taskService: TaskService, private route: Router) {
        this.getAllTasks();
      }

    ngOnInit() {
  }
  onChangedPage(pageData: PageEvent) {
    console.log(pageData)
  }

    getAllTasks() {
        // this.tasks = []
        this.taskService.getAllTasks().subscribe(t => {
            this.tasks = t;
            console.log(t);
        });
    }
  goBack() {
    this.route.navigate(['']);
  }
}
