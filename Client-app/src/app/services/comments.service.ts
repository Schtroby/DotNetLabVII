import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Comment } from '../models/comments';
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
export class CommentService {
  private commentsSubject: BehaviorSubject<Comment[]>;
  public comments: Comment[];

  constructor(private http: HttpClient) {
    this.commentsSubject = new BehaviorSubject<Comment[]>([]);
  }

  getAllComments() {


    return this.http.get<Comment[]>(`https://localhost:44387/api/comments`)
      .pipe(map(response => {
        this.comments = response;
        this.commentsSubject.next(this.comments);
        return response;
      }));
  }
  getAllFilteredComments(filter): Observable<any>{
    const url = `${`https://localhost:44387/api/comments?filter=`}${filter}`;
        return this.http.get<any>(url, filter);
  }
}
