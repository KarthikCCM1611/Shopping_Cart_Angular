import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ILogin } from '../../models/master/login';
import { IRegister } from '../../models/master/register';
import { IUser } from '../../models/master/user';

@Injectable({
  providedIn: 'root'
})
export class MasterService {

  // userObj: any = null;
  private userSubject = new BehaviorSubject<IUser | null>(null);
  public user$ = this.userSubject.asObservable();

  private baseUrl = 'https://localhost:7168/api/master';
  private http = inject(HttpClient);
  constructor() {
    const user: any = localStorage.getItem("userObj");
    if (user) {
      // this.userObj = user;
      this.setUser(JSON.parse(user));
    }
  }

  // Set user
  setUser(userObj: (IUser | null)) {
    this.userSubject.next(userObj);
  }

  // Optional: get current value
  getUser() {
    return this.userSubject.getValue();
  }

  logIn(logInObj: ILogin): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, logInObj);
  }

  register(registerObj: IRegister) {
    return this.http.post<any>(`${this.baseUrl}/register`, registerObj);
  }
}
