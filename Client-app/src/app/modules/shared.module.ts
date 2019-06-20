import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { WidgetsModule } from './widgets.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    WidgetsModule
  ],
  exports: [
    WidgetsModule,
  ],
  providers: [
  ]
}) export class SharedModule { };
