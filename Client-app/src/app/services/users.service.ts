import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User } from '../models/users';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

//@Injectable({ providedIn: 'root' })
//export class UserService {
//     private usersSubject: BehaviorSubject<any>;
//    public users: any;

//    constructor(private http: HttpClient) {
//      this.usersSubject = new BehaviorSubject<any>(null);
//    }

//    getAllUsers() : Observable<any> {
//        return this.http.get<any>(
//            `https://localhost:44387/api/users`);

//         //return this.http.get<any>(`https://localhost:44387/api/users`)
//         //    .pipe(map(response => {
//         //    this.users = response;
//         //    this.usersSubject.next(this.users);
//         //        return response;
//         //    }));
//    }
//}

@Injectable({ providedIn: 'root' })
export class UserService {
  private usersSubject: BehaviorSubject<User[]>;
  public users: User[];

  constructor(private http: HttpClient) {
    this.usersSubject = new BehaviorSubject<User[]>([]);
  }

  getAllUsers() {


    return this.http.get<User[]>(`https://localhost:44387/api/users`)
      .pipe(map(response => {
        this.users = response;
        this.usersSubject.next(this.users);
        return response;
      }));
  }
  GetUserRoleNameById(id): Observable<any> {
    const url = `${`https://localhost:44387/api/UserUserRoles`}/${id}`;
    return this.http.get<any>(url, id);
  }
}
