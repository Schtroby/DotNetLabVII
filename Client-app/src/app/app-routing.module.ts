import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {TasksComponent} from './components/tasks/tasks.component';
import {UsersComponent} from './components/users/users.component';

const routes: Routes = [

  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: 'tasks',
        component: TasksComponent
      },

      {
        path: 'users',
        component: UsersComponent
      }
    ]

    
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
