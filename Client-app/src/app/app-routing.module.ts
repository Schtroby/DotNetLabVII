import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {TasksComponent} from './components/tasks/tasks.component';
import {UsersComponent} from './components/users/users.component';
import { CommentsComponent } from './components/comments/comments.component';
import { UserRolesComponent } from './components/userroles/userroles.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { UsersGuard } from './guards/users.guard';
import { TasksGuard } from './guards/tasks.guard';
import { CommentsGuard } from './guards/comments.guard';
import { UserRolesGuard } from './guards/userroles.guard';


const routes: Routes = [

  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'tasks',
        component: TasksComponent,
        canActivate: [TasksGuard],
      },

      {
        path: 'users',
        component: UsersComponent,
        canActivate: [UsersGuard],
      },

      {
        path: 'comments',
        component: CommentsComponent,
        canActivate: [CommentsGuard],
      },
      {
        path: 'userroles',
        component: UserRolesComponent,
        canActivate: [UserRolesGuard],
      }
    ]

    
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
