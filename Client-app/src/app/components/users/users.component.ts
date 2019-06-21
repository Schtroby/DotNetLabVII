import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/users';
import { UserService } from 'src/app/services/users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  public users: any = null;
 
  public displayedColumns: string[] = ['Username', 'Email'];

  constructor(private userService: UserService) {
    this.getAllUsers();
  }

  ngOnInit() {
  }

  getAllUsers() {
    //this.users = []
    this.userService.getAllUsers().subscribe(u => {
      this.users = u;
      console.log(u);
    });
  }
}
