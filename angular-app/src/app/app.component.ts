import { JsonPipe, NgIf, UpperCasePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { MasterService } from './services/master/master.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, NgIf, UpperCasePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  title = 'angular-app';
  userName: string = "";
  masterSrc = inject(MasterService)
  router = inject(Router);
  ngOnInit(): void {
    this.masterSrc.user$.subscribe(userObj => {
      if (userObj?.userName) {
        this.userName = userObj.userName;
        this.router.navigateByUrl("/home")
      }
      else {
        this.userName = "";
        this.router.navigateByUrl("/login")
      }
    });
    // if (this.masterSrc.userObj) {
    //   // this.userName = JSON.parse(this.masterSrc.userObj)?.UserName;
    //   this.router.navigateByUrl("/todo")
    // }
    // else {
    //   this.router.navigateByUrl("/login")
    // }
  }
  logout() {
    localStorage.removeItem("userObj");
    // this.masterSrc.userObj = null;
    this.masterSrc.setUser(null);
    this.router.navigateByUrl("/login")
  }

  // public get userName(): string {
  //   try {
  //     return JSON.parse(this.masterSrc.userObj)?.UserName || '';
  //   } catch {
  //     return '';
  //   }
  // }
}
