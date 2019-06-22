import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserRole } from 'src/app/models/userroles';
import { UserRoleService } from 'src/app/services/userroles.service';

@Component({
  selector: 'app-userroles',
  templateUrl: './userroles.component.html',
  styleUrls: ['./userroles.component.scss']
})
export class UserRolesComponent implements OnInit {

  public userroles: any = null;

  public displayedColumns: string[] = ['Name', 'Description'];

  constructor(private userrolesService: UserRoleService, private route: Router) {
    this.getAllUserRoles();
  }

  ngOnInit() {
  }

  getAllUserRoles() {
    //this.users = []
    this.userrolesService.getAllUserRoles().subscribe(ur => {
      this.userroles = ur;
      console.log(ur);
    });
  }
  goBack() {
    this.route.navigate(['']);
  }
}
