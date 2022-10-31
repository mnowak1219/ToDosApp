import { UserInfo } from './models/user-info';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Task } from './models/task';
import { Account } from './models/account'

@Injectable()
export class HttpService {
  private baseUrl = 'https://localhost:5001/';
  private tasks = new BehaviorSubject<Array<Task>>([]);
  tasks$ = this.tasks.asObservable();

  constructor(private http: HttpClient) { }

  /** Logowanie użytkownika */
  accountLogin(login: string, password: string): (Observable<string>) {
    const params = new HttpParams().set('login', login).set('password',password);
    return this.http.get<string>(this.baseUrl + 'account/token', { params: params });
  }

  /** Informacje o użytkowniku */
  accountInfo(authorizationToken: string): (Observable<UserInfo>) {
    const headers = new HttpHeaders().set('Authorization', 'Bearer ' + authorizationToken);
    return this.http.get<UserInfo>(this.baseUrl + 'account/info', { headers: headers });
  }

  /** Pobieramy wszystkie zadania */
  getAllTasks(): (Observable<Array<Task>>) {
    return this.http.get<Array<Task>>(this.baseUrl + 'todos');
  }

  /** Pobieramy jedno zadanie */
  getTask(id: string): (Observable<Task>) {
    return this.http.get<Task>(this.baseUrl + 'todos/' + id);
  }

  /** Pobieramy tylko zadania o nazwie dłuższej niż podana wartość */
  getLongTasks(signsAmount: string): (Observable<Array<Task>>) {
    const params = new HttpParams().set('signsAmount', signsAmount)
    return this.http.get<Array<Task>>(this.baseUrl + 'todos/long', { params: params });
  }

  /** Dodajemy nowe zadanie */
  addTask(task: Task): Observable<Task> {
    return this.http.post<Task>(this.baseUrl + 'todos', task)
  }

  /** Zastąpienie zadania nowym, aktualnym */
  updateTask(task: Task, authorizationToken: string): Observable<Task> {
    const params = new HttpParams().set('id', task.id);
    const headers = new HttpHeaders().set('Authorization', 'Bearer ' + authorizationToken);
    return this.http.put<Task>(this.baseUrl + 'todos/update', task, { params: params, headers: headers })
  }

  /** Usunięcie zadania */
  removeTask(id: string, authorizationToken: string): (Observable<Task>) {
    const headers = new HttpHeaders().set('Authorization', 'Bearer ' + authorizationToken);
    return this.http.delete<Task>(this.baseUrl + 'todos/' + id, { headers: headers });
  }
}
