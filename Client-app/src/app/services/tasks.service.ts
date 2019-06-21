import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Task } from '../models/tasks';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

//@Injectable({ providedIn: 'root' })
//export class TaskService {
//    // private tasksSubject: BehaviorSubject<any>;
//    public tasks: any;

//    constructor(private http: HttpClient) {
//        // this.tasksSubject = new BehaviorSubject<any>(null);
//    }

//    getAllTasks() : Observable<any> {
//        return this.http.get<any>(
//            `https://localhost:44387/api/tasks`);

//        // return this.http.get<any>(`https://localhost:44387/api/tasks`)
//        //     .pipe(map(response => {
//        //         this.tasks = response;
//        //         this.tasksSubject.next(this.tasks);
//        //         return response;
//        //     }));
//    }
//}

 @Injectable({ providedIn: 'root' })
 export class TaskService {
     private tasksSubject: BehaviorSubject<Task[]>;
     public tasks: Task[];

     constructor(private http: HttpClient) {
         this.tasksSubject = new BehaviorSubject<Task[]>([]);
     }

     getAllTasks() {


         return this.http.get<Task[]>(`https://localhost:44387/api/tasks`)
             .pipe(map(response => {
                 this.tasks = response;
                 this.tasksSubject.next(this.tasks);
                 return response;
             }));
     }
 }
