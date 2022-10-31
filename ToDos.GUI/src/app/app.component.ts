import { UserInfo } from './models/user-info';
import { HttpService } from './http.service';
import { Component, OnInit } from '@angular/core';
import { Task } from './models/task';
import { BehaviorSubject, Observable, retry } from 'rxjs';
import { Account } from './models/account';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  accountUserInfo: UserInfo = {
    userId: '-',
    userName: '-',
    userRole: '-'
  };
  accountUserInfoError = 0;
  accountLoginValue: string = null;
  accountPasswordValue: string = null;
  accountLoginSuccess = false;
  accountLoginError = 0;
  accountLogoutSuccess = false;
  accountLogoutError = 0;
  authorizationToken: string = null;
  getAllTasksList: Array<Task> = null;
  getAllTasksError = 0;
  getAllTasksShowed = false;
  getTaskTask: Task = null;
  getTaskInputId: string = null;
  getTaskError = 0;
  getLongTasksList: Array<Task> = null;
  getLongTasksSignsAmount: string = null;
  getLongTasksError = 0;
  addTaskNewTaskName: string = null;
  addTaskError = 0;
  addTaskTaskAdded = false;
  updateTaskInputId: string = null;
  updateTaskInputValue: string = null;
  updateTaskInputIsCompleted = false;
  updateTaskError = 0;
  updateTaskTaskUpdated = false;
  removeTaskInputId: string = null;
  removeTaskError = 0;
  removeTaskTaskRemoved = false;

  constructor(private httpService: HttpService) {
  }

  ngOnInit(): void {
  }

  accountInfo() {
    this.httpService.accountInfo(this.authorizationToken).subscribe({
      next: userInfo => {
        this.accountUserInfo = userInfo;
      },
      error: HttpErrorResponse => this.accountUserInfoError = HttpErrorResponse.status
    });
  }

  accountLogin() {
    this.accountLoginError = 0;
    this.accountLogoutError = 0;
    this.accountLoginSuccess = false;
    this.accountLogoutSuccess = false;
    this.httpService.accountLogin(this.accountLoginValue, this.accountPasswordValue).subscribe({
      next: token => {
        this.authorizationToken = token
        this.accountInfo();
      },
      error: HttpErrorResponse => this.accountLoginError = HttpErrorResponse.status,
      complete: () => {
        console.log(this.accountLoginError);
        if (this.accountLoginError == 0) {
          this.accountLoginSuccess = true;
        }
      }
    });
    this.accountLoginValue = null;
    this.accountPasswordValue = null;
  }

  accountLogout() {
    this.accountLoginError = 0;
    this.accountLogoutError = 0;
    this.accountLoginSuccess = false;
    this.accountLogoutSuccess = false;
    if (typeof this.authorizationToken != 'undefined' && this.authorizationToken) {
      this.authorizationToken = null;
      this.accountLogoutSuccess = true;
      this.accountUserInfo.userId = '-';
      this.accountUserInfo.userName = '-';
      this.accountUserInfo.userRole = '-';
    }
    else {
      this.accountLogoutError = 1;
    }
  }

  getAllTasks() {
    this.getAllTasksError = 0;
    this.getAllTasksShowed = true;
    this.httpService.getAllTasks().pipe(retry(3)).subscribe({
      next: tasks => this.getAllTasksList = tasks,
      error: HttpErrorResponse => this.getAllTasksError = HttpErrorResponse.status
    });
  }

  getAllTasksHide() {
    this.getAllTasksShowed = false;
  }

  getTask() {
    this.getTaskError = 0;
    this.getAllTasksShowed = false;
    this.httpService.getTask(this.getTaskInputId).subscribe({
      next: task => this.getTaskTask = task,
      error: HttpErrorResponse => this.getTaskError = HttpErrorResponse.status
    });
    this.getTaskInputId = null;
  }

  getLongTasks() {
    this.getLongTasksError = 0;
    this.httpService.getLongTasks(this.getLongTasksSignsAmount).subscribe({
      next: longTasks => this.getLongTasksList = longTasks,
      error: HttpErrorResponse => this.getLongTasksError = HttpErrorResponse.status
    })
    this.getLongTasksSignsAmount = null;
  }

  addTask() {
    this.addTaskError = 0;
    this.addTaskTaskAdded = false;
    const newTask: Task = ({
      value: this.addTaskNewTaskName
    });
    this.httpService.addTask(newTask).subscribe({
      error: HttpErrorResponse => this.addTaskError = HttpErrorResponse.status
    });
    this.addTaskNewTaskName = null;
    this.addTaskTaskAdded = true;
  }

  updateTask() {
    this.updateTaskError = 0;
    this.updateTaskTaskUpdated = false;
    const newTask: Task = ({
      id: this.updateTaskInputId,
      value: this.updateTaskInputValue,
      isCompleted: this.updateTaskInputIsCompleted
    });
    this.httpService.updateTask(newTask, this.authorizationToken).subscribe({
      next: task => task = newTask,
      error: HttpErrorResponse => this.updateTaskError = HttpErrorResponse.status
    });
    this.updateTaskInputId = null;
    this.updateTaskInputValue = null;
    this.updateTaskInputIsCompleted = false;
    this.updateTaskTaskUpdated = true;
  }

  removeTask() {
    this.removeTaskError = 0;
    this.removeTaskTaskRemoved = false;
    this.httpService.removeTask(this.removeTaskInputId, this.authorizationToken).subscribe({
      error: HttpErrorResponse => this.removeTaskError = HttpErrorResponse.status
    });
    this.removeTaskInputId = null;
    this.removeTaskTaskRemoved = true;
  }

}




